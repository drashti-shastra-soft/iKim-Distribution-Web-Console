$(document).ready(function () {
    $("[data-form]").click(function () {
        if ($(this).data("form") === "frmSaveAddress" && fnValidationSaveAddress()) {
            $('#' + $(this).data("form")).submit();
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
        }
    }

});

function fnValidationSaveAddress() {
    var isValid = true;

    var line1 = $.trim($("#Line1").val());
    var city = $.trim($("#City").val());
    var state = $.trim($("#State").val());
    var zipCode = $.trim($("#ZipCode").val());


    // line1
    if (line1 == null || line1 == "" || line1 == undefined) {
        $("#Line1").next("span").text("Line 1 is required!");
        isValid = false;
    } else {
        $("#Line1").next("span").text("");
    }

    // city
    if (city == null || city == "" || city == undefined) {
        $("#City").next("span").text("City is required!");
        isValid = false;
    } else {
        $("#City").next("span").text("");
    }

    // State
    if (state == null || state == "" || state == undefined) {
        $("#State").next("span").text("State is required!");
        isValid = false;
    } else {
        $("#State").next("span").text("");
    }

    // ZipCode
    if (zipCode == null || zipCode == "" || zipCode == undefined) {
        $("#ZipCode").next("span").text("ZipCode is required!");
        isValid = false;
    } else {
        $("#ZipCode").next("span").text("");
    }

    return isValid;
}

