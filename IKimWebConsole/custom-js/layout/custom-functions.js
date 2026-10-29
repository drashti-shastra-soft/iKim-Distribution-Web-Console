function fnValidateEmail(email) {
    if (!email || typeof email !== 'string') return false;
    const regex = /^\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return regex.test(email.trim());
}

function toggleLoading(show) {
    $('#loading-element').toggle(show);
}

function resetPagination() {
    $('#Pagination_Skip').val(0);
    $('#Pagination_TotalRecord').val(0);
}

function fnHandleAjaxRequest({ url, method = 'GET', dataType = 'html', contentType = 'application/json', processData = true, data = null, onSuccess, onError }) {
    $.ajax({
        url: url,
        method: method,
        contentType: contentType,
        dataType: dataType,
        processData: processData,
        data: data,
        beforeSend: function () {
            toggleLoading(true);
        },
        success: function (response) {
            if (typeof onSuccess === 'function') {
                onSuccess(response);
            }
        },
        error: function () {
            if (typeof onError === 'function') {
                onError();
            }
        },
        complete: function () {
            toggleLoading(false);
        }
    });
}

function ExpandRow(index, el) {
    var $icon = $(el);
    var $childRow = $(".child-" + index);

    if ($icon.hasClass("fa-plus")) {
        $childRow.show();
        $icon.removeClass("fa-plus").addClass("fa-minus");
    } else {
        $childRow.hide();
        $icon.removeClass("fa-minus").addClass("fa-plus");
    }
}

function parseJsonDate(jsonDate) {
    return new Date(parseInt(jsonDate.replace(/\/Date\((\d+)\)\//, '$1')));
}