var finput = finput || {};
(function ($) {
    var currentRfid = '';
    finput.rfidLength = {
        workerRfidLength: 5,
        articleRfidLength: 4,
        boxRfidLength: 8,
    };

    finput.onWorker = function () { alert('onWorker'); };
    finput.onWorkerConfirm = function () { alert('onWorkerConfirm'); };
    finput.onArticle = function() { alert('onArticle');};
    finput.onBox = function() { alert('onBox');};

    finput.onkeydown = function() {
        var keyCode = event.keyCode;
        var c = String.fromCharCode(keyCode);
        if (keyCode == 13 && currentRfid != '') {
            if (currentRfid.length == finput.rfidLength.workerRfidLength)
                finput.onWorker(currentRfid);
            else if (currentRfid.length == finput.rfidLength.articleRfidLength)
                finput.onArticle(currentRfid);
            else if (currentRfid.length == finput.rfidLength.boxRfidLength)
                finput.onBox(currentRfid);
            else
                abp.notify.error("输入的Rfid长度不对");
            currentRfid = '';
        }
        else {
            currentRfid += c;
        }
    }

    $(function () {
        // Set rfidLength
        finput.rfidLength.workerRfidLength = abp.setting.get('Const.WorkerRfidLength');
        finput.rfidLength.articleRfidLength = abp.setting.get('Const.ArticleRfidLength');
        finput.rfidLength.boxRfidLength = abp.setting.get('Const.BoxRfidLength');

        // set envent
        window.document.onkeydown = finput.onkeydown;
     });
})(jQuery);
