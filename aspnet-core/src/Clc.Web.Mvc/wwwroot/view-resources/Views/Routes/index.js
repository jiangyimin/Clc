(function() {        
    $(function() {    
        isCaptain = true;
        // get today
        abp.services.app.work.getTodayString().done(function (d) {
            mds.today = d;
            $('#dd').datebox('setValue', mds.today);

            $('#dg').datagrid({
                url: 'Routes/GridData',
                queryParams: {CarryoutDate: mds.today}
            });
        });

        $('#tb').children('a[name="activate"]').click(function (e) {
            var checkedRows = $('#dg').datagrid("getChecked");
            if (checkedRows.length === 0) {
                abp.notify.error("请先选中要激活的线路。");
                return;
            }

            ids = [];
            for (var row of checkedRows) {
                if (row.status == "安排") ids.push(row.id);
            };

            abp.services.app.route.activate(ids).done(function (count) {
                abp.notify.info('有' + count + '个线路被激活');
                mds.reload('');
            })
        });

        $('#tb').children('a[name="back"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择线路");
                return;
            };
            if (mds.masterCurrentRow.status !== "激活") {
                abp.notify.error("激活状态才允许回退");
                return;
            };
            abp.services.app.route.back(mds.masterCurrentRow.id).done(function () {
                mds.reload('');
            });
        })            

        $('#tb').children('a[name="close"]').click(function (e) {
            var checkedRows = $('#dg').datagrid("getChecked");
            if (checkedRows.length === 0) {
                abp.notify.error("请先选中要关闭的线路。");
                return;
            }

            ids = [];
            for (var row of checkedRows) {
                if (row.status == "激活") ids.push(row.id);
            };

            abp.services.app.route.activate(ids).done(function (count) {
                abp.notify.info('有' + count + '个线路被关闭');
                mds.reload('');
            })
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
    
                    abp.services.app.route.createFrom($('#dd').datebox('getValue'), from).done(function (data) {
                        abp.notify.info('已生成'+data+'个任务');
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
})();
