(function() {  
    $(function() {    
        abp.services.app.work.getToday().done(function (today) {
            $('#dd').datebox('setValue', today);
            abp.services.app.field.getComboItems('Depot').done(function (data) {
                $('#depot').combobox({
                    data: data,
                    valueField: 'value',
                    textField: 'displayText'
                })
                $('#depot').combobox('setValue', window.parent.me.depotId);
            });
            showRoutes();
        });

        $('#depot').combobox({
            onChange: function(val) {
                showRoutes();
            }
        });

        $('#dg').datagrid({
            onSelect: function() {
                var row = $('#dg').datagrid('getSelected');
                $('#dgTask').datagrid({
                    url: "GridDataTask/" + row.id
                });
            }
        });

        initMap();
    });

    function showRoutes() {
        var depotId = $('#depot').combobox('getValue');
        if ($('#seld').val() == 0 && depotId != window.parent.me.depotId) {
            abp.notify.error('你不允许查看其他大队的线路');
            return;
        };

        $('#dg').datagrid({
            url: 'QueryGridData',
            queryParams: { CarryoutDate: $('#dd').datebox('getValue'), DepotId: depotId }
        });
    }
})();

// show Map
var map = {};
function initMap()
{
    // alert('show map');
    map = new BMap.Map("allmap");    // 创建Map实例
    map.centerAndZoom(new BMap.Point(114.022, 22.634), 12);  // 初始化地图,设置中心点坐标和地图级别
    //添加地图类型控件
    map.addControl(new BMap.MapTypeControl({
        mapTypes:[
            BMAP_NORMAL_MAP,
            BMAP_HYBRID_MAP
        ]}));	  
    map.setCurrentCity("运城");          // 设置地图显示的城市 此项是必须设置的
    map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
}


