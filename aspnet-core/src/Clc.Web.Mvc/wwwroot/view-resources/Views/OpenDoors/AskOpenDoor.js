(function() {        
    $(function() {
        abp.services.app.work.getMyCheckinAffair().done(function (wk) {
            work.me = wk;
            // alert(work.me.affairId);
            if (!work.validate()) return;
            $('#dd').datebox('setValue', work.me.today);
            $('#dg').datagrid({
                url: 'GridDataAskDoor?Dt=' + work.me.today 
            });
        });

        // #tb Buttons
        $('#yesterday').checkbox({
            onChange: function() {
                if ($('#yesterday').checkbox('options').checked) {
                    var t = work.getYesterday(work.me.today);
                    // alert(t);
                    $('#dd').datebox('setValue', t);
                    $('#dg').datagrid({
                        url: 'GridDataAskDoor?Dt=' + t
                    });
                }
                else {
                    $('#dd').datebox('setValue', work.me.today);
                    $('#dg').datagrid({
                        url: 'GridDataAskDoor?Dt=' + work.me.today
                    });
                }
            }
        });

        $('#tb').children('a[name="open"]').click(function (e) {
            var row = $('#dg').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要开门的申请", "", { positionClass : 'toast-top-center'} );
                return;
            };
            
            askDoorRecordId = row.id;
            doorIp = row.workplaceDoorIp;
            $('#dlg').dialog('open');
        });
    
        // register event
        window.parent.abp.event.on('askOpenDoor', function () {
            $('#dg').datagrid('reload');
        });
        // alert('event set on');
    });
})();
