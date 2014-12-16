$(function () {
    var _$_datagrid = $("#DG_SubWayBillDetail");
    var PrintURL = "";
    var _$_ddlSwbStatus = $("#ddlSwbStatus");
    var _$_ddlSortingTimes = $("#ddlSortingTimes");
    var LoadStatusJSONUrl = "/CustomerMain/CreateStatusJSON";
    var LoadSortJSONUrl = "/CustomerMain/CreateSortJSON";

    var RejectSubWayBillURL = "/ViewSubWayBill/PatchRejectSubWayBill?strSwbIds=";
    var UnReleaseSubWayBillURL = "/ViewSubWayBill/PatchUpdateSwbNeedCheck?strSwbIds=";

    _$_ddlSwbStatus.combotree({
        url: LoadStatusJSONUrl,
        valueField: 'id',
        textField: 'text',
        onLoadSuccess: function (node, data) {
            switch ($("#hid_swbStatus").val()) {
                case "":
                    _$_ddlSwbStatus.combotree("setValues", ["-99"]);
                    break;
                default:
                    _$_ddlSwbStatus.combotree("setValues", eval("[" + $("#hid_swbStatus").val() + "]"));
                    //_$_ddlSwbStatus.combotree("setValue", $("#hid_swbStatus").val());
                    break;
            }
        }
    });

    switch ($("#hid_swbStatus").val()) {
        case "":
            _$_ddlSwbStatus.combotree("setValues", ["-99"]);
            break;
        default:
            _$_ddlSwbStatus.combotree("setValues", eval("[" + $("#hid_swbStatus").val() + "]"));
            //_$_ddlSwbStatus.combotree("setValue", $("#hid_swbStatus").val());
            break;
    }

    _$_ddlSortingTimes.combobox({
        url: LoadSortJSONUrl,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    _$_ddlSortingTimes.combobox("setValue", "---请选择---");

    var QueryURL = "/ViewSubWayBill/GetData?Detail_wbSerialNum=" + encodeURI($("#txtCode_Detail").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_Detail").val()) + "&txtswbDescription_CHN=" + encodeURI($("#txtswbDescription_CHN").val()) + "&txtswbDescription_ENG=" + encodeURI($("#txtswbDescription_ENG").val()) + "&txtSwbStatus=" + encodeURI(_$_ddlSwbStatus.combotree("getValues").join(',')) + "&ddlSortingTimes=" + encodeURI(_$_ddlSortingTimes.combobox("getValue"));

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    $("#btnDetailQuery").click(function () {
        QueryURL = "/ViewSubWayBill/GetData?Detail_wbSerialNum=" + encodeURI($("#txtCode_Detail").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_Detail").val()) + "&txtswbDescription_CHN=" + encodeURI($("#txtswbDescription_CHN").val()) + "&txtswbDescription_ENG=" + encodeURI($("#txtswbDescription_ENG").val()) + "&txtSwbStatus=" + encodeURI(_$_ddlSwbStatus.combotree("getValues").join(',')) + "&ddlSortingTimes=" + encodeURI(_$_ddlSortingTimes.combobox("getValue"));
        window.setTimeout(function () {
            $.extend(_$_datagrid.treegrid("options"), {
                url: QueryURL
            });
            _$_datagrid.treegrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#btnPrint_dlg").click(function () {
        PrintURL = "/ViewSubWayBill/Print?Detail_wbSerialNum=" + encodeURI($("#txtCode_Detail").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_Detail").val()) + "&txtswbDescription_CHN=" + encodeURI($("#txtswbDescription_CHN").val()) + "&txtswbDescription_ENG=" + encodeURI($("#txtswbDescription_ENG").val()) + "&txtSwbStatus=" + encodeURI(_$_ddlSwbStatus.combotree("getValues").join(',')) + "&ddlSortingTimes=" + encodeURI(_$_ddlSortingTimes.combobox("getValue")) + "&order=" + _$_datagrid.treegrid("options").sortOrder + "&sort=" + _$_datagrid.treegrid("options").sortName + "&page=1&rows=10000000";
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


    $("#btnExcel_dlg").click(function () {
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

        PrintURL = "/ViewSubWayBill/Excel?Detail_wbSerialNum=" + encodeURI($("#txtCode_Detail").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode_Detail").val()) + "&txtswbDescription_CHN=" + encodeURI($("#txtswbDescription_CHN").val()) + "&txtswbDescription_ENG=" + encodeURI($("#txtswbDescription_ENG").val()) + "&txtSwbStatus=" + encodeURI(_$_ddlSwbStatus.combotree("getValues").join(',')) + "&ddlSortingTimes=" + encodeURI(_$_ddlSortingTimes.combobox("getValue")) + "&order=" + _$_datagrid.treegrid("options").sortOrder + "&sort=" + _$_datagrid.treegrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.treegrid("getRoots").length > 0) {
            window.open(PrintURL);
        } else {
            reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            return false;
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
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        singleSelect: false,
        idField: 'ID',
        treeField: 'swbSerialNum',
        columns: [[
                    { field: 'cb', checkbox: true },
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
                    { field: 'differentiateColor', title: '区分标记', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var sRet = "";
                            if (rowData.parentID != "top") {
                                if (rowData.mismatchCargoName == "1") {
                                    sRet = sRet + "<span type='mismatchCargoName' strID=" + rowData.ID + " class='ExplainDiffrentColor' font_color='Red' style='width:40px;background-color:Red'>&nbsp;</span>&nbsp;&nbsp;";
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
                    { field: 'swbNeedCheck', title: '预检状态', width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: true,
        toolbar: "#toolBarDetail",
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
        },
        onSortColumn: function (sort, order) {
            //_$_datagrid.datagrid("reload");
        },
        onLoadSuccess: function (row, data) {
            var allShowTaxRateSettingInfoBtn = $(".cls_ShowTaxRateSettingInfo");
            var allShowLastPickGoodsInfoBtn = $(".cls_ShowLastPickGoodsInfo");
            var allExplainDiffrentColorBtn = $(".ExplainDiffrentColor");

            _$_datagrid.treegrid("expandAll");
            delete _$_datagrid.treegrid('options').queryParams['id'];

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
                            url: ExplainDiffrentColorTipURL + encodeURI(strID) + "&Type=" +encodeURI(type) + "&font_color=" + encodeURI(font_color),
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

            if ($("#hid_bEnableReject").val() == "1" || $("#hid_bEnableUnRelease").val() == "1" || $("#hid_bEnableRelease").val() == "1") {
                //var AllChks = $(".datagrid-row input:checkbox");
                var AllSonChks = $(".treegrid-tr-tree input:checkbox");
                //            $.each(AllChks, function (i, item) {
                //                $(item).hide();
                //            });
                $.each(AllSonChks, function (i, item) {
                    $(item).hide();
                });
            }

        }
    });

    $(".datagrid-header-check input").hide();

    //    _$_datagrid.datagrid({
    //        iconCls: 'icon-save',
    //        nowrap: true,
    //        autoRowHeight: false,
    //        striped: true,
    //        collapsible: true,
    //        url: QueryURL,
    //        sortName: 'swbID',
    //        sortOrder: 'desc',
    //        remoteSort: true,
    //        border: false,
    //        idField: 'swbID',
    //        columns: [[
    //                    { field: 'cb', title: '', checkbox: true
    //                    },
    //                    { field: 'wbSerialNum', title: '总运单号', width: 100, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'wbCompany', title: '货代公司', width: 100, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'wbStorageDate', title: '报关日期', width: 60, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //					{ field: 'swbSerialNum', title: '分运单号', width: 100, sortable: true,
    //					    sorter: function (a, b) {
    //					        return (a > b ? 1 : -1);
    //					    }
    //					},
    //					{ field: 'swbDescription_CHN', title: '货物中文名', width: 160, sortable: true,
    //					    sorter: function (a, b) {
    //					        return (a > b ? 1 : -1);
    //					    }
    //					},
    //                    { field: 'swbDescription_ENG', title: '货物英文名', width: 140, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'swbNumber', title: '件数', width: 40, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'swbActualNumber', title: '实际扫描件数', width: 90, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'swbWeight', title: '重量(公斤)', width: 70, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //					{ field: 'swbNeedCheck', title: '状态', width: 80, sortable: true,
    //					    sorter: function (a, b) {
    //					        return (a > b ? 1 : -1);
    //					    }
    //					},
    //					{ field: 'swbID', title: '主键', hidden: true, width: 120, sortable: true,
    //					    sorter: function (a, b) {
    //					        return (a > b ? 1 : -1);
    //					    }
    //					}
    //				]],
    //        pagination: true,
    //        toolbar: "#toolBarDetail",
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

    var hid_bEnableReject = $("#hid_bEnableReject").val();
    var hid_bEnableUnRelease = $("#hid_bEnableUnRelease").val();
    var hid_bEnableRelease = $("#hid_bEnableRelease").val();
    _$_datagrid.treegrid('hideColumn', "cb");
    $("#btnSeleAll").hide();
    $("#btnInverseSele").hide();
    $("#btnReleaseWayBill").hide();
    $("#btnUnReleaseWayBill").hide();
    $("#btnRejectWayBill").hide();
    $("#TipForRejectSubWayBill").hide();
    if (hid_bEnableReject == "1" || hid_bEnableUnRelease == "1" || hid_bEnableRelease == "1") {
        _$_datagrid.treegrid('showColumn', "cb");
        $("#btnSeleAll").show();
        $("#btnInverseSele").show();
    }

    if (hid_bEnableReject == "1") {
        $("#btnRejectWayBill").show();
        $("#TipForRejectSubWayBill").show();
    }

    if (hid_bEnableUnRelease == "1") {
        $("#btnUnReleaseWayBill").show();
    }

    if (hid_bEnableRelease == "1") {
        $("#btnReleaseWayBill").show();
    }

    $("#btnSeleAll").click(function () {
        SeleAll();
    });

    $("#btnInverseSele").click(function () {
        InverseSele();
    });

    $("#btnRejectWayBill").click(function () {
        //RejectSubWayBill();
        UnReleaseSubWayBill(99)
    });

    $("#btnUnReleaseWayBill").click(function () {
        UnReleaseSubWayBill(3);
    });

    $("#btnReleaseWayBill").click(function () {
        UnReleaseSubWayBill(2);
    });

    function SeleAll() {
        var roots = _$_datagrid.treegrid("getRoots");
        for (var i = 0; i < roots.length; i++) {
            //console.info(roots[i].ID);
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

    function RejectSubWayBill() {
        reWriteMessagerConfirm("提示", "您确定需要将所选的分运单进行退货吗？</br><font style='color:red;font-weight:bold'>(退货后将无法再次入库，请慎重选择)</font>",
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.treegrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].ID);
                            }
                            if (selects.length == 0) {
                                reWriteMessagerAlert("提示", "<center>请选择需要进行退货的分运单</center>", "error");
                                return false;
                            }
                            $.ajax({
                                type: "POST",
                                url: RejectSubWayBillURL + encodeURI(ids.join(',')),
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
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });
                            _$_datagrid.treegrid("reload");
                            _$_datagrid.treegrid("unselectAll");
                        } else {

                        }
                    }
                );
    }

    function UnReleaseSubWayBill(iNeedCheck) {
        //        reWriteMessagerConfirm("提示", "您确定需要将所选的分运单进行退货吗？</br><font style='color:red;font-weight:bold'>(退货后将无法再次入库，请慎重选择)</font>",
        //                    function (ok) {
        //                        if (ok) {
        //                           
        //                        } else {

        //                        }
        //                    }
        //                );
        var selects = _$_datagrid.treegrid("getSelections");
        var ids = [];
        for (var i = 0; i < selects.length; i++) {
            ids.push(selects[i].ID);
        }
        if (selects.length == 0) {
            reWriteMessagerAlert("提示", "<center>请先选择分运单</center>", "error");
            return false;
        }
        $.ajax({
            type: "POST",
            url: UnReleaseSubWayBillURL + encodeURI(ids.join(',')) + "&iNeedCheck=" + iNeedCheck,
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
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
        _$_datagrid.treegrid("reload");
        _$_datagrid.treegrid("unselectAll");
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:150px;"></div>').appendTo('body');
        var fields = _$_datagrid.treegrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.treegrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "swbserialnum":
                    break;
                case "swbid":
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
                if ($(item.text).attr("id") == "swbSerialNum") {

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
