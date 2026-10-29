$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

});

function fnValidation() {
    var isValid = true;

    var name = $.trim($("#Name").val());
    var multiply = $.trim($("#Multiply").val());

    //Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $("#Name").next("span").text("");
    }
    
    //Multiply
    if (multiply == null || multiply == "" || multiply == undefined || multiply == 0) {
        $("#Multiply").next("span").text("Multiply is required!");
        isValid = false;
    } else {
        $("#Multiply").next("span").text("");
    }
    
    return isValid;
}


$(document).on("change", "#file", function () {

    var fileName = $(this).val().split("\\").pop();

    $(this).next(".custom-file-label").html(fileName);
});
