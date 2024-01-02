// intial for menumobile
$(document).ready(function () {

    // make select menu item
    var pathname = window.location.pathname;
    $(".pushmenu li a[href='" + pathname + "']").addClass('selected'); // menu mobile    
});

var touch = $('#touch-menu');
var menu = $('.menu');

$(function () {
    $('#productmenu').hover(function () {
        $('#submenu').toggleClass('active');
    });
});

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



$(function () {
    var pgurl = window.location.pathname;
    $(".li-menu").hover(function () {
        $(".li-menu").removeClass("active");
        $(this).addClass("active");
    })
});

$(touch).click(function () {
    menu.slideToggle();
});

function AddToCartFail(result) {
    var data = $.parseJSON(result.responseText);
    if (data.result == "redirect") {
        window.location.href = "/" + data.url;
    }
    if(data.error)
    {
        alert(data.error);
    }
}

function AddToCartSuccess(){
    $('#addToCartModal').modal('show');
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

$('#btnAddToCart').click(function () {
    var datastring = $("#version_form").serialize();

    $.ajax({
        type: "POST",
        url: "/ShoppingCart/AddToCart",
        data: datastring,
        success: function (data) {
            $('#product_modal_info').html(data);
            AddToCartSuccess();
            UpdateCartNumber();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
});

function UpdateCartNumber(){
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

function SelectVersionSuccess(result) {
    if (result.result == "ok") {
        var data = JSON.parse(result.data);
        $('#ProductPrice').text(data.Price.toLocaleString(curLang, { minimumFractionDigits: 0 }) + " VNĐ");
        $('#ProductStatusValue').text(data.Condition);
        $('#ProductCountValue').text(data.InventoryNumber);
        if (data.ListImages.length > 0) {
            $('#productThumb').attr({
                "src": data.ListImages[0],
                "data-zoom-image": data.ListImages[0]
            });
        }
        
        mySwiper.removeAllSlides();
        $('.zoomContainer').remove();
        thumb.removeData('elevateZoom');
        thumb.removeData('zoomImage');

        for (i = 0; i < data.ListImages.length; i++) {
            mySwiper.appendSlide(
                $('<a>').attr({
                    href: '#',
                    'data-image': data.ListImages[i],
                    class: "swiper-slide"
                }).append(
                    $('<img>').attr("src", data.ListImages[i])
                )
            );
        }

        thumb.elevateZoom({
            gallery: 'product-gallery',
            cursor: 'pointer',
            galleryActiveClass: 'active'
        });

    }
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
    var baseUrl = '/BankAccount/GetDistrict';
    var id = $(_this).val();
    PopulateDropdownFromJson(baseUrl + "/" + id, districtSelector, "", districtPlaceholder);
    $(districtSelector).trigger('change');
};

function ChangeSelectedDistrict(_this, wardSelector, wardPlaceholder) {
    var baseUrl = '/BankAccount/GetWard';
    var id = $(_this).val();
    PopulateDropdownFromJson(baseUrl + "/" + id, wardSelector, "", wardPlaceholder);
};

function InitAjaxForm(selector) {
    if (typeof selector != 'undefined'){
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