var winput = winput || {};
(function ($) {
    winput.rfidLength = {
        workerRfidLength: 5,
        articleRfidLength: 3,
        boxRfidLength: 8,
    };
    winput.currentRfid = '';

    winput.onWorker = function (rfid) { 
        window.parent.displayRfid1(rfid);
        winput.matchWorker(rfid);
    }
    winput.matchWorker = function () { alert("请用鼠标点一下此页面！") }

    winput.onkeydown = function() {
        var keyCode = event.keyCode;
        var c = String.fromCharCode(keyCode);
        if (keyCode == 13 && winput.currentRfid != '') {
            if (winput.currentRfid.length == winput.rfidLength.workerRfidLength) {
                winput.onWorker(winput.currentRfid);
            }
            else {
                abp.notify.error("输入的Rfid长度不对");
            }
            winput.currentRfid = '';
        }
        else {
            winput.currentRfid += c;
        }
    }

    $(function () {
        // Set rfidLength
        winput.rfidLength.workerRfidLength = abp.setting.get('Const.WorkerRfidLength');
        winput.rfidLength.articleRfidLength = abp.setting.get('Const.ArticleRfidLength');
        winput.rfidLength.boxRfidLength = abp.setting.get('Const.BoxRfidLength');

        // set envent
        window.document.onkeydown = winput.onkeydown;
    });
})(jQuery);
