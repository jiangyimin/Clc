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

            $('#tbDoor').children('a[name="allowActivate"]').linkbutton('disable');  
            $('#tbDoor').children('a[name="open"]').linkbutton('disable');  
        });

        abp.services.app.work.getLeaders().done(function(data) {
            $('#leader').combobox({
                data: data, 
                valueField: 'value', 
                textField: 'displayText'
            })
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
            if (status != '') return;   // important

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

        $('#tbDoor').children('a[name="leaderFinger"]').click(function (e) {
            if ($('#leader').combobox('getValue') == null) return;

            abp.notify.info('请领导将指纹放到指纹仪上');
            var finger = window.parent.getFingerCode();
            if (finger != '') {
                abp.services.app.work.confirmByFinger(finger, $('#leader').combobox('getValue')).done(function(ret) {
                    if (ret.item1) {
                        $('#tbDoor').children('a[name="allowActivate"]').linkbutton('enable');  
                        $('#tbDoor').children('a[name="open"]').linkbutton('enable');  
                    }
                    else
                        abp.notify.error(ret.item2);
                });
            }

        });

        $('#tbDoor').children('a[name="allowActivate"]').click(function (e) {
            if ($('#tbDoor').children('a[name="allowActivate"]').linkbutton('options').disabled) return;

            var row = $('#dgDoor').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要允许激活的大队", "", { positionClass : 'toast-top-center'} );
                return;
            };

            abp.services.app.work.setReportTime(row.depotName).done(function(ret) {
                abp.notify.success(row.depotName + "设置成功");
                $('#tbDoor').children('a[name="allowActivate"]').linkbutton('disable');  
                $('#tbDoor').children('a[name="open"]').linkbutton('disable');  
            });
        });

        $('#tbDoor').children('a[name="open"]').click(function (e) {
            if ($('#tbDoor').children('a[name="open"]').linkbutton('options').disabled) return;

            if (status != '') return;   // important

            var row = $('#dgDoor').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的门禁", "", { positionClass : 'toast-top-center'} );
                return;
            };

            status = 'emerg';
            doorIp = row.doorIp;
            openConfirmDialog(work.aws);
            $('#tbDoor').children('a[name="allowActivate"]').linkbutton('disable');  
            $('#tbDoor').children('a[name="open"]').linkbutton('disable');  
        }); 


        // 定时器
        setInterval(function() {
            if (status == '') $('#dg').datagrid('reload');
        }, 30000);
        
        // register event
        window.parent.abp.event.on('emergOpenDoor', function () {
            $("#sounds")[0].play();
            $('#dg').datagrid('reload');
        });

        $('#dlgConfirm').dialog({
            onClose: function() {
                cds = [];
                confirms.innerHTML = '';
                status = '';
                doorIp = 0;
                doorRecordId = 0;
            }
        });
    });

    winput.matchWorker = function(rfid) {
        if (status == '') {
            abp.notify.warn('现在不需刷卡');
            return;
        }
        var found = false;
        for (var i = 0; i < cds.length; i++) {
            if (cds[i].rfid == rfid) {
                found = true;
                cds[i].confirmed = true;
                confirms.innerHTML = createHTML();
            
                if (allConfirmed()) doOpenDoor();
                break;
            }
        }

        if (!found) abp.notify.warn('无此RFID');
    }  
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
    abp.services.app.doorRecord.carryoutEmergOpen(doorRecordId, work.me.affairId).done(function() {
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

