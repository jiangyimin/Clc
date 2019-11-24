(function() {  
    $(function() {    
        abp.services.app.work.getToday().done(function (today) {
            $('#dd').datebox('setValue', today);
            $('#depot').combobox('setValue', window.parent.me.depotId);
            showRoutes();
        });

        $('#dd').datebox({
            onChange: function() {
                showRoutes();
            }
        });

        $('#depot').combobox({
            onChange: function(val) {
                showRoutes();
            }
        });

        $('#tb').children('a[name="event"]').click(function (e) {
            if (mds.masterCurrentRow === null ) {   
                abp.notify.error("先选择线路");
                return;
            };
            $('#dlgEvent').dialog('open');
            $('#dgEvent').datagrid({
                url: "GridDataEvent/" + mds.masterCurrentRow.id
            });
        });
    });

    function showRoutes() {
        var depotId = $('#depot').combobox('getValue');
        if ($('#seld').val() == 0 && depotId != window.parent.me.depotId) {
            abp.notify.error('你不允许查询其他大队的线路');
            return;
        };

        $('#dg').datagrid({
            url: 'QueryGridData',
            queryParams: { CarryoutDate: $('#dd').datebox('getValue'), DepotId: depotId }
        });
    }
})();
