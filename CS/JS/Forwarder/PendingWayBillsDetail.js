$(function () {
    var _$_datagrid = $("#DG_SubWayBillDetail");
    var _$_ddlCheckResult = $('#ddlCheckResult');
    var _$_ddlHandleSuggestion = $('#ddlHandleSuggestion');

    var QueryCheckResultURL = "/Forwarder_PendingWayBillsDetail/LoadComboJSONByType?type=CHECK_RESULT";
    var QueryHandleSuggestionURL = "/Forwarder_PendingWayBillsDetail/LoadComboJSONByType?type=HANDLE_SUGGESTION";

    var UpdateCheckResultDlg = null;
    var UpdateCheckResultURL = "/Forwarder_PendingWayBillsDetail/UpdateCheckResult?swbdID_Local=";

    var UpdateSwbNeedCheckDlg = null;
    var UpdateSwbNeedCheckURL = "/Forwarder_PendingWayBillsDetail/UpdateSwbNeedCheck?swbID=";

    $("#txtCheckResultDescription").hide();
    $("#txtHandleSuggestionDescription").hide();

    _$_ddlCheckResult.combobox({
        url: QueryCheckResultURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onChange: function (newValue, oldValue) {
            if (newValue == "99") {
                $("#txtCheckResultDescription").show();
            } else {
                $("#txtCheckResultDescription").hide();
            }
        }
    });

    _$_ddlHandleSuggestion.combobox({
        url: QueryHandleSuggestionURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onChange: function (newValue, oldValue) {
            if (newValue == "99") {
                $("#txtHandleSuggestionDescription").show();
            } else {
                $("#txtHandleSuggestionDescription").hide();
            }
        }
    });

    _$_ddlCheckResult.combobox("setValue", "-99");
    _$_ddlHandleSuggestion.combobox("setValue", "-99");

    var QueryURL = "/Forwarder_PendingWayBillsDetail/GetData?Detail_wbID=" + encodeURI($("#hid_wbID").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode").val());

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    $("#btnQuery_Detail").click(function () {
        QueryURL = "/Forwarder_PendingWayBillsDetail/GetData?Detail_wbID=" + encodeURI($("#hid_wbID").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid.treegrid("options"), {
                url: QueryURL
            });
            _$_datagrid.treegrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#txtSubWayBillCode").focus();

    _$_datagrid.treegrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'swbSerialNum',
        sortOrder: 'asc',
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
                    { field: 'FinalCheckResultDescription', title: '查验结果', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID != "top") {
                                sRet = "<table width='100%' border='0'><tr><td>" + rowData.FinalCheckResultDescription + "</td><td align='right'>" + "<input  type='button' class='cls_Update_CheckResult' swbdID='" + rowData.swbdID + "' swbDescription_CHN='" + rowData.swbDescription_CHN + "' swbSerialNum='" + rowData.swbSerialNum + "' IDForUpdate='" + rowData.parentID + "' CheckResult='" + rowData.CheckResult + "' HandleSuggestion='" + rowData.HandleSuggestion + "' FinalCheckResultDescription='" + rowData.FinalCheckResultDescription + "' FinalHandleSuggestDescription='" + rowData.FinalHandleSuggestDescription + "' value='处理'>" + "</td></tr></table>";
                            } else {
                                sRet = "";
                            }
                            return sRet;
                        }
                    },
                    { field: 'FinalHandleSuggestDescription', title: '处理意见', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID != "top") {
                                sRet = "<table width='100%' border='0'><tr><td>" + rowData.FinalHandleSuggestDescription + "</td><td align='right'>" + "<input  type='button' class='cls_Update_HandleSuggestion' swbdID='" + rowData.swbdID + "' swbDescription_CHN='" + rowData.swbDescription_CHN + "' swbSerialNum='" + rowData.swbSerialNum + "' IDForUpdate='" + rowData.parentID + "' CheckResult='" + rowData.CheckResult + "' HandleSuggestion='" + rowData.HandleSuggestion + "' FinalCheckResultDescription='" + rowData.FinalCheckResultDescription + "' FinalHandleSuggestDescription='" + rowData.FinalHandleSuggestDescription + "' value='处理'>" + "</td></tr></table>";
                            } else {
                                sRet = "<table width='100%' border='0'><tr><td align='right'>" + "<input  type='button' class='cls_Update_swbNeedCheck' swbID='" + rowData.ID + "' swbSerialNum='" + rowData.swbSerialNum + "'  swbNeedCheck='" + rowData.swbNeedCheck + "'  value='处理'>" + "</td></tr></table>";
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
					}
				]],
        toolbar: "#toolBarDetail",
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
            var allUpdate_CheckResultBtn = $(".cls_Update_CheckResult");
            var allUpdate_HandleSuggestionBtn = $(".cls_Update_HandleSuggestion");
            var allUpdate_swbNeedCheckBtn = $(".cls_Update_swbNeedCheck");
            $.each(allUpdate_CheckResultBtn, function (i, item) {
                $(item).unbind("click");
                $(item).click(function () {
                    var swbdID = $(item).attr("swbdID");
                    var swbDescription_CHN = $(item).attr("swbDescription_CHN");
                    var swbSerialNum = $(item).attr("swbSerialNum");
                    var IDForUpdate = $(item).attr("IDForUpdate");
                    var CheckResult = $(item).attr("CheckResult");
                    var HandleSuggestion = $(item).attr("HandleSuggestion");
                    var FinalCheckResultDescription = $(item).attr("FinalCheckResultDescription");
                    var FinalHandleSuggestDescription = $(item).attr("FinalHandleSuggestDescription");

                    UpdateCheckResultAndGaveSuggestion(swbdID, swbDescription_CHN, swbSerialNum, IDForUpdate, CheckResult, HandleSuggestion, FinalCheckResultDescription, FinalHandleSuggestDescription,true);
                });

            });

            $.each(allUpdate_HandleSuggestionBtn, function (i, item) {
                $(item).unbind("click");
                $(item).click(function () {
                    var swbdID = $(item).attr("swbdID");
                    var swbDescription_CHN = $(item).attr("swbDescription_CHN");
                    var swbSerialNum = $(item).attr("swbSerialNum");
                    var IDForUpdate = $(item).attr("IDForUpdate");
                    var CheckResult = $(item).attr("CheckResult");
                    var HandleSuggestion = $(item).attr("HandleSuggestion");
                    var FinalCheckResultDescription = $(item).attr("FinalCheckResultDescription");
                    var FinalHandleSuggestDescription = $(item).attr("FinalHandleSuggestDescription");

                    UpdateCheckResultAndGaveSuggestion(swbdID, swbDescription_CHN, swbSerialNum, IDForUpdate, CheckResult, HandleSuggestion, FinalCheckResultDescription, FinalHandleSuggestDescription, true);
                });
            });

            $.each(allUpdate_swbNeedCheckBtn, function (i, item) {
                $(item).unbind("click");
                $(item).click(function () {
                    var swbID = $(item).attr("swbID");
                    var swbSerialNum = $(item).attr("swbSerialNum");

                    UpdateSwbNeedCheck(swbID, swbSerialNum, true);
                });
            });

            delete _$_datagrid.treegrid('options').queryParams['id'];
            _$_datagrid.treegrid("expandAll");

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
        },
        onClickRow: function (rowData) {
            $("#txtSubWayBillCode").focus();
        }
    });

    UpdateCheckResultAndGaveSuggestion("", "", "", "", "", "", "", "", "", false);
    UpdateSwbNeedCheck("", "", false);

    function UpdateCheckResultAndGaveSuggestion(swbdID, swbDescription_CHN, swbSerialNum, IDForUpdate, CheckResult, HandleSuggestion, FinalCheckResultDescription, FinalHandleSuggestDescription, iShow) {
        $("#dlg_UpdateCheckResult").attr("display", "block");
        var _$_ddlCheckResult = $("#ddlCheckResult");
        var _$_ddlHandleSuggestion = $("#ddlHandleSuggestion");
        _$_ddlCheckResult.combobox("setValue", "-99");
        _$_ddlHandleSuggestion.combobox("setValue", "-99");

        $("#txtCheckResultDescription").hide();
        $("#txtHandleSuggestionDescription").hide();

        switch (CheckResult) {
            case "99":
                $("#txtCheckResultDescription").show();
                break;
            default:
                break;
        }

        switch (HandleSuggestion) {
            case "99":
                $("#txtHandleSuggestionDescription").show();
                break;
            default:
                break;
        }

        $("#hid_swbdID").val(swbdID);
        $("#span_swbSerialNum").html(swbSerialNum);
        $("#span_swbDescription_CHN").html(swbDescription_CHN);
        $("#hid_IDForUpdate").val(IDForUpdate);

        if (CheckResult != "-1") {
            _$_ddlCheckResult.combobox("setValue", CheckResult);
        }
        if (HandleSuggestion != "-1") {
            _$_ddlHandleSuggestion.combobox("setValue", HandleSuggestion);
        }

        $("#txtCheckResultDescription").val(FinalCheckResultDescription);
        $("#txtHandleSuggestionDescription").val(FinalHandleSuggestDescription);

        UpdateCheckResultDlg = $('#dlg_UpdateCheckResult').dialog({
            buttons: [{
                text: '保     存',
                iconCls: 'icon-ok',
                handler: function () {
                    var swbdID_Local = UpdateCheckResultDlg.find("#hid_swbdID").val();
                    var swbSerialNum_Local = UpdateCheckResultDlg.find("#span_swbSerialNum").html();
                    var swbDescription_CHN_Local = UpdateCheckResultDlg.find("#span_swbDescription_CHN").html();
                    var IDForUpdate_Local = UpdateCheckResultDlg.find("#hid_IDForUpdate").val();
                    var CheckResult_Local = UpdateCheckResultDlg.find("#ddlCheckResult").combobox("getValue");
                    var HandleSuggestion_Local = UpdateCheckResultDlg.find("#ddlHandleSuggestion").combobox("getValue");

                    var FinalCheckResultDescription_Local = UpdateCheckResultDlg.find("#txtCheckResultDescription").val();
                    var FinalHandleSuggestDescription_Local = UpdateCheckResultDlg.find("#txtHandleSuggestionDescription").val();

                    if (CheckResult_Local != "99") {
                        FinalCheckResultDescription_Local = "";
                    }

                    if (HandleSuggestion_Local != "99") {
                        FinalHandleSuggestDescription_Local = "";
                    }

                    if (swbdID_Local == "" || swbSerialNum_Local == "" || IDForUpdate_Local == "" || CheckResult_Local == "-99" || HandleSuggestion_Local == "-99") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(查验结果、处理意见)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateCheckResultURL + encodeURI(swbdID_Local) + "&CheckResult_Local=" + encodeURI(CheckResult_Local) + "&HandleSuggestion_Local=" + encodeURI(HandleSuggestion_Local) + "&FinalCheckResultDescription_Local=" + encodeURI(FinalCheckResultDescription_Local) + "&FinalHandleSuggestDescription_Local=" + encodeURI(FinalHandleSuggestDescription_Local),
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
                        UpdateCheckResultDlg.dialog('close');
                        _$_datagrid.treegrid("reload", IDForUpdate_Local);
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    UpdateCheckResultDlg.dialog('close');
                }
            }],
            title: '处     理',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 500,
            height: 180
        });
        if (iShow) {
            $('#dlg_UpdateCheckResult').dialog("open");
        }
    }

    function UpdateSwbNeedCheck(swbID, swbSerialNum, iShow) {
        $("#dlg_UpdateSwbNeedCheck").attr("display", "block");
        $("#hid_swbID").val(swbID);
        $("#span_swbSerialNum_First").html(swbSerialNum);

        UpdateSwbNeedCheckDlg = $('#dlg_UpdateSwbNeedCheck').dialog({
            buttons: [{
                text: '查验扣留',
                iconCls: 'icon-ok',
                handler: function () {
                    var hid_swbID_Local = UpdateSwbNeedCheckDlg.find("#hid_swbID").val();

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateSwbNeedCheckURL + encodeURI(hid_swbID_Local) + "&swbNeedCheck=3",
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
                        UpdateSwbNeedCheckDlg.dialog('close');
                        $("#btnQuery_Detail").click();
                    }
                }
            }, {
                text: '查验退货',
                iconCls: 'icon-ok',
                handler: function () {
                    var hid_swbID_Local = UpdateSwbNeedCheckDlg.find("#hid_swbID").val();

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: UpdateSwbNeedCheckURL + encodeURI(hid_swbID_Local) + "&swbNeedCheck=99",
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
                        UpdateSwbNeedCheckDlg.dialog('close');
                        $("#btnQuery_Detail").click();
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    UpdateSwbNeedCheckDlg.dialog('close');
                }
            }],
            title: '处     理',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 350,
            height: 100
        });
        if (iShow) {
            $('#dlg_UpdateSwbNeedCheck').dialog("open");
        }
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

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:150px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "cb":
                    break;
                case "swbserialnum":
                    break;
                case "chkneedcheck":
                    break;
                case "swbid":
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
