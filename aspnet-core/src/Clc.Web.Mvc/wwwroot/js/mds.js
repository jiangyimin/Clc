var mds = mds || {};
(function ($) {
    // alert('enter mds');
    var _dfd = null;

    mds.dgDefault = {
        postfix: '',
        sortName: 'id',
        columns: [[]],
        operatorColumn: true, 
        singleSelect: true, 
        insert: function() {},
        update: function() {},
        delete: function() {},
    };
    mds.controllerName = '';
    mds.masterInputName = '';
    mds.masterCurrentRow = null;
    mds.today = '';

    mds.main = {};
    mds.details = [];

    mds.initfunction = function () {};
    mds.onselectfunction = function() {};
    mds.customerSetAddValue = function() {};
    mds.getUrl = function() {};

    mds.reload = function (postfix) {
        var $dg = $('#dg' + postfix);
        if (postfix === mds.main.postfix) {
            mds.masterCurrentRow = null;
            mds.reloadSelectedTab();
            $dg.datagrid('reload');
        }
        else {
            $dg.datagrid('reload');
        }
    };

    mds.add = function (postfix) {  
        if (postfix != mds.main.postfix && mds.masterCurrentRow == null) {
            abp.notify.error("先选择父记录");
            return;
        };

        _dfd = mds.getInsertDfd(postfix);
        //alert(_dfd);

        $('#dlg' + postfix).dialog('open').dialog('setTitle', '增加');
        var $fm = $('#fm' + postfix);
        $fm.form('clear');
        $fm.find('input[name="id"]').attr('value', 0);   // 将Id值置为0
        mds.customerSetAddValue(postfix);


        if (postfix !== mds.main.postfix) {           // 处理子表的父id
            var masterDom = 'input[name="' + mds.masterInputName + '"]';
            //alert(mds.masterCurrentRow.id);
            $fm.find(masterDom).attr('value', mds.masterCurrentRow.id);
        }
      
    };

    mds.operatorIsEnable = function(row) {
        return true;
    };

    mds.operator = function(val, row, index) {
        if (mds.operatorIsEnable(row)) {            
            var htmlTag = '<a href="javascript:void(0)" onclick="mds.edit(' + '\'' + row.postfix + '\', ' + index + ')">编辑</a>';
            htmlTag = htmlTag + '<span>&nbsp;&nbsp;&nbsp;</span>';
            htmlTag = htmlTag + '<a href="javascript:void(0)" onclick="mds.delete(' + '\'' + row.postfix + '\', ' + index + ')">删除</a>';
            // alert(htmlTag);
            return htmlTag;
        }
    };
    mds.edit = function(postfix, index) {
        _dfd = mds.getUpdateDfd(postfix);
        // alert(_dfd);
        $('#dlg' + postfix).dialog('open').dialog('setTitle', '编辑');

        var $fm = $('#fm' + postfix);
        var row = $('#dg' + postfix).datagrid('getRows')[index];                
        $fm.form('load', row);

        if (postfix !== '') {           // 处理子表的父id
            var masterDom = 'input[name="' + mds.masterInputName + '"]';
            $fm.find(masterDom).attr("value", mds.masterValue);
        };
    };

    mds.delete = function(postfix, index) {
        var row = $('#dg' + postfix).datagrid('getRows')[index];
        abp.message.confirm('确认要删除此记录吗?', '请确认', function (r) {
            if (r) {
                _dfd = mds.getDeleteDfd(postfix);
                // alert(_dfd);alert(row.id);
                _dfd(row.id).done(function () {
                    abp.notify.info('删除操作成功')
                    mds.reload(postfix);
                });
            };
        });
    };

    mds.save = function(postfix) {
        var $fm = $('#fm' + postfix);
        var $dlg = $('#dlg' + postfix);
        if (!$fm.form('validate')) {
            abp.notify.error('字段输入有错误');
            return;
        }
    
        var dto = $fm.serializeFormToObject(); //serializeFormToObject is defined in main.js
        //alert(_dfd);
        _dfd(dto).done(function () {
            abp.notify.info($dlg.panel('options').title+'操作成功')
            mds.reload(postfix);
            $dlg.dialog('close');
        });
    };

    // private
    mds.getInsertDfd = function(postfix) {
        if (postfix == mds.main.postfix)
            return mds.main.insert;
        else
            return mds.details[mds.getIndex(postfix)].insert;
    };
    mds.getUpdateDfd = function(postfix) {
        if (postfix == mds.main.postfix)
            return mds.main.update;
        else
            return mds.details[mds.getIndex(postfix)].update;
    };
    mds.getDeleteDfd = function(postfix) {
        if (postfix == mds.main.postfix)
            return mds.main.delete;
        else
            return mds.details[mds.getIndex(postfix)].delete;
    };

    mds.getIndex = function (postfix) {
        for (var i = 0; i < mds.details.length; i++) {
            if (mds.details[i].postfix == postfix) {
                return i;
            }
        }
    };

    mds.reloadSelectedTab = function () {
        var tab = $('#tt').tabs('getSelected');
        var postfix = mds.details[$('#tt').tabs('getTabIndex', tab)].postfix;
        // alert('reloadSelectTab' + postfix);
        $('#dg' + postfix).datagrid({ 
            url: mds.getUrl(postfix, mds.masterCurrentRow)
        });
    }

    $(function () {
        mds.initfunction();  
        
        if (mds.main.operatorColumn) {
            mds.main.columns[0].push({field: "postfix", title: "Postfix", width: 80, hidden: true });
            mds.main.columns[0].push({field: "operator", title: "操作", width: 80, align: "center", formatter: mds.operator});
        }

        for (var i = 0; i < mds.details.length; i++) {
            var detail = mds.details[i];
            if (detail.operatorColumn) {
                detail.columns[0].push({ field: "postfix", title: "Postfix", width: 80, hidden: true });
                detail.columns[0].push({field: "operator", title: "操作", width: 80, align: "center", formatter: mds.operator});
            }
            $('#dg' + detail.postfix).datagrid({
                columns: detail.columns,
                sortName: detail.sortName,
                striped: true,
                rownumbers: true,
                singleSelect: detail.singleSelect,
                fit: true,
                fitColumns: true,
            });
        };

        $('#dg' + mds.main.postfix).datagrid({
            // url: mds.getUrl(mds.main.postfix),
            columns: mds.main.columns,
            sortName: mds.main.sortName,
            striped: true,
            rownumbers: true,
            singleSelect: mds.main.singleSelect,
            fit: true,
            fitColumns: true,
        });
        
        $('#dg' + mds.main.postfix).datagrid({
            onSelect: function (index, row) {
                mds.masterCurrentRow = row;

                mds.onselectfunction(row);
                mds.reloadSelectedTab();
            }
        })
        $('#tt').tabs({
            onSelect: function(title, index) {
                mds.reloadSelectedTab();
            }
        });

     });
})(jQuery);
