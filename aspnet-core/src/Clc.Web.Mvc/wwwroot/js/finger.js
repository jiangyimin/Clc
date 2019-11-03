var finger = finger || {};
(function () {

    finger.init = function (activeX) {
        activeX.spDeviceType = 2;
        activeX.spComPort = 1;
        activeX.spBaudRate = 6;
        activeX.CharLen = 1024;
        activeX.FingerCode = "";
        activeX.TimeOut = 500;
    };

    finger.regCode = function (activeX) {
        finger.init(activeX);
        var mesg = activeX.ZAZRegFinger();
        if (mesg == "0") {
            alert(activeX.FingerCode);
            return activeX.FingerCode;
        }
        else {
            return "";
        }
    }

    finger.getCode = function (activeX) {
        finger.init(activeX);
        var mesg = activeX.ZAZGetImgCode();
        if (mesg == "0") {
            return activeX.FingerCode;
        }
        else {
            return "";
        }
    }

    finger.match = function (activeX, src, dst)
    {
        return activeX.ZAZMatch(src, dst);
    }


})();