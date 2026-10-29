$(function () {
    $(document).off('click', '.add-order').on('click', '.add-order', fnCreate);

    $(document).off('click', '.icon-edit-order').on('click', '.icon-edit-order', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    $(document).off('click', '.icon-delete-order').on('click', '.icon-delete-order', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    $(document).off('click', '.icon-pdf').on('click', '.icon-pdf', function (e) {
        e.preventDefault();
        fnPdf($(this).data('id'), $(this).data('itemid'));
    });

    $(document).off('click', '.icon-document').on('click', '.icon-document', function (e) {
        e.preventDefault();
        var orderId = $(this).data('id');
        var store = $(this).data('store');
        if (!orderId) return;
        window.location.href = '/OrderLine/Index?orderId=' + orderId + '&store=' + store;
    });

    $(document).off('click', '.icon-fulfillment-log').on('click', '.icon-fulfillment-log', function (e) {
        e.preventDefault();
        var orderId = $(this).data('id');
        fnGetFulfillmentLog(orderId);
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

    $(document).on("change", "#checkAll", function () {
        $(".pdf-checkbox").prop("checked", $(this).prop("checked"));
    });

    //This Query for child checkbox synchronization
    $(document).on("change", ".pdf-checkbox", function () {
        if ($(".pdf-checkbox:checked").length === $(".pdf-checkbox").length) {
            $("#checkAll").prop("checked", true);
        } else {
            $("#checkAll").prop("checked", false);
        }
    }); 

    $("#downloadSelectedPdf").on("click", function () {

        var selectedItems = [];

        $(".pdf-checkbox:checked").each(function () {
            selectedItems.push({
                Id: $(this).data("id"),
                ItemId: $(this).data("itemid")
            });
        });

        if (selectedItems.length === 0) {
            showToastr('error', 'Please select at least one order.');
            return;
        }

        $.ajax({
            url: "/Order/DownloadMergedPdf",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(selectedItems),
            beforeSend: function () {
                toggleLoading(true);
            },
            success: function (response) {
                if (response.StatusCode === "Ok") {
                    downloadPdfFromBase64(response.Base64String, "Delivery Challan.pdf");
                    $(".pdf-checkbox").prop("checked", false);
                    $("#checkAll").prop("checked", false);
                }
                else {
                    showToastr('error', response.Message);
                }
            },
            error: function () {
                showToastr('error', 'Failed to download PDF.');
            },
            complete: function () {
                toggleLoading(false);
            }
        });
    });

    $(document).off('click', '#downloadSelecteddailyorders').on('click', '#downloadSelecteddailyorders', function (e)
    {
        e.preventDefault();
        var selectedItems = [];
        $(".pdf-checkbox:checked").each(function () {

            var orderId = $(this).data("id");

            if (orderId) {
                selectedItems.push({
                    Id: orderId
                });
            }
        });
        if (selectedItems.length === 0) {
            showToastr('error', 'Please select at least one order.');
            return;
        }

        $.ajax({
            url: "/Order/DownloadSelectedDailyOrders",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(selectedItems),

            beforeSend: function () {
                toggleLoading(true);
            },
            success: function (response) {
                if (response.StatusCode === "Ok") {
                    downloadPdfFromBase64(
                        response.Base64String,
                        "Daily Order.pdf"
                    );
                    $(".pdf-checkbox").prop("checked", false);
                    $("#checkAll").prop("checked", false);
                }
                else if (response.StatusCode === "Unauthorized") {
                    showToastr('error', 'Session Expired! We will redirect you to the login page in 3 seconds.' );
                    setTimeout(function () { window.location.href = response.RedirectUrl; }, 3000);
                }
                else {
                    showToastr( 'error', response.Message || response.NotificationMsg || 'Failed to download Daily Order.' );
                }
            },
            error: function (xhr) {
                showToastr('error', 'Failed to download Daily Order.' );
            },
            complete: function () {
                toggleLoading(false);
            }
        });
    });
});

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Order/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-order').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function fnGetFulfillmentLog(orderId) {
    fnHandleAjaxRequest({
        url: '/Order/CreateFulfillment',
        data: { orderId: orderId },
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
            $('#modal-create-fulfillment').modal('show');
        }
    });
}

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/Order/Get/${id}`,
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
            $('#modal-order').modal('show');
        }
    });
}

function downloadPdfFromBase64(base64String, fileName) {
    // Decode Base64 into binary
    const byteCharacters = atob(base64String);
    const byteNumbers = new Array(byteCharacters.length)
        .fill()
        .map((_, i) => byteCharacters.charCodeAt(i));
    const byteArray = new Uint8Array(byteNumbers);

    // Create Blob
    const blob = new Blob([byteArray], { type: "application/pdf" });

    // Create object URL
    const url = URL.createObjectURL(blob);

    // Trigger download
    const link = document.createElement("a");
    link.href = url;
    link.download = fileName || "Merged.pdf";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    // Cleanup
    URL.revokeObjectURL(url);
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Order/Delete/${id}`,
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