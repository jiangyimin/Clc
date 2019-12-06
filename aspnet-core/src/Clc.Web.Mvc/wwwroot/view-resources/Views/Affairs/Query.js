(function() {        
    $(function() {    
        abp.services.app.work.getToday().done(function (today) {
            $('#dd').datebox('setValue', today);
        });
        abp.services.app.field.getComboItems('Depot').done(function (data) {
            $('#depot').combobox({
                data: data,
                valueField: 'value',
                textField: 'displayText'
            })

            $('#depot').combobox({
                onChange: function(val) {
                    showAffairs();
                }
            });
    
            $('#depot').combobox('setValue', window.parent.me.depotId);
        });

        $('#dd').datebox({
            onChange: function() {
                showAffairs();
            }
        });

        $('#tb').children('a[name="event"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择任务");
                return;
            };
            $('#dlgEvent').dialog('open');
            $('#dgEvent').datagrid({
                url: "GridDataEvent/" + mds.masterCurrentRow.id
            });
        });
    });

    function showAffairs() {
        var depotId = $('#depot').combobox('getValue');
        if (!depotId) return;
        if (!$('#dd').datebox('getValue')) return;
        if ($('#seld').val() == 0 && depotId != window.parent.me.depotId) {
            abp.notify.error('你不允许查询其他大队的任务');
            return;
        }

        $('#dg').datagrid({
            url: 'QueryGridData',
            queryParams: { CarryoutDate: $('#dd').datebox('getValue'), DepotId: depotId }
        });
    }

})();
