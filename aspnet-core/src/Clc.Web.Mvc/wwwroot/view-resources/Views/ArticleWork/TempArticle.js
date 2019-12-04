
(function() {        
    $(function() {
        finput.style = 0;
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            if (!work.validate2()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dg').datagrid({
                url: 'GridDataTemp/AffairId=' + work.me.affairId
            });

        });

        $('#take').checkbox({
            onChange: function() {
                if ($('#take').checkbox('options').checked) {
                    $('#tb').css("background-color","red");
                    finput.style = 1;
                }
                else {
                    $('#tb').css("background-color","");
                    finput.style = 0;
                }
            }
        });
    });
})();
