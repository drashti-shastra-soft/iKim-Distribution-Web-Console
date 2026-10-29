$(document).ready(function () {
    // Submit Form
    $(document).off("click", ".btn-submit").on("click", ".btn-submit", function (e) {
        e.preventDefault();

        if (fnValidation()) {
            $("#frmCreate").submit();
        }
    });

    // Main Item Change
    $(document).off("change", "#ItemId").on("change", "#ItemId", function () {
        var itemId = $(this).val();
        var orderId = $("#OrderId").val();

        if (!itemId) {
            $("#UnitOfMeasure").val("");
            $("#UnitOfMeasureId").val("");
            $("#Rate").val("");
            $("#Amount").val("");
            return;
        }

        fnGetUnit(itemId);
        fnGetPrice(itemId, orderId, $("#Rate"), $("#Amount"));
    });

    // Add New Item Row
    $(document).off("click", "#add-item").on("click", "#add-item", function () {
        var index = $("#item-container .item-entry").length;
        $.get("/OrderLine/GetOrderLineItemRowPartial", { index: index })
        .done(function (html) {
            $("#item-container").append(html);
        })
        .fail(function () {
            showToastr("error", "Failed to add item row!");
        });
    });

    // Additional Item Change
    //$(document).off("change", "#item-container .ItemId").on("change", "#item-container .ItemId", function () {
    //    var $row = $(this).closest(".item-entry");
    //    var itemId = $(this).val();
    //    var orderId = $("#OrderId").val();
    //    $row.find(".item-error").text("");
    //    if (!itemId) {
    //        $row.find(".Rate").val("");
    //        $row.find(".Total").val("");
    //        return;
    //    }

    //    fnGetPrice(itemId, orderId, $row.find(".Rate"), $row.find(".Total"));
    //});

    //$(document).off("change", "#item-container .ItemId").on("change", "#item-container .ItemId", function () {
    //    var $row = $(this).closest(".item-entry");
    //    var itemId = $(this).val();
    //    var orderId = $("#OrderId").val();

    //    if (!itemId) {
    //        $row.find(".Rate").val("");
    //        $row.find(".Total").val("");
    //        $row.find(".UnitOfMeasure").val("");
    //        return;
    //    }

    //    $.ajax({
    //        url: "/Order/GetUnitByItem",
    //        type: "GET",
    //        data: { itemId: itemId },
    //        success: function (response) {
    //            if (response.success) {
    //                $row.find(".UnitOfMeasure").val(response.UnitOfMeasure);
    //            } else {
    //                $row.find(".UnitOfMeasure").val("");
    //            }
    //        },
    //        error: function () {
    //            showToastr("error", "Error fetching item unit.");
    //        }
    //    });

    //    fnGetPrice(
    //        itemId, orderId, $row.find(".Rate"), $row.find(".Total")
    //    );
    //});


    $(document).off("change", "#item-container .ItemId").on("change", "#item-container .ItemId", function () {
        var $row = $(this).closest(".item-entry");
        var itemId = $(this).val();
        var orderId = $("#OrderId").val();

        if (!itemId) {
            $row.find(".Rate").val("");
            $row.find(".Total").val("");
            $row.find(".UnitOfMeasure").val("");
            $row.find(".UnitOfMeasureId").val("");
            return;
        }

        $.ajax({
            url: "/Order/GetUnitByItem",
            type: "GET",
            data: { itemId: itemId },

            success: function (response) {
                if (response.success) {
                    $row.find(".UnitOfMeasure")
                        .val(response.UnitOfMeasure);

                    $row.find(".UnitOfMeasureId")
                        .val(response.UnitOfMeasureId);
                } else {
                    $row.find(".UnitOfMeasure").val("");
                    $row.find(".UnitOfMeasureId").val("");
                }
            },

            error: function () {
                showToastr("error", "Error fetching item unit.");
            }
        });

        fnGetPrice(
            itemId,
            orderId,
            $row.find(".Rate"),
            $row.find(".Total")
        );
    });



    // Main Item Quantity
    $(document).off("input", "#QuantityRequested").on("input", "#QuantityRequested", function () {
        this.value = fnCleanQuantity(this.value);
        calculateTotal();
    });

    // Additional Item Quantity
    $(document).off("input", "#item-container .Qty").on("input", "#item-container .Qty", function () {
        this.value = fnCleanQuantity(this.value);
        var $row = $(this).closest(".item-entry");
        $row.find(".qty-error").text("");
        calculateOrderLineTotal($row);
    });

    // Remove Item Row
    $(document).off("click", ".btn-remove-item").on("click", ".btn-remove-item", function (e) {
        e.preventDefault(); 
        $(this).closest(".item-entry").remove();
        fnUpdateItemIndexes();
        calculateTotal();
    });
});

// Allow numbers and one decimal point
function fnCleanQuantity(value) {
    value = value.replace(/[^0-9.]/g, "");
    var parts = value.split(".");
    if (parts.length > 2) {
        value = parts[0] + "." + parts.slice(1).join("");
    }
    return value;
}

// Form Validation
function fnValidation() {
    var isValid = true;
    // Main Item Validation
    var itemId = $("#ItemId").val();
    var quantity = parseFloat($("#QuantityRequested").val()) || 0;

    $("#ItemId").next("span").text("");
    $("#QuantityRequested").closest(".col").find(".qty-error").text("");

    if (!itemId) {
        $("#ItemId").next("span").text("Item is required!");
        isValid = false;
    }

    if (quantity <= 0) {
        $("#QuantityRequested").closest(".col").find(".qty-error").last().text("Enter a quantity greater than zero!");
        isValid = false;
    }

    // Additional Item Validation
    $("#item-container .item-entry").each(function () {
        var $row = $(this);
        var itemId = $row.find(".ItemId").val();
        var quantity = parseFloat($row.find(".Qty").val()) || 0;
        $row.find(".item-error").text("");
        $row.find(".qty-error").text("");
        if (!itemId) {
            $row.find(".item-error").text("Item is required!");
            isValid = false;
        }

        if (quantity <= 0) {
            $row.find(".qty-error").text("Enter a quantity greater than zero!");
            isValid = false;
        }
    });
    return isValid;
}

// Get Item Price
function fnGetPrice(itemId, orderId, $rate, $total) {
    if (!itemId || !orderId) {
        $rate.val("");
        $total.val("");
        return;
    }

    $.ajax({
        url: "/Order/GetPrice",
        type: "GET",
        data: {
            orderId: orderId,
            itemId: itemId,
        },
        success: function (price) {
            price = parseFloat(price);
            if (!isNaN(price) && price > 0) {
                $rate.val(price.toFixed(2));
                if ($rate.attr("id") === "Rate") {
                    calculateTotal();
                }
                else {
                    calculateOrderLineTotal($rate.closest(".item-entry"));
                }
            } else {
                $rate.val("0.00");
                $total.val("0.00");
                showToastr("error", "Rate not found!");
            }
        },
        error: function () {
            showToastr("error", "Failed to fetch price!");
        },
    });
}

function calculateTotal() {
    var quantity = parseFloat($("#QuantityRequested").val()) || 0;
    var rate = parseFloat($("#Rate").val()) || 0;
    var total = quantity * rate;
    $("#Amount").val(total.toFixed(2));
}

// Calculate Additional Item Total
function calculateOrderLineTotal($row) {

    var quantity = parseFloat($row.find(".Qty").val()) || 0;
    var rate = parseFloat($row.find(".Rate").val()) || 0;

    // Only Current Row Total
    $row.find(".Total").val((quantity * rate).toFixed(2));
}

// Get Unit of Measure for Main Item
function fnGetUnit(itemId) {
    $.ajax({
        url: "/Order/GetUnitByItem",
        type: "GET",
        data: {
            itemId: itemId,
        },
        success: function (response) {
            if (response.success) {
                $("#UnitOfMeasure").val(response.UnitOfMeasure);
                $("#UnitOfMeasureId").val(response.UnitOfMeasureId);
            } else {
                $("#UnitOfMeasure").val("");
                $("#UnitOfMeasureId").val("");
            }
        },
        error: function () {
            showToastr("error", "Error fetching item details.");
        },
    });
}

// Update Row Numbers and Collection Indexes
/*function fnUpdateItemIndexes() {
    $("#item-container .item-entry").each(function (index) {
        var $row = $(this);
        $row.find(".number-block").text(index + 1);
        $row.find("[name]").each(function () {
            var name = $(this).attr("name");
            if (name) {
                name = name.replace(/OrderLines\[\d+\]/g, "OrderLines[" + index + "]");
                $(this).attr("name", name);
            }
        });
    });
}*/

function fnUpdateItemIndexes() {
    $("#item-container .item-entry").each(function (index) {
        var $row = $(this);
        $row.find(".number-block").text(index + 1);
        $row.find("[name]").each(function () {
            var name = $(this).attr("name");
            if (name) {
                name = name.replace(
                    /OrderLineItems\[\d+\]/g, "OrderLineItems[" + index + "]"
                );
                $(this).attr("name", name);
            }
        });
    });
}