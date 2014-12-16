$(function () {
    var _$_datagrid = $("#DG_TaxRateSetting");
    var QueryURL = "/TaxRateSetting/GetData";

    var PrintURL = "";

    var CreateCategoryDlg = null;
    var CreateCategoryUrl = "/TaxRateSetting/CreateCategory?parentTaxNo=";
    var UpdateCategoryUrl = "/TaxRateSetting/UpdateCategory?currentTaxNo=";

    var CreateItemDlg = null;
    var CreateItemUrl = "/TaxRateSetting/CreateItem?parentTaxNo=";
    var UpdateItemUrl = "/TaxRateSetting/UpdateItem?currentTaxNo=";

    var DeleteItemURL = "/TaxRateSetting/DeleteItem?trsId=";

    $("#btnAddCategory").click(function () {
        if (_$_datagrid.treegrid("getSelected")) {
            CategoryCreate(_$_datagrid.treegrid("getSelected").TaxNo, _$_datagrid.treegrid("getSelected").CargoName);
        }
        else {
            CategoryCreate("", "");
        }
    });

    $("#btnAddItem").click(function () {
        if (_$_datagrid.treegrid("getSelected")) {
            ItemCreate(_$_datagrid.treegrid("getSelected").TaxNo, _$_datagrid.treegrid("getSelected").CargoName);
        }
    });

    $("#btnRefresh").click(function () {
        _$_datagrid.treegrid("reload");
        _$_datagrid.treegrid("unselectAll");
        //ExpandAllNode();
    });

    _$_datagrid.treegrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'TaxNo',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'TaxNo',
        treeField: 'CargoName',
        columns: [[
                    { field: 'CargoName', title: '品名', width: 300
                    },
                    { field: 'TaxNo', title: '税号', width: 200
                    },
					{ field: 'Unit', title: '单位', width: 150
					},
                    { field: 'FullValue', title: '完税价格', width: 150
                    },
                     { field: 'TaxRateDescription', title: '税率', width: 100
                     },
                      { field: 'fOperate', title: '操作', width: 120,
                          formatter: function (value, rowData, rowIndex) {
                              return "<input type='button' id='btnUpdate_" + rowData.TaxNo + "' class='btnUpdateInfo_cls' currantTaxNo='" + rowData.TaxNo + "' currentCategoryName='" + rowData.CargoName + "' trsID='" + rowData.trsID + "' Unit='" + rowData.Unit + "' FullValue='" + rowData.FullValue + "' TaxRate='" + rowData.TaxRate + "' ParentTaxNo='" + rowData.ParentTaxNo + "' isLeaf='" + rowData.isLeaf + "' value='修改'></input>" + "&nbsp;|&nbsp;" + "<input type='button' id='btnDelete_" + rowData.TaxNo + "' class='btnDeleteInfo_cls' currantTaxNo='" + rowData.TaxNo + "' trsID='" + rowData.trsID + "' parentID='" + rowData.parentID + "' value='删除'></input>";
                          }
                      },
                    { field: 'trsID', title: '', hidden: true
                    },
                    { field: 'ParentTaxNo', title: '', hidden: true
                    }
				]],
        pagination: false,
        toolbar: "#toolBar",
        onContextMenu: function (e, row) {
            e.preventDefault();
            _$_datagrid.treegrid("unselectAll");
            _$_datagrid.treegrid("select", row.TaxNo);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuCreateCategory" iconCls="icon-add"/>').html("新增父类").appendTo(cmenu);
            $('<div  id="mnuItem" iconCls="icon-add"/>').html("新增子项").appendTo(cmenu);
            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html("修改").appendTo(cmenu);
            $('<div  id="mnuRemove" iconCls="icon-remove"/>').html("删除").appendTo(cmenu);
            $('<div  id="mnuRefresh" iconCls="icon-reload"/>').html("刷新").appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnurefresh":
                            _$_datagrid.treegrid("reload");
                            _$_datagrid.treegrid("unselectAll");
                            //ExpandAllNode();
                            break;
                        case "mnuupdate":
                            $("#btnUpdate_" + row.TaxNo).click();
                            break;
                        case "mnuremove":
                            $("#btnDelete_" + row.TaxNo).click();
                            break;
                        case "mnucreatecategory":
                            if (row.isLeaf == "1") {
                                reWriteMessagerAlert("操作提示", "不能新增父类", "error");
                                return;
                            }
                            $("#btnAddCategory").click();
                            break;
                        case "mnuitem":
                            if (row.isLeaf == "1") {
                                reWriteMessagerAlert("操作提示", "不能新增子项", "error");
                                return;
                            }
                            $("#btnAddItem").click();
                            break;
                    }
                }
            });

            $('#cmenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onHeaderContextMenu: function (e, field) {
            e.preventDefault();
            if (!$('#tmenu').length) {
                createColumnMenu();
            }
            $('#tmenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onSortColumn: function (sort, order) {
            //_$_datagrid.treegrid("reload");
        },
        //        onClickRow: function (row) {
        //            //_$_datagrid.treegrid("expand", row.ID);
        //            //            if (row.isLeaf == "1") {
        //            //                Update(row.frID, row.CategoryName, row.CategoryValue, row.CategoryUnit, row.mMemo, row.parentID)
        //            //            } else {
        //            //                _$_datagrid.treegrid("toggle", row.CategoryID);
        //            //            }
        //            _$_datagrid.treegrid("toggle", row.CategoryID);
        //        },
        onDblClickCell: function (field, row) {
            $("#btnUpdate_" + row.TaxNo).click();
        },
        onLoadSuccess: function (data) {
            var allUpdateInfoBtns = $(".btnUpdateInfo_cls");
            var allDeleteInfoBtns = $(".btnDeleteInfo_cls");
            $.each(allUpdateInfoBtns, function (i, item) {
                var currantTaxNo = $(item).attr("currantTaxNo");
                var currentCategoryName = $(item).attr("currentCategoryName");
                var trsID = $(item).attr("trsID");
                var Unit = $(item).attr("Unit");
                var FullValue = $(item).attr("FullValue");
                var TaxRate = $(item).attr("TaxRate");
                var ParentTaxNo = $(item).attr("ParentTaxNo");
                var isLeaf = $(item).attr("isLeaf");

                $(item).click(function () {
                    Update(currantTaxNo, currentCategoryName, trsID, Unit, FullValue, TaxRate, ParentTaxNo, isLeaf);
                });
            });

            $.each(allDeleteInfoBtns, function (i, item) {
                var currantTaxNo = $(item).attr("currantTaxNo");
                var trsID = $(item).attr("trsID");
                var parentID = $(item).attr("parentID");

                if (_$_datagrid.treegrid('getChildren', currantTaxNo).length > 0) {
                    $(item).hide();
                }

                $(item).click(function () {
                    if (_$_datagrid.treegrid('getChildren', currantTaxNo).length > 0) {
                        reWriteMessagerAlert('操作提示', '请先删除其下面子类\子项', "error");
                        return false;
                    } else {
                        DeleteInfo(trsID, parentID);
                    }
                });
            });

            delete _$_datagrid.treegrid('options').queryParams['id'];
            _$_datagrid.treegrid("expandAll");
        }
    });

    //    //设置分页控件   
    //    var p = _$_datagrid.treegrid('getPager');
    //    $(p).pagination({
    //        pageSize: 15,
    //        pageList: [10, 15, 20, 25, 30]
    //    });

    //    setTimeout(function () {
    //        _$_datagrid.treegrid("reload");
    //        _$_datagrid.treegrid("unselectAll");
    //        ExpandAllNode();
    //    }, 20);

    //    function ExpandAllNode() {
    //        setTimeout(function () {
    //            _$_datagrid.treegrid("expandAll");
    //            setTimeout(function () {
    //                _$_datagrid.treegrid("expandAll");
    //            }, 1100);
    //        }, 500);
    //    }

    function CategoryCreate(parentTaxNo, parentCategoryName) {
        $("#span_ParentCategoryName").html(parentCategoryName);
        $("#hidParentTaxNo").val(parentTaxNo);

        $("#txtTaxNo").val("");
        $("#txtCategoryName").val("");

        CreateCategoryDlg = $('#dlg_AddCategory_Create').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var parentTaxNo = "";
                    var hidParentTaxNo = $("#hidParentTaxNo").val();
                    var currentTaxNo = $("#txtTaxNo").val();
                    var currentCategoryName = $("#txtCategoryName").val();
                    parentTaxNo = hidParentTaxNo;
                    if (hidParentTaxNo == "") {
                        parentTaxNo = "top";
                    }
                    if (currentTaxNo == "" || currentCategoryName == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(税号、品名)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: CreateCategoryUrl + encodeURI(parentTaxNo) + "&currentTaxNo=" + encodeURI(currentTaxNo) + "&currentCategoryName=" + encodeURI(currentCategoryName),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                bOK = true;
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                            } else {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });
                    if (bOK) {
                        CreateCategoryDlg.dialog('close');
                        if (parentTaxNo != "top") {
                            _$_datagrid.treegrid("reload", parentTaxNo);
                        } else {
                            _$_datagrid.treegrid("reload");
                            _$_datagrid.treegrid("unselectAll");
                        }
                        //ExpandAllNode();
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateCategoryDlg.dialog('close');
                }
            }],
            title: '新增父类',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 200
        });

        $('#dlg_AddCategory_Create').dialog("open");
    }

    function ItemCreate(parentTaxNo, parentCategoryName) {
        $("#tipParentItemName").html(parentCategoryName);
        $("#hid_ParentItemTaxNo").val(parentTaxNo);

        $("#txtItemTaxNo").val("");
        $("#txtItemName").val("");
        $("#txtItemUnit").val("");
        $("#txtItemFullValue").val("");
        $("#txtItemTaxRate").val("");

        CreateItemDlg = $('#dlg_AddItem_Create').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var hidParentTaxNo = $("#hid_ParentItemTaxNo").val();
                    var currentTaxNo = $("#txtItemTaxNo").val();
                    var currentCategoryName = $("#txtItemName").val();
                    var txtItemUnit = $("#txtItemUnit").val();
                    var txtItemFullValue = $("#txtItemFullValue").val();
                    var txtItemTaxRate = $("#txtItemTaxRate").val();

                    if (currentTaxNo == "" || currentCategoryName == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(税号、品名)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: CreateItemUrl + encodeURI(hidParentTaxNo) + "&currentTaxNo=" + encodeURI(currentTaxNo) + "&currentCategoryName=" + encodeURI(currentCategoryName) + "&txtItemUnit=" + encodeURI(txtItemUnit) + "&txtItemFullValue=" + encodeURI(txtItemFullValue) + "&txtItemTaxRate=" + encodeURI(txtItemTaxRate),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                bOK = true;
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                            } else {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });
                    if (bOK) {
                        CreateItemDlg.dialog('close');
                        _$_datagrid.treegrid("reload", hidParentTaxNo);

                        //                        _$_datagrid.treegrid("reload");
                        //                        _$_datagrid.treegrid("unselectAll");
                        //ExpandAllNode();
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateItemDlg.dialog('close');
                }
            }],
            title: '新增子项',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 300
        });

        $('#dlg_AddItem_Create').dialog("open");
    }

    function Update(currantTaxNo, currentCategoryName, trsID, Unit, FullValue, TaxRate, ParentTaxNo, isLeaf) {
        switch (isLeaf) {
            case "0": //修改父类
                CategoryUpdate(currantTaxNo, currentCategoryName, trsID);
                break;
            case "1": //修改项
                ItemUpdate(currantTaxNo, currentCategoryName, Unit, FullValue, TaxRate, trsID);
                break;
            default:
                break;

        }
    }

    function CategoryUpdate(currantTaxNo, currentCategoryName, trsID) {
        var parentCategoryName = "";
        var parentTaxNo = "";
        if (_$_datagrid.treegrid("getParent", currantTaxNo)) {
            parentCategoryName = _$_datagrid.treegrid("getParent", currantTaxNo).CargoName;
            parentTaxNo = _$_datagrid.treegrid("getParent", currantTaxNo).TaxNo;
        };
        $("#span_ParentCategoryName").html(parentCategoryName);
        $("#hidParentTaxNo").val(parentTaxNo);
        $("#hidTrsId").val(trsID);

        $("#txtTaxNo").val(currantTaxNo);
        $("#txtCategoryName").val(currentCategoryName);

        CreateCategoryDlg = $('#dlg_AddCategory_Create').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var parentTaxNo = "";
                    var hidParentTaxNo = $("#hidParentTaxNo").val();
                    var currentTaxNo = $("#txtTaxNo").val();
                    var currentCategoryName = $("#txtCategoryName").val();
                    var trsID = $("#hidTrsId").val();

                    parentTaxNo = hidParentTaxNo;
                    if (hidParentTaxNo == "") {
                        parentTaxNo = "top";
                    }
                    if (currentTaxNo == "" || currentCategoryName == "" || trsID == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(税号、品名)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateCategoryUrl + encodeURI(currentTaxNo) + "&currentCategoryName=" + encodeURI(currentCategoryName) + "&trsId=" + encodeURI(trsID),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                bOK = true;
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                            } else {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });
                    if (bOK) {
                        CreateCategoryDlg.dialog('close');
                        if (parentTaxNo != "top") {
                            _$_datagrid.treegrid("reload", parentTaxNo);
                        } else {
                            _$_datagrid.treegrid("reload");
                            _$_datagrid.treegrid("unselectAll");
                        }
                        //ExpandAllNode();
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateCategoryDlg.dialog('close');
                }
            }],
            title: '修改父类',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 200
        });

        $('#dlg_AddCategory_Create').dialog("open");
    }

    function ItemUpdate(currantTaxNo, currentCategoryName, Unit, FullValue, TaxRate, trsID) {
        var parentCategoryName = "";
        var parentTaxNo = "";
        if (_$_datagrid.treegrid("getParent", currantTaxNo)) {
            parentCategoryName = _$_datagrid.treegrid("getParent", currantTaxNo).CargoName;
            parentTaxNo = _$_datagrid.treegrid("getParent", currantTaxNo).TaxNo;
        };
        $("#tipParentItemName").html(parentCategoryName);
        $("#hid_ParentItemTaxNo").val(parentTaxNo);
        $("#hid_ItemTrsId").val(trsID);

        $("#txtItemTaxNo").val(currantTaxNo);
        $("#txtItemName").val(currentCategoryName);
        $("#txtItemUnit").val(Unit);
        $("#txtItemFullValue").val(FullValue);
        $("#txtItemTaxRate").val(TaxRate);

        CreateItemDlg = $('#dlg_AddItem_Create').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var parentTaxNo = "";
                    var hidParentTaxNo = $("#hid_ParentItemTaxNo").val();
                    var currentTaxNo = $("#txtItemTaxNo").val();
                    var currentCategoryName = $("#txtItemName").val();
                    var txtItemUnit = $("#txtItemUnit").val();
                    var txtItemFullValue = $("#txtItemFullValue").val();
                    var txtItemTaxRate = $("#txtItemTaxRate").val();
                    var trsID = $("#hid_ItemTrsId").val();

                    parentTaxNo = hidParentTaxNo;

                    if (currentTaxNo == "" || currentCategoryName == "" || txtItemUnit == "" || txtItemFullValue == "" || txtItemTaxRate == "" || trsID == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(税号、品名、单位、完税价格、税率)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateItemUrl + encodeURI(currentTaxNo) + "&currentCategoryName=" + encodeURI(currentCategoryName) + "&txtItemUnit=" + encodeURI(txtItemUnit) + "&txtItemFullValue=" + encodeURI(txtItemFullValue) + "&txtItemTaxRate=" + encodeURI(txtItemTaxRate) + "&trsId=" + encodeURI(trsID),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                bOK = true;
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                            } else {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });
                    if (bOK) {
                        CreateItemDlg.dialog('close');
                        _$_datagrid.treegrid("reload", parentTaxNo);

                        //ExpandAllNode();
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateItemDlg.dialog('close');
                }
            }],
            title: '修改子项',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 300
        });

        $('#dlg_AddItem_Create').dialog("open");
    }

    function DeleteInfo(trsID, parentID) {
        if (trsID == "") {
            reWriteMessagerAlert('操作提示', "请先选择需要删除的子项或父类", 'error');
            return false;
        } else {
            reWriteMessagerConfirm("提示", "您确定需要删除所选的父类或子项吗？",
                    function (ok) {
                        $.ajax({
                            type: "POST",
                            url: DeleteItemURL + encodeURI(trsID),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
                                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                    if (parentID != "top") {
                                        _$_datagrid.treegrid("reload", parentID);
                                    } else {
                                        $("#btnRefresh").click();
                                    }
                                    
                                } else {
                                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                    });
        }
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
        var fields = _$_datagrid.treegrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.treegrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "cargoname":
                    break;
                case "trsid":
                    break;
                case "parenttaxno":
                    break;
                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "CargoName") {

                } else {
                    if (item.iconCls == 'icon-ok') {
                        _$_datagrid.treegrid('hideColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-empty'
                        });
                    } else {
                        _$_datagrid.treegrid('showColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-ok'
                        });
                    }
                }
            }
        });
    }
});
