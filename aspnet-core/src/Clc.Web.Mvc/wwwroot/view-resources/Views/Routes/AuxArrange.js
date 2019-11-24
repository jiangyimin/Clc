(function() {        
    $(function() {    
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);

            // alert(work.me.workplaceId);
            $('#dg').datagrid({
                url: 'AuxGridData',
                queryParams: {CarryoutDate: work.me.today, WorkplaceId: work.me.workplaceId }
            });
        });
    });
})();
