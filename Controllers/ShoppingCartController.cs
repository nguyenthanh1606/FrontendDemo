using AutoMapper;
using Microsoft.AspNet.Identity;
using Store.Data.Models;
using Store.Service.ProductServices;
using Store.Service.Service;
using Store.Service.SystemService;
using Frontend.Infrastructure.Helpers;
using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Store.Service.Helper;
using Resources;
using Store.Service.Helper.ExtensionMethod;
using Frontend.Infrastructure.ExtensionMethod;
using Store.Service.Authorize;
using System.Threading.Tasks;

namespace Frontend.Controllers
{
    public partial class ShoppingCartController : BaseFrontendController
    {
        #region field and constructor
        private readonly ISystemService _systemService;
        private readonly IProductService _productService;
        private readonly IProductGroupService _productGroupService;
        private readonly IProductVersionService _productVersionService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICustomerAddressService _customerAddressService;
        private readonly IOrderService _orderService;
        private readonly IProductGroupPropertiesService _productGroupPropertiesService;
        private readonly IRoutingService _routingService;
        private readonly IDiscountService _discountService;
        private readonly IShoppingCartDetailService _shoppingCartDetailService;
        private readonly IDiscountUsageHistoryService _discountUsageHistoryService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IMapper _mapper;
        private readonly IWishlistService _wishListService;
        private readonly IGeneralService _generalService;
        private readonly ApplicationUserManager _userManager;
        public const string CartKey = "CartId";

        public ShoppingCartController(ISystemService systemService, IMapper mapper, IProductGroupService productGroupService,
            IProductService productService, IShoppingCartService shoppingCartService, IRoutingService routingService,
            IProductVersionService productVersionService, IProductGroupPropertiesService productGroupPropertiesService,
            ICustomerAddressService customerAddressService, IOrderService orderService, IDiscountService discountService,
            IDiscountUsageHistoryService discountUsageHistoryService, IGeneralService generalService,
            IShoppingCartDetailService shoppingCartDetailService, IBankAccountService bankAccountService, ApplicationUserManager userManager,
            IWishlistService wishListService)
        {
            _systemService = systemService;
            _productGroupService = productGroupService;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _routingService = routingService;
            _productVersionService = productVersionService;
            _customerAddressService = customerAddressService;
            _orderService = orderService;
            _productGroupPropertiesService = productGroupPropertiesService;
            _discountService = discountService;
            _shoppingCartDetailService = shoppingCartDetailService;
            _discountUsageHistoryService = discountUsageHistoryService;
            _bankAccountService = bankAccountService;
            _mapper = mapper;
            _wishListService = wishListService;
            _generalService = generalService;
            _userManager = userManager;
        }
        #endregion

        #region Action

        /// <summary>
        /// Add to cart from a product details view
        /// </summary>
        /// <param name="productVersionId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult AddToCart(int productId, string[] properties, int quantity = 1, int customPrice = 0)
        {
            var product = _productService.Get(productId);
            if (product.Status != (int)ProductStatus.StopSelling && quantity > 0)
            {
                var version = _productVersionService.Get(productId, properties);
                if (version == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = Resource.ProductNotExisted });
                }
                ShoppingCart cart = new ShoppingCart
                {
                    ProductVersionID = version.ProductVersionID,
                    Quantity = quantity,
                    CartId = ShoppingCartHelper.GetCartId(),
                    CustomPrice = customPrice,
                };
                int? newId = _shoppingCartService.AddToCart(cart);
                if (newId.HasValue)
                {
                    CartItemViewModel model = _mapper.Map<CartItemViewModel>(_shoppingCartService.GetForCartId(newId.Value));
                    //get list of properties for display info
                    model.ListProperties = _productGroupPropertiesService.GetProductPropertiesValue(version.Properties);

                    return PartialView("ModalCartDetail", model);
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { error = Resource.UnknowErrorMessage });
        }

        /// <summary>
        /// Add to Favorite from a product details view
        /// </summary>
        /// <param name="productVersionId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public virtual JsonResult AddToWishlist(int id)
        {
            var product = _productService.Get(id);
            if (product == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { result = false, msg = Resource.ProductNotExisted });
            }

            var userId = User.Identity.GetUserId<int>();
            var oldWishlish = _wishListService.GetForCustomerIDAndProductID(id, userId);
            if (!oldWishlish.HasValue)
            {
                Wishlist wishlist = new Wishlist
                {
                    ProductId = id,
                    WishlistId = userId.ToString()
                };

                int? newId = _wishListService.Insert(wishlist);
                if (newId.HasValue)
                {
                    return Json(new { result = true, msg = Resource.AddToWishlishSuccessMsg });
                }
            }
            else
            {
                return Json(new { result = false, msg = Resource.ProductExistYourWishlist });
            }



            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { result = false, msg = Resource.UnknowErrorMessage });
        }

        [HttpPost, Authorize]
        public virtual JsonResult RemoveFromWishlist(int id)
        {
            var userId = User.Identity.GetUserId<int>();
            var oldWishlish = _wishListService.GetForCustomerIDAndProductID(id, userId);
            if (oldWishlish.HasValue)
            {
                _wishListService.Delete(oldWishlish.Value);
            }

            return Json(new { result = true, msg = Resource.RemoveFromWishlistSuccessMsg });
        }

        /// <summary>
        /// Add to cart from a general view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult AddToCart_Catalog(int id)
        {
            //Check if product has stop selling
            var product = _productService.Get(id);
            if (product.Status == (int)ProductStatus.StopSelling)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = Resource.ProductNotExisted });
            }

            //id is productId
            var versions = _productVersionService.GetListForProduct(id);
            if (versions.Count() == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = Resource.ProductNotExisted });
            }
            //if there are just 1 version, add directly
            if (versions.Count() == 1)
            {
                ProductVersion selectedVersion = versions.First();
                ShoppingCart cart = new ShoppingCart
                {
                    ProductVersionID = selectedVersion.ProductVersionID,
                    Quantity = 1,
                    CartId = ShoppingCartHelper.GetCartId()
                };
                int? newId = _shoppingCartService.AddToCart(cart);
                if (newId.HasValue)
                {
                    CartItemViewModel model = _mapper.Map<CartItemViewModel>(_shoppingCartService.GetForCartId(newId.Value));
                    //get list of properties for display info
                    model.ListProperties = _productGroupPropertiesService.GetProductPropertiesValue(selectedVersion.Properties);
                    return PartialView("ModalCartDetail", model);
                }

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = Resource.UnknowErrorMessage });
            }
            //if not, redirect to product detail page
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Redirect;
                return Json(new { result = "redirect", url = _routingService.GetFriendlyUrl(RoutingType.Product, id) });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual JsonResult UpdateCart(ShoppingCartViewModel model, int? deleteBtn)
        {
            //Check if delete button clicked
            if (deleteBtn.HasValue)
            {
                if (_shoppingCartService.RemoveFromCart(deleteBtn.Value, ShoppingCartHelper.GetCartId()))
                {
                    int count = _shoppingCartService.CountCartItems(ShoppingCartHelper.GetCartId());
                    if (count > 0)
                    {
                        return Json(new { result = "ok" });
                    }
                    else
                    {
                        return Json(new { result = "empty" });
                    }
                }
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { result = "error", message = Resource.UnknowErrorMessage });
            }
            //if not update whole cart
            if (ModelState.IsValid)
            {
                List<ShoppingCart> cartItems = _mapper.Map<List<ShoppingCart>>(model.ListItems);
                if (_shoppingCartService.UpdateCart(cartItems, ShoppingCartHelper.GetCartId()))
                {
                    return Json(new { result = "ok" });
                }
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { result = "error", message = Resource.UnknowErrorMessage });
        }

        public virtual ActionResult DeleteAllCart(ShoppingCartViewModel model)
        {
            //Check if delete button clicked
            model.ListItems = GetListCartItem();

            foreach (var pv in model.ListItems)
            {
                _shoppingCartService.RemoveFromCart(pv.ProductVersionID, ShoppingCartHelper.GetCartId());
            }

            return RedirectToAction("EmptyCart", "ShoppingCart");
        }

        public virtual ActionResult List()
        {
            int count = _shoppingCartService.CountCartItems(ShoppingCartHelper.GetCartId());
            if (count == 0)
            {
                return View("EmptyCart");
            }

            // get discount of shopping cart
            var model = new ShoppingCartViewModel();

            var cartDetail = _shoppingCartDetailService.GetByCartID(ShoppingCartHelper.GetCartId());
            if (cartDetail != null && string.IsNullOrEmpty(cartDetail.CouponCode))
            {
                var doesDiscount = _discountService.DoesApplyingDiscountToCustomer(ShoppingCartHelper.GetCartId(), model.CouponCode);
                if (doesDiscount.Key == ApplyingDiscountError.Passed)
                {
                    model.CouponCode = cartDetail.CouponCode;
                    model.DiscountValue = (int)doesDiscount.Value;
                }
            }
            model.ShippingNote = _mapper.Map<SysParaViewModel>(_systemService.GetForID(26));
            model.Hotline = _mapper.Map<SysParaViewModel>(_systemService.GetForID(24));
            return View(model);
        }

        public virtual ActionResult EmptyCart()
        {
            //int count = _shoppingCartService.CountCartItems(ShoppingCartHelper.GetCartId());
            //if (count == 0)
            //{
            //    return View("EmptyCart");
            //}
            return View();
        }

        public virtual JsonResult ListJson()
        {
            List<CartItemViewModel> model = GetListCartItem();
            return Json(new { cart = model }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetCartCount()
        {
            int count = _shoppingCartService.CountCartItems(ShoppingCartHelper.GetCartId());
            return Json(new { result = "ok", count = count }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetWishlistCount()
        {
            int count = _wishListService.GetListWishlist(ShoppingCartHelper.GetCartId()).Count();
            return Json(new { result = "ok", count = count }, JsonRequestBehavior.AllowGet);
        }

        public virtual PartialViewResult OrderSummary()
        {
            ShoppingCartViewModel model = new ShoppingCartViewModel();
            model.ListItems = GetListCartItem();
            var cartDetail = _shoppingCartDetailService.GetByCartID(ShoppingCartHelper.GetCartId());
            if (cartDetail != null && !string.IsNullOrEmpty(cartDetail.CouponCode.Trim()))
            {
                var doesDiscount = _discountService.DoesApplyingDiscountToCustomer(ShoppingCartHelper.GetCartId(), cartDetail.CouponCode.Trim());
                //if (doesDiscount.Key == ApplyingDiscountError.Passed)
                //{
                model.CouponCode = cartDetail.CouponCode.Trim();
                model.DiscountValue = (int)doesDiscount.Value;
               // model.ShippingFee = cartDetail.
                //}
            }
            return PartialView(model);
        }

        public virtual ActionResult Checkout()
        {
            int count = _shoppingCartService.CountCartItems(ShoppingCartHelper.GetCartId());
            if (count == 0)
            {
                return View("EmptyCart");
            }

            CheckoutViewModel model = new CheckoutViewModel();
            if (User.Identity.IsAuthenticated)
            {
                model.ListAddress = _mapper.Map<IEnumerable<CustomerAddressOverviewViewModel>>(
                _customerAddressService.GetListAddress(User.Identity.GetUserId<int>()));
                ViewBag.Email = _userManager.GetEmail(User.Identity.GetUserId<int>());
            }
            else
            {
                ViewBag.ListCity = new SelectList(_generalService.GetAllCity().Where(t => t.Value1.Trim() == "1"), "Id", "Name");
                ViewBag.ListDistrict = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ListWard = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ListAddressType = new SelectList(_generalService.GetAllAddressType(), "AddressTypeID", "AddressTypeName");
            }

            var paymentStatus = from PaymentType stt in Enum.GetValues(typeof(PaymentType))
                                select new
                                {
                                    Id = (int)stt,
                                    Name = stt.GetDescription()
                                };
            model.ListPaymentType = new SelectList(paymentStatus, "Id", "Name");
            model.ShippingNote = _mapper.Map<SysParaViewModel>(_systemService.GetForID(26));
            model.DirectPayment = _mapper.Map<SysParaViewModel>(_systemService.GetForID(27));
            model.Banking = _mapper.Map<SysParaViewModel>(_systemService.GetForID(28));
            model.CheckoutSuccess = _mapper.Map<SysParaViewModel>(_systemService.GetForID(29));

            var shippingMethod = from ShippingMethod stt in Enum.GetValues(typeof(ShippingMethod))
                                 select new
                                 {
                                     Id = (int)stt,
                                     Name = stt.GetDescription()
                                 };
            model.ListShippingMethod = new SelectList(shippingMethod, "Id", "Name");
            model.DeliviryMethod = _systemService.GetSystemParameter(SysParaType.DeliveryMethod);
            var listBank = _bankAccountService.GetList("all", 0);
            if (listBank != null)
            {
                model.ListBankAccount = _mapper.Map<IEnumerable<BankAccountItemViewModel>>(listBank);
            }

            var allCities = _generalService.GetAllCity();


            foreach (var bankAccount in model.ListBankAccount)
            {
               bankAccount.CityName = allCities.Where(t => t.Id == bankAccount.City).Select(o => o.Name).FirstOrDefault();
                var alldist = _generalService.GetDistrictForCity(bankAccount.City);
                bankAccount.DistrictName = alldist.Where(t => t.Id == bankAccount.District).Select(o => o.Type).FirstOrDefault() + " " + alldist.Where(t => t.Id == bankAccount.District).Select(o => o.Name).FirstOrDefault();

            }
            return View(model);
        }

        public virtual ActionResult CheckoutSuccess()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            model.CheckoutSuccess = _mapper.Map<SysParaViewModel>(_systemService.GetForID(29));
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Checkout(CheckoutViewModel model)
        {
            ModelState.Remove("AccountViewModel");
            ModelState.Remove("ShippingAddress");
            if (User.Identity.IsAuthenticated)
            {
                //set email
                model.Order.Email = _userManager.GetEmail(User.Identity.GetUserId<int>());

                ModelState.Remove("Order.CustomerAddress");
                ModelState.Remove("Order.Email");
                if (!model.Order.CustomerAddressId.HasValue)
                {
                    ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
                    return RedirectToAction("Checkout");
                }
            }
            else
            {
                if (model.Order.CustomerAddress == null)
                {
                    ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
                    return RedirectToAction("Checkout");
                }
            }

            if (ModelState.IsValid)
            {
                //use stored address of customer
                if (User.Identity.IsAuthenticated && model.Order.CustomerAddressId.HasValue)
                {
                    CustomerAddress cAddress = _customerAddressService.Get(model.Order.CustomerAddressId.Value);
                    if (cAddress == null)
                    {
                        ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
                        return RedirectToAction("Checkout");
                    }
                    if (cAddress.CustomerID != User.Identity.GetUserId<int>())
                    {
                        ModelState.AddModelError(string.Empty, Resource.AddressNotCorrectMsg);
                        return RedirectToAction("Checkout");
                    }

                    var address = _customerAddressService.GetAddressWithNameById(model.Order.CustomerAddressId.Value);
                    model.Order.AddressReceive = string.Join(", ", address.Address, address.District, address.City);
                    model.Order.CustomerAddress = new CustomerAddressViewModel { CustomerName = cAddress.CustomerName, Phone = cAddress.Phone };
                }
                //newsly input address
                else
                {
                    string wardName = _generalService.GetWardFullName(model.Order.CustomerAddress.Ward.Value);
                    model.Order.AddressReceive = string.Join(", ", model.Order.CustomerAddress.Address, wardName);
                }

                Order order = _mapper.Map<Order>(model.Order);
                if (!string.IsNullOrWhiteSpace(model.Order.DeliveryDate))
                {
                    order.DeliveryDate = DateTime.Parse(model.Order.DeliveryDate);
                }

                order.CustomerID = User.Identity.GetUserId<int>();
                order.CustomerName = model.Order.CustomerAddress.CustomerName;
                order.CustomerPhone = model.Order.CustomerAddress.Phone;

                if (model.Order.UseNameOnAddress)
                {
                    order.ReceiverName = model.Order.CustomerAddress.CustomerName;
                    order.ReceiverPhone = model.Order.CustomerAddress.Phone;
                }

                // check for applied couponcode
                var cartDetail = _shoppingCartDetailService.GetByCartID(ShoppingCartHelper.GetCartId());
                var doesApplied = new KeyValuePair<ApplyingDiscountError, double>(ApplyingDiscountError.CantApplyingDiscount, 0);
                if (cartDetail != null)
                    doesApplied = _discountService.DoesApplyingDiscountToCustomer(ShoppingCartHelper.GetCartId(), cartDetail.CouponCode.Trim());


                int? orderId = _orderService.OrderForCartId(ShoppingCartHelper.GetCartId(), order);
                // Tài: ----- insert discount history and setup discount price for orderCart------
                if (orderId != null && doesApplied.Key == ApplyingDiscountError.Passed)
                    _discountService.AppliedDiscountForOrderCart(orderId.Value, ShoppingCartHelper.GetCartId());


                return RedirectToAction("CheckoutSuccess");
            }
            ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
            return RedirectToAction("Checkout");
        }


        [NonAction]
        private List<CartItemViewModel> GetListCartItem()
        {
            IEnumerable<ShoppingCartItem> listCart = _shoppingCartService.GetShoppingCart(ShoppingCartHelper.GetCartId());
            List<CartItemViewModel> result = _mapper.Map<List<CartItemViewModel>>(listCart.ToList());
            foreach (var item in result)
            {
                item.ListProperties = _productGroupPropertiesService.GetProductPropertiesValue(item.Properties);
                item.Url = _routingService.GetFriendlyUrl(RoutingType.Product, item.ProductID);
            }
            return result;
        }


        public virtual ActionResult ApplyCoupon(string CouponCode)
        {
            string message = "";
            Discount discount = _discountService.GetByCouponCode(CouponCode);

            if (discount.DiscountLimitationId == (int)DiscountLimitationType.NTimesPerCustomer &&
                !User.Identity.IsAuthenticated) // customer have to login
            {
                return Json(new { Message = Resource.DiscountNeedLogin, DiscountPrice = 0 });
            }


            var result = _discountService.DoesApplyingDiscountToCustomer(ShoppingCartHelper.GetCartId(), CouponCode.Trim());
            if (result.Key == ApplyingDiscountError.CantApplyingDiscount)
                message = Resource.CantApplyingDiscount;
            else if (result.Key == ApplyingDiscountError.CouponCodeNotExist)
                message = Resource.CouponCodeNotExist;
            else if (result.Key == ApplyingDiscountError.ExpirationDate)
                message = Resource.ExpirationDate;
            else if (result.Key == ApplyingDiscountError.CouponCodeWasUsed)
                message = Resource.CouponCodeWasUsed;
            else if (result.Key == ApplyingDiscountError.OutOfNTimesOnly)
                message = string.Format(Resource.OutOfNTimesOnly, discount.LimitationTimes);
            else if (result.Key == ApplyingDiscountError.OutOfNTimesPerCustomer)
                message = string.Format(Resource.OutOfNTimesPerCustomer, discount.LimitationTimes);
            else if (result.Key == ApplyingDiscountError.Passed)
            {
                _shoppingCartDetailService.InsertOrUpdate(ShoppingCartHelper.GetCartId(), CouponCode.Trim());
                message = null;
            }

            return Json(new { Message = message, DiscountPrice = (int)result.Value });

        }

        // Get order info with user doesnot login
        public virtual ActionResult MyOrders(int id)
        {
            var model = _mapper.Map<CustomerOrderViewModel>(_orderService.Get(id));
            var list = new List<CustomerOrderViewModel>();
            list.Add(model);
            return View(list);
        }


        public virtual ActionResult OrderDetails(int id = 0)
        {
            Order order = _orderService.Get(id, true);
            if (order == null)
                return HttpNotFound();


            CustomerOrderViewModel model = _mapper.Map<CustomerOrderViewModel>(order);
            foreach (var item in model.OrderCart)
            {
                item.ListProperties = _productVersionService.GetProductPropertiesValue(item.ProductVersionID);
            }

            //list cancel reason
            var cancelReasons = from CancelOrderReason stt in Enum.GetValues(typeof(CancelOrderReason))
                                select new
                                {
                                    Id = (int)stt,
                                    Name = stt.GetDescription()
                                };
            ViewBag.CancelOrderReasons = new SelectList(cancelReasons, "Id", "Name");

            return View(model);
        }
        #endregion
    }
}