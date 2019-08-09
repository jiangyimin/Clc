(function() {
    $(function() {
        var _userService = abp.services.app.user;
        var _$dg = $('#dg');

        $('#tb').children('a[name="reload"]').click(function (e) {
            // alert('dd');
            _$dg.datagrid('reload');
        });

        // resetWorkerUsers
        $('#tb').children('a[name="resetToLatest"]').click(function (e) {
            abp.message.confirm('确定重置到最新吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    abp.ui.setBusy(_$dg);
                    _userService.resetWorkerUsersToLatest().done(function () {
                        abp.notify.info("成功重置");
                        _$dg.datagrid('reload');
                    }).always(function() {
                        abp.ui.clearBusy(_$dg);
                    });
                }
            });
        });

        $('#tb').children('a[name="remove"]').click(function (e) {
            var row = _$dg.datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要删除的行", "", { positionClass : 'toast-top-center'} );
                return;
            }
            abp.message.confirm('确定删除这一行吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    _userService.delete(row).done(function () {
                        abp.notify.info(row.userName + " 被删除！");
                        _$dg.datagrid('reload'); 
                   });
                }
            });
        });
    });
})();