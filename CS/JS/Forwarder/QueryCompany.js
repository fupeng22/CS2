﻿$(function () {
    var _$_datagrid = $("#DG_WayBillResult");
    var _$_ddCompany = $('#txtVoyage');
    var QueryCompanyURL = "/ForwarderMain/LoadComboxJSON_Forwarder";
    var LoadCurrentCompany = "/ForwarderMain/GetCurrentCompany_Forwarder";

    var DetailURL = "/ViewSubWayBill/Index?Detail_wbSerialNum=";

    _$_ddCompany.combobox({
        url: QueryCompanyURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    $.ajax({
        type: "POST",
        url: LoadCurrentCompany,
        data: "",
        async: false,
        cache: false,
        beforeSend: function (XMLHttpRequest) {

        },
        success: function (msg) {
            //_$_ddCompany.combobox("setValue", "---请选择---");
            _$_ddCompany.combobox("setValue", msg);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function () {

        }
    });

    var PrintURL = "";

    //var QueryURL = "/Forwarder_QueryCompany/GetData?txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&hidSearchType=" + encodeURI($("#hidSearchType").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtGSubWayBillCode=" + encodeURI($("#txtGSubWayBillCode").val());
    var QueryURL = "/Forwarder_QueryCompany/GetData?txtCode=" + encodeURI($("#txtCode").val()) + "&inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&hidSearchType=" + encodeURI($("#hidSearchType").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue"));

    $("#btnQuery").click(function () {
        if ($("#txtCode").val() == "") {
            reWriteMessagerAlert("操作提示", "请输入总运单号", "error");
        }
        else {
            $("#hidSearchType").val(1);
            QueryURL = "/Forwarder_QueryCompany/GetData?txtCode=" + encodeURI($("#txtCode").val()) + "&inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&hidSearchType=" + encodeURI($("#hidSearchType").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue"));
            window.setTimeout(function () {
                $.extend(_$_datagrid.datagrid("options"), {
                    url: QueryURL
                });
                _$_datagrid.datagrid("reload");
            }, 10); //延迟100毫秒执行，时间可以更短
        }
    });

    $("#GbtnQuery").click(function () {
        $("#hidSearchType").val(0);
        QueryURL = "/Forwarder_QueryCompany/GetData?txtCode=" + encodeURI($("#txtCode").val()) + "&inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&hidSearchType=" + encodeURI($("#hidSearchType").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue"));
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#btnReset").click(function () {
        $("#txtCode").val("");
        $("#btnQuery").click();
    });

    $("#GbtnReset").click(function () {
        $("#inputBeginDate").val("");
        $("#inputEndDate").val("");
        $("#txtGCode").val("");
        _$_ddCompany.combobox("setValue", "---请选择---");
        $("#GbtnQuery").click();
    });

    $("#btnPrint").click(function () {
        PrintURL = "/Forwarder_QueryCompany/Print?txtCode=" + encodeURI($("#txtCode").val()) + "&inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&hidSearchType=" + encodeURI($("#hidSearchType").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
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

        PrintURL = "/Forwarder_QueryCompany/Excel?txtCode=" + encodeURI($("#txtCode").val()) + "&inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&hidSearchType=" + encodeURI($("#hidSearchType").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
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
        sortName: 'wbStorageDate',
        sortOrder: 'desc',
        remoteSort: true,
        border: false,
        idField: 'wbStorageDate',
        columns: [[
					{ field: 'wbStorageDate', title: '报关日期', width: 70, sortable: true,
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
                    { field: 'wbTotalNumber_Customize', title: '分运单总件数', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbTotalWeight_Customize', title: '分运单总重量', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbStatus', title: '预检状态', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbInStoreType_Customize', title: '入库状态', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'releseNum', title: '放行件数', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'notReleseNum', title: '扣留件数', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: true,
        toolbar: "#toolBar",
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuViewSubWayBillDetail" iconCls="icon-infomation"/>').html("查看子运单明细").appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnuviewsubwaybilldetail":
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
        onLoadSuccess: function (data) {
            if (data.rows != undefined) {
                var col = [
                   'wbSerialNum'
                  ];
                row_merge(col, 'DG_WayBillResult', data.rows);
            }
        },
        onClickRow: function (rowIndex, rowData) {
            Detail(rowData.wbSerialNum);
        }
    });

    //设置分页控件   
    //    var p = _$_datagrid.datagrid('getPager');
    //    $(p).pagination({
    //        pageSize: 15,
    //        pageList: [10, 15, 20, 25, 30]
    //    });

    function Detail(wbSerialNum) {
        //        var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
        //        div_PrintDlg.show();
        //        var PrintDlg = null;
        //        div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL);
        //        PrintDlg = div_PrintDlg.window({
        //            title: '打印',
        //            href: "",
        //            modal: true,
        //            resizable: true,
        //            minimizable: false,
        //            collapsible: false,
        //            cache: false,
        //            closed: true,
        //            width: 900,
        //            height: 500
        //        });

        //        div_PrintDlg.window("open");

        var div_DetailDlg = self.parent.$("#dlg_GlobalDetail");
        if (wbSerialNum) {
            DetailDlg = div_DetailDlg.dialog({
                buttons: [{
                    text: '关 闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        DetailDlg.dialog('close');
                    }
                }],
                title: '查看子运单明细',
                href: DetailURL + wbSerialNum,
                modal: true,
                resizable: true,
                cache: false,
                left: 0,
                top: 0,
                width: 1020,
                height: 480,
                closed: true
            });
            _$_datagrid.datagrid("unselectAll");
        } else {
            var selects = _$_datagrid.datagrid("getSelections");
            if (selects.length != 1) {
                reWriteMessagerAlert("提示", "<center>请选择数据进行查看(<font style='color:red'>只可查看一行</font>)</center>", "error");
                return false;
            } else {
                var wbSerialNum = selects[0].wbSerialNum;
                DetailDlg = div_DetailDlg.dialog({
                    buttons: [{
                        text: '关 闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            DetailDlg.dialog('close');
                        }
                    }],
                    title: '查看子运单明细',
                    href: DetailURL + wbSerialNum,
                    modal: true,
                    resizable: true,
                    cache: false,
                    left: 0,
                    top: 0,
                    width: 1020,
                    height: 480,
                    closed: true
                });
                _$_datagrid.datagrid("unselectAll");
            }
        }

        div_DetailDlg.dialog("open");
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:120px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "wbserialnum":
                    break;
                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "wbStorageDate") {

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
