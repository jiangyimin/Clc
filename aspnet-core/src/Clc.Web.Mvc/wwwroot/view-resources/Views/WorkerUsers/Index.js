(function() {
    $(function() {
        var _userService = abp.services.app.user;
        var _$dg = $('#dg');

        $('#tb').children('a[name="reload"]').click(function (e) {
            // alert('dd');
            _$dg.datagrid('reload');
        });

        // resetWorkerUsers
        $('#tb').children('a[name="adds"]').click(function (e) {
            abp.message.confirm('确定批量增加用户吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    abp.ui.setBusy(_$dg);
                    _userService.addWorkerUsers().done(function () {
                        abp.notify.info("成功添加");
                        _$dg.datagrid('reload');
                    }).always(function() {
                        abp.ui.clearBusy(_$dg);
                    });
                }
            });
        });

        $('#tb').children('a[name="update"]').click(function (e) {
            var row = _$dg.datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要更新的行", "", { positionClass : 'toast-top-center'} );
                return;
            }
            abp.message.confirm('确定更新这一行吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    _userService.updateWorkerUser(row).done(function () {
                        abp.notify.info(row.userName + " 已更新！");
                        _$dg.datagrid('reload'); 
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