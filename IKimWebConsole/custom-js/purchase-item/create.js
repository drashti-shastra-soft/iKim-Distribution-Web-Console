
$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    $(document).on('input', '#Rate, #Qty', function () {

        var rate = parseFloat($('#Rate').val()) || 0;
        var qty = parseFloat($('#Qty').val()) || 0;

        $('#Amount').val((rate * qty).toFixed(2));

        calculateAll();
    });

    $(document).on('input', '#Amount', function () {
        calculateAll();
    });

    $('#ItemId').on('change', function () {
        var itemId = $(this).val();
        fnGetUnit(itemId);
    })

});


function calculateAll() {

    var rate = parseFloat($('#Rate').val()) || 0;
    var qty = parseFloat($('#Qty').val()) || 0;

    if ($('#Amount').val() == '' || $('#Amount').val() == '0') {
        $('#Amount').val((rate * qty).toFixed(2));
    }

    var amount = parseFloat($('#Amount').val()) || 0;
    var sgst = parseFloat($('#SGST').val()) || 0;
    var cgst = parseFloat($('#CGST').val()) || 0;

    var sgstAmount = (amount * sgst) / 100;
    var cgstAmount = (amount * cgst) / 100;

    var total = amount + sgstAmount + cgstAmount;

    $('#TotalAmount').val(total.toFixed(2));
}

function fnValidation() {
    var isValid = true;

    var itemId = $.trim($("#ItemId").val());
    var rate = $.trim($("#Rate").val());
    var qty = $.trim($("#Qty").val());
    var amount = $.trim($("#Amount").val());
    var sgst = $.trim($("#SGST").val());
    var cgst = $.trim($("#CGST").val());
    var totalAmount = $.trim($("#TotalAmount").val());

    // Item
    if (itemId == null || itemId == "" || itemId == undefined || itemId == "Select Item") {
        $("#ItemId").next("span").text("Item is required!");
        isValid = false;
    } else {
        $("#ItemId").next("span").text("");
    }

    // Rate
    if (rate == null || rate == "" || rate == undefined || rate == "0") {
        $("#Rate").next("span").text("Rate is required!");
        isValid = false;
    } else {
        $("#Rate").next("span").text("");
    }

    // Quantity
    if (qty == null || qty == "" || qty == undefined || qty == 0) {
        $("#Qty").closest(".col").find(".qty-error").text("Quantity is required!");
        isValid = false;
    }
    else if (parseFloat(qty) <= 0) {
        $("#Qty").closest(".col").find(".qty-error").text("Quantity must be greater than 0!");
        isValid = false;
    }
    else {
        $("#Qty").closest(".col").find(".qty-error").text("");
    }

    // Amount
    if (amount == null || amount == "" || amount == undefined) {
        $("#Amount").next("span").text("Amount is required!");
        isValid = false;
    } else {
        $("#Amount").next("span").text("");
    }

    // SGST
    if (sgst == null || sgst == "" || sgst == undefined) {
        $("#SGST").next("span").text("SGST is required!");
        isValid = false;
    } else {
        $("#SGST").next("span").text("");
    }

    // CGST
    if (cgst == null || cgst == "" || cgst == undefined) {
        $("#CGST").next("span").text("CGST is required!");
        isValid = false;
    } else {
        $("#CGST").next("span").text("");
    }

    // Total Amount
    if (totalAmount == null || totalAmount == "" || totalAmount == undefined || totalAmount == "0") {
        $("#TotalAmount").next("span").text("Total Amount is required!");
        isValid = false;
    } else {
        $("#TotalAmount").next("span").text("");
    }

    return isValid;
}

function fnGetUnit(itemId) {
    $.ajax({
        url: '/Purchase/GetGST?itemId=' + itemId,
        type: 'GET',
        data: { itemId: itemId },
        success: function (response) {
            if (response.success) {

                $('#UnitOfMeasure').val(response.UnitOfMeasure);
                $('#UnitOfMeasureId').val(response.UnitOfMeasureId);

                $('#CGST').val(response.CGST);
                $('#SGST').val(response.SGST);
            }
        },
        error: function () {
            showToastr('error', 'Error fetching item details.');
        }
    });
}
