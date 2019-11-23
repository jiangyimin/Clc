
(function() {        
    $(function() {
        finput.style = 1;   // return
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dg').datagrid({
                url: 'GridData',
                queryParams: { WpId: work.me.workplaceId, CarryoutDate: work.me.today, DepotId: work.me.depotId, AffairId: work.me.affairId }
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
