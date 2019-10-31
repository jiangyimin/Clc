var work = work || {};
(function($) {
    work.isCaptain = false;
    work.dd = '';
    work.myWork = {};
    
    work.parseMessage = function(msg) {
        var cmd = msg.split(' ', 2);
        if (cmd[0] !== work.myWork.workerCn) return;
        // alert( cmd[0] + cmd[1] + work.myWork.workerCn);

        if (cmd[1] == "lockScreen") work.lockScreen();
        else {
            if (work.isCaptain) work.unlockScreen();
            else 
                abp.services.app.work.getNow().done(function(now) {
                    var result = work.validateCanUnlock(now);
                    if (result != '') {
                        abp.notify.error(result);
                        return;
                    } 
                    work.unlockScreen();        
                });
        }
    }

    work.lockScreen = function() {
        var lockdiv = document.getElementById("lockDiv");
        if (lockdiv != null) {
            lockdiv.style.display = "block";
        }
    }
    work.unlockScreen = function() {
        var lockdiv = document.getElementById("lockDiv");
        if (lockdiv != null) {
            lockdiv.style.display = "none";
        }
    }

    work.validateCanUnlock = function(now) {
        if (work.isCaptain) return '';
        if (work.myWork.affairId == 0 || work.myWork.status == "安排")
            return '未安排或任务未激活';
        if (now < work.myWork.startTime && now > work.myWork.endTime)
            return '不在工作时段';
        return '';
    }

    work.verifyUnlockPassword = function() {
        // alert('enter verify password')
        abp.services.app.work.getNow().done(function(now) {
            var result = work.validateCanUnlock(now);
            if (result != '') {
                abp.notify.error(result);
                return;
            } 

            var pwd = $('#password').textbox('getValue');
            abp.services.app.work.verifyUnlockPassword(pwd).done(function(result) {
                if (result == true) {
                    abp.notify.info("密码正确，解锁屏幕");
                    $('#dlgUnlock').dialog('close');
                    work.unlockScreen();
                }
                else
                    abp.notify.error("密码错误");
            });    
        })
    }

    work.EscKeyUp = function() {
        if( event && event.keyCode === 27) {
            $('#dlgUnlock').dialog('open');
            $('#password').next('span').find('input').focus();  //.textbox-txt
        }
    }

    work.getTomorrow = function(today) {
        var now = today.split('-')
        now = new Date(Number(now['0']),(Number(now['1'])-1),Number(now['2']));
        now.setDate(now.getDate() + 1);
        return work.formatTime(now);
    }

    work.formatTime = function(date) {
        var year = date.getFullYear();
        var month = date.getMonth()+1, month = month < 10 ? '0' + month : month;
        var day = date.getDate(), day =day < 10 ? '0' + day : day;
        return year + '-' + month + '-' + day;
    }

    work.vehicleFormatter = function(val, row, index) {
        return row.vehicleCn + ' ' + row.vehicleLicense;
    }

    work.workerFormatter = function(val, row, index) {
        return row.workerCn + ' ' + row.workerName;
    }

    work.outletFormatter = function(val, row, index) {
        return row.outletCn + ' ' + row.outletName;
    }

    work.getWorkersString = function() {
        var str = '';
        for (var i=0; i< work.myWork.workers.length; i++)
        {
            str = str + work.myWork.workers[i].name + ' ';
        }
        return str; 
    }

    // document ready
    $(function () {
         // 侦听F1
        document.onkeyup = work.EscKeyUp;

        var chatHub = null;

        abp.signalr.startConnection(abp.appPath + 'signalr-myChatHub', function(connection) {
            chatHub = connection; // Save a reference to the hub
        
            connection.on('getMessage', function(message) { // Register for incoming messages
                work.parseMessage(message);
                console.log('received message: ' + message);
            });
        }).then(function(connection) {
            abp.log.debug('Connected to myChatHub server!');
            abp.event.trigger('myChatHub.connected');
        });
        
        abp.event.on('myChatHub.connected', function() { // Register for connect event
            chatHub.invoke('sendMessage', "Hi everybody, I'm connected to the chat!"); // Send a message to the server
            abp.notify.info("与实时推送服务连接成功");
            // work.unlockScreen();
        });
    });
})(jQuery);