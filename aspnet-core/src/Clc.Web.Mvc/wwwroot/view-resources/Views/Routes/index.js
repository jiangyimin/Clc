(function() {        
    $(function() {    
        abp.services.app.work.getToday().done(function (today) {
            mds.today = today;
            $('#dd').datebox('setValue', mds.today);

            $('#dg').datagrid({
                url: 'Routes/GridData?CarryoutDate=' + mds.today
            });
        });

        // #tb Buttons
        $('#tomorror').checkbox({
            onChange: function() {
                if ($('#tomorror').checkbox('options').checked) {
                    var t = work.getTomorrow(mds.today);
                    // alert(t);
                    $('#dd').datebox('setValue', t);
                    $('#dg').datagrid({
                        url: 'Routes/GridData?CarryoutDate=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', mds.today);
                    $('#dg').datagrid({
                        url: 'Routes/GridData?CarryoutDate=' + mds.today
                    });
                }
            }
        });

        $('#tb').children('a[name="event"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择线路");
                return;
            };
            $('#dlgEvent').dialog('open');
            $('#dgEvent').datagrid({
                url: "Routes/GridDataEvent/" + mds.masterCurrentRow.id
            });
        });

        $('#tb').children('a[name="activate"]').click(function (e) {
            work.openActivateDialog('activateRoute');
        })

        $('#tb').children('a[name="back"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择线路");
                return;
            };
            work.openActivateDialog('backRoute');
        })

        $('#tb').children('a[name="backToLend"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择线路");
                return;
            };

            var rows = $('#dgWorker').datagrid('getRows');
            if (rows != null) {
                var ur = false;
                for (var i = 0; i < rows.length; i++) {
                    var al = rows[i].articleList;
                    if (al != null && al.indexOf("未还") >= 0) {
                        ur = true;
                        break;
                    }
                }

                if (!ur) {
                    abp.notify.error("不需要回退到领用状态");
                    return;
                }
            } 

            work.openActivateDialog('backToLend');
        })

        $('#tb').children('a[name="close"]').click(function (e) {
            var checkedRows = $('#dg').datagrid("getChecked");
            if (checkedRows.length === 0) {
                abp.notify.error("请先选中要关闭的线路。");
                return;
            };
            if (mds.masterCurrentRow.status == "安排") {
                abp.notify.error("激活状态才允许关闭");
                return;
            };
            abp.message.confirm('确认要关闭吗?', '确认', function (r) {
                if (r) {

                    var ids = [];
                    for (var i = 0; i < checkedRows.length; i++) {
                        var row = checkedRows[i];
                        if (row.status == "激活") ids.push(row.id);
                    };

                    abp.services.app.route.close(ids).done(function (count) {
                        abp.notify.success('有' + count + '个线路被关闭');
                        // Cache active routes
                        abp.services.app.route.setActiveRouteCache(mds.today).done(function() {
                            abp.notify.info("刚下达最新的激活线路");
                        });
                        mds.reload('');                        
                    })
                };
            });
        });         

        $('#tb').children('a[name="createFrom"]').click(function (e) {
            e.preventDefault();
            var from = $('#fromDate').datebox('getValue');
            if (from == '') {
                abp.notify.error("请先设置来源日期!");
                return;
            }
            if ($('#dd').datebox('getValue') <= from){
                abp.notify.error("任务执行日期必须大于来源日期!");
                return;
            }
        
            abp.message.confirm('确认要生成吗?', '确认', function (r) {
                if (r) {
                    abp.ui.setBusy($('#tb'));
    
                    abp.services.app.route.createFrom($('#dd').datebox('getValue'), from).done(function (data) {
                        abp.notify.success('已生成'+data+'个任务');
                        if (data > 0) mds.reload('');
                    }).always(function () {
                        abp.ui.clearBusy($('#tb'));
                    });
                };
            });
        });

        $('#tb').children('a[name="createFromPre"]').click(function (e) {
            e.preventDefault();
            abp.message.confirm('确认要从预排生成吗?', '确认', function (r) {
                if (r) {
                    abp.ui.setBusy($('#tb'));
    
                    abp.services.app.route.createFromPre($('#dd').datebox('getValue')).done(function (data) {
                        abp.notify.info('已生成'+data+'个任务');
                        if (data > 0) mds.reload('');
                    }).always(function () {
                        abp.ui.clearBusy($('#tb'));
                    });
                };
            });
        });

    });

    work.verifyDone = function (verifyAction, style) {
        if (verifyAction == 'activateRoute')
            activate(style);
        if (verifyAction == 'backRoute')
            back(style);
        if (verifyAction == 'backToLend')
            backToLend(style);
    };

    function back(style) {
        var status = mds.masterCurrentRow.status;
        if (status == "领物" || status == "还物") {
            abp.notify.error('在领用状态不能回退');
            return;
        }

        abp.services.app.route.back(mds.masterCurrentRow.id, style).done(function () {
            mds.reload('');
        });
    };

    function backToLend(style) {
        var status = mds.masterCurrentRow.status;
        if (status !== "还物") {
            abp.notify.error('非还物状态不能回退');
            return;
        }

        abp.services.app.route.backToLend(mds.masterCurrentRow.id, style).done(function () {
            mds.reload('');
        });
    };

    function activate(style) {
        var checkedRows = $('#dg').datagrid("getChecked");
        // if (checkedRows.length === 0) {
        //     abp.notify.error("请先选中要激活的线路。");
        //     return;
        // }

        var ids = [];
        for (var i = 0; i < checkedRows.length; i++) {
            var row = checkedRows[i];
            if (row.status == "安排") ids.push(row.id);
        };


        abp.services.app.route.activate(ids, style).done(function (ret) {
            abp.notify.success('有' + ret.item2 + '个线路被激活');
            
            // Cache active routes
            abp.services.app.route.setActiveRouteCache(mds.today).done(function() {
                abp.notify.info("下达最新的激活线路");
            });
    
            if (ret.item1) abp.notify.error(ret.item1);
            mds.reload('');
        });
    }

})();
