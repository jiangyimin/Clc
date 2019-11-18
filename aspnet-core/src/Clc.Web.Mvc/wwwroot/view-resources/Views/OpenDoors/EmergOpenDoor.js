(function() {        
    $(function() {
        abp.services.app.work.getCheckinAffairWork().done(function (wk) {
            work.me = wk;
            $('#dgDoor').datagrid({
                url: 'GridDataDoor'
            });
            $('#dg').datagrid({
                url: 'GridDataEmergDoor?Day=' + work.me.today
            });
        });

        // #tb Buttons
        $('#yestoday').checkbox({
            onChange: function() {
                if ($('#yestoday').checkbox('options').checked) {
                    var t = work.getYestoday(work.me.today);
                    // alert(t);
                    $('#dd').datebox('setValue', t);
                    $('#dg').datagrid({
                        url: 'GridDataEmergDoor?CarryoutDate=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', work.me.today);
                    $('#dg').datagrid({
                        url: 'GridDataEmergDoor?Day=' + work.me.today
                    });
                }
            }
        });

        $('#tb').children('a[name="open"]').click(function (e) {
            var row = $('#dg').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的申请", "", { positionClass : 'toast-top-center'} );
                return;
            };

            status = 'emerg';
            doorIp = row.workplaceDoorIp;
            $('#dlg').dialog('open');
        });  

        $('#tbDoor').children('a[name="open"]').click(function (e) {
            var row = $('#dgDoor').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的门禁", "", { positionClass : 'toast-top-center'} );
                return;
            };

            status = '';
            doorIp = row.doorIp;
            $('#dlg').dialog('open');
        }); 

    });
})();
