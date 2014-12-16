$(function () {
    var _$_datagrid = $("#DG_InOutStoreDetail");
    var _$_ddlInOutStoreType = $('#ddlInOutStoreType');

    _$_ddlInOutStoreType.combobox({
        data: [{ "text": "---请选择---", "id": "---请选择---" }, { "text": "正常已入库", "id": "1" }, { "text": "正常已出库", "id": "3"}],
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    switch ($("#hid_InOutStoreType").val()) {
        case "1":
            _$_ddlInOutStoreType.combobox("setValue", "1");
            break;
        case "3":
            _$_ddlInOutStoreType.combobox("setValue", "3");
            break;
        default:
            _$_ddlInOutStoreType.combobox("setValue", "---请选择---");
    }

    var PrintURL = "";
    var QueryURL = "/ViewInOutStoreWayBillDetail/GetData?Detail_wbSerialNum=" + encodeURI($("#txtCode_InOutStoreDetail").val()) + "&Detail_InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combobox("getValue")) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_InOutStoreDetail").val());

    $("#btnInOutStoreDetailQuery").click(function () {
        QueryURL = "/ViewInOutStoreWayBillDetail/GetData?Detail_wbSerialNum=" + encodeURI($("#txtCode_InOutStoreDetail").val()) + "&Detail_InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combobox("getValue")) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_InOutStoreDetail").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#btnPrint_InOutStoreDetaildlg").click(function () {
        PrintURL = "/ViewInOutStoreWayBillDetail/Print?Detail_wbSerialNum=" + encodeURI($("#txtCode_InOutStoreDetail").val()) + "&Detail_InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combobox("getValue")) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_InOutStoreDetail").val()) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
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


    $("#btnExcel_InOutStoreDetaildlg").click(function () {
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

        PrintURL = "/ViewInOutStoreWayBillDetail/Excel?Detail_wbSerialNum=" + encodeURI($("#txtCode_InOutStoreDetail").val()) + "&Detail_InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combobox("getValue")) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_InOutStoreDetail").val()) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
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
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'operateDate',
        sortOrder: 'desc',
        remoteSort: true,
        border: false,
        idField: 'operateDate',
        columns: [[
                    { field: 'operateDate', title: '出入库日期', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbSerialNum', title: '总运单号', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbSerialNum', title: '分运单号', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
					{ field: 'wbCompany', title: '货代公司', width: 100, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'swbDescription_CHN', title: '货物中文名', width: 160, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'swbDescription_ENG', title: '货物英文名', width: 160, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbNumber', title: '件数', width: 50, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbWeight', title: '重量(公斤)', width: 70, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
					{ field: 'StatusDecription', title: '出入库类型', width: 90, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'Wbf_swbID', title: '主键', hidden: true, width: 120, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					}
				]],
        pagination: true,
        toolbar: "#toolBarInOutStoreDetail",
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
        }
    });

//    //设置分页控件   
//    var p = _$_datagrid.datagrid('getPager');
//    $(p).pagination({
//        pageSize: 15,
//        pageList: [10, 15, 20, 25, 30]
//    });

//    $("#btnInOutStoreDetailQuery").click();

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:150px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "swbserialnum":
                    break;
                case "wbf_swbid":
                    break;
                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "Wbf_swbID") {

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
