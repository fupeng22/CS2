$(function () {
    var _$_datagrid = $("#DG_WayBillResult");
    var _$_ddCompany = $('#ddCompany');
    var QueryCompanyURL = "/ForwarderMain/LoadComboxJSON";

    var DetailURL = "/Customer_SceneCheckDetail/Index?Detail_wbID=";
    var CheckURL = "/Customer_SceneCheck/upDateSwbNeedCheck";

    var DetailDlg = null;
    var DetailDlgForm = null;

    _$_ddCompany.combobox({
        url: QueryCompanyURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    _$_ddCompany.combobox("setValue", "---请选择---");

    var PrintURL = "";
    var QueryURL = "/Customer_SceneCheck/GetData?txtBeginDate=" + encodeURI($("#txtBeginDate").val()) + "&txtEndDate=" + encodeURI($("#txtEndDate").val()) + "&ddCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val());

    $("#btnQuery").click(function () {
        QueryURL = "/Customer_SceneCheck/GetData?txtBeginDate=" + encodeURI($("#txtBeginDate").val()) + "&txtEndDate=" + encodeURI($("#txtEndDate").val()) + "&ddCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#btnPrint").click(function () {
        PrintURL = "/Customer_SceneCheck/Print?txtBeginDate=" + encodeURI($("#txtBeginDate").val()) + "&txtEndDate=" + encodeURI($("#txtEndDate").val()) + "&ddCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
            div_PrintDlg.show();
            var PrintDlg = null;
            div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL);
            PrintDlg = div_PrintDlg.window({
                title: '打印',
                href: "",
                modal: true,
                resizable: true,
                minimizable: false,
                collapsible: false,
                cache: false,
                closed: true,
                width: 900,
                height: 500
            });
            div_PrintDlg.window("open");

        } else {
            reWriteMessagerAlert("提示", "没有数据，不可打印", "error");
            return false;
        }

    });

    function Print_CheckWayBillList(wbID, wbStorageDate, wbCompany, wbSerialNum) {
        var PrintCheckWayBillListURL = "/Customer_SceneCheck/Print_CheckListSheet?strWbId=" + encodeURI(wbID) + "&wbStorageDate=" + encodeURI(wbStorageDate) + "&wbCompany=" + encodeURI(wbCompany) + "&wbSerialNum=" + encodeURI(wbSerialNum);
        var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
        div_PrintDlg.show();
        var PrintDlg = null;
        div_PrintDlg.find("#frmPrintURL").attr("src", PrintCheckWayBillListURL);
        PrintDlg = div_PrintDlg.window({
            title: '打印',
            href: "",
            modal: true,
            resizable: true,
            minimizable: false,
            collapsible: false,
            cache: false,
            closed: true,
            width: 900,
            height: 500
        });
        div_PrintDlg.window("open");
    }

    $("#btnExcel").click(function () {
        var browserType = "";
        if ($.browser.msie) {
            browserType = "msie";
        }
        else if ($.browser.safari) {
            browserType = "safari";
        }
        else if ($.browser.mozilla) {
            browserType = "mozilla";
        }
        else if ($.browser.opera) {
            browserType = "opera";
        }
        else {
            browserType = "unknown";
        }

        PrintURL = "/Customer_SceneCheck/Excel?txtBeginDate=" + encodeURI($("#txtBeginDate").val()) + "&txtEndDate=" + encodeURI($("#txtEndDate").val()) + "&ddCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            return false;
        }
    });

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'wbID',
        sortOrder: 'desc',
        remoteSort: true,
        border: false,
        idField: 'wbID',
        columns: [[
					{ field: 'wbStorageDate', title: '导入日期', width: 120, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'wbCompany', title: '货代公司', width: 120, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'wbSerialNum', title: '总运单号', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbTotalNumber_Customize', title: '子运单总件数', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbTotalWeight_Customize', title: '子运单总重量(公斤)', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbStatus', title: '状态', width: 80, sortable: true,
                        formatter: function (value, rec, index) {
                            var s = "";
                            var arr = value.split("$");
                            if (arr.length == 2) {
                                var needCheck = arr[0];
                                var status = arr[1];
                                if (needCheck == "0") {
                                    s = "<center><div style='background-color:#00CC66;width:80px'>" + needCheck + "</div></center>";
                                } else {
                                    s = "<center><div style='background-color:#FF9966;width:80px'>" + needCheck + "</div></center>";
                                }
                            }
                            return s;
                        }
                    },
                    { field: 'wbPrintPeronalWayBillList', title: '打印', width: 180, sortable: true,
                        formatter: function (value, rec, index) {
                            var s = "";
                            var arr = rec.wbStatus.split("$");
                            if (arr.length == 2) {
                                var needCheck = arr[0];
                                var status = arr[1];
                                if (needCheck == "0") {
                                    //s = "<center><div style='background-color:#00CC66;width:120px'>" + needCheck + "</div></center>";
                                } else {
                                    s = "<input type='button' style='width:100%' wbId='" + rec.wbID + "' wbStorageDate='" + rec.wbStorageDate + "' wbCompany='" + rec.wbCompany + "'  wbSerialNum='" + rec.wbSerialNum + "'  value='打印个人物品查验清单' class='cls_PrintPeronalWayBillList'/>";
                                }
                            }
                            return s;
                        }
                    }
				]],
        pagination: true,
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        toolbar: "#toolBar",
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuDetail" iconCls="icon-search"/>').html("查看子运单明细").appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnudetail":
                            Detail();
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
            //_$_datagrid.datagrid("reload");
        },
        onClickRow: function (rowIndex, rowData) {
            Detail(rowData.wbID, rowData.wbSerialNum);
        },
        onLoadSuccess: function (data) {
            var allPrintPeronalWayBillListBtn = $(".cls_PrintPeronalWayBillList");
            $.each(allPrintPeronalWayBillListBtn, function (i, item) {
                $(item).click(function () {
                    var wbID = $(item).attr("wbId");
                    var wbStorageDate = $(item).attr("wbStorageDate");
                    var wbCompany = $(item).attr("wbCompany");
                    var wbSerialNum = $(item).attr("wbSerialNum");
                    Print_CheckWayBillList(wbID, wbStorageDate, wbCompany, wbSerialNum);
                });
            });
        }
    });

    function Detail(id, wbSerialNum) {
        var div_DetailDlg = self.parent.$("#dlg_GlobalDetail");
        if (id) {
            DetailDlg = div_DetailDlg.dialog({
                buttons: [{
                    text: '关     闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        DetailDlg.dialog('close');
                    }
                }],
                title: '查     验',
                href: DetailURL + id + "&Detail_wbSerialNum=" + wbSerialNum,
                modal: true,
                resizable: true,
                cache: false,
                left: 0,
                top: 0,
                width: 830,
                height: 460,
                closed: true
            });
            _$_datagrid.datagrid("unselectAll");
        } else {
            var selects = _$_datagrid.datagrid("getSelections");
            if (selects.length != 1) {
                reWriteMessagerAlert("提示", "<center>请选择数据进行查看(<font style='color:red'>只可查看一行</font>)</center>", "error");
                return false;
            } else {
                id = selects[0].wbID;
                var wbSerialNum = selects[0].wbSerialNum;
                DetailDlg = div_DetailDlg.dialog({
                    buttons: [{
                        text: '关 闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            DetailDlg.dialog('close');
                        }
                    }],
                    title: '查     验',
                    href: DetailURL + id + "&Detail_wbSerialNum=" + wbSerialNum,
                    modal: true,
                    resizable: true,
                    cache: false,
                    left: 0,
                    top: 0,
                    width: 830,
                    height: 460,
                    closed: true
                });
                _$_datagrid.datagrid("unselectAll");
            }
        }

        div_DetailDlg.dialog("open");

        if (DetailDlg.find('#txtSubWayBillCode')) {
            DetailDlg.find('#txtSubWayBillCode').focus();
        }
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:150px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "wbserialNum":
                    break;
                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "wbSerialNum") {

                } else {
                    if (item.iconCls == 'icon-ok') {
                        _$_datagrid.datagrid('hideColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-empty'
                        });
                    } else {
                        _$_datagrid.datagrid('showColumn', $(item.text).attr("id"));
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
