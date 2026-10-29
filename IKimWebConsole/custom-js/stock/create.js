$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    $('#reservationdate').datetimepicker({
        format: 'DD-MM-YYYY',
        defaultDate: moment()
    });

});

function fnValidation() {
    var isValid = true;

    var storeId = $.trim($("#StoreId").val());

    //storeId
    if (storeId == null || storeId == "" || storeId == undefined) {
        $("#StoreId").next("span").text("Store is required!");
        isValid = false;
    } else {
        $("#StoreId").next("span").text("");
    }

    return isValid;
}


