
(function() {        
    $(function() {
        abp.services.app.work.getMyWork().done(function (wk) {
            work.myWork = wk;
            workers.innerHTML = '金库管理人：' + work.getWorkersString();
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
                $('#dgTask').datagrid({
                    url: 'GridDataTask/' + row.id
                });
            }
        });

        $('#dl').datalist({
            data: finput.boxes,
            valueField: 'boxId',
            textField: 'displayText',
            groupField: 'outlet',
        });

    });
})();
