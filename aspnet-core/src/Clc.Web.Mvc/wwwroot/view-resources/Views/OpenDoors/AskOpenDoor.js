(function() {        
    $(function() {
        abp.services.app.work.allowCardWhenCheckin().done(function (ret) {
            allowCardWhenCheckin = ret;
        });

        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            // alert(work.me.affairId);
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dg').datagrid({
                url: 'GridDataAskDoor?Dt=' + work.me.today 
            });

            // alert(work.me.affairId);
            abp.services.app.affair.getAffairWorkersSync(work.me.affairId).done(function (aws) {
                work.aws = aws;
                // alert(aws.length);
            });
    
        });

        $('#dg').datagrid({
            onSelect: function() { photos.innerHTML = ''; } 
        });

        // #tb Buttons
        $('#yesterday').checkbox({
            onChange: function() {
                if ($('#yesterday').checkbox('options').checked) {
                    var t = work.getYesterday(work.me.today);
                    // alert(t);
                    $('#dd').datebox('setValue', t);
                    $('#dg').datagrid({
                        url: 'GridDataAskDoor?Dt=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', work.me.today);
                    $('#dg').datagrid({
                        url: 'GridDataAskDoor?Dt=' + work.me.today
                    });
                }
            }
        });

        $('#tb').children('a[name="open"]').click(function (e) {
            if (status != '') return;   // important

            var row = $('#dg').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的申请", "", { positionClass : 'toast-top-center'} );
                return;
            };
            
            status = 'ask';
            doorRecordId = row.id;
            doorIp = row.workplaceDoorIp;
            openConfirmDialog(work.aws);
        });
    
        $('#tbCamera').children('a[name="open"]').click(function (e) {
            var row = $('#dg').datagrid('getSelected');

            abp.services.app.affair.getAffairWorkersSync(row.askAffairId).done(function (ws) {
                photos.innerHTML = createPhotosHTML(ws);
            });

            // alert(row.workplaceCameraIp);
            startplay(row.workplaceCameraIp);
        });

        $('#tbCamera').children('a[name="close"]').click(function (e) {
            stopplay();
        });
    
        // register event
        window.parent.abp.event.on('askOpenDoor', function () {
            // alert('on ask');
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
})();

function createPhotosHTML(ws) {
    var h = '';
    for(var i = 0; i < ws.length; i++) {
        h += "<div class='comp'><img class='compPhoto' src='data:image/jpg;base64," + ws[i].photoString + "'>";
        h += "<p>" + ws[i].workerName + "</p></div>";
    }
    return h;
}

function config() {
    var obj = document.getElementById("EasyPlayerOcx");

    var cache = 7; //document.getElementById("cache").value;
    var playsound = 0; //document.getElementById("playsound").checked ? 1 : 0;
    var showtoscale = 1; //document.getElementById("showtoscale").checked ? 1 : 0;
    var showsatic = 1; //document.getElementById("showsatic").checked ? 1 : 0;

    obj.Config(cache, playsound, showtoscale, showsatic);
    //obj.SetOSD(1, 255, 0, 0, 255, 100, 100, 1000, 150, "EasyPlayer-RTSP OSD测试");
    //alert(cache+";"+playsound+";"+showtoscale+";"+showsatic);
}

function startplay(ip) {
    stopplay();
    config();

    var obj = document.getElementById("EasyPlayerOcx");
    var url = "rtsp://" + ip + ":554/h264/ch1/av_stream";
    var rendertype = 7; //document.getElementById("rendertype").value;
    var name = "admin"; //document.getElementById("name").value;
    var password = abp.setting.get('Const.CameraPassword');
    // alert(ip + " " + password);
    var harddecode = 1; //document.getElementById("harddecode").checked ? 1 : 0;
    var rtpovertcp = 1; // document.getElementById("rtpovertcp").checked ? 1 : 0;
    obj.Start(url, rendertype, name, password, harddecode, rtpovertcp);
    //alert(url+";"+rendertype+";"+name+";"+password);
}

function stopplay() {
    //alert("Close()!!!!!");
    var obj = document.getElementById("EasyPlayerOcx");

    obj.Close();
}

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
