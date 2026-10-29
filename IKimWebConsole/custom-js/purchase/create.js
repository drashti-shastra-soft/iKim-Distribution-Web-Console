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

    var billNumber = $.trim($("#BillNumber").val());
    var vendorId = $.trim($("#VendorId").val());

    //Bill Number
    if (billNumber == null || billNumber == "" || billNumber == undefined) {
        $("#BillNumber").next("span").text("Bill Number is required!");
        isValid = false;
    } else {
        $("#BillNumber").next("span").text("");
    }

    //Vendor
    if (vendorId == null || vendorId == "" || vendorId == undefined || vendorId == "Select Vendor") {
        $("#VendorId").next("span").text("Vendor is required!");
        isValid = false;
    } else {
        $("#VendorId").next("span").text("");
    }

    return isValid;
}


