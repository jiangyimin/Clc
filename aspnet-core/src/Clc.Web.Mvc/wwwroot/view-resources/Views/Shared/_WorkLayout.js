var isCaptain = false;
var myWork = {};

function parseMessage(msg) {
    alert(msg);
    var cmd = msg.split(' ', 2);
    if (cmd[0] !== myWork.workerCn) return;
    alert( cmd[0] + cmd[1]);

    if (cmd[1] == "lockScreen") lockScreen();
    else {
        if (isCaptain) unlockScreen();
        else 
            abp.services.app.work.getNow().done(function (now) {
                var result = validateCanUnlock(now);
                if (result != '') {
                    abp.notify.error(result);
                    return;
                } 
                unlockScreen();        
            });
    }
}

function lockScreen() {
    var lockdiv = document.getElementById("lockDiv");
    if (lockdiv != null) {
        lockdiv.style.display = "block";
    }
}
function unlockScreen() {
    var lockdiv = document.getElementById("lockDiv");
    if (lockdiv != null) {
        lockdiv.style.display = "none";
    }
}

function validateCanUnlock(now) {
    if (isCaptain) return '';
    if (myWork.affairId == 0 || myWork.status == "安排")
        return '未安排或任务未激活';
    if (now < myWork.startTime && now > myWork.endTime)
        return '不在工作时段';
    return '';
}

function verifyUnlockPassword() {
    // alert('enter verify password')
    abp.services.app.work.getNow().done(function (now) {
        var result = validateCanUnlock(now);
        if (result != '') {
            abp.notify.error(result);
            return;
        } 

        var pwd = $('#password').textbox('getValue');
        abp.services.app.work.verifyUnlockPassword(pwd).done(function (result) {
            if (result == true) {
                abp.notify.info("密码正确，解锁屏幕");
                $('#dlgUnlock').dialog('close');
                unlockScreen();
            }
        });    
    })
}

function EscKeyUp() {
    if( event && event.keyCode === 27) {
        $('#dlgUnlock').dialog('open');
        $('#password').next('span').find('input').focus();  //.textbox-txt
    }
}

function getTomorrow(today) {
    var now = today.split('-')
    now = new Date(Number(now['0']),(Number(now['1'])-1),Number(now['2']));
    now.setDate(now.getDate() + 1);
    return formatTime(now);
}

function formatTime(date) {
    var year = date.getFullYear();
    var month = date.getMonth()+1, month = month < 10 ? '0' + month : month;
    var day = date.getDate(), day =day < 10 ? '0' + day : day;
    return year + '-' + month + '-' + day;
}

function vehicleFormatter() {
    return vehicleCn + ' ' + vehicleName;
}

function workerFormatter() {
    return workerCn + ' ' + workerName;
}

(function ($) {
    $(function () {
        // 侦听F1
        document.onkeyup = EscKeyUp;

        var chatHub = null;

        abp.signalr.startConnection(abp.appPath + 'signalr-myChatHub', function (connection) {
            chatHub = connection; // Save a reference to the hub
        
            connection.on('getMessage', function (message) { // Register for incoming messages
                parseMessage(message);
                console.log('received message: ' + message);
            });
        }).then(function (connection) {
            abp.log.debug('Connected to myChatHub server!');
            abp.event.trigger('myChatHub.connected');
        });
        
        abp.event.on('myChatHub.connected', function() { // Register for connect event
            // chatHub.invoke('sendMessage', "Hi everybody, I'm connected to the chat!"); // Send a message to the server
            abp.notify.info("与实时推送服务连接成功");
            // unlockScreen();
        });
    });
})(jQuery);