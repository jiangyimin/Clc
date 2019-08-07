(function() {
    $(function() {
        var _userService = abp.services.app.user;
        var _$dg = $('#dg');
        var _$dialog = $('#dlg');
        var _$form = $('#fm');
        var _row;

        // role combobox
        _userService.getRoles(false).done(function(roles) {
            $('#roleNames').combobox({
                data: roles.items,
                valueField: 'name',
                textField: 'displayName'
            });
        });

        $('#tb').children('a[name="reload"]').click(function (e) {
            _$dg.datagrid('reload');
        });

        $('#tb').children('a[name="add"]').click(function (e) {
            _$dialog.dialog('open').dialog('setTitle', '增加');
            _$form.form('clear');
            $('#userName').next('span').find('input').focus();
        });

        $('#tb').children('a[name="edit"]').click(function (e) {
            _row = $('#dg').datagrid('getSelected');
            if (!_row) {
                abp.notify.error("选择要修改的用户", "", { positionClass : 'toast-top-center'} );
                return;
            }
            _$dialog.dialog('open').dialog('setTitle', '修改');
            _$form.form('load', _row);
        });

        $('#tb').children('a[name="remove"]').click(function (e) {
            _row = _$dg.datagrid('getSelected');
            if (!_row) {
                abp.notify.error("选择要删除的行", "", { positionClass : 'toast-top-center'} );
                return;
            }
            abp.message.confirm('确定删除这一行吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    _userService.delete(_row).done(function () {
                        abp.notify.info(_row.name + " 被删除！");
                        _$dialog.dialog('close');
                        _$dg.datagrid('reload'); 
                   });
                }
            });
        });

        $('#dlg-tb').children('a[name="save"]').click(function (e) {
            e.preventDefault();

            if (!_$form.form('validate')) {
                return;
            }

            var user = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js
            //alert(user.roleNames);
            if (user.roleNames)
                user.roleNames = [user.roleNames];
            
            abp.ui.setBusy(_$dialog);
            var _defer;
            if (_$dialog.panel('options').title === "增加") {
                _defer = _userService.create(user);
            }
            else {
                _defer = _userService.update(user);
            }

            _defer.done(function () {
                abp.notify.info(_$dialog.panel('options').title+'操作成功')
                _$dialog.dialog('close');
                _$dg.datagrid('reload');
            }).always(function() {
                abp.ui.clearBusy(_$dialog);
            });
        });
        
        $('#dlg-tb').children('a[name="cancel"]').click(function (e) {
            _$dialog.dialog('close');
        });
    });
})();