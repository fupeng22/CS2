﻿$(function () {
    var _$_datagrid = $("#DG_SubWayBillResult");

    var PrintURL = "";
    var QueryURL = "/FirstPickGoodsSheetSetting/GetData?swbNeedCheck=3&wbID=" + encodeURI($("#wbID_ForPrint").val());

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    var SendEmail_PDF_Url = "";
    var SendEmail_Excel_Url = "";

    var QueryFeeRateURL = "/Huayu_FeeRateSetting/getFeeRateValue?categoryID=";

    $('#ddlReceiptMode_ForSetting').combobox({
        data: [{ "text": "直航", "id": "直航" }, { "text": "转关", "id": "转关"}],
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    $('#ddlPayMode_ForSetting').combobox({
        data: [{ "text": "现金", "id": "现金" }, { "text": "月结", "id": "月结" }, { "text": "欠条", "id": "欠条"}],
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    $('#Receipt_ForSetting').combobox({
        data: [{ "text": "其他", "id": "其他" }, { "text": "TAT", "id": "TAT" }, { "text": "TCS", "id": "TCS" }, { "text": "CA", "id": "CA" }, { "text": "场外中转", "id": "场外中转"}],
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    $('#ddlReceiptMode_ForSetting').combobox("setValue", '直航');
    $('#ddlPayMode_ForSetting').combobox("setValue", "现金");
    $('#Receipt_ForSetting').combobox("setValue", '其他');

    var SaveDialyReportURL = "/FirstPickGoodsSheetSetting/SaveSaleInfo?FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&hid_CustomCategory_ForSetting=" + encodeURI($("#hid_CustomCategory_ForSetting").val()) + "&wbID_ForPrint=" + encodeURI($("#wbID_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue")) + "&ShouldPayUnit_ForSetting=" + encodeURI($("#ShouldPayUnit_ForSetting").val()) + "&shouldPay_ForSetting=" + encodeURI($("#shouldPay_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlReceiptMode_ForSetting=" + encodeURI($("#ddlReceiptMode_ForSetting").combobox("getValue")) + "&Receipt_ForSetting=" + encodeURI($("#Receipt_ForSetting").combobox("getValue"));

    $("#btnPrePrint").click(function () {
        if (parseInt($("#OperateFee_ForSetting").val()) < 40 || parseInt($("#PickGoodsFee_ForSetting").val()) < 40) {
            reWriteMessagerAlert('操作提示', "操作费最低收费40,提货费最低收费40", 'error');
            $("#OperateFee_ForSetting").focus();
            return false;
        }

        switch ($('#Receipt_ForSetting').combobox("getValue")) {
            case "场外中转":
                if (parseInt($("#ShiftGoodsFee_ForSetting").val()) < 200) {
                    reWriteMessagerAlert('操作提示', "场外中转的移库费每票最低收费200", 'error');
                    $("#ShiftGoodsFee_ForSetting").focus();
                    return false;
                }
                break;
            default:
                break;
        }
        PrintURL = "/FirstPickGoodsSheetSetting/Print?iPrintType=0&strWBID=" + encodeURI($("#wbID_ForPrint").val()) + "&wbSerialNum_ForPrint=" + encodeURI($("#wbSerialNum_ForPrint").val()) + "&FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&wbSerialNumber_ForPrint=" + encodeURI($("#wbSerialNumber_ForPrint").val()) + "&swbTotalNumber_ForPrint=" + encodeURI($("#swbTotalNumber_ForPrint").val()) + "&ReleaseNum_ForSetting=" + encodeURI($("#ReleaseNum_ForSetting").val()) + "&UnReleaseNum_ForSetting=" + encodeURI($("#UnReleaseNum_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue"));
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
    });

    $("#btnPreExcel").click(function () {
        if (parseInt($("#OperateFee_ForSetting").val()) < 40 || parseInt($("#PickGoodsFee_ForSetting").val()) < 40) {
            reWriteMessagerAlert('操作提示', "操作费最低收费40,提货费最低收费40", 'error');
            $("#OperateFee_ForSetting").focus();
            return false;
        }

        switch ($('#Receipt_ForSetting').combobox("getValue")) {
            case "场外中转":
                if (parseInt($("#ShiftGoodsFee_ForSetting").val()) < 200) {
                    reWriteMessagerAlert('操作提示', "场外中转的移库费每票最低收费200", 'error');
                    $("#ShiftGoodsFee_ForSetting").focus();
                    return false;
                }
                break;
            default:
                break;
        }

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

        PrintURL = "/FirstPickGoodsSheetSetting/Excel?iPrintType=0&strWBID=" + encodeURI($("#wbID_ForPrint").val()) + "&wbSerialNum_ForPrint=" + encodeURI($("#wbSerialNum_ForPrint").val()) + "&FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&wbSerialNumber_ForPrint=" + encodeURI($("#wbSerialNumber_ForPrint").val()) + "&swbTotalNumber_ForPrint=" + encodeURI($("#swbTotalNumber_ForPrint").val()) + "&ReleaseNum_ForSetting=" + encodeURI($("#ReleaseNum_ForSetting").val()) + "&UnReleaseNum_ForSetting=" + encodeURI($("#UnReleaseNum_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue")) + "&browserType=" + browserType;
        window.open(PrintURL);
    });

    $("#btnConfirmPrint").click(function () {
        if (parseInt($("#OperateFee_ForSetting").val()) < 40 || parseInt($("#PickGoodsFee_ForSetting").val()) < 40) {
            reWriteMessagerAlert('操作提示', "操作费最低收费40,提货费最低收费40", 'error');
            $("#OperateFee_ForSetting").focus();
            return false;
        }

        switch ($('#Receipt_ForSetting').combobox("getValue")) {
            case "场外中转":
                if (parseInt($("#ShiftGoodsFee_ForSetting").val()) < 200) {
                    reWriteMessagerAlert('操作提示', "场外中转的移库费每票最低收费200", 'error');
                    $("#ShiftGoodsFee_ForSetting").focus();
                    return false;
                }
                break;
            default:
                break;
        }

        reWriteMessagerConfirm("提示", "确定现在就打印吗？</br><font style='color:red;font-weight:bold'>打印成功后，本次打印信息将进入该日销售日报表</font>",
                    function (ok) {
                        var bOK = false;
                        SendEmail_PDF_Url = "/FirstPickGoodsSheetSetting/SendMail_PDF?iPrintType=1&strWBID=" + encodeURI($("#wbID_ForPrint").val()) + "&wbSerialNum_ForPrint=" + encodeURI($("#wbSerialNum_ForPrint").val()) + "&FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&wbSerialNumber_ForPrint=" + encodeURI($("#wbSerialNumber_ForPrint").val()) + "&swbTotalNumber_ForPrint=" + encodeURI($("#swbTotalNumber_ForPrint").val()) + "&ReleaseNum_ForSetting=" + encodeURI($("#ReleaseNum_ForSetting").val()) + "&UnReleaseNum_ForSetting=" + encodeURI($("#UnReleaseNum_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue"));
                        $.ajax({
                            type: "POST",
                            url: SendEmail_PDF_Url,
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {
                                $("#lblTip").html("<font style='color:blue;font-weight:bold'>正在验证并发送邮件……</font>");
                            },
                            success: function (msg) {
                                $("#lblTip").html("");
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
                                    bOK = true;
                                } else {
                                    //reWriteMessagerAlert('操作提示', JSONMsg.message,'error');
                                    //alert(JSONMsg.message);
                                    $.messager.alert('警告', JSONMsg.message);
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });

                        if (bOK == false) {
                            return;
                        }

                        bOK = false;
                        SaveDialyReportURL = "/FirstPickGoodsSheetSetting/SaveSaleInfo?FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&hid_CustomCategory_ForSetting=" + encodeURI($("#hid_CustomCategory_ForSetting").val()) + "&wbID_ForPrint=" + encodeURI($("#wbID_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue")) + "&ShouldPayUnit_ForSetting=" + encodeURI($("#ShouldPayUnit_ForSetting").val()) + "&shouldPay_ForSetting=" + encodeURI($("#shouldPay_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlReceiptMode_ForSetting=" + encodeURI($("#ddlReceiptMode_ForSetting").combobox("getValue")) + "&Receipt_ForSetting=" + encodeURI($("#Receipt_ForSetting").combobox("getValue"));
                        $.ajax({
                            type: "POST",
                            url: SaveDialyReportURL,
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
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
                            PrintURL = "/FirstPickGoodsSheetSetting/Print?iPrintType=1&strWBID=" + encodeURI($("#wbID_ForPrint").val()) + "&wbSerialNum_ForPrint=" + encodeURI($("#wbSerialNum_ForPrint").val()) + "&FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&wbSerialNumber_ForPrint=" + encodeURI($("#wbSerialNumber_ForPrint").val()) + "&swbTotalNumber_ForPrint=" + encodeURI($("#swbTotalNumber_ForPrint").val()) + "&ReleaseNum_ForSetting=" + encodeURI($("#ReleaseNum_ForSetting").val()) + "&UnReleaseNum_ForSetting=" + encodeURI($("#UnReleaseNum_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue"));
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
                        }

                    });

    });

    $("#btnConfirmExcel").click(function () {
        if (parseInt($("#OperateFee_ForSetting").val()) < 40 || parseInt($("#PickGoodsFee_ForSetting").val()) < 40) {
            reWriteMessagerAlert('操作提示', "操作费最低收费40,提货费最低收费40", 'error');
            $("#OperateFee_ForSetting").focus();
            return false;
        }

        switch ($('#Receipt_ForSetting').combobox("getValue")) {
            case "场外中转":
                if (parseInt($("#ShiftGoodsFee_ForSetting").val()) < 200) {
                    reWriteMessagerAlert('操作提示', "场外中转的移库费每票最低收费200", 'error');
                    $("#ShiftGoodsFee_ForSetting").focus();
                    return false;
                }
                break;
            default:
                break;
        }

        reWriteMessagerConfirm("提示", "确定现在就导出吗？</br><font style='color:red;font-weight:bold'>导出成功后，本次导出信息将进入该日销售日报表</font>",
                    function (ok) {
                        var bOK = false;
                        SendEmail_Excel_Url = "/FirstPickGoodsSheetSetting/SendEmail_Excel?iPrintType=1&strWBID=" + encodeURI($("#wbID_ForPrint").val()) + "&wbSerialNum_ForPrint=" + encodeURI($("#wbSerialNum_ForPrint").val()) + "&FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&wbSerialNumber_ForPrint=" + encodeURI($("#wbSerialNumber_ForPrint").val()) + "&swbTotalNumber_ForPrint=" + encodeURI($("#swbTotalNumber_ForPrint").val()) + "&ReleaseNum_ForSetting=" + encodeURI($("#ReleaseNum_ForSetting").val()) + "&UnReleaseNum_ForSetting=" + encodeURI($("#UnReleaseNum_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue")) + "&browserType=" + browserType;
                        $.ajax({
                            type: "POST",
                            url: SendEmail_Excel_Url,
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {
                                $("#lblTip").html("<font style='color:blue;font-weight:bold'>正在验证并发送邮件……</font>");
                            },
                            success: function (msg) {
                                $("#lblTip").html("");
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
                                    bOK = true;
                                } else {
                                    //reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                    //alert(JSONMsg.message);
                                    $.messager.alert('警告', JSONMsg.message);
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });

                        if (bOK == false) {
                            return;
                        }

                        bOK = false;
                        SaveDialyReportURL = "/FirstPickGoodsSheetSetting/SaveSaleInfo?FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&hid_CustomCategory_ForSetting=" + encodeURI($("#hid_CustomCategory_ForSetting").val()) + "&wbID_ForPrint=" + encodeURI($("#wbID_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue")) + "&ShouldPayUnit_ForSetting=" + encodeURI($("#ShouldPayUnit_ForSetting").val()) + "&shouldPay_ForSetting=" + encodeURI($("#shouldPay_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlReceiptMode_ForSetting=" + encodeURI($("#ddlReceiptMode_ForSetting").combobox("getValue")) + "&Receipt_ForSetting=" + encodeURI($("#Receipt_ForSetting").combobox("getValue"));
                        $.ajax({
                            type: "POST",
                            url: SaveDialyReportURL,
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
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

                            PrintURL = "/FirstPickGoodsSheetSetting/Excel?iPrintType=1&strWBID=" + encodeURI($("#wbID_ForPrint").val()) + "&wbSerialNum_ForPrint=" + encodeURI($("#wbSerialNum_ForPrint").val()) + "&FlowNum_ForPrint=" + encodeURI($("#FlowNum_ForPrint").val()) + "&wbSerialNumber_ForPrint=" + encodeURI($("#wbSerialNumber_ForPrint").val()) + "&swbTotalNumber_ForPrint=" + encodeURI($("#swbTotalNumber_ForPrint").val()) + "&ReleaseNum_ForSetting=" + encodeURI($("#ReleaseNum_ForSetting").val()) + "&UnReleaseNum_ForSetting=" + encodeURI($("#UnReleaseNum_ForSetting").val()) + "&wbActualWeight_ForPrint=" + encodeURI($("#wbActualWeight_ForPrint").val()) + "&InStoreDate_ForSetting=" + encodeURI($("#InStoreDate_ForSetting").val()) + "&PickGoodsDate_ForSetting=" + encodeURI($("#PickGoodsDate_ForSetting").val()) + "&OperateFee_ForSetting=" + encodeURI($("#OperateFee_ForSetting").val()) + "&PickGoodsFee_ForSetting=" + encodeURI($("#PickGoodsFee_ForSetting").val()) + "&KeepGoodsFee_ForSetting=" + encodeURI($("#KeepGoodsFee_ForSetting").val()) + "&ShiftGoodsFee_ForSetting=" + encodeURI($("#ShiftGoodsFee_ForSetting").val()) + "&CollectionFee_ForSetting=" + encodeURI($("#CollectionFee_ForSetting").val()) + "&TotalFee_ForSetting=" + encodeURI($("#TotalFee_ForSetting").val()) + "&ddlPayMode_ForSetting=" + encodeURI($("#ddlPayMode_ForSetting").combobox("getValue")) + "&browserType=" + browserType;
                            window.open(PrintURL);
                        }
                    });

    });

    $('#Receipt_ForSetting').combobox({
        onSelect: function (param) {
            switch ($('#Receipt_ForSetting').combobox("getValue")) {
                case "场外中转":
                    $.ajax({
                        type: "POST",
                        url: QueryFeeRateURL + encodeURI("99-1-1"),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            $("#ShiftGoodsFee_ForSetting").val(parseFloat(msg) * parseFloat($("#wbActualWeight_ForPrint").val()));
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });
                    break;
                case "其他":
                    $("#ShouldPayUnit_ForSetting").val("0.00");
                    $("#shouldPay_ForSetting").val("0.00");
                    $("#ShiftGoodsFee_ForSetting").val("0.00");
                    break;
                default:
                    $("#ShiftGoodsFee_ForSetting").val("0.00");
                    break;
            }
            ComputeTotal();
        }
    });

    $('#ddlReceiptMode_ForSetting').combobox({
        onSelect: function (param) {
            switch ($('#ddlReceiptMode_ForSetting').combobox("getValue")) {
                case "直航":
                    $("#ShouldPayUnit_ForSetting").val("0.5");
                    $("#shouldPay_ForSetting").val(parseFloat($("#wbActualWeight_ForPrint").val()) * parseFloat($("#ShouldPayUnit_ForSetting").val()) + parseFloat($("#CollectionFee_ForSetting").val()));
                    break;
                case "转关":
                    $("#ShouldPayUnit_ForSetting").val("0.7");
                    $("#shouldPay_ForSetting").val(parseFloat($("#wbActualWeight_ForPrint").val()) * parseFloat($("#ShouldPayUnit_ForSetting").val()) + parseFloat($("#CollectionFee_ForSetting").val()));
                    var categoryID = "";
                    switch ($("#hid_CustomCategory_ForSetting").val()) {
                        case "2":
                            categoryID = "2-1-4";
                            break;
                        case "3":
                            categoryID = "2-1-4";
                            break;
                        case "4":
                            categoryID = "2-1-4";
                            break;
                        case "5":
                            categoryID = "5-1-6";
                            break;
                        case "6":
                            categoryID = "6-1-6";
                            break;
                        default:
                            break;
                    }
                    if (categoryID != "") {
                        $.ajax({
                            type: "POST",
                            url: QueryFeeRateURL + encodeURI(categoryID),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                if ($("#PickGoodsFee_ForSetting").val() == "") {
                                    $("#PickGoodsFee_ForSetting").val(parseFloat(msg) * parseFloat($("#wbActualWeight_ForPrint").val()));
                                } else {
                                    $("#PickGoodsFee_ForSetting").val(parseFloat($("#PickGoodsFee_ForSetting").val()) + parseFloat(msg) * parseFloat($("#wbActualWeight_ForPrint").val()));
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                    }

                    break;
                default:
                    $("#ShouldPayUnit_ForSetting").val("0.00");
                    $("#shouldPay_ForSetting").val("0.00");
                    break;
            }

            ComputeTotal();
        }
    });

    $("#OperateFee_ForSetting").blur(function () {
        ComputeTotal();
    });

    $("#PickGoodsFee_ForSetting").blur(function () {
        ComputeTotal();
    });

    $("#KeepGoodsFee_ForSetting").blur(function () {
        ComputeTotal();
    });

    $("#ShiftGoodsFee_ForSetting").blur(function () {
        ComputeTotal();
    });

    $("#CollectionFee_ForSetting").blur(function () {
        ComputeTotal();
    });

    $("#ShouldPayUnit_ForSetting").blur(function () {
        ComputeTotal();
    });

    $("#shouldPay_ForSetting").blur(function () {
        ComputeTotal();
    });

    function ComputeTotal() {
        var OperateFee_ForSetting = $("#OperateFee_ForSetting").val() == "" ? 0.00 : parseFloat($("#OperateFee_ForSetting").val());
        var PickGoodsFee_ForSetting = $("#PickGoodsFee_ForSetting").val() == "" ? 0.00 : parseFloat($("#PickGoodsFee_ForSetting").val());
        var KeepGoodsFee_ForSetting = $("#KeepGoodsFee_ForSetting").val() == "" ? 0.00 : parseFloat($("#KeepGoodsFee_ForSetting").val());
        var ShiftGoodsFee_ForSetting = $("#ShiftGoodsFee_ForSetting").val() == "" ? 0.00 : parseFloat($("#ShiftGoodsFee_ForSetting").val());
        var CollectionFee_ForSetting = $("#CollectionFee_ForSetting").val() == "" ? 0.00 : parseFloat($("#CollectionFee_ForSetting").val());
        $("#TotalFee_ForSetting").val(OperateFee_ForSetting + PickGoodsFee_ForSetting + KeepGoodsFee_ForSetting + ShiftGoodsFee_ForSetting + CollectionFee_ForSetting);
    }

    _$_datagrid.treegrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'swbID',
        sortOrder: 'desc',
        remoteSort: true,
        border: false,
        idField: 'ID',
        singleSelect: false,
        treeField: 'swbSerialNum',
        columns: [[
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
                    { field: 'wbSerialNum', title: '总运单号', width: 180, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = rowData.wbSerialNum;
                            } else {
                                sRet = "";
                            }
                            return sRet;
                        }
                    },
                    { field: 'wbStorageDate', title: '报关日期', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = rowData.wbStorageDate;
                            } else {
                                sRet = "";
                            }
                            return sRet;
                        }
                    },
					{ field: 'wbCompany', title: '货代公司', width: 120, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    },
					    formatter: function (value, rowData, rowIndex) {
					        var sRet = "";
					        if (rowData.parentID == "top") {
					            sRet = rowData.wbCompany;
					        } else {
					            sRet = "";
					        }
					        return sRet;
					    }
					},
                    { field: 'DetainDate', title: '扣留日期', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                         formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = rowData.DetainDate;
                            } else {
                                sRet = "";
                            }
                            return sRet;
                        }
                    },
                    { field: 'differentiateColor', title: '区分标记', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID != "top") {
                                if (rowData.mismatchCargoName == "1") {
                                    sRet = sRet + "<span type='mismatchCargoName' strID=" + rowData.ID + " class='ExplainDiffrentColor' font_color='red' style='width:40px;background-color:red'>&nbsp;</span>&nbsp;&nbsp;";
                                }
                                if (rowData.belowFullPrice == "1") {
                                    sRet = sRet + "<span  type='belowFullPrice' strID=" + rowData.ID + " class='ExplainDiffrentColor'  font_color='blue'  style='width:40px;background-color:blue'>&nbsp;</span>&nbsp;&nbsp;";
                                }
                                if (rowData.above1000 == "1") {
                                    sRet = sRet + "<span  type='above1000' strID=" + rowData.ID + " class='ExplainDiffrentColor'  font_color='green'  style='width:40px;background-color:green'>&nbsp;</span>&nbsp;&nbsp;";
                                }
                            } else if (rowData.parentID == "top") {
                                if (parseFloat(rowData.TaxValueCheck) <= 50) {
                                    sRet = sRet + "<span  type='TaxValueCheck' strID=" + rowData.ID + " class='ExplainDiffrentColor'  font_color='Fuchsia'  style='width:40px;background-color:Fuchsia'>&nbsp;</span>&nbsp;&nbsp;";
                                }
                                if (parseFloat(rowData.PickGoodsAgain) == '1') {
                                    sRet = sRet + "<span  type='PickGoodsAgain' strID=" + rowData.ID + " class='ExplainDiffrentColor'  font_color='AppWorkspace'  style='width:40px;background-color:AppWorkspace'>&nbsp;</span>&nbsp;&nbsp;";
                                }
                            }
                            return sRet;
                        }
                    },
                    { field: 'FinalCheckResultDescription', title: '查验结果', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'FinalHandleSuggestDescription', title: '处理意见', width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'CheckResultOperator', title: '查验人', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'IsConfirmCheckDescription', title: '审核信息', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'ConfirmCheckOperator', title: '审核人', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'TaxValue', title: '税金', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'swbValueTotal', title: '税金合计', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'TaxValueCheck', title: '核准税金', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'TaxValueCheckOperator', title: '核准人', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
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
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID != "top") {
                                sRet = "<span class='cls_ShowTaxRateSettingInfo' TaxNO='" + rowData.TaxNo + "'>" + rowData.TaxNo + "</span>";
                            } else {
                                sRet = "";
                            }
                            return sRet;
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
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = "<span class='cls_ShowLastPickGoodsInfo' SwbId='" + rowData.ID + "' ReceiverIDCard='" + rowData.ReceiverIDCard + "'>" + rowData.ReceiverIDCard + "</span>";
                            } else {
                                sRet = "";
                            }
                            return sRet;
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
                    },
					{ field: 'chkNeedCheck', title: '是否需要预检', hidden: true, width: 120, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'swbID', title: '主键', hidden: true, width: 120, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'swbNeedCheckDescription', title: '预检状态', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = rowData.swbNeedCheckDescription;
                            } else {
                                sRet = "";
                            }
                            return sRet;
                        }
                    },
                    { field: 'Operator', title: '操作员', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = rowData.Operator;
                            } else {
                                sRet = "";
                            }
                            return sRet;
                        }
                    }
				]],
        //toolbar: "#toolBar",
        pagination: true,
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
        onLoadSuccess: function (row, data) {
            var allShowTaxRateSettingInfoBtn = $(".cls_ShowTaxRateSettingInfo");
            var allShowLastPickGoodsInfoBtn = $(".cls_ShowLastPickGoodsInfo");
            var allExplainDiffrentColorBtn = $(".ExplainDiffrentColor");
            $.each(allShowTaxRateSettingInfoBtn, function (i, item) {
                var TaxNO = $(item).attr("TaxNO");
                $(item).tooltip({
                    content: function () {
                        var bOK = false;
                        var strRet = "";
                        $.ajax({
                            type: "POST",
                            url: LoadStandardTaxRateInfoTipURL + encodeURI(TaxNO),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                strRet = msg;
                                bOK = true;
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });

                        return strRet;
                    },
                    trackMouse: true,
                    onShow: function () {
                        var t = $(this);
                        t.tooltip('tip').unbind().bind('mouseenter', function () {
                            t.tooltip('show');
                        }).bind('mouseleave', function () {
                            t.tooltip('hide');
                        });
                    }
                });
            });

            $.each(allShowLastPickGoodsInfoBtn, function (i, item) {
                var SwbId = $(item).attr("SwbId");
                var ReceiverIDCard = $(item).attr("ReceiverIDCard");
                $(item).tooltip({
                    content: function () {
                        var bOK = false;
                        var strRet = "";
                        $.ajax({
                            type: "POST",
                            url: LoadLastPickGoodsInfoTipURL + encodeURI(ReceiverIDCard) + "&strSwbId=" + encodeURI(SwbId),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                strRet = msg;
                                bOK = true;
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });

                        return strRet;
                    },
                    trackMouse: true,
                    onShow: function () {
                        var t = $(this);
                        t.tooltip('tip').unbind().bind('mouseenter', function () {
                            t.tooltip('show');
                        }).bind('mouseleave', function () {
                            t.tooltip('hide');
                        });
                    }
                });
            });

            $.each(allExplainDiffrentColorBtn, function (i, item) {
                var strID = $(item).attr("strID");
                var type = $(item).attr("type");
                var font_color = $(item).attr("font_color");
                $(item).tooltip({
                    content: function () {
                        var bOK = false;
                        var strRet = "";
                        $.ajax({
                            type: "POST",
                            url: ExplainDiffrentColorTipURL + encodeURI(strID) + "&Type=" + encodeURI(type) + "&font_color=" + encodeURI(font_color),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                strRet = msg;
                                bOK = true;
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });

                        return strRet;
                    },
                    trackMouse: true,
                    onShow: function () {
                        var t = $(this);
                        t.tooltip('tip').unbind().bind('mouseenter', function () {
                            t.tooltip('show');
                        }).bind('mouseleave', function () {
                            t.tooltip('hide');
                        });
                    }
                });
            });



            delete _$_datagrid.treegrid('options').queryParams['id'];
            _$_datagrid.treegrid("expandAll");
        },
        onCheck: function (rowData) {
            if (rowData.parentID == "top") {

            } else {
                _$_datagrid.treegrid("unselect", rowData.ID);
            }
        },
        onClickRow: function (rowData) {
            if (rowData.parentID != "top") {
                _$_datagrid.treegrid("unselect", rowData.ID);
            } else {

            }
        }
    });

//    _$_datagrid.datagrid({
//        iconCls: 'icon-save',
//        nowrap: true,
//        autoRowHeight: false,
//        autoRowWidth: false,
//        striped: true,
//        collapsible: true,
//        url: QueryURL,
//        sortName: 'swbID',
//        sortOrder: 'desc',
//        remoteSort: true,
//        border: false,
//        idField: 'swbID',
//        columns: [[
//					{ field: 'swbSerialNum', title: '分运单号', width: 120, sortable: true,
//					    sorter: function (a, b) {
//					        return (a > b ? 1 : -1);
//					    }
//					},
//                    { field: 'swbDescription_CHN', title: '货物中文名', width: 150, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'swbDescription_ENG', title: '货物英文名', width: 150, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'swbNumber', title: '件数', width: 80, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'swbWeight', title: '重量', width: 120, sortable: true, align: "right",
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'DetainDate', title: '扣留日期', width: 120, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    }
//				]],
//        pagination: true,
//        onHeaderContextMenu: function (e, field) {
//            e.preventDefault();
//            if (!$('#tmenu').length) {
//                createColumnMenu();
//            }
//            $('#tmenu').menu('show', {
//                left: e.pageX,
//                top: e.pageY
//            });
//        },
//        onSortColumn: function (sort, order) {
//            //_$_datagrid.datagrid("reload");
//        }
//    });


    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "swbserialnum":
                    break;
                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "swbSerialNum") {

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
