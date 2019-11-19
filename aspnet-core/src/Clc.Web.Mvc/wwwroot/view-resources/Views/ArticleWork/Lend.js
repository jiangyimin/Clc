
(function() {        
    $(function() {
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;

            $('#dd').datebox('setValue', work.me.today);
            $('#dg').datagrid({
                url: 'GridData',
                queryParams: {WpId: work.me.workplaceId, CarryoutDate: work.me.today, DepotId: work.me.depotId, AffairId: work.me.affairId }
            });

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
