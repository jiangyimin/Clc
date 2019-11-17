(function() {        
    $(function() {
        abp.services.app.work.allowCardWhenCheckin().done(function (ret) {
            allowCardWhenCheckin = ret;
        });

        // set work.me
        work.me.today = $('#dd').datebox('getValue');
        work.me.depotId = depotId.value,
        work.me.affairId = affairId.value;

        showAffair();

        $('#tb').children('a[name="reload"]').click(function (e) {
            showAffair();
        });
    
        $('#tb').children('a[name="askOpen"]').click(function (e) {
            if (!work.validate(affairId.value)) return; 

            abp.message.confirm('确认要申请开门吗?', '确认', function (r) {
                if (r) {
                    abp.services.app.work.askOpenDoor(work.me.today, work.me.depotId, work.me.affairId).done(function (ret) {
                       abp.notify.info("askOpen"); 
                    });
                }
            });
        });  

        $('#tb').children('a[name="askOpenTask"]').click(function (e) {
            if (!work.validate()) return; 

            abp.message.confirm('确认要申请开门吗?', '确认', function (r) {
                if (r) {
                    abp.services.app.work.askOpenDoorTask(work.me.today, work.me.depotId, work.me.affairId).done(function (ret) {
                       abp.notify.info("askOpenTask"); 
                    });
                }
            });
        });    
    });

    function showAffair() {
        if (!work.validate(affairId.value)) return; 
        
        $('#dg').datagrid({
            url: 'Checkin/GridDataWorker/' + affairId.value
        });
        $('#dgTask').datagrid({
            url: 'Checkin/GridDataTask/' + affairId.value
        });
    }
})();
