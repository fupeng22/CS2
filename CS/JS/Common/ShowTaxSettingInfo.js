$(function () {
    var LoadTaxRateSettingInfoURL = "/ForTooltip/LoadTaxRateSettingInfo?TaxNO=" + encodeURI($("#hd_TaxNO").val());

    var GetSessionTaxNOURL = "/ForTooltip/GetSessionTaxNO";

    if ($.fn.propertygrid) {
        $.fn.propertygrid.defaults.columns[0][0].title = "<span style='color: #ff0000;'>项名称</span>"; // 对应Name
        $.fn.propertygrid.defaults.columns[0][1].title = "<span style='color: #ff0000;'>项值</span>"; // 对应Value       
    }

    var bOK = false;
    var SessionTaxNO = "";
    $.ajax({
        type: "POST",
        url: GetSessionTaxNOURL,
        data: "",
        async: false,
        cache: false,
        beforeSend: function (XMLHttpRequest) {

        },
        success: function (msg) {
            SessionTaxNO = msg;
            bOK = true;
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function () {

        }
    });

    if (bOK) {
        console.info(SessionTaxNO);
        $("#pgTaxRateSettingInfo").propertygrid({
            width: 200,
            //height: 500,
            url: "/ForTooltip/LoadTaxRateSettingInfo?TaxNO=" + encodeURI(SessionTaxNO) + "&d=" + Date(),
            columns: [[
                    {
                        field: 'name', title: 'Name', width: 50, sortable: true
                    }, {
                        field: 'value', title: 'Value', width: 100, sortable: true
                    }]],
            showGroup: false,
            nowrap: false,
            scrollbarSize: 0,
            showHeader: false
        });
    }
});
   
