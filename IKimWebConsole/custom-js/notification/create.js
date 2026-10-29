$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });
});

function fnValidation() {
    var isValid = true;

    var title = $.trim($("#Title").val());

    // Title
    if (title == null || title == "" || title == undefined) {
        $("#Title").next("span").text("Title is required!");
        isValid = false;
    } else {
        $('#Title').next('span').text("");
    }

    return isValid;
}

