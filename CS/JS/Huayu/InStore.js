$(function () {
    var _$_datagrid = $("#DG_WayBillResult");
    var _$_ddCompany = $('#txtVoyage');
    var QueryCompanyURL = "/ForwarderMain/LoadComboxJSON";

    var PatchInStoreURL = "/Huayu_ExceptionStore/PatchInOutStore?iType=1&ids=";
    var PatchOutStoreURL = "/Huayu_ExceptionStore/PatchInOutStore?iType=3&ids=";

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

    $('#txtNeedCheckStatus').combotree("setValues", ["0","2"]);

    _$_ddCompany.combobox("setValue", "---请选择---");

    var PrintURL = "";
    var QueryURL = "/Huayu_InStore/GetData?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(','));

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    $("#btnQuery").click(function () {
        QueryURL = "/Huayu_InStore/GetData?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(','));
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
        _$_ddCompany.combobox("setValue", "---请选择---");
        $("#btnQuery").click();
    });

    $("#btnConfirmOutStore").click(function () {
        ConfirmPatchInOutStore("3");
    });

    $("#btnPrint").click(function () {
        PrintURL = "/Huayu_InStore/Print?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(',')) +"&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
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

        PrintURL = "/Huayu_InStore/Excel?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val()) + "&NeedCheck=" + encodeURI($('#txtNeedCheckStatus').combotree("getValues").join(',')) +"&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
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
        sortName: 'operateDate',
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
                    { field: 'operateDate', title: '入库日期', width: 90, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = rowData.operateDate;
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

    function ConfirmPatchInOutStore(iType) {
        var selects = _$_datagrid.treegrid("getSelections");
        var ids = [];
        for (var i = 0; i < selects.length; i++) {
            ids.push(selects[i].WbfID);
        }
        if (selects.length == 0) {
            reWriteMessagerAlert("提示", "<center>请您先选择数据</center>", "error");
            return false;
        } else {
            var strIds = ids.join(',');
            switch (iType) {
                case "1":
                    reWriteMessagerConfirm("操作提示", "您确定需要将所选记录进行入仓吗？", function (ok) {
                        if (ok) {
                            PatchInOutStore(strIds, iType);
                        }
                    });
                    break;
                case "3":
                    reWriteMessagerConfirm("操作提示", "您确定需要将所选记录进行出仓吗？", function (ok) {
                        if (ok) {
                            PatchInOutStore(strIds, iType);
                        }
                    });
                    break;
                default:

            }
        }

    }

    function PatchInOutStore(ids, iType) {
        var PatchURL = "";
        switch (iType) {
            case "1":
                PatchURL = PatchInStoreURL;
                break;
            case "3":
                PatchURL = PatchOutStoreURL;
                break;
            default:
        }
        if (ids == undefined || ids == "") {
            var selects = _$_datagrid.treegrid("getSelections");
            var idsArr = [];
            for (var i = 0; i < selects.length; i++) {
                idsArr.push(selects[i].WbfID);
            }
            if (selects.length == 0) {
                reWriteMessagerAlert('操作提示', '请您先选择记录!', 'error');
                return false;
            }
            var ids = idsArr.join(',');
            if (ids == undefined || ids == "") {

            } else {
                _$_datagrid.datagrid("unselectAll");
                $.ajax({
                    type: "POST",
                    url: PatchURL + encodeURI(ids),
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
        } else {
            if (ids == undefined || ids == "") {
                reWriteMessagerAlert('操作提示', '请您先选择记录!', 'error');
                return false;
            } else {
                _$_datagrid.datagrid("unselectAll");
                $.ajax({
                    type: "POST",
                    url: PatchURL + encodeURI(ids),
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
        var fields = _$_datagrid.treegrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.treegrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "wbserialnum":
                    break;
                case "cb":
                    break;
                case "wbfid":
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
