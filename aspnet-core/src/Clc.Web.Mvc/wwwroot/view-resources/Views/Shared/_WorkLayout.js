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

function verifyUnlockPassword() {
    abp.services.app.work.verifyUnlockPassword('Verify Password').done(function (result) {
        if (result == true) {
            abp.notify.info("密码正确，解锁屏幕");
            $('#dlgUnlock').dialog('close');
            unlockScreen();
        }
    });
}

function F1KeyDown(event) {
    if( event && event.keyCode === 27) {
        $('#dlgUnlock').dialog('open');
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
  
(function ($) {
    $(function () {
        // 侦听F1
        document.onkeydown = F1KeyDown;

        var chatHub = null;

        abp.signalr.startConnection(abp.appPath + 'signalr-myChatHub', function (connection) {
            chatHub = connection; // Save a reference to the hub
        
            connection.on('getMessage', function (message) { // Register for incoming messages
                console.log('received message: ' + message);
            });
        }).then(function (connection) {
            abp.log.debug('Connected to myChatHub server!');
            abp.event.trigger('myChatHub.connected');
        });
        
        abp.event.on('myChatHub.connected', function() { // Register for connect event
            chatHub.invoke('sendMessage', "Hi everybody, I'm connected to the chat!"); // Send a message to the server
            // abp.notify.info("服务器实时连接成功，现在解锁屏幕");
            unlockScreen();
        });
    });
})(jQuery);