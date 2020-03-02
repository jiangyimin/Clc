var finput = finput || {};
(function ($) {
    var confirmEnable = true;
    var waiting = false;

    finput.style = 0;           // set by outer
    finput.currentRfid = '';
    finput.dialogOpened = false;
    finput.index = 0;

    finput.rfidLength = {
        workerRfidLength: 5,
        articleRfidLength: 3,
        boxRfidLength: 8,
    };

    finput.DetectWorker = true;
    finput.DetectArticle = false;
    finput.DetectBox = false;

    finput.route = {};
    finput.worker = {};
    finput.worker2 = {};
    finput.articles = [];
    finput.articles2 = [];
    finput.boxes = [];

    // dialog open and close
    finput.onOpenDialog = function() {
        confirmEnable = true;
        finput.dialogOpened = true;
        finput.DetectArticle = true;
    }

    finput.onCloseDialog = function() {
        // alert('onClose');
        waiting = false;

        finput.dialogOpened = false;
        finput.DetectArticle = false;

        finput.index = 0;
        finput.worker = {};
        finput.worker2 = {};
        
        finput.articles = [];
        finput.articles2 = [];
        
        articles.innerHTML = '';
        articles1.innerHTML = '';
        articles2.innerHTML = '';
        // alert(finput.articles.length); alert(finput.articles2.length);
    }

    finput.error = function(msg) {
        abp.notify.error(msg);
        $('#sounde')[0].play();     // 播放声音
    }
    finput.success = function(msg) {
        if (msg) abp.notify.success(msg);
        // alert('play success');
        $('#sounds')[0].play();     // 播放声音
    }

    // onWoker
    finput.onWorker = function (rfid) { 
        if (finput.dialogOpened == true) {
            if (finput.getWorkerRfid() == rfid) {
                if (confirmEnable == true) {
                    confirmEnable = false;
                    finput.onWorkerConfirm();
                    setTimeout(function() {
                        confirmEnable = true;
                    }, 2000);
                }
            }
            else 
                finput.error("请用本人工卡确认");
        }
        else {
            window.parent.displayRfid1(rfid);

            if (waiting) {      // judge another card
                if (finput.worker2.rfid != rfid) {
                    finput.error("同组另一人的工卡不对");
                    return;
                }
                finput.success("同一组的两个人已都刷卡")
                finput.showWorker();
            }
            else {
                finput.matchWorker(rfid);
            }
        }
    }
    finput.matchWorker = function () { alert("请用鼠标点一下此页面的刷新按钮！") }

    finput.onMatchWorker = function (ret) {
        finput.route = ret.routeMatched;
        finput.worker = ret.workerMatched;
        finput.worker2 = ret.workerMatched2;
        finput.articles = ret.articles;
        finput.articles2 = ret.articles2;
        finput.outlets = ret.outlets;
        finput.boxes = ret.boxes; 
        finput.success(finput.worker.name + "已刷卡");

        if (finput.worker2 != null) {
            waiting = true;
            abp.notify.info("请同组人员(" + finput.worker2.name + ")刷卡");
        }
        else {
            finput.showWorker();
        }
    }

    finput.onWorkerConfirm = function () {
        var rwId = finput.index == 2 ? finput.worker2.routeWorkerId : finput.worker.routeWorkerId;

        if (!finput.submitArticles(rwId)) return;
        if (finput.index == 1) 
            abp.notify.info('现在请扫描第二人的物品', '', { positionClass : 'toast-top-center'} );
    }

    finput.showWorker = function () {
        if (finput.worker2 == null) {
            finput.index = 0;
            $('#dlg').dialog('open');
            routeInfo.innerHTML = finput.route.routeName + '(' + finput.route.vehicleCn + ' ' + finput.route.vehicleLicense + ')';
            workerInfo.innerHTML = finput.worker.name + ' ' + finput.worker.workRoleName;
            photo.src = 'data:image/jpg;base64, ' + finput.worker.photo;
        }
        else {
            finput.index = 1;
            $('#dlg2').dialog('open');
            routeInfo1.innerHTML = finput.route.routeName + '(' + finput.route.vehicleCn + ' ' + finput.route.vehicleLicense + ')';
            workerInfo1.innerHTML = finput.worker.name + ' ' + finput.worker.workRoleName;
            photo1.src = 'data:image/jpg;base64, ' + finput.worker.photo;
            workerInfo2.innerHTML = finput.worker2.name + ' ' + finput.worker2.workRoleName;
            photo2.src = 'data:image/jpg;base64, ' + finput.worker2.photo;
            // finput.showArticles();
        }

        // show Article Also
        finput.showArticles();
    }

    finput.submitArticlesDone = function() {
        if (finput.index == 1 && finput.worker2 != null) {
            finput.index = 2;
            finput.showArticles();
        }
        else {
            // alert('close!' + finput.index)
            if (finput.index == 2)  $('#dlg2').dialog('close');
            else $('#dlg').dialog('close');
        }
        finput.success();
    }

    finput.getArticles = function () {
        return finput.index == 2 ? finput.articles2 : finput.articles;
    }

    finput.getWorker = function() {
        return finput.index == 2 ? finput.worker2.cn + ' ' + finput.worker2.name
                                    : finput.worker.cn + ' ' + finput.worker.name;
    }
    
    finput.getWorkerRfid = function() {
        return finput.index == 2 ? finput.worker2.rfid : finput.worker.rfid;
    }

    // lead control recoredId, return control isReturn
    finput.IsInLendArticles = function(articleId) {
        var as = finput.getArticles();
        for (var i = 0; i < as.length; i++) {
            if (as[i].articleId === articleId) {
                if (as[i].recordId > 0) return 2;
                as.splice(i, 1);
                return 1;
            }
        }
        return 0;
    }

    finput.IsInReturnArticles = function(articleId) {
        var as = finput.getArticles();
        for (var i = 0; i < as.length; i++) {
            if (as[i].articleId === articleId) {
                as[i].isReturn = true;;
                return true;
            }
        }
        return false;
    }

    // used by TempArticles
    finput.rfidIsInArticles = function(rfid) {
        var as = finput.getArticles();
        for (var i = 0; i < as.length; i++) {
            if (as[i].rfid == rfid) {
                if (finput.style == 0)
                    as[i].recordId = 0;
                else    
                    as[i].isReturn = true;
                
                return true;
            }
        }
        return false;
    }

    finput.pushArticle = function(a) {
        var as = finput.getArticles();
        as.push(a);
    }

    finput.isEmptyArticles = function() {
        var as = finput.getArticles();
        return as.length == 0;
    }

    finput.articlesAllReturn = function() {
        var as = finput.getArticles();
        for (var i = 0; i < as.length; i++) {
            if (finput.style == 0 && as[i].recordId != 0) return false;   // use in tempArticles
            if (finput.style == 1 && !as[i].isReturn) return false;
        }
        return true;
    }

    finput.showArticles = function() {
        var as = finput.getArticles();

        var html = finput.style == 0 ? finput.createHTML2Lend(as) : finput.createHTML2Return(as);
        // alert(finput.index + " " + html);
        if (finput.index == 0) articles.innerHTML = html;
        if (finput.index == 1) articles1.innerHTML = html;
        if (finput.index == 2) articles2.innerHTML = html;
    }

    finput.createHTML2Lend = function(as) {
        var h = '';        
        for (var i = 0; i < as.length; i++) {
            // alert(as[i].recordId);
            if (as[i].recordId == 0)
                h += "<li><input type='checkbox' checked='true' onclick='return false'>&nbsp;" + as[i].displayText + "</li>";
            else
                h += "<li><input type='checkbox' onclick='return false'>&nbsp;" + as[i].displayText + "</li>";
        }
        return h;
    }
    finput.createHTML2Return = function(as) {
        var h = '';        
        for (var i = 0; i < as.length; i++) {
            // alert(as[i].isReturn);
            if (as[i].isReturn)
                h += "<li><input type='checkbox' checked='true' onclick='return false'>&nbsp;" + as[i].displayText + "</li>";
            else
                h += "<li><input type='checkbox' onclick='return false'>&nbsp;" + as[i].displayText + "</li>";
        }
        return h;
    }

    finput.submitArticles = function (id) { alert("submitArticles"); return true; }

    // Box region
    finput.ShowWorerForBox = function (ret) {
        finput.route = ret.routeMatched;
        finput.worker = ret.workerMatched;
        finput.outlets = ret.outlets;
        finput.boxes = ret.boxes;  
    }

    // finput.ShowWorkerDetails = function () { }


    // OnArticle
    finput.onArticle = function (rfid) {
        window.parent.displayRfid2(rfid);
        finput.articleScanned(rfid);
    };

    finput.articleScanned = function () { alert("articleScanned")}
    

    // Box
    finput.onBox = function (rfid) {
        window.parent.displayRfid2(rfid);
        finput.boxScanned(rfid);
    }
    finput.boxScanned = function() { alert("boxScanned") }

    finput.onkeydown = function() {
        var keyCode = event.keyCode;
        var c = String.fromCharCode(keyCode);
        if (keyCode == 13 && finput.currentRfid != '') {
            if (finput.currentRfid.length == finput.rfidLength.workerRfidLength) {
                if (finput.DetectWorker == true) finput.onWorker(finput.currentRfid);
            }
            else if (finput.currentRfid.length == finput.rfidLength.articleRfidLength) {
                if (finput.DetectArticle == true) finput.onArticle(finput.currentRfid);
            }
            else if (finput.currentRfid.length == finput.rfidLength.boxRfidLength) {
                if (finput.DetectBox == true) finput.onBox(finput.currentRfid);
            }
            else {
                abp.notify.error("输入的Rfid长度不对");
            }
            finput.currentRfid = '';
        }
        else {
            finput.currentRfid += c;
        }
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

    $(function () {
        // Set rfidLength
        finput.rfidLength.workerRfidLength = abp.setting.get('Const.WorkerRfidLength');
        finput.rfidLength.articleRfidLength = abp.setting.get('Const.ArticleRfidLength');
        finput.rfidLength.boxRfidLength = abp.setting.get('Const.BoxRfidLength');

        // set envent
        window.document.onkeydown = finput.onkeydown;

        $('#dlg').dialog({
            onClose: function() { finput.onCloseDialog(); },
            onOpen: function() { finput.onOpenDialog(); }
        });

        $('#dlg2').dialog({
            onClose: function() { finput.onCloseDialog(); },
            onOpen: function() { finput.onOpenDialog(); }
        })
     });
})(jQuery);
