(function() {        
    $(function() {    
        abp.services.app.work.getToday().done(function (today) {
            $('#dd').datebox('setValue', today);
            $('#depot').combobox('setValue', window.parent.me.depotId);
            showAffairs();
        });

        $('#dd').datebox({
            onChange: function() {
                showAffairs();
            }
        });

        $('#depot').combobox({
            onChange: function(val) {
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
