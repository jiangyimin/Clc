(function ($) {
    //监听加载状态改变
    //document.onreadystatechange = completeLoading;
    $.parser.onComplete = completeLoading;

    //加载状态为complete时移除loading效果
    function completeLoading() {
        if (document.readyState === "complete") {
            var loadingMask = document.getElementById('loadingDiv');
            if (loadingMask !== null)
                loadingMask.parentNode.removeChild(loadingMask);
        }
    };

    //Notification handler
    abp.event.on('abp.notifications.received', function (userNotification) {
        abp.notifications.showUiNotifyForUserNotification(userNotification);

        //Desktop notification
        Push.create("Clc", {
            body: userNotification.notification.data.message,
            icon: abp.appPath + 'images/app-logo-small.png',
            timeout: 6000,
            onClick: function () {
                window.focus();
                this.close();
            }
        });
    });

    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }

    // plugins for jQuery: setImagePreview
    $.extend({
        setImagePreview: function (domid, obj) {
            var file = obj.files[0];
            if (obj.files && file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(domid).attr("src", e.target.result);
                }
                reader.readAsDataURL(file);
            }
            //$("#PhotoImage").val('');
        },

        // formatters
        dateFormatter: function (val) {
            if (val) return val.substr(0, 10);
        },

        timeFormatter: function (val) {
            if (val) return val.substr(11, 8);
        },

        datetimeFormatter: function (val) {
            if (val) return val.substr(0, 10) + ' ' + val.substr(11, 5);
        },

        toExcel: function (tbl, title) {
            try {
                var rows = $(tbl).datagrid('getRows');
                var columns = $(tbl).datagrid("options").columns[0];
                var oXL = new ActiveXObject("Excel.Application"); //创建AX对象excel 
                var oWB = oXL.Workbooks.Add();
                var oSheet = oWB.ActiveSheet;

                oSheet.Name = title;
                //设置表头
                for (var i = 0; i < columns.length; i++) {
                    oSheet.Cells(1, i + 1).value = columns[i].title;
                }
                //设置内容部分
                for (var i = 0; i < rows.length; i++) {
                    //动态获取每一行每一列的数据值
                    for (var j = 0; j < columns.length; j++) {
                        oSheet.Cells(i + 2, j + 1).value = rows[i][columns[j].field];
                    }
                }
                oXL.Visible = true;
            } catch (e) {
                alert("无法启动Excel!\n\n如果您确信您的电脑中已经安装了Excel");
            }
        },
    });
})(jQuery);