
(function() {        
    $(function() {
        abp.services.app.work.getCheckinAffairWork().done(function (wk) {
            work.me = wk;
            $('#dgDoor').datagrid({
                url: 'GridDataDoor'
            });
            $('#dg').datagrid({
                url: 'GridDataAskDoor?Day=' + work.me.today 
            });
        });

        $('#tb').children('a[name="open"]').click(function (e) {
            var row = $('#dg').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的申请", "", { positionClass : 'toast-top-center'} );
                return;
            };

            doorIp = row.workplaceDoorIp;
            $('#dlg').dialog('open');
        });  
    });
})();
