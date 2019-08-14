
(function() {        
    $(function() {    
        // get today
        abp.services.app.work.getTodayString().done(function (dd) {
            $('#dd').datebox('setValue', dd);
            $('#dg').datagrid({
                url: 'GridData?CarryoutDate=' + dd
            });
        });

        abp.services.app.work.getMyWork().done(function (work) {
            myWork = work;
            $('#managers').textbox('setValue', '库房管理人：' + myWork.workers);
        });
    });
})();
