$(function () {
    var _$_datagrid = $("#DG_WayBillResult");
    var _$_ddCompany = $('#txtVoyage');
    var QueryCompanyURL = "/ForwarderMain/LoadComboxJSON";

    var ValidateInfoURL = "/Huayu_ConfirmInStore/ValidateInfo?wbSerialNum=";

    var DeleForeStoreInfo = "/Huayu_ConfirmInStore/DeleForeStore?swbIds=";

    var PatchInStoreURL = "/Huayu_ConfirmInStore/PatchInOutStore?iType=1&ids_wbSerialNum=";
    var PatchOutStoreURL = "/Huayu_ConfirmInStore/PatchInOutStore?iType=3&ids_wbSerialNum=";

    var CreateURL = "/Huayu_ConfirmInStore/Create";
    var CreateDlg = null;
    var CreateDlgForm = null;

    _$_ddCompany.combobox({
        url: QueryCompanyURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    $('#txtNeedCheckStatus').combotree('loadData', [
    {
        id: -99,
        text: '---请选择(可多选)---'
    },
    {
        id: 0,
        text: '放行'
    },
    {
        id: 1,
        text: '等待预检'
    },
    {
        id: 2,
        text: '查验放行'
    },
    {
        id: 3,
        text: '查验扣留'
    },
    {
        id: 4,
        text: '查验待处理'
    },
    {
        id: 99,
        text: '查验退货'
    }]);

    $('#txtNeedCheckStatus').combotree("setValue", "-99");
    _$_ddCompany.combobox("setValue", "---请选择---");

    var PrintURL = "";
    var QueryURL = "/Huayu_ConfirmInStore/GetData?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(','));

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    $("#btnQuery").click(function () {
        QueryURL = "/Huayu_ConfirmInStore/GetData?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(','));
        window.setTimeout(function () {
            $.extend(_$_datagrid.treegrid("options"), {
                url: QueryURL
            });
            _$_datagrid.treegrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#btnReset").click(function () {
        $("#txtBeginD").val("");
        $("#txtEndD").val("");
        $("#txtCode").val("");
        $("#txtSubWayBillCode").val("");
        $('#txtNeedCheckStatus').combotree("setValue", "-99");
        _$_ddCompany.combobox("setValue", "---请选择---");
        $("#btnQuery").click();
    });

    $("#btnConfirmInStore").click(function () {
        ConfirmPatchInOutStore("1");
    });

    $("#btnAddForeStoreInfo").click(function () {
        Add();
    });

    $("#btnDeleForeStoreInfo").click(function () {
        Delete();
    });

    $("#btnPrint").click(function () {
        PrintURL = "/Huayu_ConfirmInStore/Print?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(',')) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
        if (_$_datagrid.treegrid("getRoots").length > 0) {
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

        PrintURL = "/Huayu_ConfirmInStore/Excel?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(',')) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.treegrid("getRoots").length > 0) {
            window.open(PrintURL);

        } else {
            reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            return false;
        }
    });

    $("#btnSeleAll").click(function () {
        SeleAll();
    });

    $("#btnInverseSele").click(function () {
        InverseSele();
    });

    _$_datagrid.treegrid({
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
        idField: 'ID',
        singleSelect: false,
        treeField: 'swbSerialNum',
        columns: [[
                     { field: 'cb', title: '', checkbox: true, width: 40, align: 'center'
                     },
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
                    }
				]],
        toolbar: "#toolBar",
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


            //var AllChks = $(".datagrid-row input:checkbox");
            var AllSonChks = $(".treegrid-tr-tree input:checkbox");
            //            $.each(AllChks, function (i, item) {
            //                $(item).hide();
            //            });
            $.each(AllSonChks, function (i, item) {
                $(item).hide();
            });
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

    $(".datagrid-header-check input").hide();

    function Add() {
        _$_datagrid.treegrid("unselectAll");
        CreateDlg = $('#dlg_Create').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var ddlwbSerialNum = CreateDlg.find('#ddlwbSerialNum').combobox("getValue");
                    var txtSwbSerialNum = CreateDlg.find('#txtSwbSerialNum').val();
                    var ddlSwbCustomsCategory = CreateDlg.find('#ddlSwbCustomsCategory').combogrid("getValue");

                    if (ddlwbSerialNum == "" || ddlwbSerialNum == "" || ddlSwbCustomsCategory == "---请选择---") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(总运单号、分运单号、报关类别必填)', "error");
                        return false;
                    }

                    var bOK = false;
                    $.ajax({
                        type: "POST",
                        url: ValidateInfoURL + encodeURI(ddlwbSerialNum) + "&swbSerialNum=" + encodeURI(txtSwbSerialNum),
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

                    if (bOK == false) {
                        return false;
                    } else {
                        CreateDlgForm = CreateDlg.find('form');
                        CreateDlgForm.form('submit', {
                            url: CreateDlgForm.url,
                            onSubmit: function () {
                                return $(this).form('validate');
                            },
                            success: function () {
                                reWriteMessagerAlert('提示', '成功', "info");
                                CreateDlg.dialog('close');
                                _$_datagrid.treegrid("reload");
                                _$_datagrid.treegrid("unselectAll");
                            }
                        });
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateDlg.dialog('close');
                }
            }],
            title: '补录预入库单信息',
            href: CreateURL,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 800,
            height: 260
        });

        $('#dlg_Create').dialog("open");
    }

    function ConfirmPatchInOutStore(iType) {
        var selects = _$_datagrid.treegrid("getSelections");

        var ids_wbSerialNum = [];
        var ids_swbSerialNum = [];
        for (var i = 0; i < selects.length; i++) {
            ids_wbSerialNum.push(selects[i].wbSerialNum);
            ids_swbSerialNum.push(selects[i].swbSerialNum);
        }
        if (selects.length == 0) {
            reWriteMessagerAlert("提示", "<center>请您先选择数据</center>", "error");
            return false;
        } else {
            var strIds_wbSerialNum = ids_wbSerialNum.join(',');
            var strIds_swbSerialNum = ids_swbSerialNum.join(',');
            switch (iType) {
                case "1":
                    reWriteMessagerConfirm("操作提示", "您确定需要将所选记录进行入仓吗？", function (ok) {
                        if (ok) {
                            PatchInOutStore(strIds_wbSerialNum, strIds_swbSerialNum, iType);
                        }
                    });
                    break;
                case "3":
                    reWriteMessagerConfirm("操作提示", "您确定需要将所选记录进行出仓吗？", function (ok) {
                        if (ok) {
                            PatchInOutStore(strIds_wbSerialNum, strIds_swbSerialNum, iType);
                        }
                    });
                    break;
                default:

            }
        }

    }

    function PatchInOutStore(ids_wbSerialNum, ids_swbSerialNum, iType) {
        var PatchURL = "";
        switch (iType) {
            case "1":
                PatchURL = PatchInStoreURL + encodeURI(ids_wbSerialNum) + "&ids_swbSerialNum=" + encodeURI(ids_swbSerialNum);
                break;
            case "3":
                PatchURL = PatchOutStoreURL + encodeURI(ids_wbSerialNum) + "&ids_swbSerialNum=" + encodeURI(ids_swbSerialNum);
                break;
            default:

        }
        if (ids_wbSerialNum == undefined || ids_swbSerialNum == undefined || ids_wbSerialNum == "" || ids_swbSerialNum == "") {

        } else {
            _$_datagrid.treegrid("unselectAll");
            $.ajax({
                type: "POST",
                url: PatchURL,
                data: "",
                async: false,
                cache: false,
                beforeSend: function (XMLHttpRequest) {

                },
                success: function (msg) {
                    var JSONMsg = eval("(" + msg + ")");
                    if (JSONMsg.result.toLowerCase() == 'ok') {
                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');

                        _$_datagrid.treegrid("unselectAll");
                        _$_datagrid.treegrid("reload");
                    } else {
                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                        return false;
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {

                },
                error: function () {

                }
            });
        }

    }

    function Delete() {
        reWriteMessagerConfirm("提示", "您确定需要删除所选的预入库信息吗？</br><font style='color:red;font-weight:bold'>(删除后无法恢复,请谨慎选择)</font>？",
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.treegrid("getSelections");
                            var ids_swbID = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids_swbID.push(selects[i].swbID);
                            }

                            if (selects.length == 0) {
                                reWriteMessagerAlert("提示", "<center>请选择需要删除的预入库信息</center>", "error");
                                //reWriteMessagerAlert("提示", "<center>请您先选择数据</center>", "error");
                                return false;
                            }
                            var strIds_swbID = ids_swbID.join(',');
                            $.ajax({
                                type: "POST",
                                url: DeleForeStoreInfo + encodeURI(strIds_swbID),
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
                                        return false;
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });

                            _$_datagrid.treegrid("unselectAll");
                            _$_datagrid.treegrid("reload");
                        } else {

                        }
                    }
                );
    }

    function SeleAll() {
        var roots = _$_datagrid.treegrid("getRoots");
        for (var i = 0; i < roots.length; i++) {
            _$_datagrid.treegrid("select", roots[i].ID);
        }
    }

    function InverseSele() {
        var roots = _$_datagrid.treegrid("getRoots");
        var selects = _$_datagrid.treegrid("getSelections");
        var bSele = false;
        for (var i = 0; i < roots.length; i++) {
            bSele = false;
            for (var j = 0; j < selects.length; j++) {
                if (roots[i].ID == selects[j].ID) {
                    bSele = true;
                }
            }
            if (bSele) {
                _$_datagrid.treegrid("unselect", roots[i].ID);
            } else {
                _$_datagrid.treegrid("select", roots[i].ID);
            }
        }
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
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
