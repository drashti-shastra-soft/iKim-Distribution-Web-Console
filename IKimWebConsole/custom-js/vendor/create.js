var emailAddressRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

$(document).ready(function () {

    $('#GSTNumber').mask('00AAAAA0000A0A*', {
        translation: {
            '0': { pattern: /[0-9]/ },
            'A': { pattern: /[A-Za-z]/ },
            '*': { pattern: /[A-Za-z0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            val = val.toUpperCase();
            field.val(val);
        }
    });

    $('#PhoneNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    $('#AlternatePhoneNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    $('#EmailAddress').on('input', function () {
        let val = $(this).val();

        // Allow only valid email characters
        val = val.replace(/[^a-zA-Z0-9@._\-]/g, '');

        // Prevent double @ symbols
        const parts = val.split('@');
        if (parts.length > 2) { val = parts[0] + '@' + parts[1]; }

        $(this).val(val);
    });


    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    var notificationType = $.trim($('#NotificationType').val());
    var notificationMsg = $.trim($('#NotificationMsg').val());

    if (notificationType != null && notificationType != undefined && notificationType != '') {
        if (notificationMsg != null && notificationMsg != undefined && notificationMsg != '') {
            showToastr(notificationType, notificationMsg)
            $('#NotificationMsg').val('');
            $('#NotificationType').val('')
            $('.modal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $('#frmList').submit();
        }
    }
});

function fnValidation() {
    var isValid = true;

    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    var name = $.trim($("#Name").val());
    var emailAddress = $.trim($("#EmailAddress").val());
    var phoneNumber = $.trim($("#PhoneNumber").val());


    //  Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $('#Name').next('span').text("");
    }


    // Email Address
    if (emailAddress == null || emailAddress == "" || emailAddress == undefined) {
        $("#EmailAddress").next("span").text("Email Address is required!");
        isValid = false;
    } else if (!emailRegex.test(emailAddress)) {
        $("#EmailAddress").next("span").text("Invalid Email Format!");
        isValid = false;
    } else {
        $("#EmailAddress").next("span").text("");
    }

    //Mobile Number
    if (phoneNumber == null || phoneNumber == "" || phoneNumber == undefined) {
        $("#PhoneNumber").next("span").text("Phone Number is required!");
        isValid = false;
    } else {
        var digits = phoneNumber.replace(/\D/g, '');
        if (digits.length !== 12) {
            $("#PhoneNumber").next("span").text("Phone Number must be exactly 10 digits!");
            isValid = false;
        } else {
            $("#PhoneNumber").next("span").text("");
        }
    }
    return isValid;
}

