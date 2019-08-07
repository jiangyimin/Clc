(function() {
    $(function() {
        var _userService = abp.services.app.user;
        var _$dg = $('#dg');

        $('#tb').children('a[name="reload"]').click(function (e) {
            // alert('dd');
            _$dg.datagrid('reload');
        });

        // resetWorkerUsers
        $('#tb').children('a[name="resetWorkerUsers"]').click(function (e) {
            abp.message.confirm('确定重置工作人员用户吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    abp.ui.setBusy(_$dg);
                    _userService.updateWorkerUsers().done(function () {
                        abp.notify.info("成功重置");
                        _$dg.datagrid('reload');
                    }).always(function() {
                        abp.ui.clearBusy(_$dg);
                    });
                }
            });
        });
    });
})();