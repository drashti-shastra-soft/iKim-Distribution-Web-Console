$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    $('#ItemId').on('change', function () {
        var itemId = $(this).val();
        fnGetUnit(itemId);
    })

    calculateTotal();

});

function fnValidation() {
    var isValid = true;

    var itemId = $("#ItemId").val();
    var quantity = $("#Quantity").val();
    var rate = $("#Rate").val();

    // Item
    if (itemId == null || itemId == "" || itemId == undefined || itemId == "Select Item") {
        $("#ItemId").next("span").text("Item is required!");
        isValid = false;
    } else {
        $("#ItemId").next("span").text("");
    }

    // Quantity
    if (quantity == null || quantity == "" || quantity == undefined || quantity == 0) {
        $("#Quantity").closest(".col").find(".qty-error").text("Quantity is required!");
        isValid = false;
    }
    else if (parseFloat(quantity) <= 0) {
        $("#Quantity").closest(".col").find(".qty-error").text("Quantity must be greater than 0!");
        isValid = false;
    }
    else {
        $("#Quantity").closest(".col").find(".qty-error").text("");
    }


    // Rate
    if (rate == null || rate == "" || rate == undefined || rate == 0) {
        $("#Rate").next("span").text("Rate is required!");
        isValid = false;
    } else {
        $("#Rate").next("span").text("");
    }
    return isValid;
}


$(document).on('keyup change', '#Quantity, #Rate, #WaiterCharge', function () {
    calculateTotal();
});

function calculateTotal() {
    var quantity = parseFloat($("#Quantity").val()) || 0;
    var rate = parseFloat($("#Rate").val()) || 0;

    // if waiter charge is blank, treat as 0
    var waitercharge = parseFloat($("#WaiterCharge").val());
    if (isNaN(waitercharge)) {
        waitercharge = 0;
    }

    var total = (quantity * rate) + waitercharge;

    $("#Total").val(total.toFixed(2));
}

function fnGetUnit(itemId) {
    $.ajax({
        url: '/PartyOrderLine/GetUnitByItem',
        type: 'GET',
        data: { itemId: itemId },
        success: function (response) {
            if (response.success) {

                $('#UnitOfMeasure').val(response.UnitOfMeasure);
                $('#UnitOfMeasureId').val(response.UnitOfMeasureId);

            }
        },
        error: function () {
            showToastr('error', 'Error fetching item details.');
        }
    });
}
