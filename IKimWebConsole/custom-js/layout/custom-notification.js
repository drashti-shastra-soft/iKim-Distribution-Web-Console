
function showToastr(type, message) {
    switch (type.toLowerCase()) {
        case 'success':
            toastr.success(message);
            break;
        case 'info':
            toastr.info(message);
            break;
        case 'error':
            toastr.error(message);
            break;
        case 'warning':
            toastr.warning(message);
            break;
        default:
            console.warn('Unknown toastr type:', type);
    }
}
