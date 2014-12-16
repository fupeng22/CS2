$(function () {
    var SaveDataURL = "/ValueAddedTaxSheetSetting/SaveData";
    $("#btnSave").click(function () {
        SaveData();
    });

    function SaveData() {
        $('#form_TaxSheetItems').form('submit', {
            url: SaveDataURL,
            onSubmit: function () {
                var win = $.messager.progress({
                    title: '请稍等',
                    msg: '正在处理数据……'
                });
            },
            success: function (data) {
                $.messager.progress('close');
                var JSONMsg = eval("(" + data + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                } else {
                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                }
            }
        });
    }
});