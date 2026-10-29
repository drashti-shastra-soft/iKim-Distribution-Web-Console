$(document).ready(function () {

    $('#MobileNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    $('#Name').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    var notificationType = $.trim($('#NotificationType').val());
    var notificationMsg = $.trim($('#NotificationMsg').val());

    if (notificationType != null && notificationType != undefined && notificationType != '') {
        if (notificationMsg != null && notificationMsg != undefined && notificationMsg != '') {

            showToastr(notificationType, notificationMsg)
            $('.modal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $('#frmList').submit();
        }
    }

});

function fnValidation() {
    var isValid = true;

    var billNumber = $.trim($("#BillNumber").val());
    var mobileNumber = $.trim($("#MobileNumber").val());
    var name = $.trim($("#Name").val());
    var storeId = $.trim($("#StoreId").val());
    var address = $.trim($("#Address").val());

    //Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $("#Name").next("span").text("");
    }

    //Bill Number
    if (billNumber == null || billNumber == "" || billNumber == undefined) {
        $("#BillNumber").next("span").text("Bill Number is required!");
        isValid = false;
    } else {
        $("#BillNumber").next("span").text("");
    }

    //Mobile Number
    if (mobileNumber == null || mobileNumber == "" || mobileNumber == undefined) {
        $("#MobileNumber").next("span").text("Mobile Number is required!");
        isValid = false;
    } else {
        var digits = mobileNumber.replace(/\D/g, '');
        if (digits.length !== 12) {
            $("#MobileNumber").next("span").text("Mobile Number must be exactly 10 digits!");
            isValid = false;
        } else {
            $("#MobileNumber").next("span").text("");
        }
    }

    //Store
    if (storeId == null || storeId == "" || storeId == undefined) {
        $("#StoreId").next("span").text("Store is required!");
        isValid = false;
    } else {
        $("#StoreId").next("span").text("");
    }

    //Address
    if (address == null || address == "" || address == undefined) {
        $("#Address").next("span").text("Address is required!");
        isValid = false;
    } else {
        $("#Address").next("span").text("");
    }

    return isValid;
}


