$(document).ready(function () {
    $(document).off('keyup', '.search-text').on('keyup', '.search-text', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            if (typeof resetPagination === 'function') { resetPagination(); }
            $('#frmList').trigger('submit');
        }
    });

    // Stock Update
    $(document).off('blur', '.stock-update').on('blur', '.stock-update', function () {
        var $input = $(this);
        var valueText = $.trim($input.val());
        if (valueText === '') {
            return;
        }
        var value = parseFloat(valueText);
        if (isNaN(value)) {
            showToastr('error', 'Please enter a valid stock quantity.');
            return;
        }
        if (value < 0) {
            openModal($input);
        } else {
            saveStock($input, '');
        }
    });

    // Category click
    $(document).off('click', '.category-card').on('click', '.category-card', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        if (!id) { return; }
        window.location.href = '/StockUpdate/Items?categoryId=' + id;
    });

    // Page size change
    $(document).off('change', '.search-pagination').on('change', '.search-pagination', function () {
        // Items page
        if ($('#frmItems').length) { $('#Pagination_Skip').val(0);
            $('#frmItems').trigger('submit');
        }
        // Existing list page
        else if ($('#frmList').length) {
            if (typeof resetPagination === 'function') {resetPagination(); }
            $('#frmList').trigger('submit');
        }
    });

    // Pagination navigation
    $(document).off('click', '.navigation').on('click', '.navigation', function (e) {
        e.preventDefault();
        var direction = $(this).data('direction');
        if (!direction) {
            return;
        }
        if ($('#frmItems').length) {
            triggerPageNavigation(direction);
        } else if ($('#frmList').length) {
            if (typeof triggerPageNavigation === 'function') {
                triggerPageNavigation(direction);
            }
        }
    });

});


// Open reason modal when stock is reduced
function openModal($input) {
    $.ajax({
        url: '/StockUpdate/Create',
        type: 'GET',
        cache: false,
        success: function (html) {
            $('#modal-stock-update').remove();
            $('body').append(html);
            var itemId = $input.data('id');
            $('#ItemId').val(itemId);
            $('#modal-stock-update').data('input', $input);
            $('#modal-stock-update').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        error: function () {
            showToastr('error', 'Unable to open reason modal.');
        }
    });
}

// Save stock
function saveStock($input, reason) {
    var id = $input.data('id');
    var current = parseFloat($input.data('current')) || 0;
    var entered = parseFloat($input.val());
    if (isNaN(entered)) {
        showToastr('error', 'Please enter a valid stock quantity.');
        return;
    }
    var finalStock = parseFloat((current + entered).toFixed(2));
    if (finalStock < 0) {
        showToastr('error', 'Stock cannot be less than zero.');
        return;
    }
    $input.prop('disabled', true);
    $.ajax({
        url: '/StockUpdate/UpdateStock',
        type: 'POST',
        data: { Id: id, FactoryStock: finalStock, Reason: reason},
        success: function (res) {
            if (res.IsSuccess) {
                $input.closest('tr').find('td:eq(2)').text(finalStock);
                $input.data('current', finalStock);
                $input.val('');
                showToastr('success', res.Message || 'Stock updated successfully!');
            } else {
                showToastr('error', res.Message || 'Stock update failed.');
            }
        },

        error: function () {
            showToastr('error', 'Something went wrong.');
        },

        complete: function () {
            $input.prop('disabled', false);
        }
    });
}


// Pagination
function triggerPageNavigation(direction) {
    var config = window.paginationConfig;
    if (!config || !$('#frmItems').length) {
        return;
    }
    var pageSize = parseInt(config.pageSize, 10) || 10;
    var totalPage = parseInt(config.totalPage, 10) || 0;
    var currentPage = parseInt(config.currentPage, 10) || 1;
    var currentSkip = parseInt($('#Pagination_Skip').val(), 10) || 0;

    var newSkip = currentSkip;

    switch (direction) {
        case 'First':
            newSkip = 0;
            break;

        case 'Previous':
            newSkip = Math.max(0, currentSkip - pageSize);
            break;

        case 'Next':
            if (currentPage < totalPage) { newSkip = currentSkip + pageSize; }
            break;

        case 'Last':
            if (totalPage > 0) { newSkip = (totalPage - 1) * pageSize;}
            break;

        default:
            return;
    }
    if (newSkip !== currentSkip) {
        $('#Pagination_Skip').val(newSkip);
        $('#frmItems').trigger('submit');
    }
}
