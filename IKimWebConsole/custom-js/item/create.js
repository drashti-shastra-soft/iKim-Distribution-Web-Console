$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    function calculateTotal() {

        var cgst = parseFloat($('#CGST').val()) || 0;
        var sgst = parseFloat($('#SGST').val()) || 0;

        var gst = sgst + cgst;

        $('#GST').val(gst.toFixed(2));
    }

    $(document).on('input', '#CGST, #SGST', function () {
        calculateTotal();
    });

    function toggleRetailerCanOrder() {
        if ($('#IsRetailerCanOrder').is(':checked')) {
            $('#priceSection').show();
        } else {
            $('#priceSection').hide();
        }
    }

    toggleRetailerCanOrder();

    $('#IsRetailerCanOrder').on('change', function () {
        toggleRetailerCanOrder();
    });

    function toggleGodownStock() {

        var categoryId = $("#CategoryId").val();

        if (categoryId == "5") {
            $("#godownStockRow").show();
        }
        else {
            $("#godownStockRow").hide();
            $("#GodownStock").val("");
        }
    }

    toggleGodownStock();

    $(document).on("change", "#CategoryId", function () {
        toggleGodownStock();
    });


    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })


});

function fnValidation() {
    var isValid = true;

    var categoryId = $.trim($("#CategoryId").val());
    var name = $.trim($("#Name").val());
    var unitOfMeasureId = $.trim($("#UnitOfMeasureId").val());
    var gST = $.trim($("#GST").val());
    var cGST = $.trim($("#CGST").val());
    var sGST = $.trim($("#SGST").val());


    // Category
    if (categoryId == null || categoryId == "" || categoryId == undefined) {
        $("#CategoryId").next("span").text("Category is required!");
        isValid = false;
    } else {
        $("#CategoryId").next("span").text("");
    }

    // Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").closest(".col").find(".qty-error").text("Name is required!");
        isValid = false;
    }
    else if (parseFloat(name) <= 0) {
        $("#Name").closest(".col").find(".qty-error").text("Name must be greater than 0!");
        isValid = false;
    }
    else {
        $("#Name").closest(".col").find(".qty-error").text("");
    }

    // unitOfMeasureId
    if (unitOfMeasureId == null || unitOfMeasureId == "" || unitOfMeasureId == undefined) {
        $("#UnitOfMeasureId").closest(".col").find(".unit-error").text("Unit Of Measure is required!");
        isValid = false;
    }
    else {
        $("#UnitOfMeasureId").closest(".col").find(".unit-error").text("");
    }

    // GST
    if (gST == null || gST == "" || gST == undefined) {
        $("#GST").next("span").text("GST is required!");
        isValid = false;
    } else {
        $("#GST").next("span").text("");
    }

    // CGST
    if (cGST == null || cGST == "" || cGST == undefined) {
        $("#CGST").next("span").text("CGST is required!");
        isValid = false;
    } else {
        $("#CGST").next("span").text("");
    }

    // SGST
    if (sGST == null || sGST == "" || sGST == undefined) {
        $("#SGST").next("span").text("SGST is required!");
        isValid = false;
    } else {
        $("#SGST").next("span").text("");
    }

    return isValid;
}

$(document).on("change", "#file", function () {

    var fileName = $(this).val().split("\\").pop();

    $(this).next(".custom-file-label").html(fileName);
});