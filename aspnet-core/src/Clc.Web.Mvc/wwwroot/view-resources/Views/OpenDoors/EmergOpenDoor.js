(function() {        
    $(function() {
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dgDoor').datagrid({
                url: 'GridDataDoor'
            });
            $('#dg').datagrid({
                url: 'GridDataEmergDoor?Date=' + work.me.today
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
                        url: 'GridDataEmergDoor?Date=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', work.me.today);
                    $('#dg').datagrid({
                        url: 'GridDataEmergDoor?Date=' + work.me.today
                    });
                }
            }
        });

        $('#tb').children('a[name="open"]').click(function (e) {
            var row = $('#dg').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要应急开门申请", "", { positionClass : 'toast-top-center'} );
                return;
            };

            // judge emergPassword.
            alert(row.emergDoorPassword);
            alert(row.workplaceEmergPassword);
            if (row.emergDoorPassword != row.workplaceEmergPassword) {
                abp.notify.error("应急密码不符！", "", { positionClass : 'toast-top-center'} );
                return;
            };

            status = 'emerg';
            doorIp = row.workplaceDoorIp;
            emergDoorRecordId = row.id;

            $('#dlg').dialog('open');
        });  

        $('#tbDoor').children('a[name="open"]').click(function (e) {
            var row = $('#dgDoor').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的门禁", "", { positionClass : 'toast-top-center'} );
                return;
            };

            alert(row.emergPassword);
            alert($('#password').val());
            if ($('#password').val() != row.emergPassword) {
                abp.notify.error("应急密码不符！", "", { positionClass : 'toast-top-center'} );
                return;
            };

            status = '';
            doorIp = row.doorIp;
            $('#dlg').dialog('open');
        }); 

        // register event
        window.parent.abp.event.on('emergOpenDoor', function () {
            $('#dg').datagrid('reload');
        });
    });
})();
