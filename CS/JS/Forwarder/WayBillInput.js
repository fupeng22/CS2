var settime = null;
function redraw() {
    self.parent.$("#layout_Main").layout('resize');
    $('#mainLayout').layout('panel', 'north').panel('resize', { width: $('#mainLayout').width() });
    $('#mainLayout').layout('panel', 'center').panel('resize', { width: $('#mainLayout').width() });
    //$('#mainLayout').layout('resize');
    //self.parent.$("#layout_Main").layout('resize');
}

$(function () {
    $(window).resize(function () {
        if (settime != null)
            clearTimeout(settime);
        settime = setTimeout("redraw()", 100);
    });

    $('#mainLayout').layout('panel', 'north').panel({
        onCollapse: function () {
            if (settime != null)
                clearTimeout(settime);
            settime = setTimeout("redraw()", 100);
        },
        onExpand: function () {
            if (settime != null)
                clearTimeout(settime);
            settime = setTimeout("redraw()", 100);
        },
        onResize: function (width, height) {
            if (settime != null)
                clearTimeout(settime);
            settime = setTimeout("redraw()", 100);
        }
    });

    $('#mainLayout').layout('panel', 'center').panel({
        onCollapse: function () {
            if (settime != null)
                clearTimeout(settime);
            settime = setTimeout("redraw()", 100);
        },
        onExpand: function () {
            if (settime != null)
                clearTimeout(settime);
            settime = setTimeout("redraw()", 100);
        },
        onResize: function (width, height) {
            if (settime != null)
                clearTimeout(settime);
            settime = setTimeout("redraw()", 100);
        }
    });


    var _$_datagrid = $("#DG_WayBillResult");
    var _$_datagrid_sub = $("#DG_SubWayBillResult");
    var _$_ddCompany = $('#txtVoyage');
    var _$_txtSwbDescription_CHN_Detail = $('#txtSwbDescription_CHN_Detail');
    var _$_txtTaxRate_Detail = $('#txtTaxRate_Detail');
    var _$_txtSwbCustomsCategory_Main = $("#txtSwbCustomsCategory_Main");

    var QueryCompanyURL = "/ForwarderMain/LoadComboxJSON_Forwarder";
    var QueryCargoNameURL = "/TaxRateSetting/LoadComboxJSON_CargoNameByTaxNO?type=0&TaxNO=" + encodeURI($("#txtTaxNo_Detail").val());
    var QueryTaxRateURL = "/TaxRateSetting/LoadComboxJSON_CargoNameByTaxNO?type=1&TaxNO=" + encodeURI($("#txtTaxNo_Detail").val());
    var QueryCustomCategoryURL = "/ForwarderMain/LoadAllCustomCategory";
    var LoadCurrentCompany = "/ForwarderMain/GetCurrentCompany_Forwarder";

    var DetailURL = "/ViewSubWayBill/Index?Detail_wbSerialNum=";

    var CreateWayBillDlg = null;
    var CreateWayBillURL = "/Forwarder_WayBillInput/CreateWayBill?txtwbSerialNum=";
    var UpdateWayBillURL = "/Forwarder_WayBillInput/UpdateWayBill?txtwbSerialNum=";
    var DeleWayBillURL = "/Forwarder_WayBillInput/DeleWayBill?ids=";

    var CreateSubWayBillMainDlg = null;
    var CreateSubWayBillMainURL = "/Forwarder_WayBillInput/CreateSubWayBillMain?hid_wbId_Sele=";
    var UpdateSubWayBillMainURL = "/Forwarder_WayBillInput/UpdateSubWayBillMain?hid_swbId_Main=";
    var DeleSubWayBillMainURL = "/Forwarder_WayBillInput/DeleSubWayBillMain?ids=";

    var CreateSubWayBillDetailDlg = null;
    var CreateSubWayBillDetailURL = "/Forwarder_WayBillInput/CreateSubWayBillDetail?hid_swbId_Detail=";
    var UpdateSubWayBillDetailURL = "/Forwarder_WayBillInput/UpdateSubWayBillDetail?hid_swbId_Detail=";
    var DeleSubWayBillDetailURL = "/Forwarder_WayBillInput/DeleSubWayBillDetail?ids=";

    _$_ddCompany.combobox({
        url: QueryCompanyURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    _$_txtSwbDescription_CHN_Detail.combobox({
        url: QueryCargoNameURL,
        valueField: 'id',
        textField: 'text',
        editable: true,
        panelHeight: null
        //        onHidePanel: function () {
        //            alert("sdf");
        //        }
    });

    _$_txtTaxRate_Detail.combobox({
        url: QueryTaxRateURL,
        valueField: 'id',
        textField: 'text',
        editable: true,
        panelHeight: null
    });

    _$_txtSwbCustomsCategory_Main.combobox({
        url: QueryCustomCategoryURL,
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
            _$_ddCompany.combobox("setValue", msg);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function () {

        }
    });

    var PrintURL = "";

    var QueryURL = "/Forwarder_WayBillInput/GetData?inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue"));

    $("#GbtnQuery").click(function () {
        QueryURL = "/Forwarder_WayBillInput/GetData?inputBeginDate=" + encodeURI($("#inputBeginDate").val()) + "&inputEndDate=" + encodeURI($("#inputEndDate").val()) + "&txtGCode=" + encodeURI($("#txtGCode").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue"));
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#btnSeleAll_WayBill").click(function () {
        SeleAll();
    });

    $("#btnInverseSele_WayBill").click(function () {
        InverseSele();
    });

    $("#btnAdd_WayBill").click(function () {
        CreateWayBill();
    });

    $("#btnUpdate_WayBill").click(function () {
        var selects = _$_datagrid.datagrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert("提示", "<center>请选择数据进行修改(<font style='color:red'>只可选择一行</font>)</center>", "error");
            return false;
        } else {
            UpdateWayBill(selects[0].wbSerialNum, selects[0].wbVoyage, selects[0].wbIOmark, selects[0].wbChinese, selects[0].wbEnglish, selects[0].wbTransportMode, selects[0].wbEntryDate, selects[0].wbSRport, selects[0].wbPortCode, selects[0].wbTotalWeight.substring(0, selects[0].wbTotalWeight.length - 2), selects[0].wbTotalNumber, selects[0].wbSubNumber, selects[0].wbID);
        }
    });

    $("#btnDele_WayBill").click(function () {
        DeleWayBill();
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
        idField: 'wbSerialNum',
        columns: [[
                    { field: 'cb', checkbox: true },
					{ field: 'wbSerialNum', title: '总运单号', width: 100, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'wbCompany', title: '货代公司', width: 80, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'wbStorageDate', title: '报关日期', width: 70, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbVoyage', title: '运输工具航次', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbIOmark', title: '舱单进出口标志', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbChinese', title: '中文运输工具', width: 90, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbEnglish', title: '英文运输工具', width: 90, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbTransportMode', title: '运输方式', width: 70, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbEntryDate', title: '进港日期', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbSRport', title: '起运港/抵运地', width: 90, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbPortCode', title: '进出口岸代码', width: 90, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbTotalWeight', title: '总重量', width: 70, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbTotalNumber', title: '总件数', width: 60, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbSubNumber', title: '分运单数', width: 60, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: true,
        toolbar: "#toolBar_MainWayBill",
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
        onClickRow: function (rowIndex, rowData) {
            $("#txtWbSerialNum_Sub").val(rowData.wbSerialNum);
            $("#hid_wbId_Sele").val(rowData.wbID);
            $("#btnQuerySubWayBill").click();
        },
        onDblClickRow: function (rowIndex, rowData) {
            UpdateWayBill(rowData.wbSerialNum, rowData.wbVoyage, rowData.wbIOmark, rowData.wbChinese, rowData.wbEnglish, rowData.wbTransportMode, rowData.wbEntryDate, rowData.wbSRport, rowData.wbPortCode, rowData.wbTotalWeight.substring(0, rowData.wbTotalWeight.length - 2), rowData.wbTotalNumber, rowData.wbSubNumber, rowData.wbID);
        }
    });

    var QueryURL_Sub = "/Forwarder_WayBillInput/GetData_Sub?txtWbSerialNum_Sub=" + encodeURI($("#txtWbSerialNum_Sub").val()) + "&txtSwbSerialNum_Sub=" + encodeURI($("#txtSwbSerialNum_Sub").val());

    $("#btnQuerySubWayBill").click(function () {
        QueryURL_Sub = "/Forwarder_WayBillInput/GetData_Sub?txtWbSerialNum_Sub=" + encodeURI($("#txtWbSerialNum_Sub").val()) + "&txtSwbSerialNum_Sub=" + encodeURI($("#txtSwbSerialNum_Sub").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid_sub.treegrid("options"), {
                url: QueryURL_Sub
            });
            _$_datagrid_sub.treegrid("reload");
        }, 100); //延迟100毫秒执行，时间可以更短
    });

    $("#btnSeleAll_SubWayBill").click(function () {
        SeleAll_Sub();
    });

    $("#btnInverseSele_SubWayBill").click(function () {
        InverseSele_Sub();
    });

    $("#btnAdd_SubWayBill_Main").click(function () {
        CreateSubWayBill_Main();
    });

    $("#btnUpdate_SubWayBill_Main").click(function () {
        var selects = _$_datagrid_sub.treegrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert("提示", "<center>请选择数据进行修改(<font style='color:red'>只可选择一行</font>)</center>", "error");
            return false;
        } else {
            if (selects[0].parentID == "top") {
                UpdateSubWayBillMain(selects[0].ID, selects[0].swbSerialNum, selects[0].swbRecipients, selects[0].swbCustomsCategory, selects[0].Sender, selects[0].ReceiverIDCard, selects[0].ReceiverPhone, selects[0].EmailAddress);
            } else {
                reWriteMessagerAlert("提示", "<center>请选择主运单头方可进行修改</center>", "error");
                return false;
            }
        }
    });

    $("#btnDelete_SubWayBill_Main").click(function () {
        DeleSubWayBillMain();
    });

    $("#btnAdd_SubWayBill_Detail").click(function () {
        CreateSubWayBill_Detail();
    });

    $("#btnUpdate_SubWayBill_Detail").click(function () {
        var selects = _$_datagrid_sub.treegrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert("提示", "<center>请选择数据进行修改(<font style='color:red'>只可选择一行</font>)</center>", "error");
            return false;
        } else {
            if (selects[0].parentID != "top") {
                UpdateSubWayBillDetail(selects[0].swbSerialNum, selects[0].ID, selects[0].parentID, selects[0].swbDescription_CHN, selects[0].swbDescription_ENG, selects[0].swbNumber, selects[0].swbWeight.substring(0, selects[0].swbWeight.length - 2), selects[0].swbValue, selects[0].TaxNo, selects[0].TaxRate, selects[0].swbMonetary);
            } else {
                reWriteMessagerAlert("提示", "<center>请选择分运单明细方可进行修改</center>", "error");
                return false;
            }
        }
    });

    $("#btnDelete_SubWayBill_Detail").click(function () {
        DeleSubWayBillDetail();
    });

    $("#txtTaxNo_Detail").blur(function () {
        QueryCargoNameURL = "/TaxRateSetting/LoadComboxJSON_CargoNameByTaxNO?type=0&TaxNO=" + encodeURI($("#txtTaxNo_Detail").val());
        _$_txtSwbDescription_CHN_Detail.combobox({
            url: QueryCargoNameURL,
            valueField: 'id',
            textField: 'text',
            editable: true,
            panelHeight: null
        });
        QueryTaxRateURL = "/TaxRateSetting/LoadComboxJSON_CargoNameByTaxNO?type=1&TaxNO=" + encodeURI($("#txtTaxNo_Detail").val());
        _$_txtTaxRate_Detail.combobox({
            url: QueryTaxRateURL,
            valueField: 'id',
            textField: 'text',
            editable: true,
            panelHeight: null
        });

    });

    _$_datagrid_sub.treegrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL_Sub,
        sortName: 'swbSerialNum',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        singleSelect: false,
        idField: 'ID',
        treeField: 'swbSerialNum',
        columns: [[
                    { field: 'cb1', checkbox: true },
					{ field: 'swbSerialNum', title: '分运单号', width: 180, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    },
					    formatter: function (value, rowData, rowIndex) {
					        var sRet = "";
					        if (rowData.parentID == "top") {
					            sRet = rowData.swbSerialNum;
					        } else {
					            sRet = "";
					        }
					        return sRet;
					    }
					},
					{ field: 'swbDescription_CHN', title: '中文货物名称', width: 90, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'swbDescription_ENG', title: '英文货物名称', width: 90, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbNumber', title: '件数', width: 50, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbWeight', title: '重量', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbValue', title: '价值', width: 70, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'TaxNo', title: '税号', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'TaxRateDescription', title: '税率', width: 50, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbMonetary', title: '币制', width: 50, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'Sender', title: '发件人', width: 60, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'ReceiverIDCard', title: '收件人身份证号', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'ReceiverPhone', title: '联系方式', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'EmailAddress', title: '邮件地址', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbRecipients', title: '收件人地址', width: 300, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbCustomsCategory', title: '报关类别', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: true,
        toolbar: "#toolBar_SubWayBill",
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
        onClickRow: function (rowIndex, rowData) {
            //Detail(rowData.wbSerialNum);
        },
        onDblClickRow: function (rowData) {
            if (rowData.parentID == "top") {
                UpdateSubWayBillMain(rowData.ID, rowData.swbSerialNum, rowData.swbRecipients, rowData.swbCustomsCategory, rowData.Sender, rowData.ReceiverIDCard, rowData.ReceiverPhone, rowData.EmailAddress);
            } else {
                UpdateSubWayBillDetail(rowData.swbSerialNum, rowData.ID, rowData.parentID, rowData.swbDescription_CHN, rowData.swbDescription_ENG, rowData.swbNumber, rowData.swbWeight.substring(0, rowData.swbWeight.length - 2), rowData.swbValue, rowData.TaxNo, rowData.TaxRate, rowData.swbMonetary);
            }
        },
        onLoadSuccess: function (row, data) {
            _$_datagrid_sub.treegrid("expandAll");
            delete _$_datagrid_sub.treegrid('options').queryParams['id'];
        }
    });

    $("#btnFullScreen_SubWayBill").click(function () {
        self.parent.$("#layout_Main").layout("collapse", "north");
        self.parent.$("#layout_Main").layout("collapse", "west");
        $("#mainLayout").layout("collapse", "north");
    });

    $("#btnResetScreen_SubWayBill").click(function () {
        self.parent.$("#layout_Main").layout("expand", "north");
        self.parent.$("#layout_Main").layout("expand", "west");
        $("#mainLayout").layout("expand", "north");
    });

    $("#btnFullScreen_WayBill").click(function () {
        self.parent.$("#layout_Main").layout("collapse", "north");
        self.parent.$("#layout_Main").layout("collapse", "west");
        $("#mainLayout").layout("collapse", "center");
    });

    $("#btnResetScreen_WayBill").click(function () {
        self.parent.$("#layout_Main").layout("expand", "north");
        self.parent.$("#layout_Main").layout("expand", "west");
        $("#mainLayout").layout("expand", "center");
    });

    function CreateWayBill() {
        $("#txtwbSerialNum").val("");
        $("#txtwbVoyage").val("");
        $("#txtwbIOmark").val("");
        $("#txtwbChinese").val("");
        $("#txtwbEnglish").val("");
        $("#txtwbTransportMode").val("");
        $("#txtwbEntryDate").val("");
        $("#txtwbSRport").val("");
        $("#txtwbPortCode").val("");
        $("#txtwbTotalWeight").val("");
        $("#txtwbTotalNumber").val("");
        $("#txtwbSubNumber").val("");
        CreateWayBillDlg = $('#span_WayBill').dialog({
            buttons: [{
                text: '保  存',
                iconCls: 'icon-ok',
                handler: function () {
                    var txtwbSerialNum = $("#txtwbSerialNum").val();
                    var txtwbVoyage = $("#txtwbVoyage").val();
                    var txtwbIOmark = $("#txtwbIOmark").val();
                    var txtwbChinese = $("#txtwbChinese").val();
                    var txtwbEnglish = $("#txtwbEnglish").val();
                    var txtwbTransportMode = $("#txtwbTransportMode").val();
                    var txtwbEntryDate = $("#txtwbEntryDate").val();
                    var txtwbSRport = $("#txtwbSRport").val();
                    var txtwbPortCode = $("#txtwbPortCode").val();
                    var txtwbTotalWeight = $("#txtwbTotalWeight").val();
                    var txtwbTotalNumber = $("#txtwbTotalNumber").val();
                    var txtwbSubNumber = $("#txtwbSubNumber").val();

                    if (txtwbSerialNum == "" || txtwbTotalWeight == "" || txtwbTotalNumber == "" || txtwbSubNumber == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(总运单号、总重量、总件数、分运单数)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: CreateWayBillURL + encodeURI(txtwbSerialNum) + "&txtwbVoyage=" + encodeURI(txtwbVoyage) + "&txtwbIOmark=" + encodeURI(txtwbIOmark) + "&txtwbChinese=" + encodeURI(txtwbChinese) + "&txtwbEnglish=" + encodeURI(txtwbEnglish) + "&txtwbTransportMode=" + encodeURI(txtwbTransportMode) + "&txtwbEntryDate=" + encodeURI(txtwbEntryDate) + "&txtwbSRport=" + encodeURI(txtwbSRport) + "&txtwbPortCode=" + encodeURI(txtwbPortCode) + "&txtwbTotalWeight=" + encodeURI(txtwbTotalWeight) + "&txtwbTotalNumber=" + encodeURI(txtwbTotalNumber) + "&txtwbSubNumber=" + encodeURI(txtwbSubNumber),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                bOK = true;
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
                        CreateWayBillDlg.dialog('close');
                        _$_datagrid.datagrid("reload");
                        _$_datagrid.datagrid("unselectAll");
                    }
                }
            }, {
                text: '关  闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateWayBillDlg.dialog('close');
                }
            }],
            title: '添加总运单',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 770,
            height: 200
        });

        $('#span_WayBill').dialog("open");
    }

    function UpdateWayBill(txtwbSerialNum, txtwbVoyage, txtwbIOmark, txtwbChinese, txtwbEnglish, txtwbTransportMode, txtwbEntryDate, txtwbSRport, txtwbPortCode, txtwbTotalWeight, txtwbTotalNumber, txtwbSubNumber, wbId) {
        $("#txtwbSerialNum").val(txtwbSerialNum);
        $("#txtwbVoyage").val(txtwbVoyage);
        $("#txtwbIOmark").val(txtwbIOmark);
        $("#txtwbChinese").val(txtwbChinese);
        $("#txtwbEnglish").val(txtwbEnglish);
        $("#txtwbTransportMode").val(txtwbTransportMode);
        $("#txtwbEntryDate").val(txtwbEntryDate);
        $("#txtwbSRport").val(txtwbSRport);
        $("#txtwbPortCode").val(txtwbPortCode);
        $("#txtwbTotalWeight").val(txtwbTotalWeight);
        $("#txtwbTotalNumber").val(txtwbTotalNumber);
        $("#txtwbSubNumber").val(txtwbSubNumber);
        $("#hid_wbId").val(wbId);
        CreateWayBillDlg = $('#span_WayBill').dialog({
            buttons: [{
                text: '保  存',
                iconCls: 'icon-ok',
                handler: function () {
                    var txtwbSerialNum = $("#txtwbSerialNum").val();
                    var txtwbVoyage = $("#txtwbVoyage").val();
                    var txtwbIOmark = $("#txtwbIOmark").val();
                    var txtwbChinese = $("#txtwbChinese").val();
                    var txtwbEnglish = $("#txtwbEnglish").val();
                    var txtwbTransportMode = $("#txtwbTransportMode").val();
                    var txtwbEntryDate = $("#txtwbEntryDate").val();
                    var txtwbSRport = $("#txtwbSRport").val();
                    var txtwbPortCode = $("#txtwbPortCode").val();
                    var txtwbTotalWeight = $("#txtwbTotalWeight").val();
                    var txtwbTotalNumber = $("#txtwbTotalNumber").val();
                    var txtwbSubNumber = $("#txtwbSubNumber").val();
                    var txtwbId = $("#hid_wbId").val();

                    if (txtwbId == "" || txtwbSerialNum == "" || txtwbTotalWeight == "" || txtwbTotalNumber == "" || txtwbSubNumber == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(总运单号、总重量、总件数、分运单数)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateWayBillURL + encodeURI(txtwbSerialNum) + "&txtwbVoyage=" + encodeURI(txtwbVoyage) + "&txtwbIOmark=" + encodeURI(txtwbIOmark) + "&txtwbChinese=" + encodeURI(txtwbChinese) + "&txtwbEnglish=" + encodeURI(txtwbEnglish) + "&txtwbTransportMode=" + encodeURI(txtwbTransportMode) + "&txtwbEntryDate=" + encodeURI(txtwbEntryDate) + "&txtwbSRport=" + encodeURI(txtwbSRport) + "&txtwbPortCode=" + encodeURI(txtwbPortCode) + "&txtwbTotalWeight=" + encodeURI(txtwbTotalWeight) + "&txtwbTotalNumber=" + encodeURI(txtwbTotalNumber) + "&txtwbSubNumber=" + encodeURI(txtwbSubNumber) + "&txtwbId=" + encodeURI(txtwbId),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                bOK = true;
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
                        CreateWayBillDlg.dialog('close');
                        _$_datagrid.datagrid("reload");
                        _$_datagrid.datagrid("unselectAll");
                    }
                }
            }, {
                text: '关  闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateWayBillDlg.dialog('close');
                }
            }],
            title: '修改总运单',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 770,
            height: 200
        });

        $('#span_WayBill').dialog("open");
    }

    function DeleWayBill() {
        reWriteMessagerConfirm("提示", "您确定需要删除所选的总运单信息吗？",
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].wbID);
                            }
                            if (selects.length == 0) {
                                reWriteMessagerAlert("提示", "<center>请选择需要删除的数据</center>", "error");
                                return false;
                            }
                            $.ajax({
                                type: "POST",
                                url: DeleWayBillURL + encodeURI(ids.join(",")),
                                data: "",
                                async: false,
                                cache: false,
                                beforeSend: function (XMLHttpRequest) {

                                },
                                success: function (msg) {
                                    var JSONMsg = eval("(" + msg + ")");
                                    if (JSONMsg.result.toLowerCase() == 'ok') {
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                    } else {
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                        return;
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });
                            _$_datagrid.datagrid("reload");
                            _$_datagrid.datagrid("unselectAll");
                        } else {

                        }
                    }
                );
    }

    function CreateSubWayBill_Main() {
        $("#txtSwbSerialNum_Main").val("");
        $("#txtSwbRecipients").val("");
        //$("#txtSwbCustomsCategory_Main").val("");
        $("#txtSwbCustomsCategory_Main").combobox("setValue", "");
        $("#txtSender").val("");
        $("#txtReceiverIDCard").val("");
        $("#txtReceiverPhone").val("");
        $("#txtEmailAddress").val("");

        if ($("#hid_wbId_Sele").val() == "") {
            reWriteMessagerAlert('操作提示', '请先选择需要添加分运单的主运单', "error");
            return false;
        }

        CreateSubWayBillMainDlg = $('#span_SubWayBill_Main').dialog({
            buttons: [{
                text: '保存并继续添加',
                iconCls: 'icon-ok',
                handler: function () {
                    var txtSwbSerialNum_Main = $("#txtSwbSerialNum_Main").val();
                    var txtSwbRecipients = $("#txtSwbRecipients").val();
                    var txtSwbCustomsCategory_Main = $("#txtSwbCustomsCategory_Main").combobox("getValue"); //$("#txtSwbCustomsCategory_Main").combobox("textbox").val(); // $("#txtSwbCustomsCategory_Main").val();
                    var txtSender = $("#txtSender").val();
                    var txtReceiverIDCard = $("#txtReceiverIDCard").val();
                    var txtReceiverPhone = $("#txtReceiverPhone").val();
                    var txtEmailAddress = $("#txtEmailAddress").val();
                    var hid_wbId_Sele = $("#hid_wbId_Sele").val();

                    if (txtSwbSerialNum_Main == "" || hid_wbId_Sele == "" || txtSwbCustomsCategory_Main == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(分运单号,报关类别)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: CreateSubWayBillMainURL + encodeURI(hid_wbId_Sele) + "&txtSwbSerialNum_Main=" + encodeURI(txtSwbSerialNum_Main) + "&txtSwbRecipients=" + encodeURI(txtSwbRecipients) + "&txtSwbCustomsCategory_Main=" + encodeURI(txtSwbCustomsCategory_Main) + "&txtSender=" + encodeURI(txtSender) + "&txtReceiverIDCard=" + encodeURI(txtReceiverIDCard) + "&txtReceiverPhone=" + encodeURI(txtReceiverPhone) + "&txtEmailAddress=" + encodeURI(txtEmailAddress),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                bOK = true;
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
                        //CreateSubWayBillMainDlg.dialog('close');
                        $("#btnQuerySubWayBill").click();
                    }
                }
            }, {
                text: '关  闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateSubWayBillMainDlg.dialog('close');
                }
            }],
            title: '添加分运单头',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 350,
            height: 300
        });

        $('#span_SubWayBill_Main').dialog("open");
    }

    function UpdateSubWayBillMain(ID, swbSerialNum, swbRecipients, swbCustomsCategory, Sender, ReceiverIDCard, ReceiverPhone, EmailAddress) {
        $("#hid_swbId_Main").val(ID);
        $("#txtSwbSerialNum_Main").val(swbSerialNum);
        $("#txtSwbRecipients").val(swbRecipients);
        //$("#txtSwbCustomsCategory_Main").val(swbCustomsCategory);
        $("#txtSwbCustomsCategory_Main").combobox("setValue", swbCustomsCategory);
        $("#txtSender").val(Sender);
        $("#txtReceiverIDCard").val(ReceiverIDCard);
        $("#txtReceiverPhone").val(ReceiverPhone);
        $("#txtEmailAddress").val(EmailAddress);

        CreateSubWayBillMainDlg = $('#span_SubWayBill_Main').dialog({
            buttons: [{
                text: '保  存',
                iconCls: 'icon-ok',
                handler: function () {
                    var hid_swbId_Main = $("#hid_swbId_Main").val();
                    var txtSwbSerialNum_Main = $("#txtSwbSerialNum_Main").val();
                    var txtSwbRecipients = $("#txtSwbRecipients").val();
                    var txtSwbCustomsCategory_Main = $("#txtSwbCustomsCategory_Main").combobox("getValue"); // $("#txtSwbCustomsCategory_Main").combobox("textbox").val(); // $("#txtSwbCustomsCategory_Main").val();
                    var txtSender = $("#txtSender").val();
                    var txtReceiverIDCard = $("#txtReceiverIDCard").val();
                    var txtReceiverPhone = $("#txtReceiverPhone").val();
                    var txtEmailAddress = $("#txtEmailAddress").val();
                    var hid_wbId_Sele = $("#hid_wbId_Sele").val();

                    if (hid_swbId_Main == "" || txtSwbSerialNum_Main == "" || txtSwbCustomsCategory_Main == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(分运单号、报关类别)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateSubWayBillMainURL + encodeURI(hid_swbId_Main) + "&hid_wbId_Sele=" + encodeURI(hid_wbId_Sele) + "&txtSwbSerialNum_Main=" + encodeURI(txtSwbSerialNum_Main) + "&txtSwbRecipients=" + encodeURI(txtSwbRecipients) + "&txtSwbCustomsCategory_Main=" + encodeURI(txtSwbCustomsCategory_Main) + "&txtSender=" + encodeURI(txtSender) + "&txtReceiverIDCard=" + encodeURI(txtReceiverIDCard) + "&txtReceiverPhone=" + encodeURI(txtReceiverPhone) + "&txtEmailAddress=" + encodeURI(txtEmailAddress),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                bOK = true;
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
                        CreateSubWayBillMainDlg.dialog('close');
                        $("#btnQuerySubWayBill").click();
                    }
                }
            }, {
                text: '关  闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateSubWayBillMainDlg.dialog('close');
                }
            }],
            title: '修改分运单头',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 350,
            height: 300
        });

        $('#span_SubWayBill_Main').dialog("open");
    }

    function DeleSubWayBillMain() {
        reWriteMessagerConfirm("提示", "您确定需要删除所选的分运单头信息吗？(<font style='color:red;font-weight:bold'>删除时将连同其下分运单明细一并删除</font>)",
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid_sub.treegrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                if (selects[i].parentID == "top") {
                                    ids.push(selects[i].ID);
                                }
                            }
                            if (ids.length == 0) {
                                reWriteMessagerAlert("提示", "<center>请选择需要删除的数据</center>", "error");
                                return false;
                            }
                            $.ajax({
                                type: "POST",
                                url: DeleSubWayBillMainURL + encodeURI(ids.join(",")),
                                data: "",
                                async: false,
                                cache: false,
                                beforeSend: function (XMLHttpRequest) {

                                },
                                success: function (msg) {
                                    var JSONMsg = eval("(" + msg + ")");
                                    if (JSONMsg.result.toLowerCase() == 'ok') {
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                    } else {
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                        return;
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });
                            $("#btnQuerySubWayBill").click();
                        } else {

                        }
                    }
                );
    }

    function CreateSubWayBill_Detail() {
        $("#txtSwbDescription_CHN_Detail").combobox("setValue", "");
        $("#txtSwbDescription_ENG_Detail").val("");
        $("#txtSwbNumber_Detail").val("");
        $("#txtSwbWeight_Detail").val("");
        $("#txtSwbValue_Detail").val("");
        $("#txtTaxNo_Detail").val("");
        $("#txtTaxRate_Detail").combobox("setValue", "");
        $("#txtSwbMonetary_Detail").val("");

        var selects = _$_datagrid_sub.treegrid("getSelections");
        var ids = [];
        for (var i = 0; i < selects.length; i++) {
            if (selects[i].parentID == "top") {
                ids.push(selects[i].ID);
                $("#span_swbSerialNum_Tip").html(selects[i].swbSerialNum);
            }
        }

        if (ids.length != 1) {
            reWriteMessagerAlert('操作提示', "请选择需要增加分运单明细的分运单头(只可选择一行)", 'error');
            return false;
        }

        $("#hid_swbId_Detail").val(ids[0]);

        CreateSubWayBillDetailDlg = $('#span_SubWayBill_Detail').dialog({
            buttons: [{
                text: '保存并继续添加',
                iconCls: 'icon-ok',
                handler: function () {
                    var txtSwbDescription_CHN_Detail = $("#txtSwbDescription_CHN_Detail").combobox("textbox").val(); // $("#txtSwbDescription_CHN_Detail").combobox("getValue");
                    var txtSwbDescription_ENG_Detail = $("#txtSwbDescription_ENG_Detail").val();
                    var txtSwbNumber_Detail = $("#txtSwbNumber_Detail").val();
                    var txtSwbWeight_Detail = $("#txtSwbWeight_Detail").val();
                    var txtSwbValue_Detail = $("#txtSwbValue_Detail").val();
                    var txtTaxNo_Detail = $("#txtTaxNo_Detail").val();
                    var txtTaxRate_Detail = $("#txtTaxRate_Detail").combobox("textbox").val(); // $("#txtTaxRate_Detail").val();
                    var txtSwbMonetary_Detail = $("#txtSwbMonetary_Detail").val();
                    var hid_swbId_Detail = $("#hid_swbId_Detail").val();

                    if (txtSwbDescription_CHN_Detail == "" || txtSwbNumber_Detail == "" || txtSwbWeight_Detail == "" || txtSwbValue_Detail == "" || txtTaxNo_Detail == "" || txtTaxRate_Detail == "" || hid_swbId_Detail == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(品名,件数,重量,货值,税号,税率)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: CreateSubWayBillDetailURL + encodeURI(hid_swbId_Detail) + "&txtSwbDescription_CHN_Detail=" + encodeURI(txtSwbDescription_CHN_Detail) + "&txtSwbDescription_ENG_Detail=" + encodeURI(txtSwbDescription_ENG_Detail) + "&txtSwbNumber_Detail=" + encodeURI(txtSwbNumber_Detail) + "&txtSwbWeight_Detail=" + encodeURI(txtSwbWeight_Detail) + "&txtSwbValue_Detail=" + encodeURI(txtSwbValue_Detail) + "&txtTaxNo_Detail=" + encodeURI(txtTaxNo_Detail) + "&txtTaxRate_Detail=" + encodeURI(txtTaxRate_Detail) + "&txtSwbMonetary_Detail=" + encodeURI(txtSwbMonetary_Detail),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                bOK = true;
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
                        //CreateSubWayBillDetailDlg.dialog('close');
                        //$("#btnQuerySubWayBill").click();
                        _$_datagrid_sub.treegrid("reload", hid_swbId_Detail);
                    }
                }
            }, {
                text: '关  闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateSubWayBillDetailDlg.dialog('close');
                }
            }],
            title: '添加分运单明细',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 600,
            height: 300
        });

        $('#span_SubWayBill_Detail').dialog("open");
    }

    function UpdateSubWayBillDetail(swbSerialNum, ID, parentID, swbDescription_CHN, swbDescription_ENG, swbNumber, swbWeight, swbValue, TaxNo, TaxRate, swbMonetary) {
        $("#hid_swbId_Detail").val(parentID);
        $("#hid_swbdId_Detail").val(ID);
        $("#txtSwbDescription_CHN_Detail").combobox("setValue", swbDescription_CHN);
        //$("#txtSwbDescription_CHN_Detail").combobox("clear");
        $("#txtSwbDescription_ENG_Detail").val(swbDescription_ENG);
        $("#txtSwbNumber_Detail").val(swbNumber);
        $("#txtSwbWeight_Detail").val(swbWeight);
        $("#txtSwbValue_Detail").val(swbValue);
        $("#txtTaxNo_Detail").val(TaxNo);
        //$("#txtTaxRate_Detail").val(TaxRate);
        $("#txtTaxRate_Detail").combobox("setValue", TaxRate);
        $("#txtSwbMonetary_Detail").val(swbMonetary);
        $("#span_swbSerialNum_Tip").html(swbSerialNum);

        CreateSubWayBillDetailDlg = $('#span_SubWayBill_Detail').dialog({
            buttons: [{
                text: '保  存',
                iconCls: 'icon-ok',
                handler: function () {
                    var txtSwbDescription_CHN_Detail = $("#txtSwbDescription_CHN_Detail").combobox("textbox").val(); // $("#txtSwbDescription_CHN_Detail").combobox("getValue");
                    var txtSwbDescription_ENG_Detail = $("#txtSwbDescription_ENG_Detail").val();
                    var txtSwbNumber_Detail = $("#txtSwbNumber_Detail").val();
                    var txtSwbWeight_Detail = $("#txtSwbWeight_Detail").val();
                    var txtSwbValue_Detail = $("#txtSwbValue_Detail").val();
                    var txtTaxNo_Detail = $("#txtTaxNo_Detail").val();
                    var txtTaxRate_Detail = $("#txtTaxRate_Detail").combobox("textbox").val(); // $("#txtTaxRate_Detail").val();
                    var txtSwbMonetary_Detail = $("#txtSwbMonetary_Detail").val();
                    var hid_swbId_Detail = $("#hid_swbId_Detail").val();
                    var hid_swbdId_Detail = $("#hid_swbdId_Detail").val();

                    if (txtSwbDescription_CHN_Detail == "" || txtSwbNumber_Detail == "" || txtSwbWeight_Detail == "" || txtSwbValue_Detail == "" || txtTaxNo_Detail == "" || txtTaxRate_Detail == "" || hid_swbId_Detail == "" || hid_swbdId_Detail == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(品名,件数,重量,货值,税号,税率)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateSubWayBillDetailURL + encodeURI(hid_swbId_Detail) + "&hid_swbdId_Detail=" + encodeURI(hid_swbdId_Detail) + "&txtSwbDescription_CHN_Detail=" + encodeURI(txtSwbDescription_CHN_Detail) + "&txtSwbDescription_ENG_Detail=" + encodeURI(txtSwbDescription_ENG_Detail) + "&txtSwbNumber_Detail=" + encodeURI(txtSwbNumber_Detail) + "&txtSwbWeight_Detail=" + encodeURI(txtSwbWeight_Detail) + "&txtSwbValue_Detail=" + encodeURI(txtSwbValue_Detail) + "&txtTaxNo_Detail=" + encodeURI(txtTaxNo_Detail) + "&txtTaxRate_Detail=" + encodeURI(txtTaxRate_Detail) + "&txtSwbMonetary_Detail=" + encodeURI(txtSwbMonetary_Detail),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                bOK = true;
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
                        CreateSubWayBillDetailDlg.dialog('close');
                        _$_datagrid_sub.treegrid("reload", hid_swbId_Detail);
                    }
                }
            }, {
                text: '关  闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateSubWayBillDetailDlg.dialog('close');
                }
            }],
            title: '修改分运单头',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 600,
            height: 300
        });

        $('#span_SubWayBill_Detail').dialog("open");
    }

    function DeleSubWayBillDetail() {
        reWriteMessagerConfirm("提示", "您确定需要删除所选的分运单明细信息吗？",
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid_sub.treegrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                if (selects[i].parentID != "top") {
                                    ids.push(selects[i].ID);
                                }
                            }
                            if (ids.length == 0) {
                                reWriteMessagerAlert("提示", "<center>请选择需要删除的数据</center>", "error");
                                return false;
                            }
                            $.ajax({
                                type: "POST",
                                url: DeleSubWayBillDetailURL + encodeURI(ids.join(",")),
                                data: "",
                                async: false,
                                cache: false,
                                beforeSend: function (XMLHttpRequest) {

                                },
                                success: function (msg) {
                                    var JSONMsg = eval("(" + msg + ")");
                                    if (JSONMsg.result.toLowerCase() == 'ok') {
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                    } else {
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                        return;
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });
                            $("#btnQuerySubWayBill").click();
                        } else {

                        }
                    }
                );
    }

    function Detail(wbSerialNum) {
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

    function SeleAll() {
        var rows = _$_datagrid.datagrid("getRows");
        for (var i = 0; i < rows.length; i++) {
            var m = _$_datagrid.datagrid("getRowIndex", rows[i]);
            _$_datagrid.datagrid("selectRow", m)
        }
    }

    function InverseSele() {
        var rows = _$_datagrid.datagrid("getRows");
        var selects = _$_datagrid.datagrid("getSelections");
        for (var i = 0; i < rows.length; i++) {
            var bSele = false;
            var m = _$_datagrid.datagrid("getRowIndex", rows[i]);
            for (var j = 0; j < selects.length; j++) {
                var n = _$_datagrid.datagrid("getRowIndex", selects[j]);
                if (m == n) {
                    bSele = true;
                }
            }
            if (bSele) {
                _$_datagrid.datagrid("unselectRow", m)
            } else {
                _$_datagrid.datagrid("selectRow", m)
            }
        }
    }

    function SeleAll_Sub() {
        _$_datagrid_sub.treegrid("selectAll");
    }

    function InverseSele_Sub() {
        _$_datagrid_sub.treegrid("unselectAll");
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:120px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "wbserialnum":
                    break;
                case "cb":
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
