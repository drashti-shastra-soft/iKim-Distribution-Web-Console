$(function () {
    $(document).off('click', '.add-banner').on('click', '.add-banner', fnCreate);

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
});


function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Banner/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-banner').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Banner/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                showToastr(response.NotificationType, response.NotificationMsg);
                $('#frmList').submit();
            }
        },
        onError: function () {
            showToastr("Failed to delete banner.");
        }
    });
}