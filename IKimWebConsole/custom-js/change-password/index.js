$(document).ready(function () {
    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $("form").submit();
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

function fnValidation() {
    var isValid = true;


    var currentPassword = $.trim($("#CurrentPassword").val());
    var newPassword = $.trim($("#NewPassword").val());
    var confirmPassword = $.trim($("#ConfirmPassword").val());

    if (currentPassword == null || currentPassword === "" || currentPassword == undefined) {
        $("#CurrentPassword").next("span").text("Current Password is required!");
        isValid = false;
    } else {
        $("#CurrentPassword").next("span").text("");
    }

    if (newPassword == null || newPassword === "" || newPassword == undefined) {
        $("#NewPassword").next("span").text("New Password is required!");
        isValid = false;
    } else {
        $("#NewPassword").next("span").text("");
    }

    if (confirmPassword == null || confirmPassword === "" || confirmPassword == undefined) {
        $("#ConfirmPassword").next("span").text("Confirm Password is required!");
        isValid = false;
    } else if (confirmPassword && newPassword) {
        if (confirmPassword != newPassword) {
            $("#ConfirmPassword").next("span").text("New Password and Confirm Password must be same!");
            isValid = false;
        } else {
            $("#ConfirmPassword").next("span").text("");
        }
    } else {
        $("#ConfirmPassword").next("span").text("");
    }

    return isValid;
}