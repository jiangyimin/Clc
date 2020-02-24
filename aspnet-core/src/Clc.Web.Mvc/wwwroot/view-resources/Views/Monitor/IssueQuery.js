
(function() {        
    $(function() {
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            // alert(work.me.affairId);
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);
        });

        // #tb Buttons
        $('#dd').datebox({
            onChange: function() {
                $('#dg').datagrid({
                    url: 'GridDataIssue?Dt=' + $('#dd').datebox('getValue') 
                });
            }
        });
    });
})();
