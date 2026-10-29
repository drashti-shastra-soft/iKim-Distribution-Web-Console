$(function () {
    $(document).off('click', '.add-purchase').on('click', '.add-purchase', fnCreate);

    $(document).off('click', '.icon-edit').on('click', '.icon-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    $(document).off('click', '.icon-purchase').on('click', '.icon-purchase', function (e) {
        e.preventDefault();
        var purchaseId = $(this).data('id');
        var vendor = $(this).data('vendor');
        if (!purchaseId) return;
        //window.location.href = '/PurchaseItem/Index?purchaseId=' + purchaseId;
        window.location.href = '/PurchaseItem/Index?purchaseId=' + purchaseId + '&vendor=' + vendor;
    });


    // Reset pagination and submit the form on Enter key in search fields
    $(document).off('keyup', '.search-text').on('keyup', '.search-text', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            resetPagination();
            $('#frmList').trigger('submit');
        }
    });

    // Submit form when page size filter changes
    $(document).off('change', '.search-dropdown').on('change', '.search-dropdown', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('change', '.search-pagination').on('change', '.search-pagination', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('click', '.navigation').on('click', '.navigation', function (e) {
        e.preventDefault();
        var direction = $(this).data('direction').trim();
        if (direction != null && direction != undefined && direction != '') {
            triggerPageNavigation(direction);
        }
        $('#frmList').trigger('submit');
    });

    $(document).off('click', '.toggle-child').on('click', '.toggle-child', function (e) {

        const id = $(this).data('id');
        const $childRow = $('#child_' + id);
        const $icon = $(this).find('i');
        const $content = $('#childContent_' + id);

        const isVisible = $childRow.is(':visible');

        // Close all other child rows
        $('.child-row').hide();
        $('.toggle-child i').removeClass('fa-minus').addClass('fa-plus');
        $('.child-row div[id^="childContent_"]').empty();

        if (!isVisible) {
            $childRow.show();
            $icon.removeClass('fa-plus').addClass('fa-minus');
            fnGetPurchaseItem(id, $content); // Load content only when opening
        }
    });

});


function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Purchase/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-purchase').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/Purchase/Get/${id}`,
        onSuccess: function (response) {
            var parsed;
            try {
                parsed = JSON.parse(response);
            } catch (e) {
                parsed = null;
            }
            if (parsed && parsed.StatusCode) {
                if (parsed.StatusCode === "Unauthorized") {
                    showToastr('error', 'Session Expired! We will redirect you to the login page in 3 seconds.');
                    setTimeout(function () {
                        window.location.href = parsed.RedirectUrl;
                    }, 3000);
                    return;
                } else if (parsed.StatusCode === "BadRequest") {
                    showToastr(parsed.NotificationType, parsed.NotificationMsg);
                    return;
                } else if (parsed.StatusCode === "InternalServerError") {
                    showToastr(parsed.NotificationType, parsed.NotificationMsg);
                    return;
                }
            }

            $('#divCreate').html(response);
            $('#modal-purchase').modal('show');
        }
    });
}


function fnGetPurchaseItem(id, $content) {
    fnHandleAjaxRequest({
        url: `/Purchase/GetPurchaseItem/${id}`,
        dataType: 'html',
        onSuccess: function (html) {
            $content.html(html);
        },
        onError: function () {
            $content.html('<div class="text-danger">Unable to load purchase item.</div>');
        }
    });
}


function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Purchase/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                if (response.StatusCode == "Ok") {
                    showToastr(response.NotificationType, response.NotificationMsg);
                    $('#frmList').submit();
                } else if (response.StatusCode == "Unauthorized") {
                    showToastr('error', 'Session Expired! We will redirect you to the login page in 3 seconds.');
                    setTimeout(function () { window.location.href = response.RedirectUrl; }, 3000);
                } else if (response.StatusCode == "BadRequest") {
                    showToastr(response.NotificationType, response.NotificationMsg);
                } else if (response.StatusCode == "InternalServerError") {
                    showToastr(response.NotificationType, response.NotificationMsg);
                }
            }
        },
        onError: function () {
            showToastr("Failed to delete purchase.");
        }
    });
}

function triggerPageNavigation(btn) {
    const config = window.paginationConfig;
    const currentSkip = parseInt($('#Pagination_Skip').val()) || 0;

    let newSkip = currentSkip;

    switch (btn) {
        case "First":
            if (config.currentPage > 1) newSkip = 0;
            break;

        case "Next":
            if (config.currentPage < config.totalPage) newSkip += config.pageSize;
            break;

        case "Previous":
            if (currentSkip >= config.pageSize) newSkip -= config.pageSize;
            break;

        case "Last":
            if (config.currentPage < config.totalPage) newSkip = (config.totalPage - 1) * config.pageSize;
            break;

        default:
            return;
    }

    if (newSkip !== currentSkip || btn === "First") {
        $('#Pagination_Skip').val(newSkip);
        $('#frmList').submit();
    }
}