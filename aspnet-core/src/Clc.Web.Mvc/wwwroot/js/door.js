var ws = null;
var jjjj = 'ddd';
(function() {
    // web socket
    function initWS() {
        // alert('initWs');
        ws = new WebSocket("ws://127.0.0.1:4649/M500Net");
        ws.onopen = function () {
            console.log("Open connection to websocket");
            abp.notify.info('连接到门禁控制')
        };
        ws.onclose = function () {
            console.log("Close connection to websocket");
            // 断线重连
            initWS();
        }

        ws.onmessage = function (e) {
            abp.notify.info(e.data);
        }    
    }

    $(function () {
        initWS();

        // alert('dataliost');
        $('#dlCard').datalist({
            data: work.askWorkers,
            valueField: 'name',
            textField: 'name',
            lines: true,
            textFormatter: function(value,row,index) {
                return '<span style="font-size:24px">'+value+'</span>';
            }
        });

        $('#dlg').dialog({
            onClose: function() {
                status = '';
                work.askWorkers = [];
                $('#dlCard').datalist('loadData', []);
            }
        });
    });
})();