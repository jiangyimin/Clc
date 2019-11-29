(function() {       
    $(function() {    
        abp.services.app.work.getToday().done(function (today) {
            mds.today = today;
            $('#dd').datebox('setValue', mds.today);
            $('#dg').datagrid({
                url: 'Affairs/GridData?CarryoutDate=' + mds.today
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
                        url: 'Affairs/GridData?CarryoutDate=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', mds.today);
                    $('#dg').datagrid({
                        url: 'Affairs/GridData?CarryoutDate=' + mds.today
                    });
                }
            }
        })

        $('#tb').children('a[name="event"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择任务");
                return;
            };
            $('#dlgEvent').dialog('open');
            $('#dgEvent').datagrid({
                url: "Affairs/GridDataEvent/" + mds.masterCurrentRow.id
            });
        })

        $('#tb').children('a[name="activate"]').click(function (e) {
            window.parent.openActivateDialog('activateAffair');
        })

        $('#tbTask').children('a[name="add"]').click(function (e) {
            window.parent.openActivateDialog('addTask');
        })

        $('#tb').children('a[name="back"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择任务");
                return;
            };
            if (mds.masterCurrentRow.status !== "激活") {
                abp.notify.error("激活状态才允许回退");
                return;
            };
            window.parent.openActivateDialog('backAffair');
        })

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
    
                    abp.services.app.affair.createFrom($('#dd').datebox('getValue'), from).done(function (data) {
                        abp.notify.info('已生成'+data+'个任务');
                        if (data > 0) mds.reload('');
                    }).always(function () {
                        abp.ui.clearBusy($('#tb'));
                    });
                }
            })
        });

        window.parent.abp.event.on('verifyDone', function(p) {
            if (p.name == 'activateAffair')
                activate(p.style);
            
            if (p.name == 'backAffair')
                back(p.style);
            
            if (p.name == 'addTask') {
                mds.add('Task');
            }
        });
    });

    function back(style) {
        abp.services.app.affair.back(mds.masterCurrentRow.id, style).done(function () {
            mds.reload('');
        });
    };

    function activate(style) {
        var checkedRows = $('#dg').datagrid("getChecked");
        // if (checkedRows.length === 0) {
        //     abp.notify.error("请先选中要激活的任务。");
        //     return;
        // }

        var ids = [];
        for (var i = 0; i < checkedRows.length; i++) {
            var row = checkedRows[i];
            if (row.status == "安排") ids.push(row.id);
        };
        // alert(ids);
        abp.services.app.affair.activate(ids, style).done(function (ret) {
            abp.notify.info('有' + ret.item2 + '个任务被激活');
            // Cache active affairs
            abp.services.app.affair.setActiveAffairCache(mds.today).done(function() {
                abp.notify.info("下达最新的激活任务信息");
            });

            if (ret.item1) abp.notify.error(ret.item1);
            mds.reload('');
        })
    };

})();
