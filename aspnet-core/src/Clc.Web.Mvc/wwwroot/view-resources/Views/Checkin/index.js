(function() {        
    $(function() {
        abp.services.app.work.allowCardWhenCheckin().done(function (ret) {
            allowCardWhenCheckin = ret;
        });

        // set work.me
        work.me.today = $('#dd').datebox('getValue');
        work.me.altcheck = document.getElementById('altcheck').value;
        work.me.depotId = document.getElementById('depotId').value;
        work.me.affairId = document.getElementById('affairId').value;
        work.me.workplaceId = document.getElementById('workplaceId').value;

        showAffair();

        $('#tb').children('a[name="reload"]').click(function (e) {
            showAffair();
        });
    
        $('#tb').children('a[name="askOpen"]').click(function (e) {
            if (status != '') return;   // important
 
            if (!work.validate()) return; 
       
            // alert('enter askOpen button');
            status = 'ask';
            doorId = work.me.workplaceId;
            openConfirmDialog($('#dg').datagrid('getRows'));
        });  

        $('#tb').children('a[name="askGuard"]').click(function (e) {
            e.preventDefault();
            if (!work.validate()) return; 
            abp.message.confirm('确认要申请枪库设防吗?', '请确认', function (r) {
                if (r) {
                    abp.ajax({
                        contentType: 'application/x-www-form-urlencoded',
                        url: 'Checkin/AskGuard',
                        data: {wpId: work.me.workplaceId, affairId: work.me.affairId, workerCn: window.parent.me.workerCn }  
                    }).done(function (ret) {
                        abp.notify.info(ret.message);
                    });
                }
            });            
        });

        $('#tb').children('a[name="askVaultGuard"]').click(function (e) {
            if (!work.validate()) return; 
            abp.message.confirm('确认要申请金库设防吗?', '请确认', function (r) {
                if (r) {
                    abp.ajax({
                        contentType: 'application/x-www-form-urlencoded',
                        url: 'Checkin/AskVaultGuard',
                        data: {depotId: work.me.depotId, affairId: work.me.affairId, workerCn: window.parent.me.workerCn }  
                    }).done(function (ret) {
                        abp.notify.info(ret.message);
                    });
                }
            });            
        });

        $('#dlgConfirm').dialog({
            onClose: function () {
                cds = [];
                confirms.innerHTML = '';
                status = '';
                doorIp = 0;
            }
        });

    });

})();

function showAffair() {
    if (!work.validate()) return; 

    $('#dg').datagrid({
        url: 'Checkin/GridDataWorker/' + work.me.affairId
    });

    $('#dgTask').datagrid({
        url: 'Checkin/GridDataTask/' + work.me.affairId
    });
}

var cds = [];
function openConfirmDialog(d) {
    for (var i = 0; i < d.length; i++) {
        if (d[i].checkoutTime) continue;
        cds.push({ confirmed: false, displayText: d[i].workerCn + ' ' + d[i].workerName, workerId: d[i].workerId, rfid: d[i].workerRfid });
    };
    $('#dlgConfirm').dialog('open');
    confirms.innerHTML = createHTML();
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

function closeConfirmDialog() {
    // alert('closeDlg')
    $('#dlgConfirm').dialog('close');
    showAffair();
}

function getConfirmedWorkers()
{
    var ids = [];
    for (var i = 0; i < cds.length; i++) {
        if (cds[i].confirmed)
            ids.push(cds[i].workerId);
    }
    return ids;
}

function rfidConfirm(rfid) {
    for (var i = 0; i < cds.length; i++) {
        if (cds[i].rfid == rfid) {
            cds[i].confirmed = true;
            break;
        }
    }
    confirms.innerHTML = createHTML();
}

function send(wait) {
    var start = '';
    var end = '';
    var taskId = 0;
    if (status == 'ask') {
        start = $('#fm').find('input[name="startTime"]').val();
        end = $('#fm').find('input[name="endTime"]').val();
    }
    else {
        start = rowTask.startTime;
        end = rowTask.endTime;
        taskId = rowTask.id;
    }

    abp.ajax({
        contentType: 'application/x-www-form-urlencoded',
        url: 'Checkin/AskOpen',
        data: {workers: getConfirmedWorkers(), affairId: work.me.affairId, doorId: doorId, start: start, end: end, taskId: taskId, wait: wait }  
    }).done(function (ret) {
        abp.notify.info(ret.message);
        if (ret.success) 
            closeConfirmDialog();
    });            
}

