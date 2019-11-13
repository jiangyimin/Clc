
(function() {        
    $(function() {
        abp.services.app.work.getMyWork().done(function (wk) {
            work.myWork = wk;
            workersInfo.innerHTML = '库房管理人：' + work.getWorkersString();
        });

        $('#dg').datagrid({
            onSelect: function (index, row) {
                $('#dgWorker').datagrid({
                    url: 'GridDataWorker/' + row.id
                });
            }
        });

        $('#dl').datalist({
            data: finput.articles,
            valueField: 'articleId',
            textField: 'displayText',
            lines: true,
            textFormatter: function(value,row,index) {
                return '<span style="font-size:24px">'+value+'</span>';
            }
        });
    });
})();
