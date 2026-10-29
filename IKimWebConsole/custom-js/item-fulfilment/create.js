$(document).ready(function () {

    $('[data-toggle="tooltip"]').tooltip();

    checkFulfillmentStatus();

    $('.item-fulfillment').each(function () {
        updateRemaining($(this));
    });

    $(document).on('input', '.qty-fulfilled', function () {
        var $row = $(this).closest('tr');
        updateRemaining($row);
    });

    $('.btn-submit').on('click', function () {
        if (!fnValidation()) {
            return;
        }
        $('#loading-element').show();
        toggleLoading(true);

        var totalRows = $('.qty-fulfilled').length;
        var processedRows = 0;

        $('.item-fulfillment').each(function () {
            var $row = $(this);

            if ($row.find('.qty-fulfilled').length === 0) return;
            var mItemFulfillment = {};

            mItemFulfillment.OrderLineId = parseInt($row.find('td').eq(1).text().trim()) || 0;
            mItemFulfillment.ItemId = parseInt($row.find('td').eq(2).text().trim()) || 0;
            mItemFulfillment.OrderId = parseInt($row.find('td').eq(3).text().trim()) || 0;
            mItemFulfillment.QuantityRequested = parseFloat($row.find('td').eq(5).text().trim()) || 0;
            mItemFulfillment.QuantityFulfilled = parseFloat($row.find('.qty-fulfilled').val()) || 0;

            setTimeout(function () {
                $.ajax({
                    url: '/Order/SaveFulfillment',
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(mItemFulfillment),
                    success: function (result) {
                        processedRows += 1;
                        if (result.StatusCode === "Ok") {
                            if (processedRows === totalRows) {
                                showToastr(result.NotificationType, result.NotificationMsg);
                                $('.modal').modal('hide');
                                $('body').removeClass('modal-open');
                                $('.modal-backdrop').remove();
                                $('#frmList').submit();
                                $('#loading-element').hide();
                            }
                        } else {
                            $('#loading-element').hide();
                            showToastr(result.NotificationType, result.NotificationMsg);
                        }
                    },
                    error: function () {
                        $('#loading-element').hide();
                        showToastr("error", "Something went wrong!");
                    }
                });
            }, processedRows * 50);
        });
    });

});

function updateRemaining($row) {

    var requested = parseFloat($row.find('td').eq(4).text().trim()) || 0;
    var alreadyFulfilled = parseFloat($row.find('td').eq(5).text().trim()) || 0;
    var entered = parseFloat($row.find('.qty-fulfilled').val()) || 0;

    var remaining = requested - alreadyFulfilled - entered;

    if (remaining < 0) remaining = 0;

    $row.find('.remaining-qty').text(remaining);
}

//function fnValidation() {
//    var isValid = true;
//    $('.qty-fulfilled').each(function () {
//        var $el = $(this);
//        var $row = $el.closest('tr');
//        $row.find('.stock-error').text("");
//        var qty = parseFloat($el.val()) || 0;
//        if ($el.val() === '') {
//            $row.find('.stock-error').text("Quantity Fulfilled is required.");
//            isValid = false;
//            return;
//        }
//        var itemId = parseInt($row.find('td').eq(2).text());
//        $.ajax({
//            url: '/Order/GetFactoryStock',
//            type: 'GET',
//            data: { itemId: itemId },
//            async: false,
//            success: function (result) {
//                if (qty > result.Stock) {
//                    $row.find('.stock-error').text("Only " + result.Stock + " Quantity available.");
//                    isValid = false;
//                }
//            }
//        });
//    });

//    return isValid;
//}

function fnValidation() {
    var isValid = true;

    $('.qty-fulfilled').each(function () {
        var $el = $(this);
        var enteredQty = parseFloat($el.val()) || 0;

        if ($el.val() === '') {
            $el.next("span").text("Quantity Fulfilled is required!");
            isValid = false;
        } else {
            $el.next("span").text("");
        }
    });

    return isValid;
}

function updateRow($row, isChecked) {
    const $checkbox = $row.find('.row-checkbox');
    const $qtyInput = $row.find('.qty-fulfilled');

    if (!$checkbox.length || !$qtyInput.length) return;

    const requested = parseFloat($qtyInput.data('requested')) || 0;
    const fulfilled = parseFloat($qtyInput.data('fulfilled')) || 0;
    const remainingQty = Math.max(requested - fulfilled, 0);

    $qtyInput.tooltip('hide');

    $('#select-all-items').tooltip('hide');

    $checkbox.prop('checked', isChecked);
    $qtyInput.val(isChecked ? remainingQty : '');

    $qtyInput.tooltip('hide');
}


$(document).on('change', '#select-all-items', function () {
    const isChecked = $(this).prop('checked');

    $(this).tooltip('hide');

    $('.item-fulfillment').each(function () {
        updateRow($(this), isChecked);
    });
});


$(document).on('change', '.row-checkbox', function () {
    const $row = $(this).closest('tr');

    $(this).tooltip('hide');

    updateRow($row, $(this).prop('checked'));

    const total = $('.row-checkbox').length;
    const checked = $('.row-checkbox:checked').length;
    $('#select-all-items').prop('checked', total === checked);
});

function checkFulfillmentStatus() {
    var pendingItems = $('.qty-fulfilled').length;
    if (pendingItems === 0) {
        $('.btn-submit').prop('disabled', true).addClass('disabled')
        .css({
            'pointer-events': 'none',
            'opacity': '0.6'
        });

    } else {
        $('.btn-submit').prop('disabled', false).removeClass('disabled')
        .css({
            'pointer-events': '',
            'opacity': ''
        });
    }
}


$(document).on('blur', '.qty-fulfilled', function () {
    var $el = $(this);
    var $row = $el.closest('tr');
    $el.next("span").text("");
    var qty = parseFloat($el.val()) || 0;
    if ($el.val() === '') {
        $el.next("span").text("Quantity Fulfilled is required!");
        return;
    }
    var itemId = parseInt($row.find('td').eq(2).text().trim()) || 0;
    $.ajax({
        url: '/Order/GetFactoryStock',
        type: 'GET',
        data: { itemId: itemId },
        success: function (result) {
            if (qty > result.Stock) {
                $el.next("span").text("Only " + result.Stock + " Quantity available.");
            }
            else {
                $el.next("span").text("");
            }
        },
        error: function () {
            $el.next("span").text("Unable to check stock.");
        }
    });

});