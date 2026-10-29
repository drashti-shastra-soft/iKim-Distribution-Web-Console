$(document).ready(function () {
    $(".btn-submit").click(function () {
        if (fnFormValidation()) {
            $("#frmLogin").submit();
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
    var password = $.trim($("#Password").val());

    if (emailAddress == null || emailAddress === "" || emailAddress == undefined) {
        $("#EmailAddress").next("span").text("Email is required!");
        $(".LoginFormSection span").removeClass("d-none");
        isValid = false;
    } else {
        $("#EmailAddress").next("span").text("");
    }

    if (password == null || password === "") {
        $("#Password").next("span").text("Password is required!");
        $(".externalBody span").removeClass("d-none");
        isValid = false;
    } else {
        $("#Password").next("span").text("");
    }

    return isValid;
}