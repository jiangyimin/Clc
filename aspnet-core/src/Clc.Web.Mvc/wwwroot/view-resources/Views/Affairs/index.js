function affairOperate (value, row, index) {
    if (row.status == "安排") {
        var e = '<a href="javascript:void(0)" onclick="editrow(this)">编辑</a> ';
        var d = '<a href="javascript:void(0)" onclick="deleterow(this)">删除</a>';
        return e + d;
    }
};

function editrow(row) {
    alert(row.id);
}

function deleterow(row) {
    alert(row.id);
}

function reload() {
    $('#dg').datagrid('reload');
}

function save()
{
    if (!$('#fm').form('validate')) {
        abp.notify.error('字段输入有错误');
        return;
    }

    var affair = $('#fm').serializeFormToObject(); //serializeFormToObject is defined in main.js
    
    var defer;
    if ($('#dlg').panel('options').title === "增加") {
        defer = abp.services.app.affair.insert(affair);
    }
    else {
        defer = abp.services.app.affair.update(affair);
    }

    defer.done(function () {
        abp.notify.info($('#dlg').panel('options').title+'操作成功')
        $('#dlg').dialog('close');
    });
}

function createFrom()
{
    var from = $('#fromDate').datebox('getValue');
    if (from == '') {
        abp.notify.error("请先设置来源日期!");
        return;
    }
    if ($('#dd').datebox('getValue') <= from){
        abp.notify.error("任务执行日期必须大于来源日期!");
        return;
    }

    abp.message.confirm('确认要生成吗?', '确认', function (r) {
        if (r) {
            abp.ui.setBusy($('#tb'));
            abp.services.app.affair.createFrom($('#dd').datebox('getValue'), from).done(function (data) {
                abp.notify.info('已生成'+data+'个任务');
                if (data > 0) 
                    crudMS.reload();
            }).always(function () {
                abp.ui.clearBusy($('#tb'));
            });
        }
    })
}

(function() {        
    $(function() {
        var _today;
        var _tomorrow;
        var _affairId;
        var _status;
        
        abp.services.app.field.getWorkplaceItems().done(function (data) {
            $('#workplace').combobox({
                data: data, 
                valueField: 'id', 
                textField: 'name'
            });    
        });

        abp.services.app.work.getTodayString().done(function (d) {
            _today = d;
            $('#dd').datebox('setValue', _today);
            $('#dg').datagrid({
                url: 'Affairs/GridData?CarryoutDate=' + _today
            });

            _tomorrow = getTomorrow(_today);
            _affairId = 0;
        })

        $('#dg').datagrid({
            onSelect: function (index, row) {
                _affairId = row.id;
                _status = row.status;

                $('#dgWorker').datagrid({
                    url: 'Affairs/WorkersGridData/' + _affairId
                })
            }
        })

        // #tb Buttons
        $('#tomorror').checkbox({
            onChange: function() {
                if ($('#tomorror').checkbox('options').checked) {
                    $('#dd').datebox('setValue', _tomorrow);
                    $('#dg').datagrid({
                        url: 'Affairs/GridData?CarryoutDate=' + _tomorrow
                    });
                }
                else {
                    $('#dd').datebox('setValue', _today);
                    $('#dg').datagrid({
                        url: 'Affairs/GridData?CarryoutDate=' + _today
                    });
                }
            }
        })


        $('#tb').children('a[name="reload"]').click(function (e) {
            $('#dg').datagrid('reload');
        })

        $('#tb').children('a[name="activate"]').click(function (e) {
            if (_affairId === 0 ) {   
                abp.notify.error("先选择任务");
                return;
            }
            if (_status !== "安排") {
                abp.notify.error("安排状态才允许激活");
                return;
            };
            abp.services.app.affair.activate(_affairId).done(function () {
                _affairId = 0;
                reload();
            })
        })

        $('#tb').children('a[name="back"]').click(function (e) {
            if (_affairId === 0 ) {   
                abp.notify.error("先选择任务");
                return;
            };
            if (_status !== "激活") {
                abp.notify.warn("激活状态才允许回退");
                return;
            };
            abp.services.app.affair.activate(_affairId, false).done(function () {
                _affairId = 0;
                reload();
            });
        })            

        $('#tb').children('a[name="createFrom"]').click(function (e) {
            e.preventDefault();
            createFrom();
            _affairId = 0;
            reload();
        })

        $('#tb').children('a[name="add"]').click(function (e) {
            $('#dlg').dialog('open').dialog('setTitle', '增加');
            $('#fm').form('clear');

            $('#id').attr('value', '0');
            $('#carryoutDate').attr('value', $('#dd').datebox('getValue'));
        })

        $('#dlg-buttons').children('a[name="save"]').click(function (e) {
            e.preventDefault();
            save();
            _affairId = 0;
            reload();
        })
    })
})();
