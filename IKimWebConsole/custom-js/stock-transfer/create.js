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

    $('#reservationdate').datetimepicker({
        format: 'DD-MM-YYYY',
        defaultDate: moment()
    });

});

function fnValidation() {
    var isValid = true;

    var itemId = $.trim($("#ItemId").val());
    var date = $.trim($('#Date').val());
    var quantity = $.trim($('#Quantity').val());
    var unitOfMeasureId = $.trim($('#UnitOfMeasureId').val());


    // Item
    if (itemId == null || itemId == "" || itemId == undefined || itemId == "Select Item") {
        $("#ItemId").next("span").text("Item is required!");
        isValid = false;
    } else {
        $("#ItemId").next("span").text("");
    }

    if (date == null || date == "" || date == undefined) {
        $("#Date").next("span").text("Date is required!");
        isValid = false;
    } else {
        $('#Date').next('span').text("");
    }


    // quantity
    if (quantity == null || quantity == "" || quantity == undefined) {
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

    // unitOfMeasureId
    if (unitOfMeasureId == null || unitOfMeasureId == "" || unitOfMeasureId == undefined) {
        $("#UnitOfMeasureId").closest(".col").find(".unit-error").text("Unit Of Measure is required!");
        isValid = false;
    }
    else {
        $("#UnitOfMeasureId").closest(".col").find(".unit-error").text("");
    }


    return isValid;
}

function fnGetUnit(itemId) {
    $.ajax({
        url: '/StockTransfer/GetUnitByItem',
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