
(function() {        
    $(function() {
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            // alert(work.me.affairId);
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dg').datagrid({
                url: 'GridDataIssue?Dt=' + work.me.today 
            });
        });

        // #tb Buttons
        $('#yesterday').checkbox({
            onChange: function() {
                if ($('#yesterday').checkbox('options').checked) {
                    var t = work.getYesterday(work.me.today);
                    // alert(t);
                    $('#dd').datebox('setValue', t);
                    $('#dg').datagrid({
                        url: 'GridDataIssue?Dt=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', work.me.today);
                    $('#dg').datagrid({
                        url: 'GridDataIssue?Dt=' + work.me.today
                    });
                }
            }
        });
    });
})();
