var emailAddressRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

$(document).ready(function () {
    $('#MobileNumber').mask('+91 00000 00000', {
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

    $('#FirstName').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $('#FirstName').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    $('#EmailAddress').blur(function () {
        var emailAddress = $.trim($('#EmailAddress').val());
        if (emailAddress == null || emailAddress == "" || emailAddress == undefined) {
            $("#EmailAddress").next("span").text("Email Address  is required!");
        } else if (!emailAddressRegex.test(emailAddress)) {
            $("#EmailAddress").next("span").text("Invalid email address!");
        }
        else {
            CheckEmailAddressExist(emailAddress);
        }
    });

});

function fnValidation() {
    var isValid = true;

    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    var userTypeId = $.trim($("#UserTypeId").val());
    var storeId = $.trim($("#StoreId").val());
    var firstName = $.trim($("#FirstName").val());
    var lastName = $.trim($("#LastName").val());
    var emailAddress = $.trim($("#EmailAddress").val());
    var mobileNumber = $.trim($("#MobileNumber").val());

    // User Type
    if (userTypeId == null || userTypeId == "" || userTypeId == undefined || userTypeId == "Select User Type") {
        $("#UserTypeId").next("span").text("User Type is required!");
        isValid = false;
    } else {
        $("#UserTypeId").next("span").text("");
    }

    // Store
    if (storeId == null || storeId == "" || storeId == undefined || storeId == "Select Store") {
        $("#StoreId").next("span").text("Store is required!");
        isValid = false;
    } else {
        $("#StoreId").next("span").text("");
    }

    // First Name
    if (firstName == null || firstName == "" || firstName == undefined) {
        $("#FirstName").next("span").text("First Name is required!");
        isValid = false;
    } else {
        $('#FirstName').next('span').text("");
    }

    // Last Name
    if (lastName == null || lastName == "" || lastName == undefined) {
        $("#LastName").next("span").text("Last Name is required!");
        isValid = false;
    } else {
        $('#LastName').next('span').text("");
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
    if (mobileNumber == null || mobileNumber == "" || mobileNumber == undefined) {
        $("#MobileNumber").next("span").text("Mobile Number is required!");
        isValid = false;
    } else {
        var digits = mobileNumber.replace(/\D/g, '');
        if (digits.length !== 12) {
            $("#MobileNumber").next("span").text("Mobile Number must be exactly 10 digits!");
            isValid = false;
        } else {
            $("#MobileNumber").next("span").text("");
        }
    }

    return isValid;
}

function CheckEmailAddressExist(emailAddress) {
    var mUser = {};
    mUser.Id = parseInt($('#Id').val());
    mUser.EmailAddress = emailAddress;

    fnHandleAjaxRequest({
        url: `/User/CheckEmailAddressExist`,
        method: 'POST',
        dataType: 'json',
        data: JSON.stringify(mUser),
        onSuccess: function (response) {
            if (response) {
                if (response.StatusCode === "Ok") {
                    if (response.IsExist === true) {
                        $('.btn-submit').removeClass('btn-submit').addClass('disabled-link');
                        $("#EmailAddress").next("span").text("This email is already linked to an account.");
                    }
                    else if (response.IsExist === false) {
                        $('.disabled-link').removeClass('disabled-link').addClass('btn-submit');
                        $("#EmailAddress").next("span").text("");
                    }
                } else {

                }
            }
        },
        onError: function () {
            showToastr("Failed to check duplicate user!");
        }
    });
}
