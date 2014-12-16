$(function () {
    var _$_datagrid = $("#DG_MainQueryResult");
    var _$_ddCompany = $('#txtVoyage');
    var _$_ddlInOutStoreType = $("#ddlInOutStoreType");
    var QueryCompanyURL = "/ForwarderMain/LoadComboxJSON";

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    _$_ddlInOutStoreType.combotree('loadData', [
    {
        id: -99,
        text: '---请选择(可多选)---'
    },
    {
        id: 1,
        text: '正常已入库'
    },
    {
        id: 3,
        text: '正常已出库'
    }]);

    _$_ddlInOutStoreType.combotree("setValue", "-99");

    var PrintSubWayBill = "";
    var LoadSubWayBillURL = "/Huayu_QueryMainFrame/GetSubWayBill?txtWBID=";

    _$_ddCompany.combobox({
        url: QueryCompanyURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    $('#ddlCheckStatus').combobox({
        data: [{ "text": "---请选择---", "id": "---请选择---" }, { "text": "放行", "id": "0" }, { "text": "等待预检", "id": "1" }, { "text": "查验放行", "id": "2" }, { "text": "查验扣留", "id": "3" }, { "text": "查验待处理", "id": "4"}, { "text": "查验退货", "id": "99"}],
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    //    $('#ddlInOutStoreType').combobox({
    //        data: [{ "text": "---请选择---", "id": "---请选择---" }, { "text": "正常已入库", "id": "1" }, { "text": "正常已出库", "id": "3"}],
    //        valueField: 'id',
    //        textField: 'text',
    //        editable: false,
    //        panelHeight: null
    //    });

    $('#txtNeedCheckStatus').combobox({
        data: [{ "text": "---请选择---", "id": "---请选择---" }, { "text": "放行", "id": "0" }, { "text": "等待预检", "id": "1" }, { "text": "查验放行", "id": "2" }, { "text": "查验扣留", "id": "3" }, { "text": "查验待处理", "id": "4"}, { "text": "查验退货", "id": "99"}],
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null
    });

    //$('#ddlInOutStoreType').combobox("setValue", "---请选择---");
    $('#ddlCheckStatus').combobox("setValue", "---请选择---");
    _$_ddCompany.combobox("setValue", "---请选择---");

    var PrintURL = "";
    var QueryURL = "/Huayu_QueryMainFrame/GetData?inputBeginDate=" + encodeURI($("#txtBeginD").val()) + "&inputEndDate=" + encodeURI($("#txtEndD").val()) + "&txtCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtWBSerialNum=" + encodeURI($("#txtCode").val()) + "&txtSWBSerialNum=" + encodeURI($("#txtSubWayBillCode").val() + "&txtSWBNeedCheck=" + encodeURI($('#ddlCheckStatus').combobox("getValue")) + "&InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combotree("getValues").join(',')));

    $("#btnQuery").click(function () {
        QueryURL = "/Huayu_QueryMainFrame/GetData?inputBeginDate=" + encodeURI($("#txtBeginD").val()) + "&inputEndDate=" + encodeURI($("#txtEndD").val()) + "&txtCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtWBSerialNum=" + encodeURI($("#txtCode").val()) + "&txtSWBSerialNum=" + encodeURI($("#txtSubWayBillCode").val() + "&txtSWBNeedCheck=" + encodeURI($('#ddlCheckStatus').combobox("getValue")) + "&InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combotree("getValues").join(',')));
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 20); //延迟100毫秒执行，时间可以更短
    });

    $("#btnReset").click(function () {
        $("#txtBeginD").val("");
        $("#txtEndD").val("");
        $("#txtCode").val("");
        $("#txtSubWayBillCode").val("");
        _$_ddCompany.combobox("setValue", "---请选择---");
        $('#ddlCheckStatus').combobox("setValue", "---请选择---");
        _$_ddlInOutStoreType.combotree("setValue", "-99");
        $('#txtNeedCheckStatus').combotree("setValue", "-99");
        $("#btnQuery").click();
    });

    $("#btnPrint").click(function () {
        PrintURL = "/Huayu_QueryMainFrame/PrintWayBillInfo?inputBeginDate=" + encodeURI($("#txtBeginD").val()) + "&inputEndDate=" + encodeURI($("#txtEndD").val()) + "&txtCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtWBSerialNum=" + encodeURI($("#txtCode").val()) + "&txtSWBSerialNum=" + encodeURI($("#txtSubWayBillCode").val() + "&txtSWBNeedCheck=" + encodeURI($('#ddlCheckStatus').combobox("getValue")) + "&InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combotree("getValues").join(','))) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
            div_PrintDlg.show();
            var PrintDlg = null;

//           div_PrintDlg.find("#p").show();
//            div_PrintDlg.find("#frmPrintURL").load(function(){
//                div_PrintDlg.find("#p").hide();
//            });
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

        PrintURL = "/Huayu_QueryMainFrame/ExcelWayBillInfo?inputBeginDate=" + encodeURI($("#txtBeginD").val()) + "&inputEndDate=" + encodeURI($("#txtEndD").val()) + "&txtCompany=" + encodeURI(_$_ddCompany.combobox("getValue")) + "&txtWBSerialNum=" + encodeURI($("#txtCode").val()) + "&txtSWBSerialNum=" + encodeURI($("#txtSubWayBillCode").val() + "&txtSWBNeedCheck=" + encodeURI($('#ddlCheckStatus').combobox("getValue")) + "&InOutStoreType=" + encodeURI(_$_ddlInOutStoreType.combotree("getValues").join(','))) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
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
        sortName: 'wbID',
        sortOrder: 'desc',
        remoteSort: true,
        border: false,
        idField: 'wb_swb_ID',
        view: detailview,
        singleSelect: true,
        showFooter: true,
        detailFormatter: function (index, row) {
            //return '<div style="padding:2px"><center><span style="color:red;font-weight:bold">[<span id="WayBillSerialNum' + index + '"></span>]子运单明细</span></center><table  id="ddv-' + index + '"></table></div>';
            //return '<div style="padding:2px"><table  id="ddv-' + index + '"></table></div>';
            return '<div style="padding:2px"><center><span style="color:red;font-weight:bold">[<span id="WayBillSerialNum' + index + '"></span>]子运单明细</span></center><table  id="ddv-' + index + '"></table></div></br>';
        },
        columns: [[
                    { field: 'wbSerialNum', title: '总运单号', width: 150
                        //                    ,
                        //                        formatter: function (value, row, index) {
                        //                            return "<a href='#' class='link_wbSerialNum' wbSerialNum='" + row.wbSerialNum + "'>" + row.wbSerialNum + "</a>";
                        //                        }
                    },
					{ field: 'wbCompany', title: '货代公司', width: 200
					},
					{ field: 'wbTotalNumbe', title: '总运单件数', width: 150, align: "center",
					    formatter: function (value, rowData, rowIndex) {
                            if (rowData.wbTotalNumbe.indexOf("wbTotalNumbe")>-1) 
                            {
                                    return rowData.wbTotalNumbe.replace("wbTotalNumbe","");
                            }else{
                                return "<center><a href='#' class='load_InOutStoreSubWayBill' wbID='" + rowData.wbID + "' InOutType='-99' rowIndex='" + rowIndex + "'>" + rowData.wbTotalNumbe + "</a></center>";
                                }
					        
					    }
					},
                    { field: 'wbTotalWeight', title: '总运单重量', width: 150, align:"center"
                    },
					{ field: 'InStoreCount', title: '已入库分单数', width: 110,align: "center",
					    formatter: function (value, rowData, rowIndex) {
                            if (rowData.InStoreCount.indexOf("InStoreCount")>-1) 
                            {
                                    return rowData.InStoreCount.replace("InStoreCount","");
                            }else{
                                return "<center><a href='#' class='load_InOutStoreSubWayBill' wbID='" + rowData.wbID + "' InOutType='1' rowIndex='" + rowIndex + "'>" + rowData.InStoreCount + "</a></center>";
                                }
					        
					    }
					},
                    { field: 'OutStoreCount', title: '已出库分单数', width: 110,  align: "center",
                        formatter: function (value, rowData, rowIndex) {
                        if (rowData.OutStoreCount.indexOf("OutStoreCount")>-1) 
                            {
                                    return rowData.OutStoreCount.replace("OutStoreCount","");
                            }else{
                            return "<center><a href='#' class='load_InOutStoreSubWayBill' wbID='" + rowData.wbID + "' InOutType='3' rowIndex='" + rowIndex + "'>" + rowData.OutStoreCount + "</a></center>";
                            }
                        }
                    },
                    { field: 'StoreCount', title: '库存件数', width: 110, align: "center",
                        formatter: function (value, rowData, rowIndex) {
                        if (rowData.StoreCount.indexOf("StoreCount")>-1) 
                            {
                                    return rowData.StoreCount.replace("StoreCount","");
                            }else{
                            return "<center><a href='#' class='load_InOutStoreSubWayBill' wbID='" + rowData.wbID + "' InOutType='-1' rowIndex='" + rowIndex + "'>" + rowData.StoreCount + "</a></center>";
                            }
                        }
                    },
                    { field: 'NotInStoreCount', title: '未入库件数', width: 130,  align: "center",
                        formatter: function (value, rowData, rowIndex) {
                        if (rowData.NotInStoreCount.indexOf("NotInStoreCount")>-1) 
                            {
                                    return rowData.NotInStoreCount.replace("NotInStoreCount","");
                            }else{
                            return "<center><a href='#' class='load_InOutStoreSubWayBill' wbID='" + rowData.wbID + "' InOutType='99' rowIndex='" + rowIndex + "'>" + rowData.NotInStoreCount + "</a></center>";
                            }
                        }
                    },
                    { field: 'wbID', title: '', hidden: true, width: 120, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: true,
        pageSize: 15,
        pageList: [15,20,25,30,35,40,45,50],
        toolbar: "#toolBar",
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
            _$_datagrid.datagrid("reload");
        },
        onLoadSuccess: function (data) {
            delete $(this).datagrid('options').queryParams['id'];
            if ($("#txtSubWayBillCode").val() != "") {
                for (var i = 0; i < data.rows.length; i++) {
                    _$_datagrid.datagrid("expandRow", i);
                }
            }

            var allViewInOutStoreLnk = $(".load_InOutStoreSubWayBill");
//            $.each(allViewInOutStoreLnk, function (i, item) {
//                $(item).click(function () {
//                    var _$_wbID = $(item).attr("wbID");
//                    var _$_InOutType = $(item).attr("InOutType");
//                    var _$_rowIndex = $(item).attr("rowIndex");

//                    var _$_subDataGrid = $('#ddv-' + _$_rowIndex);
//                    $("#WayBillSerialNum" + _$_rowIndex).html(data.rows[_$_rowIndex].wbSerialNum);

//                    var expander = _$_datagrid.datagrid('getExpander', _$_rowIndex);
////                    if (expander.hasClass('datagrid-row-collapse')) {
////                        _$_datagrid.datagrid("collapseRow",  _$_rowIndex);
////                    } else {
//                        _$_datagrid.datagrid("expandRow", _$_rowIndex);
//                        window.setTimeout(function () {
//                            $.extend(_$_subDataGrid.datagrid("options"), {
//                                url: LoadSubWayBillURL + _$_wbID + "&InOutType="+_$_InOutType+"&strSwbSerialNum=",
//                            });
//                            _$_subDataGrid.datagrid("reload");
//                        },10);
////                    }

//                });
//            });

            $.each(allViewInOutStoreLnk, function (i, item) {
                $(item).click(function () {
                    var _$_wbID = $(item).attr("wbID");
                    var _$_InOutType = $(item).attr("InOutType");
                    var _$_rowIndex = $(item).attr("rowIndex");

                    var _$_subDataGrid = $('#ddv-' + _$_rowIndex);
                    $("#WayBillSerialNum" + _$_rowIndex).html(data.rows[_$_rowIndex].wbSerialNum);

                    var expander = _$_datagrid.datagrid('getExpander', _$_rowIndex);
                    window.setTimeout(function () {
                        $.extend(_$_subDataGrid.treegrid("options"), {
                            url: LoadSubWayBillURL + _$_wbID + "&InOutType="+_$_InOutType+"&strSwbSerialNum=",
                        });
                        _$_subDataGrid.treegrid("reload");
                    },10);
                   _$_datagrid.datagrid("expandRow", _$_rowIndex);
                });
            });
        },
        onClickRow: function (index, row) {
            var expander = _$_datagrid.datagrid('getExpander', index);
            if (expander.hasClass('datagrid-row-collapse')) {
                _$_datagrid.datagrid("collapseRow", index);
            } else {
                _$_datagrid.datagrid("expandRow", index);
            }
        },
        onExpandRow: function (index, row) {
            var _$_subDataGrid = $('#ddv-' + index);
            $("#WayBillSerialNum" + index).html(row.wbSerialNum);

            _$_subDataGrid.treegrid({
                    iconCls: 'icon-save',
                    nowrap: true,
                    autoRowHeight: false,
                    autoRowWidth: false,
                    striped: true,
                    collapsible: true,
                    url: LoadSubWayBillURL + row.wbID + "&InOutType=-99&strSwbSerialNum=" + encodeURI($("#txtSubWayBillCode").val()),
                    sortName: 'swbID',
                    sortOrder: 'desc',
                    remoteSort: true,
                    border: true,
                    height: 'auto',
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
                               { field: 'FinalStatusDecription', title: '出入库类型', width: 100, sortable: true,
                                    sorter: function (a, b) {
                                        return (a > b ? 1 : -1);
                                    },
                                    formatter: function (value, rowData, rowIndex) {
                                        var sRet = "";
                                        if (rowData.parentID == "top") {
                                            sRet = rowData.FinalStatusDecription;
                                        } else {
                                            sRet = "";
                                        }
                                        return sRet;
                                    }
                                },
                                { field: 'InOutStoreDate', title: '出入库日期', width: 100, sortable: true,
                                    sorter: function (a, b) {
                                        return (a > b ? 1 : -1);
                                    },
                                    formatter: function (value, rowData, rowIndex) {
                                        var sRet = "";
                                        if (rowData.parentID == "top") {
                                            sRet = rowData.InOutStoreDate;
                                        } else {
                                            sRet = "";
                                        }
                                        return sRet;
                                    }
                                },
                                { field: 'InOutStoreOperator', title: '出入库操作员', width: 120, sortable: true,
                                    sorter: function (a, b) {
                                        return (a > b ? 1 : -1);
                                    },
                                    formatter: function (value, rowData, rowIndex) {
                                        var sRet = "";
                                        if (rowData.parentID == "top") {
                                            sRet = rowData.InOutStoreOperator;
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

                        delete _$_subDataGrid.treegrid('options').queryParams['id'];
                        _$_subDataGrid.treegrid("expandAll");
                    },
                    onCheck: function (rowData) {
                        if (rowData.parentID == "top") {

                        } else {
                            //_$_subDataGrid.treegrid("unselect", rowData.ID);
                        }
                    },
                    onClickRow: function (rowData) {
                        if (rowData.parentID != "top") {
                            //_$_subDataGrid.treegrid("unselect", rowData.ID);
                        } else {

                        }
                    }
                });
                _$_subDataGrid.treegrid('fixDetailRowHeight', index);





//            _$_subDataGrid.datagrid({
//                url: LoadSubWayBillURL + row.wbID + "&InOutType=-99&strSwbSerialNum=" + encodeURI($("#txtSubWayBillCode").val()),
//                fitColumns: true,
//                //singleSelect: true,
//                //rownumbers: true,
//                loadMsg: '',
//                height: 'auto',
//                iconCls: 'icon-save',
//                nowrap: true,
//                autoRowHeight: false,
//                autoRowWidth: false,
//                striped: true,
//                collapsible: true,
//                sortName: 'swbID',
//                sortOrder: 'desc',
//                remoteSort: true,
//                border: true,
//                idField: 'swbID',
//                columns: [[
//                    { field: 'swbSerialNum', title: '分运单号', width: 170, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },{ field: 'swbDescription_CHN', title: '货物中文名', width: 200, sortable: true,
//					    sorter: function (a, b) {
//					        return (a > b ? 1 : -1);
//					    }
//					},{ field: 'swbDescription_ENG', title: '货物英文名', width: 200, sortable: true,
//					    sorter: function (a, b) {
//					        return (a > b ? 1 : -1);
//					    }
//					},
//					{ field: 'swbNumber', title: '件数', width: 60, sortable: true,
//					    sorter: function (a, b) {
//					        return (a > b ? 1 : -1);
//					    }
//					},
//					{ field: 'swbWeight', title: '重量', width: 80, sortable: true,
//					    sorter: function (a, b) {
//					        return (a > b ? 1 : -1);
//					    }
//					},
//                    { field: 'FinalStatusDecription', title: '出入库类型', width: 100, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'InOutStoreDate', title: '出入库日期', width: 100, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'InOutStoreOperator', title: '出入库操作员', width: 120, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'swbNeedCheckDescription', title: '海关预检状态', width: 120, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'swbID', title: '', hidden: true, width: 120, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    }
//				]],
//                pagination: true,
//                toolbar: [{
//                    id: 'btnPrint',
//                    text: '打印',
//                    disabled: false,
//                    iconCls: 'icon-print',
//                    handler: function () {
//                    var _$_url=_$_subDataGrid.datagrid('options').url;
//                     PrintSubWayBillInfo(_$_subDataGrid, row.wbID,_$_url.substring(_$_url.indexOf("InOutType"),_$_url.length).split("&")[0].split("=")[1]);
//                    }
//                }, '-', {
//                    id: 'btnExcel',
//                    text: '导出',
//                    disabled: false,
//                    iconCls: 'icon-excel',
//                    handler: function () {
//                    var _$_url=_$_subDataGrid.datagrid('options').url;
//                        ExcelSubWayBillInfo(_$_subDataGrid, row.wbID,_$_url.substring(_$_url.indexOf("InOutType"),_$_url.length).split("&")[0].split("=")[1]);
//                    }
//                }],
//                onResize: function () {
//                    _$_datagrid.datagrid('fixDetailRowHeight', index);
//                },
//                onLoadSuccess: function () {
//                    setTimeout(function () {
//                        _$_datagrid.datagrid('fixDetailRowHeight', index);
//                    }, 0);
//                }
//            });
//            _$_datagrid.datagrid('fixDetailRowHeight', index);







        }
    });

    function PrintSubWayBillInfo(dg, wbID,InOutType) {
        PrintSubWayBill = "/Huayu_QueryMainFrame/PrintSubWayBillInfo?txtWBID=" + encodeURI(wbID) +"&InOutType="+InOutType+ "&strSwbSerialNum=" + encodeURI($("#txtSubWayBillCode").val()) + "&order=" + dg.datagrid("options").sortOrder + "&sort=" + dg.datagrid("options").sortName + "&page=1&rows=10000000";
        if (dg.datagrid("getData").rows.length > 0) {
            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
            div_PrintDlg.show();
            var PrintDlg = null;
            div_PrintDlg.find("#frmPrintURL").attr("src", PrintSubWayBill);
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
    }

    function ExcelSubWayBillInfo(dg, wbID,InOutType) {
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

        PrintURL = "/Huayu_QueryMainFrame/ExcelSubWayBillInfo?txtWBID=" + encodeURI(wbID) +"&InOutType="+InOutType+ "&strSwbSerialNum=" + encodeURI($("#txtSubWayBillCode").val()) + "&order=" + dg.datagrid("options").sortOrder + "&sort=" + dg.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (dg.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            return false;
        }
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:200px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "wbserialnum":
                    break;
                case "wbid":
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
