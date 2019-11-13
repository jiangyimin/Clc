(function() {        
    $(function() {
        abp.services.app.work.getMyAffairWork().done(function (wk) {
            work.me = wk;
            $('#dd').datebox('setValue', work.me.today);
            if (!work.validate()) return; 
            showAffair(work.me.affairId);
        });

        $('#tb').children('a[name="reload"]').click(function (e) {
            if (!work.validate()) return; 
            showAffair(work.me.affairId);
        });
    
        $('#tb').children('a[name="askOpenDoor"]').click(function (e) {
            if (!work.validate()) return; 

            abp.message.confirm('确认要申请开门吗?', '确认', function (r) {
                if (r) {
                    abp.services.app.work.askOpenDoor(work.me.today, work.me.depotId, work.me.affairId).done(function (ret) {
                       abp.notify.info("askOpen"); 
                    });
                }
            });
        });  

        $('#tb').children('a[name="askOpenDoorTask"]').click(function (e) {
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

    function showAffair(id) {
        abp.services.app.work.allowCardWhenCheckin().done(function (ret) {
            allowCardWhenCheckin = ret;
        });

        abp.services.app.affair.getAffair(id).done(function (dto) {
            $('#fm').form('load', dto);
        });
        
        $('#dg').datagrid({
            url: 'Checkin/GridDataWorker/' + id
        });
        $('#dgTask').datagrid({
            url: 'Checkin/GridDataTask/' +id
        });
    }
})();
