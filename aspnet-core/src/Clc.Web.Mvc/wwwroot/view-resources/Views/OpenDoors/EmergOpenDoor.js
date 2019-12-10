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
            //if (row.emergDoorPassword != row.workplaceEmergPassword) {
            //    abp.notify.error("应急密码不符！", "", { positionClass : 'toast-top-center'} );
            //    return;
            //};

            status = 'emerg';
            doorIp = row.workplaceDoorIp;
            doorRecordId = row.id;
            openConfirmDialog(work.aws);
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
                cds = [];
                confirms.innerHTML = '';
                status = '';
                doorIp = 0;
                doorRecordId = 0;
            }
        });
    });
})();

var cds = [];
function openConfirmDialog(d) {
    for (var i = 0; i < d.length; i++) {
        cds.push({ confirmed: false, displayText: d[i].workerCn + ' ' + d[i].workerName, workerId: d[i].workerId, rfid: d[i].workerRfid });
    };
    $('#dlgConfirm').dialog('open');
    confirms.innerHTML = createHTML();
    // alert(confirms.innerHTML);
}

function createHTML() {
    var h = '';        
    for (var i = 0; i < cds.length; i++) {
        // alert(as[i].recordId);
        if (cds[i].confirmed)
            h += "<li><input type='checkbox' checked='true' onclick='return false'>&nbsp;" + cds[i].displayText + "&nbsp;&nbsp;";
        else
            h += "<li><input type='checkbox' onclick='return false'>&nbsp;" + cds[i].displayText + "&nbsp;&nbsp;";

        h += "<a href='javascript:void(0)' onclick='fingerConfirm(" + i + ")'>指纹确认</a>" + "</li>";
    }
    return h;
}


function allConfirmed() {
    for (var i = 0; i < cds.length; i++) {
        if (!cds[i].confirmed) return false;
    }
    return true;
}

function doOpenDoor() {
    notifyAskWorkers(doorRecordId);
    // udpate askDoorRecord
    abp.services.app.doorRecord.carryoutAskOpen(doorRecordId, work.me.affairId).done(function() {
        $('#dg').datagrid('reload');
        // alert('before close')
        $('#dlgConfirm').dialog('close');
    });

    abp.notify.success('已发送开门命令到对应的门禁', '', { positionClass : 'toast-top-center' });
    ws && ws.send(doorIp);
}

function notifyAskWorkers(doorRecordId) {
    abp.ajax({
        contentType: 'application/x-www-form-urlencoded',
        url: 'NotifyAskWorkers',
        data: {doorRecordId: doorRecordId }  
    }).done(function (ret) {
        // abp.notify.info(ret.message);
    });
}
