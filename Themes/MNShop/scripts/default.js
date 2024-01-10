var vmShoppingCart = {
  created: function () {
    this.GetCart();
  },
  methods: {
    GetCart: function () {
      var xhr = new XMLHttpRequest();
      var self = this;
      xhr.open("GET", "/ShoppingCart/ListJson");
      xhr.onload = function () {
        var data = JSON.parse(xhr.responseText);
        self.cart = data.cart;
      };
      xhr.send();
    },
  },
  computed: {
    totalItems: function () {
      var self = this;
      var key = "Quantity";
      if (self.cart) {
        return self.cart.reduce(function (total, item) {
          return total + item[key];
        }, 0);
      } else {
        return 0;
      }
    },
    sumMoney: function () {
      var key1 = "Price";
      var key3 = "CustomPrice";
      var key2 = "Quantity";
      var self = this;
        if (self.cart !== null) {

        return self.cart.reduce(function (total, item) {
            return total + item[key3] * item[key2];
        }, 0);
      } else {
        return 0;
      }
    },
    getTotalPrice: function () {
      return this.sumMoney - this.discountValue;
    },
  },
};

function AddToCart() {
  var datastring = $("#version_form").serialize();

  $.ajax({
    type: "POST",
    url: "/ShoppingCart/AddToCart",
    data: datastring,
    success: function (data) {
      $("#product_modal_info").html(data);
      AddToCartSuccess();
      UpdateCartNumber();
    },
    error: function (jqXHR, textStatus, errorThrown) {
      toastr.error("Vui lòng thử lại sau", "Error");
    },
  });
}

function AddToCartSuccess(data) {
  //$('#product_modal_info').html(data);
  //$('#addToCartModal').modal('show');
  //$('#addToCartModal').css({
  //    "opacity": "1"
  //});
  //vmMenuCart.GetCart();
  toastr.success("Thêm vào giỏ hàng thành công!", "Success");
  UpdateCartNumber();
}
