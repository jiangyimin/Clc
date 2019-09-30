(function() {        
    $(function() {    
        work.isCaptain = true;
       // get today
        abp.services.app.work.getTodayString().done(function (d) {
            mds.today = d;
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

        $('#tb').children('a[name="activate"]').click(function (e) {
            var checkedRows = $('#dg').datagrid("getChecked");
            if (checkedRows.length === 0) {
                abp.notify.error("请先选中要激活的任务。");
                return;
            }

            ids = [];
            for (var i = 0; i < checkedRows.length; i++) {
                var row = checkedRows[i];
                if (row.status == "安排") ids.push(row.id);
            };
            // alert(ids);
            abp.services.app.affair.activate(ids).done(function (count) {
                abp.notify.info('有' + count + '个任务被激活');
                mds.reload('');
            })
        });

        $('#tb').children('a[name="back"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择任务");
                return;
            };
            if (mds.masterCurrentRow.status !== "激活") {
                abp.notify.error("激活状态才允许回退");
                return;
            };
            abp.services.app.affair.back(mds.masterCurrentRow.id).done(function () {
                mds.reload('');
            });
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
    });
})();
