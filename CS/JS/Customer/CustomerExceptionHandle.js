﻿$(function () {
    var _$_datagrid = $("#DG_SubWayBillDetail");
    var _$_ddCompany = $('#txtVoyage');
    var QueryCompanyURL = "/ForwarderMain/LoadComboxJSON";

    var UpdateWBNeedCheck = "/Customer_ExceptionHandle/UpdateWbNeedCheck?swbIds=";

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    _$_ddCompany.combobox({
        url: QueryCompanyURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    _$_ddCompany.combobox("setValue", "---请选择---");

    var QueryURL = "/Customer_ExceptionHandle/GetData?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val());

    $("#btnQuery").click(function () {
        QueryURL = "/Customer_ExceptionHandle/GetData?txtBeginD=" + encodeURI($("#txtBeginD").val()) + "&txtEndD=" + encodeURI($("#txtEndD").val()) + "&txtVoyage=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtCode=" + encodeURI($("#txtCode").val()) + "&txtSubWayBillCode=" + encodeURI($("#txtSubWayBillCode").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid.treegrid("options"), {
                url: QueryURL
            });
            _$_datagrid.treegrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#btnSeleAll").click(function () {
        SeleAll();
    });

    $("#btnInverseSele").click(function () {
        InverseSele();
    });

    $("#btnOutControl").click(function () {
        var sele = [];
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
                sele.push(roots[i].ID);
            }
        }
        if (sele.length == 0) {
            reWriteMessagerAlert('操作提示', "请选择需要解控的分运单", 'error');
            return false;
        } else {
            UpdateTaxFeeConfirmStatus(sele.join(','));
        }
    });

    _$_datagrid.treegrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'swbSerialNum',
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
                    { field: 'swbNeedCheckDescription', title: '审核人', width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'operatorOutControl', title: '解控', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == "top") {
                                sRet = "<input type='button' class='cls_OutOfControl' swbId='" + rowData.ID + "' swbNeedCheck='" + rowData.swbNeedCheck + "' value='解控'/>";
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
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID == 'top') {
                                sRet = "<table width='100%' border='0'><tr><td>" + rowData.TaxValueCheck + "</td><td align='right'>" + "<input  type='button' class='cls_UpdateTaxValueCheck' swbID='" + rowData.swbID + "'  IDForUpdate='" + rowData.ID + "' TaxValueCheck='" + rowData.TaxValueCheck + "' swbSerialNum='" + rowData.swbSerialNum + "' value='核准'>" + "</td></tr></table>";
                            }

                            return sRet;
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
                                sRet = "<span class='cls_ShowTaxRateSettingInfo' type TaxNO='" + rowData.TaxNo + "'>" + rowData.TaxNo + "</span>";
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
        toolbar: "#toolBar",
        pagination: true,
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
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
            var allOutOfControlBtn = $(".cls_OutOfControl");
            $.each(allOutOfControlBtn, function (i, item) {
                $(item).unbind("click");
                $(item).click(function () {
                    var swbId = $(item).attr("swbId");
                    var swbNeedCheck = $(item).attr("swbNeedCheck");
                    UpdateTaxFeeConfirmStatus(swbId);
                });

            });

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

    function UpdateTaxFeeConfirmStatus(swbIDs) {
        var swbIDs_local = encodeURI(swbIDs);

        var bOK = false;
        $.ajax({
            type: "POST",
            url: UpdateWBNeedCheck + swbIDs_local,
            data: "",
            async: false,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'Info');
                    $("#btnQuery").click();
                } else {
                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
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
