$(function () {
    var PrintURL = "";
    var SavePrintTaxSheetInfoURL = "/PrintTaxSheet/AddPrintTaxSheetInfo?oldOrderNumber=";
    var CurrencyToUpperURL = "/PrintTaxSheet/CurrencyToUpper?TaxValue=";

    $("#txtTotalTaxValue_CustomsTax").blur(function () {
        $.ajax({
            type: "POST",
            url: CurrencyToUpperURL + encodeURI($("#txtTotalTaxValue_CustomsTax").val()),
            data: "",
            async: false,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                $("#txtTotalTaxValueToUpper_CustomsTax").val(msg);
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
    });

    $("#txtTotalTaxValue_ValueAddedTax").blur(function () {
        $.ajax({
            type: "POST",
            url: CurrencyToUpperURL + encodeURI($("#txtTotalTaxValue_ValueAddedTax").val()),
            data: "",
            async: false,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                $("#txtTotalTaxValueToUpper_ValueAddedTax").val(msg);
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
    });

    $("#btnPrintPreview").click(function () {
        PrintURL = "/PrintTaxSheet/Print?wbID=" + encodeURI($("#hid_txtWbID").val());
        var params = "&sIDC=" + encodeURI($("#span_IssuanceDate_CustomsTax").val())
                    + "&sNC=" + encodeURI($("#span_NO_CustomsTax").val())
                    + "&tICOC=" + encodeURI($("#txtInComeOffice_CustomsTax").val())
                    + "&tSC=" + encodeURI($("#txtSubject_CustomsTax").val())
                    + "&tBLC=" + encodeURI($("#txtBudgetLevels_CustomsTax").val())
                    + "&tRTC=" + encodeURI($("#txtRecipientTreasury_CustomsTax").val())
                    + "&tPUC=" + encodeURI($("#txtPaymentUnit_CustomsTax").val())
                    + "&tANC=" + encodeURI($("#txtAccountNo_CustomsTax").val())
                    + "&tBNC=" + encodeURI($("#txtBankName_CustomsTax").val())
                    + "&tTNC=" + encodeURI($("#txtTaxNo_CustomsTax").val())
                    + "&tDCC=" + encodeURI($("#txtDescription_CHN_CustomsTax").val())
                    + "&tNC=" + encodeURI($("#txtNumber_CustomsTax").val())
                    + "&tUC=" + encodeURI($("#txtUnit_CustomsTax").val())
                    + "&tFVC=" + encodeURI($("#txtFullValue_CustomsTax").val())
                    + "&tTRC=" + encodeURI($("#txtTaxRate_CustomsTax").val())
                    + "&tTVC=" + encodeURI($("#txtTaxValue_CustomsTax").val())
                    + "&tTTVTUC=" + encodeURI($("#txtTotalTaxValueToUpper_CustomsTax").val())
                    + "&tTTVC=" + encodeURI($("#txtTotalTaxValue_CustomsTax").val())
                    + "&tACNC=" + encodeURI($("#txtApplyCompanyNo_CustomsTax").val())
                    + "&tBNEC=" + encodeURI($("#txtBillNOofEntry_CustomsTax").val())
                    + "&tCNC=" + encodeURI($("#txtContractNO_CustomsTax").val())
                    + "&tTTC=" + encodeURI($("#txtTransportTools_CustomsTax").val())
                    + "&tELOPC=" + encodeURI($("#txtEndLineOfPay_CustomsTax").val())
                    + "&tPGNC=" + encodeURI($("#txtPickGoodsNO_CustomsTax").val())
                    + "&hdMC=" + encodeURI($("#hd_taMemo_CustomsTax").val())
                    + "&tTMC=" + encodeURI($("#txtTableMaker_CustomsTax").val())
                    + "&tTCC=" + encodeURI($("#txtTableChecker_CustomsTax").val())
                    + "&tNTC=" + encodeURI($("#txtNationTreasury_CustomsTax").val())
                    + "&sIDV=" + encodeURI($("#span_IssuanceDate_ValueAddedTax").val())
                    + "&sNV=" + encodeURI($("#span_NO_ValueAddedTax").val())
                    + "&tICOV=" + encodeURI($("#txtInComeOffice_ValueAddedTax").val())
                    + "&tSV=" + encodeURI($("#txtSubject_ValueAddedTax").val())
                    + "&tBLV=" + encodeURI($("#txtBudgetLevels_ValueAddedTax").val())
                    + "&tRTV=" + encodeURI($("#txtRecipientTreasury_ValueAddedTax").val())
                    + "&tPUV=" + encodeURI($("#txtPaymentUnit_ValueAddedTax").val())
                    + "&tANV=" + encodeURI($("#txtAccountNo_ValueAddedTax").val())
                    + "&tBNV=" + encodeURI($("#txtBankName_ValueAddedTax").val())
                    + "&tTNV=" + encodeURI($("#txtTaxNo_ValueAddedTax").val())
                    + "&tDCV=" + encodeURI($("#txtDescription_CHN_ValueAddedTax").val())
                    + "&tNV=" + encodeURI($("#txtNumber_ValueAddedTax").val())
                    + "&tUV=" + encodeURI($("#txtUnit_ValueAddedTax").val())
                    + "&tFVV=" + encodeURI($("#txtFullValue_ValueAddedTax").val())
                    + "&tTRV=" + encodeURI($("#txtTaxRate_ValueAddedTax").val())
                    + "&tTVV=" + encodeURI($("#txtTaxValue_ValueAddedTax").val())
                    + "&tTTVTUV=" + encodeURI($("#txtTotalTaxValueToUpper_ValueAddedTax").val())
                    + "&tTTVV=" + encodeURI($("#txtTotalTaxValue_ValueAddedTax").val())
                    + "&tACNV=" + encodeURI($("#txtApplyCompanyNo_ValueAddedTax").val())
                    + "&tBNEV=" + encodeURI($("#txtBillNOofEntry_ValueAddedTax").val())
                    + "&tCNV=" + encodeURI($("#txtContractNO_ValueAddedTax").val())
                    + "&tTTV=" + encodeURI($("#txtTransportTools_ValueAddedTax").val())
                    + "&tELOPV=" + encodeURI($("#txtEndLineOfPay_ValueAddedTax").val())
                    + "&tPGNV=" + encodeURI($("#txtPickGoodsNO_ValueAddedTax").val())
                    + "&hdMV=" + encodeURI($("#hd_taMemo_ValueAddedTax").val())
                    + "&tTMV=" + encodeURI($("#txtTableMaker_ValueAddedTax").val())
                    + "&tTCV=" + encodeURI($("#txtTableChecker_ValueAddedTax").val())
                    + "&tNTV=" + encodeURI($("#txtNationTreasury_ValueAddedTax").val());

        var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
        div_PrintDlg.show();
        var PrintDlg = null;
        div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL + params);
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

    $("#btnConfirmPrint").click(function () {
        reWriteMessagerConfirm("提示", "确定现在就打印吗？</br><font style='color:red;font-weight:bold'>打印成功后，单号将增加1</font>",
                    function (ok) {
                        var bOK = false;
                        SavePrintTaxSheetInfoURL = "/PrintTaxSheet/AddPrintTaxSheetInfo?oldOrderNumber=" + encodeURI($("#hid_TaxSheetMaxNO").val());
                        $.ajax({
                            type: "POST",
                            url: SavePrintTaxSheetInfoURL,
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
                            PrintURL = "/PrintTaxSheet/Print?wbID=" + encodeURI($("#hid_txtWbID").val());
                            var params = "&sIDC=" + encodeURI($("#span_IssuanceDate_CustomsTax").val())
                                        + "&sNC=" + encodeURI($("#span_NO_CustomsTax").val())
                                        + "&tICOC=" + encodeURI($("#txtInComeOffice_CustomsTax").val())
                                        + "&tSC=" + encodeURI($("#txtSubject_CustomsTax").val())
                                        + "&tBLC=" + encodeURI($("#txtBudgetLevels_CustomsTax").val())
                                        + "&tRTC=" + encodeURI($("#txtRecipientTreasury_CustomsTax").val())
                                        + "&tPUC=" + encodeURI($("#txtPaymentUnit_CustomsTax").val())
                                        + "&tANC=" + encodeURI($("#txtAccountNo_CustomsTax").val())
                                        + "&tBNC=" + encodeURI($("#txtBankName_CustomsTax").val())
                                        + "&tTNC=" + encodeURI($("#txtTaxNo_CustomsTax").val())
                                        + "&tDCC=" + encodeURI($("#txtDescription_CHN_CustomsTax").val())
                                        + "&tNC=" + encodeURI($("#txtNumber_CustomsTax").val())
                                        + "&tUC=" + encodeURI($("#txtUnit_CustomsTax").val())
                                        + "&tFVC=" + encodeURI($("#txtFullValue_CustomsTax").val())
                                        + "&tTRC=" + encodeURI($("#txtTaxRate_CustomsTax").val())
                                        + "&tTVC=" + encodeURI($("#txtTaxValue_CustomsTax").val())
                                        + "&tTTVTUC=" + encodeURI($("#txtTotalTaxValueToUpper_CustomsTax").val())
                                        + "&tTTVC=" + encodeURI($("#txtTotalTaxValue_CustomsTax").val())
                                        + "&tACNC=" + encodeURI($("#txtApplyCompanyNo_CustomsTax").val())
                                        + "&tBNEC=" + encodeURI($("#txtBillNOofEntry_CustomsTax").val())
                                        + "&tCNC=" + encodeURI($("#txtContractNO_CustomsTax").val())
                                        + "&tTTC=" + encodeURI($("#txtTransportTools_CustomsTax").val())
                                        + "&tELOPC=" + encodeURI($("#txtEndLineOfPay_CustomsTax").val())
                                        + "&tPGNC=" + encodeURI($("#txtPickGoodsNO_CustomsTax").val())
                                        + "&hdMC=" + encodeURI($("#hd_taMemo_CustomsTax").val())
                                        + "&tTMC=" + encodeURI($("#txtTableMaker_CustomsTax").val())
                                        + "&tTCC=" + encodeURI($("#txtTableChecker_CustomsTax").val())
                                        + "&tNTC=" + encodeURI($("#txtNationTreasury_CustomsTax").val())
                                        + "&sIDV=" + encodeURI($("#span_IssuanceDate_ValueAddedTax").val())
                                        + "&sNV=" + encodeURI($("#span_NO_ValueAddedTax").val())
                                        + "&tICOV=" + encodeURI($("#txtInComeOffice_ValueAddedTax").val())
                                        + "&tSV=" + encodeURI($("#txtSubject_ValueAddedTax").val())
                                        + "&tBLV=" + encodeURI($("#txtBudgetLevels_ValueAddedTax").val())
                                        + "&tRTV=" + encodeURI($("#txtRecipientTreasury_ValueAddedTax").val())
                                        + "&tPUV=" + encodeURI($("#txtPaymentUnit_ValueAddedTax").val())
                                        + "&tANV=" + encodeURI($("#txtAccountNo_ValueAddedTax").val())
                                        + "&tBNV=" + encodeURI($("#txtBankName_ValueAddedTax").val())
                                        + "&tTNV=" + encodeURI($("#txtTaxNo_ValueAddedTax").val())
                                        + "&tDCV=" + encodeURI($("#txtDescription_CHN_ValueAddedTax").val())
                                        + "&tNV=" + encodeURI($("#txtNumber_ValueAddedTax").val())
                                        + "&tUV=" + encodeURI($("#txtUnit_ValueAddedTax").val())
                                        + "&tFVV=" + encodeURI($("#txtFullValue_ValueAddedTax").val())
                                        + "&tTRV=" + encodeURI($("#txtTaxRate_ValueAddedTax").val())
                                        + "&tTVV=" + encodeURI($("#txtTaxValue_ValueAddedTax").val())
                                        + "&tTTVTUV=" + encodeURI($("#txtTotalTaxValueToUpper_ValueAddedTax").val())
                                        + "&tTTVV=" + encodeURI($("#txtTotalTaxValue_ValueAddedTax").val())
                                        + "&tACNV=" + encodeURI($("#txtApplyCompanyNo_ValueAddedTax").val())
                                        + "&tBNEV=" + encodeURI($("#txtBillNOofEntry_ValueAddedTax").val())
                                        + "&tCNV=" + encodeURI($("#txtContractNO_ValueAddedTax").val())
                                        + "&tTTV=" + encodeURI($("#txtTransportTools_ValueAddedTax").val())
                                        + "&tELOPV=" + encodeURI($("#txtEndLineOfPay_ValueAddedTax").val())
                                        + "&tPGNV=" + encodeURI($("#txtPickGoodsNO_ValueAddedTax").val())
                                        + "&hdMV=" + encodeURI($("#hd_taMemo_ValueAddedTax").val())
                                        + "&tTMV=" + encodeURI($("#txtTableMaker_ValueAddedTax").val())
                                        + "&tTCV=" + encodeURI($("#txtTableChecker_ValueAddedTax").val())
                                        + "&tNTV=" + encodeURI($("#txtNationTreasury_ValueAddedTax").val());

                            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
                            div_PrintDlg.show();
                            var PrintDlg = null;
                            div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL + params);
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
});
