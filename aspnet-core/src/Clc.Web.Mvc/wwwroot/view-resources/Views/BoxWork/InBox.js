
(function() {        
    $(function() {
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
                $('#dgBox').datagrid({
                    url: 'GridDataInBox/' + row.id
                });
            }
        });

    });
})();

        finput.boxScanned = function(rfid) {
            abp.services.app.work.matchInBox(work.dd, work.myWork.affairId, finput.route.id, rfid).done(function(ret) {
                if (ret.item1 != '') {
                    abp.notify.error(ret.item1);
                    return;
                };
                var b = ret.item2;
                if (finput.isInBoxes(b.boxId)) {
                    abp.notify.warn('此尾箱已扫描');
                    return;
                };

                // alert(b.boxId);
                abp.services.app.boxRecord.getBoxStatus(b.boxId).done(function (status) {
                    if (status != null) abp.notify.warn(status);
                    finput.boxes.push(b);
                    $('#dl').datalist('loadData', finput.boxes);
                });
            });
        }

        finput.onWorkerConfirm = function() {
            if (finput.boxes.length == 0) {
                abp.notify.warn('未入库任何尾箱')
                return;
            }

            // alert(finput.boxes.length);
            abp.services.app.boxRecord.in(finput.route.id, finput.boxes, work.myWork.workers).done(function(count) {
                abp.notify.info(finput.worker.name+'入库了'+count+'个尾箱');
                $('#dlg').dialog('close');
            });
        }
