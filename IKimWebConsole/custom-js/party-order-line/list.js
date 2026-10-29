$(function () {
    $(document).off('click', '.add-new-item').on('click', '.add-new-item', fnCreate);

    $(document).off('click', '.icon-fulfillment').on('click', '.icon-fulfillment', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGetItemFulfillment(id);
    });

    $(document).off('click', '.icon-edit-party-order').on('click', '.icon-edit-party-order', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    $(document).off('keyup', '.search-text').on('keyup', '.search-text', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            resetPagination();
            $('#frmList').trigger('submit');
        }
    });

    $(document).off('change', '.search-dropdown').on('change', '.search-dropdown', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('change', '.search-pagination').on('change', '.search-pagination', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('click', '.navigation').on('click', '.navigation', function (e) {
        e.preventDefault();
        var direction = $(this).data('direction').trim();
        if (direction) {
            triggerPageNavigation(direction);
        }
        $('#frmList').trigger('submit');
    });
});

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/PartyOrderLine/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-party-order-line').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/PartyOrderLine/Get/${id}`,
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
            $('#modal-party-order-line').modal('show');
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/PartyOrderLine/Delete/${id}`,
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
            showToastr("Failed to delete stock item.");
        }
    });
}


//function fnCreate() {
//    var partyorderId = $(this).data('party-orderid');
//    fnHandleAjaxRequest({
//        url: '/PartyOrder/CreatePartyOrderLine?partyorderId=' + partyorderId,
//        onSuccess: function (html) {
//            $('#divCreatePartyOrderLine').html(html);
//            $('#modal-party-order-line').modal('show');
//        },
//        onError: function () {
//            showToastr("Failed to load form.");
//        }
//    });
//}


//function fnGet(id) {
//    fnHandleAjaxRequest({
//        url: `/PartyOrder/GetPartyOrderLine/${id}`,
//        onSuccess: function (html) {
//            $('#divCreatePartyOrderLine').html(html);
//            $('#modal-party-order-line').modal('show');
//        }
//    });
//}

//function fnEditPartyOrderLine(id) {
//    fnHandleAjaxRequest({
//        url: `/PartyOrder/EditPartyOrderLine/${id}`,
//        onSuccess: function (html) {
//            $('#divCreatePartyOrderLine').html(html);
//            $('#modal-party-order-line').modal('show');
//        }
//    });
//}

//function fnDelete(id) {
//    fnHandleAjaxRequest({
//        url: `/PartyOrder/DeletePartyOrderLine/${id}`,
//        dataType: 'json',
//        onSuccess: function (response) {
//            if (response) {
//                showToastr(response.NotificationType, response.NotificationMsg);
//                $('#frmList').submit();
//            }
//        },
//        onError: function () {
//            showToastr("Failed to delete party order line.");
//        }
//    });
//}
