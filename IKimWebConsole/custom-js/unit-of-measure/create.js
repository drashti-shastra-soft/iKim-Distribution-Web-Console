$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });
});

function fnValidation() {
    var isValid = true;

    var name = $.trim($("#Name").val());

    // Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $('#Name').next('span').text("");
    }

    return isValid;
}

