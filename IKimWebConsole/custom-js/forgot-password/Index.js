$(document).ready(function () {
    $(".btn-submit").click(function () {
        if (fnFormValidation()) {
            $("#frmForgotPassword").submit();
        }
    });

    var notificationType = $.trim($('#NotificationType').val());
    var notificationMsg = $.trim($('#NotificationMsg').val());

    if (notificationType != null && notificationType != undefined && notificationType != '') {
        if (notificationMsg != null && notificationMsg != undefined && notificationMsg != '') {
            showToastr(notificationType, notificationMsg);
        }
    }
});

function fnFormValidation() {
    var isValid = true;

    var emailAddress = $.trim($("#EmailAddress").val());

    if (emailAddress == null || emailAddress === "" || emailAddress == undefined) {
        $("#EmailAddress").next("span").text("Email Address is required!");
        $(".LoginFormSection span").removeClass("d-none");
        isValid = false;
    } else {
        $("#EmailAddress").next("span").text("");
    }

    return isValid;
}

