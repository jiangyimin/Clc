
(function() {        
    $(function() {
        abp.services.app.work.getMyWork().done(function (wk) {
            work.myWork = wk;
            workers.innerHTML = '监控人员：' + work.getWorkersString();
            $('#dg').datagrid({
                url: 'GridData'
            });
        });

        $('#dg').datagrid({
            onSelect: function (index, row) {
                $('#dgRecord').datagrid({
                    url: 'GridDataRecord?WorkplaceId=' + row.id
                });
            }
        });
    });
})();
