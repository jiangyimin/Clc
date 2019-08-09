var _today;
var _tomorrow;
var _affairId = 0;
var _status = '';

function affairOperate (value, row, index) {
    if (row.status == "安排") {
        var e = '<a href="javascript:void(0)" onclick="editAffair(' + index + ')">编辑</a>';
        var space = "<span>&nbsp;&nbsp;&nbsp;</span>"
        var d = '<a href="javascript:void(0)" onclick="deleteAffair(' + index + ')">删除</a>';
        return e + space + d;
    }
};

function reload() {
    $('#dg').datagrid('reload');
    _affairId = 0;
    _status = '';
}

function editAffair(index) {
    var row = $('#dg').datagrid('getRows')[index];
    if (!row) return;
    $('#dlg').dialog('open').dialog('setTitle', '编辑');        
    $('#fm').form('load', row);
}

function deleteAffairr(index) {
    var row = $('#dg').datagrid('getRows')[index];
    if (!row) return;
    abp.message.confirm('确认要删除此记录吗?', '请确认', function (r) {
        if (r) {
            abp.services.app.affair.delete(row.id).done(function () {
                abp.notify.info('删除成功')
                reload();
                $('#dlg').dialog('close');
            });
        };
    });
}

function saveAffair()
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
        reload();
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
            var dfd = abp.services.app.affair.createFrom;

            dfd($('#dd').datebox('getValue'), from).done(function (data) {
                abp.notify.info('已生成'+data+'个任务');
                if (data > 0) reload();
            }).always(function () {
                abp.ui.clearBusy($('#tb'));
            });
        }
    })
}

// Worker
function workerOperate (value, row, index) {
    if (_status == "安排") {
        var e = '<a href="javascript:void(0)" onclick="editWorker(' + index + ')">编辑</a>';
        var space = "<span>&nbsp;&nbsp;&nbsp;</span>"
        var d = '<a href="javascript:void(0)" onclick="deleteWorker(' + index + ')">删除</a>';
        return e + space + d;
    }
};

function editWorker(index) {
    var row = $('#dgWorker').datagrid('getRows')[index];
    if (!row) return;
    $('#dlgWorker').dialog('open').dialog('setTitle', '编辑');        
    $('#fmWorker').form('load', row);
}

function deleteWorker(index) {
    var row = $('#dgWorker').datagrid('getRows')[index];
    if (!row) return;
    abp.message.confirm('确认要删除此记录吗?', '请确认', function (r) {
        if (r) {
            abp.services.app.affair.deleteWorker(row.id).done(function () {
                abp.notify.info('删除成功')
                $('#dgWorker').datagrid('reload');
                $('#dlgWorker').dialog('close');
            });
        };
    });
}

function saveWorker()
{
    if (!$('#fmWorker').form('validate')) {
        abp.notify.error('字段输入有错误');
        return;
    }

    var worker = $('#fmWorker').serializeFormToObject(); //serializeFormToObject is defined in main.js
    
    var defer;
    if ($('#dlgWorker').panel('options').title === "增加") {
        defer = abp.services.app.affair.insertWorker(worker);
    }
    else {
        defer = abp.services.app.affair.updateWorker(worker);
    }

    defer.done(function () {
        abp.notify.info($('#dlgWorker').panel('options').title+'操作成功')
        $('#dgWorker').datagrid('reload');
        $('#dlgWorker').dialog('close');
    });
}

(function() {        
    $(function() {    
        abp.services.app.field.getWorkerListItems().done(function (data) {
            _allWorkers = data;
            $('#worker').combobox({
                data: data,
                valueField: 'id',
                textField: 'cnName'
            })
        });

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

                abp.services.app.field.getWorkRoleItems(row.workplaceId).done(function (data) {
                    // alert(data.length);
                    $('#workRole').combobox({
                        data: data, 
                        valueField: 'id', 
                        textField: 'name'
                    });    
                });
                        
                $('#dgWorker').datagrid({
                    url: 'Affairs/WorkersGridData/' + _affairId
                });

                $('#dgEvent').datagrid({
                    url: 'Affairs/EventsGridData/' + _affairId
                });                
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
            reload();
        })

        $('#tb').children('a[name="activate"]').click(function (e) {
            var checkedRows = $('#dg').datagrid("getChecked");
            if (checkedRows.length === 0) {
                abp.notify.error("请先选中要激活的任务。");
                return;
            }

            ids = [];
            for (var row of checkRows) {
                if (row.strtTime);
                ids.push(row.id);
            };

            abp.services.app.affair.activate(ids).done(function (count) {
                abp.notify.info('有' + count + '个任务被激活');
                reload();
            })
        })

        $('#tb').children('a[name="back"]').click(function (e) {
            if (_affairId === 0 ) {   
                abp.notify.error("先选择任务");
                return;
            };
            if (_status !== "激活") {
                abp.notify.error("激活状态才允许回退");
                return;
            };
            abp.services.app.affair.back(_affairId).done(function () {
                reload();
            });
        })            

        $('#tb').children('a[name="createFrom"]').click(function (e) {
            e.preventDefault();
            createFrom();
        })

        $('#tb').children('a[name="add"]').click(function (e) {
            $('#dlg').dialog('open').dialog('setTitle', '增加');
            $('#fm').form('clear');

            $('#id').attr('value', '0');
            $('#carryoutDate').attr('value', $('#dd').datebox('getValue'));
        })

        $('#dlg-buttons').children('a[name="save"]').click(function (e) {
            e.preventDefault();
            saveAffair();
        })

        // AffairWorker
        $('#tbWorker').children('a[name="reload"]').click(function (e) {
            // alert(_affairId);
            if (_affairId === 0) return;
            $('#dgWorker').datagrid('reload');
        })

        $('#tbWorker').children('a[name="add"]').click(function (e) {
            if (_affairId === 0 ) {   
                abp.notify.error("先选择任务");
                return;
            };
            if (_status != "安排") {
                abp.notify.error("安排状态才允许增加");
                return;
            }
            $('#dlgWorker').dialog('open').dialog('setTitle', '增加');
            $('#fmWorker').form('clear');
            $('#workerId').attr('value', 0);
            $('#workerAffairId').attr('value', _affairId);
        })

        $('#dlgWorker-buttons').children('a[name="save"]').click(function (e) {
            e.preventDefault();
            saveWorker();
        })
    })
})();
