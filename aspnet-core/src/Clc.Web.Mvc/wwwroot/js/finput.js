var finput = finput || {};
(function ($) {
    var currentRfid = '';
    finput.rfidLength = {
        workerRfidLength: 5,
        articleRfidLength: 3,
        boxRfidLength: 8,
    };

    finput.route = {};
    finput.worker = {};
    finput.articles = [];

    // onWoker
    finput.onWorker = function (rfid) { 
        window.parent.displayRfid1(rfid);
        finput.onWorkerSelf(rfid);
    };
    finput.onWorkerSelf = function () { };

    finput.showWorkerCommon = function (ret) {
        finput.route = ret.routeMatched;
        finput.worker = ret.workerMatched;
        if (ret.artilces != null)
            finput.articles = ret.articles;
        else
            finput.articles = [];
        $('#dl').datalist({
            data: finput.articles,
            valueField: 'articleId',
            textField: 'displayText',
        });
    
        routeName.innerHTML = finput.route.routeName;
        vehicle.innerHTML = finput.route.vehicleCn + ' ' + finput.route.vehicleLicense;
        worker.innerHTML = finput.worker.cn + ' ' + finput.worker.name;
        photo.src = "data:image/jpg;base64, " + finput.worker.photo;
        
        finput.showWorkerSelf();
    };
    finput.ShowWorkerSelf = function () { }

    finput.onWorkerConfirm = function () { alert('onWorkerConfirm'); };

    // OnArticle
    finput.onArticle = function (rfid) {
        window.parent.displayRfid2(rfid);
        finput.onArticleSelf(rfid);
    };
    finput.onArticleSelf = function () {};
    finput.showArticleCommon = function (a) {
        // alert(a.displayText);
        if (finput.IsInArticles(a.articleId)) {
            abp.notify.warn('此物品已扫描');
            return;
        }

        abp.services.app.articleRecord.getArticleStatus(a.articleId).done(function (status) {
            if (status == null) {
                finput.articles.push(a);
                $('#dl').datalist('loadData', finput.articles);
            }
            else {
                abp.notify.error(status);
            }
        })
    };

    finput.onBox = function () { alert('onBox');};

    finput.onkeydown = function() {
        var keyCode = event.keyCode;
        var c = String.fromCharCode(keyCode);
        if (keyCode == 13 && currentRfid != '') {
            if (currentRfid.length == finput.rfidLength.workerRfidLength) {
                if ($('#dlg').dialog('options').closed == true)
                    finput.onWorker(currentRfid);
                else
                    finput.onWorkerConfirm(currentRfid);
            }
            else if (currentRfid.length == finput.rfidLength.articleRfidLength) {
                if ($('#dlg').dialog('options').closed == false)
                    finput.onArticle(currentRfid);
            }
            else if (currentRfid.length == finput.rfidLength.boxRfidLength) {
                if ($('#dlg').dialog('options').closed == false)
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

    //
    finput.IsInArticles = function(articleId) {
        for (var i = 0; i < finput.articles.length; i++)
        {
            if (finput.articles[i].articleId === articleId)
                return true;
        }
        return false;
    }

    finput.getArticleIds = function() {
        var ids = [];
        for (var i = 0; i < finput.articles.length; i++)
            ids.push(finput.articles[i].articleId)
        return ids;
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
