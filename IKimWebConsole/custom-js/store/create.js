var emailAddressRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

$(document).ready(function () {

    $('#PhoneNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '')
                    .substring(0, 10)
                    .replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    $('#AlternativePhoneNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '')
                    .substring(0, 10)
                    .replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
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
            showToastr(notificationType, notificationMsg);
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

    var name = $.trim($("#Name").val());
    var contactPerson = $.trim($("#ContactPerson").val());
    var phoneNumber = $.trim($("#PhoneNumber").val());
    var alternativePhoneNumber = $.trim($("#AlternativePhoneNumber").val());
    var storeTypeId = $.trim($("#StoreTypeId").val());
    var pricingTierId = $.trim($("#PricingTierId").val());
    var parentStoreId = $.trim($("#ParentStoreId").val());

    //Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $("#Name").next("span").text("");
    }
    //ContactPerson
    if (contactPerson == null || contactPerson == "" || contactPerson == undefined) {
        $("#ContactPerson").next("span").text("Contact Person is required!");
        isValid = false;
    } else {
        $("#ContactPerson").next("span").text("");
    }


    //Phone Number
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

    if (alternativePhoneNumber != null && alternativePhoneNumber != "" && alternativePhoneNumber != undefined) {
        var altDigits = alternativePhoneNumber.replace(/\D/g, '');
        if (altDigits.length !== 12) {
            $("#AlternativePhoneNumber").next("span").text("Alternative Phone Number must be exactly 10 digits!");
            isValid = false;
        } else {
            $("#AlternativePhoneNumber").next("span").text("");
        }
    }

    if (storeTypeId == null || storeTypeId == "" || storeTypeId == undefined || storeTypeId == "Select Store Type") {
        $("#StoreTypeId").next("span").text("Store Type is required!");
        isValid = false;
    } else if ($.trim(parseInt(storeTypeId)) == 2) {
        if (parentStoreId == null || parentStoreId == "" || parentStoreId == undefined || parentStoreId == "Select Parent Store") {
            $("#ParentStoreId").next("span").text("Parent Store is required!");
            isValid = false;
        } else {
            $("#ParentStoreId").next("span").text("");
        }
    } else {
        $("#StoreTypeId").next("span").text("");
    }

    //PricingTierId
    if (pricingTierId == null || pricingTierId == "" || pricingTierId == undefined || pricingTierId == "Select Price Tier") {
        $("#PricingTierId").next("span").text("Pricing Tier is required!");
        isValid = false;
    } else {
        $("#PricingTierId").next("span").text("");
    }
    return isValid;
}


