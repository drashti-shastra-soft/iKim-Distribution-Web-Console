$(function () {
    $(document).off('click', '.add-sales').on('click', '.add-sales', fnCreate);

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
            fnGetSalesItem(id, $content);
        }
    });

});

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Sales/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-sales').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/Sales/Get/${id}`,
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-sales').modal('show');
        }
    });
}

function fnGetSalesItem(id, $content) {
    fnHandleAjaxRequest({
        url: `/Sales/GetSalesItem/${id}`,
        dataType: 'html',
        onSuccess: function (html) {
            $content.html(html);
        },
        onError: function () {
            $content.html('<div class="text-danger">Unable to load sales item.</div>');
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Sales/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                showToastr(response.NotificationType, response.NotificationMsg);
                $('#frmList').submit();
            }
        },
        onError: function () {
            showToastr("Failed to delete sales.");
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