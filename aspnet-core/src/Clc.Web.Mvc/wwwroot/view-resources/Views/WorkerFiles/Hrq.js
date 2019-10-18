(function() {
    $(function() {
        var _$dg = $('#dg');

        $('#tb').children('a[name="search"]').click(function (e) {
            _$dg.datagrid({
                url: 'GetFilePagedData/1'
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