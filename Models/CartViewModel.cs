using Store.Service.ProductServices;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Service.Helper.Attributes;

namespace Frontend.Models
{
    public class ShoppingCartViewModel
    {
        public List<CartItemViewModel> ListItems { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string CouponCode { get; set; }
        public int DiscountValue { get; set; }
        public int ShippingFee { get; set; }
        public SysParaViewModel ShippingNote { get; set; }
        public SysParaViewModel Hotline { get; set; }
        public ShoppingCartViewModel()
        {
            DiscountValue = 0;
            ShippingFee = 0;
            ShippingNote = new SysParaViewModel();
            Hotline = new SysParaViewModel();
        }
    }

    public class CartItemViewModel
    {
        public CartItemViewModel()
        {
            ListProperties = new List<Tuple<string, string>>();
        }

        public int ProductVersionID { get; set; }
        public int CartId { get; set; }
        public int ProductID { get; set; }
        [DisplayResource(Name = "ProductName", ResourceType = typeof(Resource))]
        public string Title { get; set; }
        public string Summary { get; set; }
        [DisplayResource(Name = "Image", ResourceType = typeof(Resource))]
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Properties { get; set; }
        public List<Tuple<string, string>> ListProperties { get; set; }

        [DisplayResource(Name = "Price", ResourceType = typeof(Resource))]
        public int Price { get; set; }
        public int CustomPrice { get; set; }
        public int OriginalPrice { get; set; }
        [DisplayResource(Name = "Quantity", ResourceType = typeof(Resource))]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            Order = new OrderViewModel();
            ShippingNote = new SysParaViewModel();
            DirectPayment = new SysParaViewModel();
            Banking = new SysParaViewModel();
            CheckoutSuccess = new SysParaViewModel();
        }
        public string DeliviryMethod { get; set; }

        public SysParaViewModel ShippingNote { get; set; }
        public SysParaViewModel DirectPayment { get; set; }
        public SysParaViewModel Banking { get; set; }
        public SysParaViewModel CheckoutSuccess { get; set; }
        public IEnumerable<CustomerAddressOverviewViewModel> ListAddress { get; set; }
        public CustomerAddressOverviewViewModel ShippingAddress { get; set; }
        public OrderViewModel Order { get; set; }
        public IEnumerable<SelectListItem> ListPaymentType { get; set; }
        public IEnumerable<SelectListItem> ListShippingMethod { get; set; }
        public IEnumerable<BankAccountItemViewModel> ListBankAccount { get; set; }
        public AccountViewModel AccountViewModel { get; set; }
    }

    public class BankAccountItemViewModel
    {
        public int Id { get; set; }
        public virtual string BankName { get; set; }
        public virtual string Alias { get; set; }
        public virtual string AccountName { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual int City { get; set; }
        public virtual int District { get; set; }
        public virtual int Ward { get; set; }
        public virtual string AgencyName { get; set; }
        public virtual string Value1 { get; set; }
        public virtual string Value2 { get; set; }
        public virtual int intValue1 { get; set; }
        public virtual int intValue2 { get; set; }
        public virtual int Status { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
    }


    public class CustomerOrderViewModel
    {
        public int OrderID { get; set; }
        public PaymentType PaymentType { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
        public CancelOrderReason? CancelReason { get; set; }

        [DisplayResource(Name = "AddressReceive", ResourceType = typeof(Resource))]
        public string AddressReceive { get; set; }

        public string Email { get; set; }

        [DisplayResource(Name = "AddressPay", ResourceType = typeof(Resource))]
        public string AddressPay { get; set; }
        public DateTime? OrderDate { get; set; }

        [DisplayResource(Name = "Subtotal", ResourceType = typeof(Resource))]
        public double Subtotal { get; set; }

        [DisplayResource(Name = "Total", ResourceType = typeof(Resource))]
        public double Total { get; set; }

        [DisplayResource(Name = "Discount", ResourceType = typeof(Resource))]
        public double? Discount { get; set; }

        [DisplayResource(Name = "ShippingFee", ResourceType = typeof(Resource))]
        public string ShippingFee { get; set; }
        public OrderStatus Status { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public IEnumerable<OrderCartViewModel> OrderCart { get; set; }
    }

    public class OrderCartViewModel
    {
        public int OrderCartID { get; set; }
        public string Title { get; set; }
        public int ProductVersionID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
        public string ImageUrl { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string FriendlyUrl { get; set; }
        public List<Tuple<string, string>> ListProperties { get; set; }
    }

    public class OrderViewModel
    {
        [DisplayResource(Name = "PaymentMethod", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int PaymentType { get; set; }

        [DisplayResource(Name = "ShippingMethod", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int ShippingMethod { get; set; }

        //[DisplayResource(Name = "ShippingDate", ResourceType = typeof(Resource))]
        //public string ShippingDate { get; set; }

        public bool UseNameOnAddress { get; set; }

        [EmailAddress]
        [DisplayResource(Name = "Email", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [DisplayResource(Name = "ReceiverName", ResourceType = typeof(Resource))]
        [MaxLength(128, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string ReceiverName { get; set; }

        [DisplayResource(Name = "ReceiverPhone", ResourceType = typeof(Resource))]
        [MaxLength(15, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string ReceiverPhone { get; set; }

        public string ShippingFee { get; set; }
        public int? CustomerAddressId { get; set; }
        public string AddressReceive { get; set; }
        public string AddressPay { get; set; }
        public string DeliveryDate { get; set; }

        public CustomerAddressViewModel CustomerAddress { get; set; }
    }
}