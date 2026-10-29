$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    $('#MobileNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    function toggleWaiterCharge() {
        if ($('#WithWaiter').val() === 'Yes') {
            $('#waiterChargeDiv').show();
        } else {
            $('#waiterChargeDiv').hide();
            $('#WaiterCharge').val('');
        }
    }

    toggleWaiterCharge();

    $('#WithWaiter').on('change', function () {
        toggleWaiterCharge();
    });

    $(document).on('keyup change', '#Quantity, #Rate, #WaiterCharge', function () {
        calculateTotal();
    });

});

function fnValidation() {
    var isValid = true;

    var storeId = $.trim($("#StoreId").val());
    var name = $.trim($("#Name").val());
    var mobileNumber = $.trim($("#MobileNumber").val());
    var deliveryLocation = $.trim($("#DeliveryLocation").val());
    var withWaiter = $.trim($("#WithWaiter").val());
    var waiterCharge = $.trim($("#WaiterCharge").val());

    //Store
    if (storeId == null || storeId == "" || storeId == undefined) {
        $("#StoreId").next("span").text("Store is required!");
        isValid = false;
    } else {
        $("#StoreId").next("span").text("");
    }

    //Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $("#Name").next("span").text("");
    }

    //Mobile Number
    if (mobileNumber == null || mobileNumber == "" || mobileNumber == undefined) {
        $("#MobileNumber").next("span").text("Mobile Number is required!");
        isValid = false;
    } else {
        var digits = mobileNumber.replace(/\D/g, '');
        if (digits.length !== 12) {
            $("#MobileNumber").next("span").text("Mobile Number must be exactly 10 digits!");
            isValid = false;
        } else {
            $("#MobileNumber").next("span").text("");
        }
    }

    //Delivery Location
    if (deliveryLocation == null || deliveryLocation == "" || deliveryLocation == undefined) {
        $("#DeliveryLocation").next("span").text("Delivery Location is required!");
        isValid = false;
    } else {
        $("#DeliveryLocation").next("span").text("");
    }

    // With Waiter
    if (withWaiter === "" || withWaiter === "Select With Waiter") {
        $("#WithWaiter").next("span").text("With Waiter is required!");
        isValid = false;
    }
    else if (withWaiter === "Yes" && !waiterCharge) {
        $("#WithWaiter").next("span").text("");
        $("#WaiterCharge").next("span").text("Waiter Charge is required!");
        isValid = false;
    }
    else {
        $("#WithWaiter").next("span").text("");
        $("#WaiterCharge").next("span").text("");
    }

    return isValid;
}

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