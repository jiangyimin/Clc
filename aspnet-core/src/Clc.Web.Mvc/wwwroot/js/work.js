var work = work || {};
(function($) {
    work.me = {};
    work.aws = [];
    
    work.validate = function() {
        if (work.me.affairId == 0) {
            abp.notify.error("你目前无可用任务！", "", { positionClass : 'toast-top-center'} );
            return false;
        };
        return true;
    }

    work.validate2 = function() {
        if (work.me.affairId == 0) {
            abp.notify.error("请先验入任务，然后关闭窗口重新进入！", "", { positionClass : 'toast-top-center'} );
            return false;
        };
        return true;
    }

    work.getTomorrow = function(today) {
        var now = today.split('-')
        now = new Date(Number(now['0']),(Number(now['1'])-1),Number(now['2']));
        now.setDate(now.getDate() + 1);
        return work.formatTime(now);
    }

    work.getYesterday = function(today) {
        var now = today.split('-')
        now = new Date(Number(now['0']),(Number(now['1'])-1),Number(now['2']));
        now.setDate(now.getDate() - 1);
        return work.formatTime(now);
    }

    work.formatTime = function(date) {
        var year = date.getFullYear();
        var month = date.getMonth()+1, month = month < 10 ? '0' + month : month;
        var day = date.getDate(), day =day < 10 ? '0' + day : day;
        return year + '-' + month + '-' + day;
    }
})(jQuery);