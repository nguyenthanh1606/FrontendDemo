var $filter = $("#filterAttributes");
var $filterPrice = $("#filterPrice");
var $filterBrand = $("#filterBrand");
var $filterName = $("#filterName");

// initial silder price
function initialSliderPrice($value1, $value2) {
    $("#filter_slider").slider({
        range: true,
        min: minPrice,
        max: maxPrice,
        values: [$value1, $value2],
        slide: function (event, ui) {
            $("#price").html("Từ <span style=\"color:red\">" + ui.values[0] + "</span> đến <span style=\"color:red\">" + ui.values[1] + "</span>");
        },
        change: function (event, ui) {
            if (ui.values[1] == maxPrice && ui.values[0] == minPrice)
                $filterPrice.val('');
            else
                $filterPrice.val(ui.values[0] + "." + ui.values[1]);

        }
    });
    $("#price").html("Từ <span style=\"color:red\">" + $("#filter_slider").slider("values", 0) + "</span> đến <span style=\"color:red\">" +
    $("#filter_slider").slider("values", 1) + "</span>");
}



$(document).ready(function () {
    var listAttr = $filter.val();
    if (listAttr) {
        var splits = listAttr.split(".");
        for (var i = 0; i < splits.length; i++) {
            $('.type-search ul[name="attribute"] li input[value="' + splits[i] + '"]').prop("checked", true);
        }
        $('.type-search ul[name="brand"] li input[value="' + $filterBrand.val() + '"]').prop("checked", true);
    }

    if ($filterPrice.val() == '')
        initialSliderPrice(minPrice, maxPrice);
    else {
        splits = $filterPrice.val().split(".");
        initialSliderPrice(splits[0], splits[1]);
    }

    // when attr checked/unchecked in filter category
    $('ul[name="attribute"] .productype').change(function () {

        var $fil = $filter;

        // update hidden filterAttr
        if (this.checked) {
            if ($fil.val() == "" || $fil.val() == null) {
                $fil.val(this.value);
            } else {
                $fil.val($fil.val() + '.' + this.value);
            }

            $("#SearchBar").append('<li value="' + this.value + '"><span>' + this.value + '</span>' + $(this).next().text() + '</li>');
        }
        else {
            $fil.val(($fil.val() + '.').replace(this.value + '.', ''));
            $fil.val($fil.val().substr(0, $fil.val().length - 1));

            // update search bar
            $('li[value="' + this.value + '"]').remove();
        }


        loadSearchBar();
        // update product list
        loadProduct();
});

    $('ul[name="brand"] .productype').change(function () {

        var $fil = $filterBrand;

        // update hidden filterAttr
        if (this.checked) {
            $fil.val(this.value);

            // make single checkbox
            $('ul.category.brand li input').removeAttr("checked");
            $(this).prop("checked", true);

            // update search bar
            $('#SearchBar li[name="brand"]').remove();
            $("#SearchBar").append('<li name="brand" value="' + this.value + '"><span>' + this.value + '</span>' + $(this).next().text() + '</li>');
        }
        else {
            $fil.val(($fil.val() + '.').replace(this.value + '.', ''));
            $fil.val($fil.val().substr(0, $fil.val().length - 1));

            // update search bar
            $('li[value="' + this.value + '"]').remove();
        }


        loadSearchBar();
        // update product list
        loadProduct();
    });

    loadSearchBar();
});

function loadProduct() {

    // if url have 'attr', just update it
    var url, index = document.URL.indexOf('?');
    if (index > 0) {
        url = document.URL.substr(0, index);
    }
    else
        url = document.URL;

    url += $('#currentQuery').val() ? "?query=" + $('#currentQuery').val() : "?";
    url += $filter.val() ? "&attr=" + $filter.val() : "";
    url += $filterBrand.val() ? "&brand=" + $filterBrand.val() : "";
    url += $filterPrice.val() ? "&price=" + $filterPrice.val() : "";
    url += $filterName.val() ? "&name=" + $filterName.val() : "";

    window.location.href = url;
}


function loadSearchBar() {
    //$('#SearchBar li').remove();
    //// update search bar for price
    ////$('#SearchBar li[value="rangePrice"]').remove();
    //if ($filterPrice.val() != '' && $filterPrice.val() != null) {
    //    var prices = $filterPrice.val().replace('?price=', '').split('.');
    //    $("#SearchBar").append('<li value="rangePrice"><span></span>' + prices[0] + ' đến ' + prices[1] + '</li>');
    //}

    //var t = window.URL;
    //var listAttr = $filter.val();
    //if (listAttr) {
    //    var attrs = listAttr.split(".");
    //    $.each(attrs, function (index, value) {
    //        if (value != null && value != '') {
    //            $("#SearchBar").append('<li value="' + value + '"><span>' + value + '</span>' + $('#' + value).next().text() + '</li>');
    //        }
    //    });
    //}
    //// update for attribute
    
}