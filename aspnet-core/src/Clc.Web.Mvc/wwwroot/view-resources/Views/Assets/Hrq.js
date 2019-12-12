(function() {
    $(function() {
        abp.services.app.field.getComboItems('Depot').done(function (d) {
            $('#depot').combobox({
                data: d, valueField: 'value', textField: 'displayText'
            });
        });

        abp.services.app.type.getComboItems('Category').done(function (d) {
            $('#category').combobox({
                data: d, valueField: 'value', textField: 'displayText'
            });
        });

        $('#fmSearch').children('a[name="search"]').click(function (e) {
            $('#dg').datagrid({
                url: 'GetPagedData',
                queryParams: $('#fmSearch').serializeFormToObject()
            });
        });

        $('#dg').datagrid({
            onSelect: function() {
                var row = $('#dg').datagrid('getSelected');
                $('#fm').form('load', row);
            }
        })
    });
})();