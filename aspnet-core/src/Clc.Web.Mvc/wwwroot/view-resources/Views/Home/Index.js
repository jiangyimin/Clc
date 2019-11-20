(function () {
    $(function() {
        $("#main-tab").tabs({
            onContextMenu: function (e, title) {
                e.preventDefault();
                $("#tab-menu").menu("show", { left: e.pageX, top: e.pageY })
                    .data("tabTitle", title); //将点击的Tab标题加到菜单数据中
            }
        });

        $("#tab-menu").menu({
            onClick: function (item) {
                tabHandle(this, item.id);
            }
        });
        
        var x = document.getElementsByClassName('easyui-tree');
        for (var i = 0; i < x.length; i++)
        {
            $(x[i]).tree({
                onClick: function(node) {
                    addTab(node.text, node.attributes.url);
                }
            });
        }

        // common: finger and  signalR
        initFingerActivex();
        // abp.notify.info('指纹仪已准备好');
        abp.services.app.work.getMe().done(function(ret) {
            meRoleName = ret.item1;
            meCn = ret.item2;
            if (meRoleName == "system") unlockScreen();
        });

         // 侦听ESC for unlockscreen, 
         document.onkeyup = EscKeyUp;
         // Signalr
         var chatHub = null;
         abp.signalr.startConnection(abp.appPath + 'signalr-myChatHub', function(connection) {
             chatHub = connection; // Save a reference to the hub
         
             connection.on('getMessage', function(message) { // Register for incoming messages
                 parseMessage(message);
                 console.log('received message: ' + message);
             });
         }).then(function(connection) {
             abp.log.debug('Connected to myChatHub server!');
             abp.event.trigger('myChatHub.connected');
         });
         
         abp.event.on('myChatHub.connected', function() { // Register for connect event
             chatHub.invoke('sendMessage', "Hi everybody, I'm connected to the chat!"); // Send a message to the server
             abp.notify.info("与实时推送服务连接成功");
         });
    });

    function addTab(title, url, icon) {
        var $mainTabs = $("#main-tab");
        if ($mainTabs.tabs("exists", title)) {
            $mainTabs.tabs("select", title);
        } else {
            $mainTabs.tabs("add", {
                title: title,
                closable: true,
                icon: icon,
                content: createFrame(url)
            });
        }
    }

    function createFrame(url) {
        var html = '<iframe scrolling="auto" frameborder="0" src="' + url + '" style="width:100%; height:99%"></iframe>';
        return html;
    }

    // utils for sub
    function closeTab(title) {
        $("#main-tab").tabs('close', title);
    }

    function tabHandle(menu, type) {
        var title = $(menu).data("tabTitle");
        var $tab = $("#main-tab");
        var tabs = $tab.tabs("tabs");
        var index = $tab.tabs("getTabIndex", $tab.tabs("getTab", title));
        var closeTitles = [];
        switch (type) {
            case "tab-menu-refresh":
                var iframe = $(".tabs-panels .panel").eq(index).find("iframe");
                if (iframe) {
                    var url = iframe.attr("src");
                    iframe.attr("src", url);
                }
                break;
            case "tab-menu-openFrame":
                var iframe = $(".tabs-panels .panel").eq(index).find("iframe");
                if (iframe) {
                    window.open(iframe.attr("src"));
                }
                break;
            case "tab-menu-close":
                closeTitles.push(title);
                break;
            case "tab-menu-closeleft":
                if (index == 0) {
                    abp.notify.warn("左边没有可关闭标签。");
                    return;
                }
                for (var i = 0; i < index; i++) {
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
            case "tab-menu-closeright":
                if (index == tabs.length - 1) {
                    abp.notify.warn("右边没有可关闭标签。");
                    return;
                }
                for (var i = index + 1; i < tabs.length; i++) {
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
            case "tab-menu-closeother":
                for (var i = 0; i < tabs.length; i++) {
                    if (i == index) {
                        continue;
                    }
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
            case "tab-menu-closeall":
                for (var i = 0; i < tabs.length; i++) {
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
        }
        for (var i = 0; i < closeTitles.length; i++) {
            $tab.tabs("close", closeTitles[i]);
        }
    }
})();

window.onbeforeunload=function(event){
    var ret = ZAZFingerActivex.ZAZCloseOCX();
}

function displayRfid1(str) {
    rfid1.innerHTML = str;
}
function displayRfid2(str) {
    rfid2.innerHTML = str;
}

function initFingerActivex() {
    ZAZFingerActivex.spDeviceType = 2;
    ZAZFingerActivex.spComPort = 1;
    ZAZFingerActivex.spBaudRate = 6;
    ZAZFingerActivex.CharLen = 512;
    ZAZFingerActivex.FingerCode = "";
    ZAZFingerActivex.TimeOut = 2000;
}

function regFingerCode() {
    var mesg = ZAZFingerActivex.ZAZRegFinger();
    if (mesg == "0") {
        return ZAZFingerActivex.FingerCode;
    }
    else {
        return "";
    }
}

function getFingerCode() {
    var mesg = ZAZFingerActivex.ZAZGetImgCode();
    if (mesg == "0") {
        return ZAZFingerActivex.FingerCode;
    }
    else {
        return "";
    }
}

function matchFinger(src, dst)
{
    return ZAZFingerActivex.ZAZMatch(src, dst);
}

var meRoleName = '';
var meCn = '';

function EscKeyUp() {
    if (meRoleName == 'system') return;
    if( event && event.keyCode === 27) {
        $('#dlgUnlock').dialog('open');
        $('#password').next('span').find('input').focus();  //.textbox-txt
    }
}

function verifyUnlockPassword() {
    var pwd = $('#password').textbox('getValue');
    abp.services.app.work.verifyUnlockPassword(pwd).done(function(result) {
        if (result == true) {
            abp.notify.info("密码正确，解锁屏幕");
            $('#dlgUnlock').dialog('close');
            unlockScreen();
        }
        else
            abp.notify.error("密码错误");
    });    
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

function parseMessage(msg) {
    var cmd = msg.split(' ', 2);
    if (cmd[0] == "lockScreen") {
        if (cmd[1] == meCn) lockScreen();
    }
    else if (cmd[0] == "unlockScreen") {
        if (cmd[1] == meCn) unlockScreen();
    }
    else if (cmd[0] == "askOpenDoor") {
        if (meRoleName.indexOf("Monitor") != -1) {
            abp.event.trigger('askOpenDoor');
            abp.notify.info(cmd[1]);
        }
    }
    else if (cmd[0] == "emergOpenDoor") {
        if (meRoleName.indexOf("Monitor") != -1) {
            abp.event.trigger('emergOpenDoor');
            abp.notify.info(cmd[1]);
        }
    }
}
