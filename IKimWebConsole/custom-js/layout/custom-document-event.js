$(document).on('keypress', 'form input', function (e) {
    if (e.which === 13) {
        e.preventDefault();
        $(this).closest('form').submit();
    }
});