(function() {        
    $(function() {
        finput.DetectWorker = false;
        finput.DetectArticle = false;
        finput.DetectBox = true;
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            if (!work.validate2()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dg').datagrid({
                url: 'GridData',
                queryParams: {WpId: work.me.workplaceId, CarryoutDate: work.me.today, DepotId: work.me.depotId, AffairId: work.me.affairId }
            });
        });

        $('#dg').datagrid({
            onSelect: function (index, row) {
                $('#dgTask').datagrid({
                    url: 'GridDataTask/' + row.id
                });
            }
        });

        $('#dgTask').datagrid({
            onSelect: function (index, row) {
                taskRow = row;
                abp.services.app.client.getBoxes(row.outletId).done(function(boxes) {
                    $('#boxId').combobox({data: boxes, valueField: 'value', textField: 'displayText' });
                })
                $('#dgBox').datagrid({
                    url: 'GridDataInBox/' + row.id
                });
            }
        });

    });
})();

var taskRow = null;

function addBox() {
    if ($('#boxId').val() == null) {
        abp.notify.error("请选择尾箱"); return;
    }
    //alert(taskRow.id+rfid+work.me.workers);
    abp.services.app.boxRecord.inBox(taskRow.id, $('#boxId').val(), work.me.workers).done(function(ret) {
        if (ret != null) {
            abp.notify.error(ret);
            return;
        };

        abp.notify.success("成功入箱");
        $('#dgBox').datagrid('reload');
    }); 
}

finput.boxScanned = function(rfid) {
    //alert(rfid);
    if (taskRow == null) {
        abp.notify.error("请选择任务线路"); return;
    }

    if (taskRow.outletCn != rfid.substr(0, 6)) {
        abp.message.confirm('非本网点尾箱，确认继续?', '请确认', function (r) {
            if (!r) {
                return;
            }
        });
    }

    //alert(taskRow.id+rfid+work.me.workers);
    abp.services.app.boxRecord.inBox(taskRow.id, rfid, work.me.workers).done(function(ret) {
        if (ret != null) {
            abp.notify.error(ret);
            return;
        };
        
        abp.notify.success("成功入箱");
        $('#dgBox').datagrid('reload');
    });
}
