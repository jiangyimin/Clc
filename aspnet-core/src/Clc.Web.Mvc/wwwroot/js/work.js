﻿var work = work || {};
(function($) {
    work.me = {};
    
    work.validate = function(now) {
        if (work.me.affairId == 0) {
            abp.notify.error("你未被安排或任务未激活", "", { positionClass : 'toast-top-center'} );
            return false;
        }
        if (work.me.now < work.me.startTime && work.me.now > work.me.endTime) {
            abp.notify.error("不在工作时段", "", { positionClass : 'toast-top-center'} );
            return false;
        }
        return true;
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
})(jQuery);