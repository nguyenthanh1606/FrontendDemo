function PopulateDropdownFromJson(sourceUrl, cssSelector, selectedValue, defaultOption) {
    var ddl = $(cssSelector);
    ddl.empty();

    ddl.append($('<option/>', {
        value: ''
    }).html(defaultOption))

    $.getJSON(sourceUrl, function (result) {
        $(result).each(function () {
            ddl.append(
            $('<option/>', {
                value: this.Value
            }).html(this.Text));
        });

        ddl.val(selectedValue);
    });
};

function AddToCartAjax(productId, callbackFunction) {
    var url = '/ShoppingCart/AddToCart_Catalog/' + productId;
    $.ajax({
        url: url,
        method: 'POST',
        error: function (jqXHR, textStatus, errorThrown) {
            AddToCartFail(jqXHR);
        },
        success: function (data) {
            if (callbackFunction && typeof (callbackFunction) === "function") {
                callbackFunction(data);
            }
        },
    });
}

function AddToWishlistAjax(productId, callbackFunction) {
    var url = '/ShoppingCart/AddToWishlist/' + productId;
    $.ajax({
        url: url,
        method: 'POST',
        error: function (jqXHR, textStatus, errorThrown) {
            ShowAlertMessage(jqXHR.msg);
        },
        success: function (data) {
            if (callbackFunction && typeof (callbackFunction) === "function") {
                callbackFunction(data);
            }
        },
    });
}

function RemoveFromWishlistAjax(productId, callbackFunction) {
    var url = '/ShoppingCart/RemoveFromWishlist/' + productId;
    $.ajax({
        url: url,
        method: 'POST',
        error: function (jqXHR, textStatus, errorThrown) {
            ShowAlertMessage(jqXHR.msg);
        },
        success: function (data) {
            if (callbackFunction && typeof (callbackFunction) === "function") {
                callbackFunction(data);
            }
        },
    });
}

function AddToCartFail(result) {
    var data = $.parseJSON(result.responseText);
    if (data.result == "redirect") {
        window.location.href = "/" + data.url;
    }
    if (data.error) {
        alert(data.error);
    }
}

function ProductSelectColor(_this, id) {
    $('.selectedColor').removeClass('selectedColor');
    $(_this).addClass('selectedColor');
    $(_this).parent().find('input').val(id);
    $('#version_form').submit();
}

$('.product_properties').change(function () {
    $('#version_form').submit();
});

function AddToCart(callbackFunction) {
    var datastring = $("#version_form").serialize();

    $.ajax({
        type: "POST",
        url: "/ShoppingCart/AddToCart",
        data: datastring,
        success: function (data) {
            if (callbackFunction && typeof (callbackFunction) === "function") {
                callbackFunction(data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function UpdateCartNumber() {
    $.ajax({
        type: "GET",
        url: "/ShoppingCart/GetCartCount",
        success: function (data) {
            if (data.result === "ok") {
                $('#cart_count_number').text(data.count);
            }
            else {
                alert(data.error)
            }
        },
        error: function (jqXHR, textStatus) {
            alert(textStatus);
        }
    });
}

function UpdateWishlistNumber() {
    $.ajax({
        type: "GET",
        url: "/ShoppingCart/GetWishlistCount",
        success: function (data) {
            if (data.result === "ok") {
                $('#wishlist_count_number').text(data.count);
            }
            else {
                alert(data.error)
            }
        },
        error: function (jqXHR, textStatus) {
            alert(textStatus);
        }
    });
}

function ShowAlertMessage(data) {
    alert(data.message);
}

function AlertFailure(data) {
    $("#alertMessage").text(data.errorMsg);
    $('#alertModal').modal('show');
}

function UpdateCartSuccessful(data) {
    if (data.result === "ok") {
        vmCart.GetCart();
        UpdateCartNumber();
    }
    else {
        location.reload();
    }
}

function UpdateAccountInfoSuccess(data) {
    vmInfo.isEdited = false;
    initCustomerInfo = JSON.parse(JSON.stringify(vmInfo.customerInfo));
}

function UpdateAccountInfoFailed(data) {
    alert(data.errorMsg);
}

function ChangeSelectedCity(_this, districtSelector, districtPlaceholder) {
    var baseUrl = '/Common/GetDistrict';
    var id = $(_this).val();
    PopulateDropdownFromJson(baseUrl + "/" + id, districtSelector, "", districtPlaceholder);
    $(districtSelector).trigger('change');
};

function ChangeSelectedDistrict(_this, wardSelector, wardPlaceholder) {
    var baseUrl = '/Common/GetWard';
    var id = $(_this).val();
    PopulateDropdownFromJson(baseUrl + "/" + id, wardSelector, "", wardPlaceholder);
};

function InitAjaxForm(selector) {
    if (typeof selector != 'undefined') {
        var form = $(selector)
        .removeData("validator")
        .removeData("unobtrusiveValidation");

        $.validator.unobtrusive.parse(form);
    }

};

function LoginDone(data) {
    if (data.errors) {
        var loginError = $('#login_error_summary');
        loginError.empty();
        for (index = 0; index < data.errors.length; ++index) {
            loginError.append($('<li>').append(
                $('<span>').append(data.errors[index])
            ));
        }
    }
    else {
        if (data.result === "Success") {
            location.reload();
        }
    }
}

function RegisterDone(data) {
    if (data.result === "Success") {
        location.reload();
    }
}

function ShowAlertDialog(message) {
    $("#alertMessage").text(message);
    $('#alertModal').modal('show');
}

function ChangeUrl(object) {
    if (typeof (history.pushState) != "undefined") {
        var url, index = document.URL.indexOf('?');
        if (index > 0) {
            url = document.URL.substr(0, index);
        }
        else
            url = document.URL;
        var isFirstAtt = true;
        for (var i in object) {
            if (object.hasOwnProperty(i)) {
                if (isFirstAtt) {
                    if (object[i] != '') {
                        url += "?" + i + "=" + object[i];
                        isFirstAtt = false;
                    }
                }
                else {
                    url += object[i] != '' ? "&" + i + "=" + object[i] : "";
                }
            }
        }

        history.pushState({}, null, url);
    }
}


function UpdateQueryStringParameter(key, value) {
    var uri = document.URL;
    if (value) {
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.match(re)) {
            return uri.replace(re, '$1' + key + "=" + value + '$2');
        }
        else {
            return uri + separator + key + "=" + value;
        }
    }
    else {
        return uri;
    }
}

$('#search_area').submit(function (e) {
    e.preventDefault();
    window.location.href = $('#search_area').attr("action") + "?query=" + $('#query').val();
});

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}