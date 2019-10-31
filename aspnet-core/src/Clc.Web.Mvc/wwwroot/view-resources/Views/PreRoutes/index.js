(function() {        
    $(function() {    
        work.isCaptain = true;
        abp.services.app.work.getMyWork().done(function (wk) {
            work.myWork = wk;
        });

        $('#dg').datagrid({
            url: 'PreRoutes/GridData'
        });
    });
})();
