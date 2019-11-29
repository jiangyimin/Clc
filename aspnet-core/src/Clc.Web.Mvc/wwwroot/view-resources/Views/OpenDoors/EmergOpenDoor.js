(function() {        
    $(function() {
        abp.services.app.work.allowCardWhenCheckin().done(function (ret) {
            allowCardWhenCheckin = ret;
        });

        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dgDoor').datagrid({
                url: 'GridDataDoor'
            });
            $('#dg').datagrid({
                url: 'GridDataEmergDoor?Dt=' + work.me.today
            });

            abp.services.app.affair.getAffairWorkers(work.me.affairId).done(function (aws) {
                work.aws = aws;
                // alert(aws.length);
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
                        url: 'GridDataEmergDoor?Dt=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', work.me.today);
                    $('#dg').datagrid({
                        url: 'GridDataEmergDoor?Dt=' + work.me.today
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
           if (row.emergDoorPassword != row.workplaceEmergPassword) {
                abp.notify.error("应急密码不符！", "", { positionClass : 'toast-top-center'} );
                return;
            };

            status = 'emerg';
            doorIp = row.workplaceDoorIp;
            doorRecordId = row.id;

            $('#dlg').dialog('open');
        });  

        $('#tbDoor').children('a[name="open"]').click(function (e) {
            var row = $('#dgDoor').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的门禁", "", { positionClass : 'toast-top-center'} );
                return;
            };

            // alert(row.emergPassword);
            // alert($('#password').val());
            if ($('#password').val() != row.emergPassword) {
                abp.notify.error("应急密码不符！", "", { positionClass : 'toast-top-center'} );
                return;
            };

            status = 'emerg';
            doorIp = row.doorIp;
            openConfirmDialog(work.aws);
        }); 

        // register event
        window.parent.abp.event.on('emergOpenDoor', function () {
            $("#sounds")[0].play();
            $('#dg').datagrid('reload');
        });

        $('#dlgConform').dialog({
            onClose: function() {
                status = '';
                doorIp = 0;
                doorRecordId = 0;
            }
        });
    });
})();

function openConfirmDialog(data) {
    for (var i = 0; i < data.length; i++) data[i].confirmed = '';

    $('#dlgConfirm').dialog('open');
    $('#dgConfirm').datagrid('loadData', { rows: data });
}

function allConfirmed() {
    for (var i = 0; i < work.aws.length; i++) {
        if (work.aws[i].confirmed == '') return false;
    }
    return true;
}

function doOpenDoor() {
    // udpate askDoorRecord
    if (doorRecordId == 0)
        $('#dlgConfirm').dialog('close');
    else
        abp.services.app.doorRecord.carryoutEmergOpen(doorRecordId, work.me.affairId).done(function() {
            $('#dg').datagrid('reload');
            $('#dlgConfirm').dialog('close');
        });

    abp.notify.success('已发送开门命令到对应的门禁', '', { positionClass : 'toast-top-center' });
    ws && ws.send(doorIp);
}
