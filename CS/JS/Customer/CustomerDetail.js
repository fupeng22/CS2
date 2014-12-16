function Append_hid_Checked_swbSerialNum(v) {
    if ($("#hid_unChecked_swbSerialNum").val() != "") {
        var hid_unChecked_swbSerialNum_val = $("#hid_unChecked_swbSerialNum").val().split("*");
        var hid_unChecked_swbSerialNum_newVal = "";
        for (var i = 0; i < hid_unChecked_swbSerialNum_val.length; i++) {
            if (hid_unChecked_swbSerialNum_val[i] != "") {
                if (hid_unChecked_swbSerialNum_val[i] == v) {

                } else {
                    hid_unChecked_swbSerialNum_newVal = hid_unChecked_swbSerialNum_newVal + hid_unChecked_swbSerialNum_val[i] + "*";
                }
            }

        }
        $("#hid_unChecked_swbSerialNum").val(hid_unChecked_swbSerialNum_newVal);
    }

    var hid_Checked_swbSerialNum_val = $("#hid_Checked_swbSerialNum").val().split("*");
    var bExist = false;
    for (var i = 0; i < hid_Checked_swbSerialNum_val.length; i++) {
        if (hid_Checked_swbSerialNum_val[i] == v) {
            bExist = true;
        }
    }
    if (!bExist) {
        $("#hid_Checked_swbSerialNum").val($("#hid_Checked_swbSerialNum").val() + v + "*");
    }
}

function Append_hid_unChecked_swbSerialNum(v) {
    if ($("#hid_Checked_swbSerialNum").val() != "") {
        var hid_Checked_swbSerialNum_val = $("#hid_Checked_swbSerialNum").val().split("*");
        var hid_Checked_swbSerialNum_newVal = "";
        for (var i = 0; i < hid_Checked_swbSerialNum_val.length; i++) {
            if (hid_Checked_swbSerialNum_val[i] != "") {
                if (hid_Checked_swbSerialNum_val[i] == v) {

                } else {
                    hid_Checked_swbSerialNum_newVal = hid_Checked_swbSerialNum_newVal + hid_Checked_swbSerialNum_val[i] + "*";
                }
            }
        }
        $("#hid_Checked_swbSerialNum").val(hid_Checked_swbSerialNum_newVal);

    }

    var hid_unChecked_swbSerialNum_val = $("#hid_unChecked_swbSerialNum").val().split("*");
    var bExist = false;
    for (var i = 0; i < hid_unChecked_swbSerialNum.length; i++) {
        if (hid_unChecked_swbSerialNum[i] == v) {
            bExist = true;
        }
    }
    if (!bExist) {
        $("#hid_unChecked_swbSerialNum").val($("#hid_unChecked_swbSerialNum").val() + v + "*");
    }
}

function Append_hid_Default_Checked_swbId(v) {
    if ($("#hid_Default_UnSele").val() != "") {
        var hid_unChecked_swbId_val = $("#hid_Default_UnSele").val().split("*");
        var hid_unChecked_swbId_newVal = "";
        for (var i = 0; i < hid_unChecked_swbId_val.length; i++) {
            if (hid_unChecked_swbId_val[i] != "") {
                if (hid_unChecked_swbId_val[i] == v) {

                } else {
                    hid_unChecked_swbId_newVal = hid_unChecked_swbId_newVal + hid_unChecked_swbId_val[i] + "*";
                }
            }

        }
        $("#hid_Default_UnSele").val(hid_unChecked_swbId_newVal);
    }

    var hid_Checked_swbId_val = $("#hid_Default_Sele").val().split("*");
    var bExist = false;
    for (var i = 0; i < hid_Checked_swbId_val.length; i++) {
        if (hid_Checked_swbId_val[i] == v) {
            bExist = true;
        }
    }
    if (!bExist) {
        $("#hid_Default_Sele").val($("#hid_Default_Sele").val() + v + "*");
    }
}

function Append_hid_Default_UnChecked_swbId(v) {
    if ($("#hid_Default_Sele").val() != "") {
        var hid_Checked_swbId_val = $("#hid_Default_Sele").val().split("*");
        var hid_Checked_swbId_newVal = "";
        for (var i = 0; i < hid_Checked_swbId_val.length; i++) {
            if (hid_Checked_swbId_val[i] != "") {
                if (hid_Checked_swbId_val[i] == v) {

                } else {
                    hid_Checked_swbId_newVal = hid_Checked_swbId_newVal + hid_Checked_swbId_val[i] + "*";
                }
            }
        }
        $("#hid_Default_Sele").val(hid_Checked_swbId_newVal);

    }

    var hid_unChecked_swbId_val = $("#hid_Default_UnSele").val().split("*");
    var bExist = false;
    for (var i = 0; i < hid_unChecked_swbId_val.length; i++) {
        if (hid_unChecked_swbId_val[i] == v) {
            bExist = true;
        }
    }
    if (!bExist) {
        $("#hid_Default_UnSele").val($("#hid_Default_UnSele").val() + v + "*");
    }
}

$(function () {
    var _$_datagrid = $("#DG_SubWayBillDetail");
    var QueryURL = "/Customer_Detail/GetData?Detail_wbID=" + encodeURI($("#hid_wbID").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode").val());

    var LoadStandardTaxRateInfoTipURL = "/ForTooltip/LoadTaxRateSettingInfo_Html?TaxNO=";
    var LoadLastPickGoodsInfoTipURL = "/ForTooltip/LoadLastPickGoodInfo_Html?IDCard=";
    var ExplainDiffrentColorTipURL = "/ForTooltip/ExplainDiffrentColorInfo_Html?strID=";

    $("#btnQuery_Detail").click(function () {
        QueryURL = "/Customer_Detail/GetData?Detail_wbID=" + encodeURI($("#hid_wbID").val()) + "&Detail_swbSerialNum=" + encodeURI($("#txtSubWayBillCode").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid.treegrid("options"), {
                url: QueryURL
            });
            _$_datagrid.treegrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    $("#txtSubWayBillCode").focus();

    $("#hid_Checked_swbSerialNum").val("");
    $("#hid_unChecked_swbSerialNum").val("");

    $("#hid_Default_UnSele").val("");
    $("#hid_Default_Sele").val("");

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
        rowStyler: function (rowData) {
            //任务完成100%， 并且已审核通过，背景色置灰 
            if (rowData.chkNeedCheck == "1") {
                return 'background-color:#FF9966;';
            }
            //            if (rowData && rowData.status && (rowData.status == 'TASK_ASSIGNER_AUDITED' || rowData.status == 'TASK_MONITOR_AUDITED') && rowData.finishRate == 100) {
            //                return 'background-color:#d9d9c2;';
            //            }
        },
        columns: [[
                    { field: 'cb', title: '', checkbox: true, width: 40, align: 'center'
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
					{ field: 'swbSerialNum', title: '分运单号', width: 180, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    },
					    formatter: function (value, rowData, rowIndex) {
					        var sRet = "";
					        if (rowData.parentID == "top") {
					            //sRet = "<input type='checkbox' id='chk_SubWayBillMain_" + rowData.ID + "' name='chk_SubWayBillMain_" + rowData.ID + "' class='clsSubWayBillMain'>" + rowData.swbSerialNum;
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
                    { field: 'currentSele', title: '<font style="color:red;">当前所选行</font>', width: 120,
                        formatter: function (value, rowData, rowIndex) {
                            return "<span class='currentRows' select='0' currentRowSwbSerialNum='" + rowData.ID + "'></span>";
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
            var rowData = data.rows;
            if (rowData) {
                for (var i = 0; i < rowData.length; i++) {
                    if (rowData[i].chkNeedCheck == "1") {
                        //_$_datagrid.treegrid("select", rowData[i].ID);
                        Append_hid_Default_Checked_swbId(rowData[i].ID);
                    }
                    if (rowData[i].chkNeedCheck == "0") {
                        //_$_datagrid.treegrid("unselect", rowData[i].ID);
                        Append_hid_Default_UnChecked_swbId(rowData[i].ID);
                    }
                }
            }

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

            $("#hid_Checked_swbSerialNum").val("");
            $("#hid_unChecked_swbSerialNum").val("");

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

            var _globalIds_Sele = $("#hid_Default_Sele").val().split("*");
            var _globalIds_Unsele = $("#hid_Default_UnSele").val().split("*");

            var allRoots = null;
            allRoots = _$_datagrid.treegrid("getRoots");
            if (allRoots) {
                for (var j = 0; j < allRoots.length; j++) {
                    for (var i = 0; i < _globalIds_Sele.length; i++) {
                        if (allRoots[j].ID == _globalIds_Sele[i]) {
                            _$_datagrid.treegrid("select", _globalIds_Sele[i]);
                        }
                    }

                    for (var k = 0; k < _globalIds_Unsele.length; k++) {
                        if (allRoots[j].ID == _globalIds_Unsele[k]) {
                            _$_datagrid.treegrid("unselect", _globalIds_Unsele[k]);
                        }
                    }
                }
            }

        },
        onCheck: function (rowData) {
            if (rowData.parentID == "top") {
                Append_hid_Checked_swbSerialNum(rowData.swbID);
            } else {
                _$_datagrid.treegrid("unselect", rowData.ID);
            }
        },
        onSelect: function (rowData) {
            if (rowData.parentID == "top") {
                Append_hid_Checked_swbSerialNum(rowData.swbID);
            } else {
                _$_datagrid.treegrid("unselect", rowData.ID);
            }
        },
        onUnselect: function (rowData) {
            Append_hid_unChecked_swbSerialNum(rowData.swbID);
        },
        onUncheck: function (rowData) {
            Append_hid_unChecked_swbSerialNum(rowData.swbID);
        },
        onClickRow: function (rowData) {
            if (rowData.parentID != "top") {
                _$_datagrid.treegrid("unselect", rowData.ID);
            } else {
                setCurrentSele(rowData.ID, true, 0);
            }
            $("#txtSubWayBillCode").focus();
        }
    });

    $(".datagrid-header-check input").hide();

    function getRowIndexBySwbSerialNum(ID) {
        var i = -1;
        var rows = _$_datagrid.treegrid("getRoots");
        for (var j = 0; j < rows.length; j++) {
            if (rows[j].ID == ID) {
                i = j;
            }
        }
        return i;
    }

    function setCurrentSele(ID, bMouseClick, keyCode) {
        var bSele = false;
        var rowIndex = -1;
        var preSeleSwbSerialNum = "";
        var allCurrentRows = $(".currentRows");
        if (bMouseClick) {//鼠标单击
            $.each(allCurrentRows, function (i, item) {
                if ($(item).attr("currentRowSwbSerialNum") == ID) {
                    $(item).html("<img  src='../../images/Customer/currentsele.jpg' />");
                    $(item).attr("select", "1");
                } else {
                    $(item).html("");
                    $(item).attr("select", "0");
                }
            });
        } else {
            bSele = false;
            $.each(allCurrentRows, function (i, item) {
                if ($(item).attr("select") == "1") {
                    bSele = true;
                    preSeleSwbSerialNum = $(item).attr("currentRowSwbSerialNum");
                }
            });
            if (bSele) {//先前有选择
                rowIndex = getRowIndexBySwbSerialNum(preSeleSwbSerialNum);

                if (rowIndex == -1) {//先前没选择
                    switch (keyCode) {
                        case 38: // up
                            SetSeleStyle(_$_datagrid.treegrid("getRoots").length - 1);
                            break;
                        case 40: // down
                            SetSeleStyle(0);
                            break;
                        case 13: //回车
                            SaveStatus();
                            break;
                        case 9: //Tab键
                            ProcessTab();
                            break;
                    }
                } else {//先前选择了第rowIndex行
                    switch (keyCode) {
                        case 38: // up
                            if (rowIndex == 0) {//先前选择的是第一行,现在就不管

                            } else {
                                SetSeleStyle(rowIndex - 1);
                            }
                            break;
                        case 40: // down
                            if (rowIndex == _$_datagrid.treegrid("getRoots").length - 1) {//先前选择的是最后一行,现在就不管

                            } else {
                                SetSeleStyle(rowIndex + 1);
                            }
                            break;
                        case 13: //回车
                            SaveStatus();
                            break;
                        case 9: //Tab键
                            ProcessTab();
                            break;
                    }
                }
            } else {//先前没选择
                switch (keyCode) {
                    case 38: // up
                        SetSeleStyle(_$_datagrid.treegrid("getRoots").length - 1);
                        break;
                    case 40: // down
                        SetSeleStyle(0);
                        break;
                    case 13: //回车
                        SaveStatus();
                        break;
                    case 9: //Tab键
                        ProcessTab();
                        break;
                }
            }
        }
    }

    _$_datagrid.treegrid('getPanel').keydown(function (e) {
        if (e.ctrlKey && e.which == 13 || e.which == 10) {
            ProcessCtrlEnter();
        } else if (e.shiftKey && e.which == 13 || e.which == 10) {
            ProcessCtrlEnter();
        } else {
            setCurrentSele("", false, e.keyCode);
        }
    });

    $("#txtSubWayBillCode").keyup(function (e) {
        switch (e.keyCode) {
            case 13:
                $("#btnQuery_Detail").click();
                break;
            default:
                break;
        }
    });

    function SetSeleStyle(rowIndex) {
        var allCurrentRows = $(".currentRows");
        var ID = "";
        ID = _$_datagrid.treegrid('getRoots')[rowIndex].ID;

        $.each(allCurrentRows, function (i, item) {
            if ($(item).attr("currentRowSwbSerialNum") == ID) {
                $(item).html("<img  src='../../images/Customer/currentsele.jpg' />");
                $(item).attr("select", "1");
            } else {
                $(item).html("");
                $(item).attr("select", "0");
            }
        });

        var bCheckPre = false;

        var chkRows = _$_datagrid.treegrid('getSelections');

        for (var i = 0; i < chkRows.length; i++) {
            if (chkRows[i].ID == ID) {
                bCheckPre = true;
            }
        }
        if (bCheckPre) {
            _$_datagrid.treegrid('unselect', ID);
        } else {
            _$_datagrid.treegrid('select', ID);
        }
    }

    function SaveStatus() {

    }

    function ProcessCtrlEnter() {
        self.parent.$("#dlg_GlobalDetail").find(".dialog-button").find(".icon-ok").click();
        //        var allSelectNode = $("#hid_Checked_swbSerialNum").val();
        //        var allUnSelectNode = $("#hid_unChecked_swbSerialNum").val();
        $("#btnQuery_Detail").click();
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
