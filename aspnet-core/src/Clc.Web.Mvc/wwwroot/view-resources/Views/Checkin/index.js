(function() {        
    $(function() {
        abp.services.app.work.allowCardWhenCheckin().done(function (ret) {
            allowCardWhenCheckin = ret;
        });

        // set work.me
        work.me.today = $('#dd').datebox('getValue');
        work.me.altcheck = document.getElementById('altcheck').value;
        work.me.depotId = document.getElementById('depotId').value;
        work.me.affairId = document.getElementById('affairId').value;
        work.me.workplaceId = document.getElementById('workplaceId').value;

        showAffair();

        $('#tb').children('a[name="reload"]').click(function (e) {
            showAffair();
        });
    
        $('#tb').children('a[name="askOpen"]').click(function (e) {
            if (!work.validate()) return; 
            if (work.me.altcheck) {
                abp.notify.error("帮验金库任务，不用申请开门", "", { positionClass : 'toast-top-center'} );
                return false;
            };
       
            status = 'ask';
            $('#dlgAsk').dialog('open');
        });  

        $('#tbTask').children('a[name="askOpenTask"]').click(function (e) {
           if (!work.validate()) return; 
            if (work.me.altcheck) {
                abp.notify.error("帮验金库任务，不用申请开门", "", { positionClass : 'toast-top-center'} );
                return false;
            };
                   
            var rows = $('#dgTask').datagrid('getRows');
            if (rows.length == 0) {
                abp.notify.error("无金库子任务！", "", { positionClass : 'toast-top-center'} );
                return false;
            }

            vdoorId = rows[0].workplaceId;
            status = 'askTask';
            $('#dlgAsk').dialog('open');
        }); 

        $('#dlCard').datalist({
            data: work.askWorkers,
            valueField: 'name',
            textField: 'name',
            lines: true,
            textFormatter: function(value,row,index) {
                return '<span style="font-size:24px">'+value+'</span>';
            }
        });


        $('#dlgAsk').dialog({
            onClose: function() {
                status = '';
                vdoorId = 0;
                work.askWorkers = [];
                $('#dlCard').datalist('loadData', []);
            }
        });

    });

    function showAffair() {
        if (!work.validate()) return; 

        $('#dg').datagrid({
            url: 'Checkin/GridDataWorker/' + work.me.affairId
        });
        $('#dgTask').datagrid({
            url: 'Checkin/GridDataTask/' + work.me.affairId
        });
    }
})();
