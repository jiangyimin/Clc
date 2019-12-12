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

    // for Captain Verify
    work.verifyAction = '';
    work.verifyDone = function () { alert("未定义"); }

    work.openActivateDialog = function(action) {
        work.verifyAction = action;
        $('#dlgActivate').dialog('open');
        $('#passwordActivate').next('span').find('input').focus();
    }

    work.closeActivateDialog = function () {
        // alert(verifyAction);
        $('#fmActivate').form('clear');
        $('#dlgActivate').dialog('close');
    }

    work.verifyPasswordAndTrigger = function () {
        var pwd = $('#passwordActivate').val();
        abp.services.app.work.verifyUnlockPassword(pwd).done(function(result) {
            if (result == true) {
                abp.notify.success("密码验证正确");
                // alert(verifyAction);
                work.closeActivateDialog();
                work.verifyDone(work.verifyAction, false);
            }
            else
                abp.notify.error("密码错误");
        });    
    }

    work.verifyFingerAndTrigger = function () {
        abp.notify.info("请把指纹放到指纹仪上");
        var finger = window.parent.getFingerCode();
        if (finger != '') {
            //alert(finger + window.parent.me.workerCn);
            abp.services.app.work.verifyFinger(finger, window.parent.me.workerCn).done(function(ret) {
                abp.notify.info(ret.item2);
                if (ret.item1) {
                    work.closeActivateDialog();
                    work.verifyDone(work.verifyAction, true);
                };
            });
        }
    }
})(jQuery);