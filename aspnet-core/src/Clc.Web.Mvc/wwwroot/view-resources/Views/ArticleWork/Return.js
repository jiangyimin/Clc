
(function() {        
    $(function() {
        abp.services.app.work.getMyWork().done(function (wk) {
            work.myWork = wk;
            workers.innerHTML = '库房管理人：' + work.myWork.workers;
            // get today
            abp.services.app.work.getTodayString().done(function (dd) {
                $('#dd').datebox('setValue', dd);
                work.dd = dd;
                $('#dg').datagrid({
                    url: 'GridData',
                    queryParams: {CarryoutDate: work.dd, AffairId: work.myWork.affairId }
                });
            });
        });

        $('#dg').datagrid({
            onSelect: function (index, row) {
                $('#dgWorker').datagrid({
                    url: 'GridDataWorker/' + row.id
                });
            }
        });
    });
})();
