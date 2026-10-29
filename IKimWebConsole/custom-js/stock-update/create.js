$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            var $modal = $("#modal-stock-update");
            var $input = $modal.data("input");
            var reason = $.trim($("#Reason").val());
            if ($input) { saveStock($input, reason); }

            $modal.modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
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
    var reason = $.trim($("#Reason").val());

    //Reason
    if (reason == null || reason == "" || reason == undefined) {
        $("#Reason").next("span").text("Reason is required!");
        isValid = false;
    } else {
        $("#Reason").next("span").text("");
    }

    return isValid;
}

