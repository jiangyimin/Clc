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
            if (!work.validate()) return; 
       
            status = 'ask';
            doorId = work.me.workplaceId;
            openConfirmDialog($('#dg').datagrid('getRows'));
        });  

        $('#tb').children('a[name="askVaultGuard"]').click(function (e) {
            if (!work.validate()) return; 
            abp.message.confirm('确认要申请金库设防吗?', '请确认', function (r) {
                if (r) {
                    abp.ajax({
                        contentType: 'application/x-www-form-urlencoded',
                        url: 'Checkin/AskVaultGuard',
                        data: {depotId: work.me.depotId }  
                    }).done(function (ret) {
                        abp.notify.info(ret.message);
                    });
                }
            });            
        });

        $('dlgConfirm').dialog({
            onClose: function () { 
                status = '';
                doorIp = 0;
            }
        });

     });

    function showAffair() {
        if (!work.validate()) return; 

        $('#dg').datagrid({
            url: 'Checkin/GridDataWorker/' + work.me.affairId
        });

        $('#dgTask').datagrid({
            url: 'Checkin/GridDataTask/' + work.me.affairId
        });
    }


})();

// var confirmData;
function openConfirmDialog(data) {
    for (var i = 0; i < data.length; i++) data[i].confirmed = '';

    $('#dlgConfirm').dialog('open');
    $('#dgConfirm').datagrid('loadData', { rows: data });
}

function closeConfirmDialog() {
    // alert('closeDlg')
    $('#dlgConfirm').dialog('close');
}

function getConfirmedWorkers()
{
    var ids = [];
    var rows = $('#dg').datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].confirmed == "是")
            ids.push(rows[i].workerId);
    }
    return ids;
}

function rfidConfirm(rfid) {
    var rows = $('#dg').datagrid('getRows');
    // alert(rows.length);
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].workerRfid == rfid) {
            rows[i].confirmed = "是";
            $('#dgConfirm').datagrid('loadData', { rows: rows });
            break;
        }
    }
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

