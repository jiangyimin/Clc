var finput = finput || {};
(function ($) {
    var currentRfid = '';
    var dialogClosed = true;
    finput.rfidLength = {
        workerRfidLength: 5,
        articleRfidLength: 3,
        boxRfidLength: 8,
    };
    

    finput.route = {};
    finput.worker = {};
    finput.articles = [];
    finput.boxes = [];

    // onWoker
    finput.onWorker = function (rfid) { 
        window.parent.displayRfid1(rfid);
        finput.matchWorker(rfid);
    };
    finput.matchWorker = function () { alert("matchWorker") }

    finput.showWorker = function (ret) {
        finput.route = ret.routeMatched;
        finput.worker = ret.workerMatched;
        finput.articles = ret.articles;
        finput.outlets = ret.outlets;
        finput.boxes = ret.boxes;  
        // alert(finput.articles.length);

        $('#dlg').dialog('open');
        dialogClosed = false;
        routeName.innerHTML = finput.route.routeName;
        vehicle.innerHTML = finput.route.vehicleCn + ' ' + finput.route.vehicleLicense;
        worker.innerHTML = finput.worker.cn + ' ' + finput.worker.name;
        photo.src = "data:image/jpg;base64, " + finput.worker.photo;
        
        finput.showWorkerDetails();
    };
    finput.ShowWorkerDetails = function () { }

    finput.onWorkerConfirm = function () { alert('onWorkerConfirm') };

    // OnArticle
    finput.onArticle = function (rfid) {
        window.parent.displayRfid2(rfid);
        finput.articleScanned(rfid);
    };
    finput.articleScanned = function () { alert("articleScanned")}
    
    finput.onBox = function (rfid) {
        window.parent.displayRfid2(rfid);
        finput.boxScanned(rfid);
    }
    finput.boxScanned = function() { alert("boxScanned") }

    finput.onkeydown = function() {
        var keyCode = event.keyCode;
        var c = String.fromCharCode(keyCode);
        if (keyCode == 13 && currentRfid != '') {
            if (currentRfid.length == finput.rfidLength.workerRfidLength) {
                if (dialogClosed == true)
                    finput.onWorker(currentRfid);
                else
                    finput.onWorkerConfirm(currentRfid);
            }
            else if (currentRfid.length == finput.rfidLength.articleRfidLength) {
                if (dialogClosed == false)
                    finput.onArticle(currentRfid);
            }
            else if (currentRfid.length == finput.rfidLength.boxRfidLength) {
                if (dialogClosed == false)
                    finput.onBox(currentRfid);
            }
            else {
                abp.notify.error("输入的Rfid长度不对");
            }
            currentRfid = '';
        }
        else {
            currentRfid += c;
        }
    }

    finput.IsInArticles = function(articleId) {
        for (var i = 0; i < finput.articles.length; i++)
        {
            if (finput.articles[i].articleId === articleId)
                return true;
        }
        return false;
    }

    finput.isInBoxes = function(boxId) {
        // alert("boxId="+boxId);
        for (var i = 0; i < finput.boxes.length; i++)
        {
            if (finput.boxes[i].boxId === boxId)
                return true;
        }
        return false;
    }

    finput.closeDialog = function() {
        dialogClosed = true;
        finput.articles = [];
        $('#dl').datalist('loadData', []);
    }

    $(function () {
        // Set rfidLength
        finput.rfidLength.workerRfidLength = abp.setting.get('Const.WorkerRfidLength');
        finput.rfidLength.articleRfidLength = abp.setting.get('Const.ArticleRfidLength');
        finput.rfidLength.boxRfidLength = abp.setting.get('Const.BoxRfidLength');

        // set envent
        window.document.onkeydown = finput.onkeydown;

        $('#dlg').dialog({
            onClose: function() {
                finput.closeDialog();
            }
        })
     });
})(jQuery);
