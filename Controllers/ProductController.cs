using AutoMapper;
using Store.Service.ProductServices;
using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Store.Data.Models;
using Store.Service.SystemService;
using Store.Service.Helper;
using Store.Service.Service;
using Microsoft.AspNet.Identity;
using System.Net;
using Store.Service.Authorize;
using Resources;
using Frontend.Infrastructure.Utility;
using Store.Service.Helper.ExtensionMethod;
using Frontend.Infrastructure.Helpers;

namespace Frontend.Controllers
{
    public partial class ProductController : BaseFrontendController
    {
        #region field and constructor
        private readonly IProductService _productService;
        private readonly IProductGroupService _productGroupService;
        private readonly IProductVersionService _productVersionService;
        private readonly IBrandService _brandService;
        private readonly IRoutingService _routingService;
        private readonly IProductGroupPropertiesService _productGroupPropertiesService;
        private readonly ISystemService _systemService;
        private readonly IProductCommentService _productCommentService;
        private readonly ApplicationUserManager _userManager;
        private readonly IWishlistService _wishlistService;
        private readonly IMapper _mapper;
        private const int commentPerPage = 10;

        public ProductController(IMapper mapper, IProductService productService, IProductGroupService productGroupService, IProductVersionService productVersionService,
            IProductGroupPropertiesService productGroupPropertiesService, IBrandService brandService,
            IRoutingService routingService, ISystemService systemService, IProductCommentService productCommentService,
            ApplicationUserManager userManager,
            IWishlistService wishlistService)
        {
            _productService = productService;
            _productGroupService = productGroupService;
            _productVersionService = productVersionService;
            _productGroupPropertiesService = productGroupPropertiesService;
            _brandService = brandService;
            _routingService = routingService;
            _systemService = systemService;
            _productCommentService = productCommentService;
            _userManager = userManager;
            _wishlistService = wishlistService;
            _mapper = mapper;
        }
        #endregion

        #region Product Details
        // GET: Product
        public virtual ActionResult Details(int id = 0)
        {
            var product = _productService.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var model = _mapper.Map<ProductDetailsViewModel>(product);

            //get current select properties
            var selectedProperties = _productService.GetAllPropertiesForProduct(id);
            var productVersions = _productVersionService.GetListForProduct(id).ToList();
            List<int> firstVersionProperties = new List<int>();
            //group all property and it selection
            foreach (ProductGroupProperty item in selectedProperties)
            {
                //get display name and type for group property
                PropertyVersionViewModel propertyVersion = new PropertyVersionViewModel
                {
                    Id = item.GroupPropertyID,
                    Name = item.Title,
                    Type = item.Type
                };
                //add list of selection to each group
                foreach (ProductProperty property in item.ListProductProperty)
                {
                    propertyVersion.ListProperty.Add(_mapper.Map<ProductPropertyViewModel>(property));
                }
                //save the info about fist version
                if (item.ListProductProperty.Count >= 1)
                {
                    firstVersionProperties.Add(item.ListProductProperty.First().ProductPropertyID);
                }

                model.ListVersionProperty.Add(propertyVersion);
            }

            //get the 1st version info

            var version = _productVersionService.Get(productVersions[0].ProductVersionID);
            if (version != null)
            {
                if (version.DefaultPrice.HasValue)
                {
                    model.DefaultPrice = version.DefaultPrice.Value;
                }
                //if (!string.IsNullOrEmpty(version.ImageUrls))
                //{
                //    model.ListImages = version.ImageUrls.Split('|').ToList();
                //}

                //check quantity
                if (version.InventoryNumber.HasValue && version.InventoryNumber.Value == 0)
                {
                    model.Condition = ProductCondition.OutOfStock;
                }
                else
                {
                    model.Condition = ProductCondition.InStock;
                }
                ViewBag.DisplayInventoryNumber = product.DisplayCurrentStockQuantity;
                if (!product.DisplayCurrentStockQuantity)
                {
                    model.InventoryNumber = null;
                }
            }

            if (model.DefaultImageUrls != null)
            {
                model.ListImages = model.DefaultImageUrls.Split('|').ToList();
            }
            //Get brand info
            if (model.BrandId.HasValue)
            {
                var brand = _brandService.Get(model.BrandId.Value);
                if (brand != null)
                {
                    model.Brand = new Tuple<string, string>(brand.Name, _routingService.GetFriendlyUrl(RoutingType.Brand, model.BrandId.Value));
                }
            }

            if (model.ProductGroupID != 0)
            {
                model.ProductGroup = _mapper.Map<ProductGroupViewModel>(_productGroupService.Get(model.ProductGroupID));
                model.ProductGroup.LastGroupParentID = _productGroupService.GetLastProductGroupParentId(model.ProductGroup.GroupID);
                model.ProductGroup.GroupParentName = _productGroupService.Get(model.ProductGroup.LastGroupParentID).Title;
            }
            //get comment
            model.CommentSummary = _mapper.Map<ProductCommentSummaryViewModel>(_productCommentService
            .GetSummaryForProduct(id));

            //Get list sortorder for comment
            ViewBag.CommentSortOrder = from CommentSortOrder stt in Enum.GetValues(typeof(CommentSortOrder))
                                       select new SelectListItem
                                       {
                                           Value = ((int)stt).ToString(),
                                           Text = stt.GetDescription()
                                       };


            ViewBag.ProductId = id;
            ViewBag.CommentPerPage = commentPerPage;
            if (User.Identity.IsAuthenticated)
            {
                model.ExistInWishList = _wishlistService.GetForCustomerIDAndProductID(model.ProductID, User.Identity.GetUserId<int>()).HasValue;
            }
            model.PaymentMethod = _mapper.Map<SysParaViewModel>(_systemService.GetForID(30));

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult VersionDetails(int productId, string[] properties)
        {
            var version = _productVersionService.Get(productId, properties);
            if (version == null)
            {
                return Json(new { result = "null" });
            }

            var product = _productService.Get(productId);
            if (!version.DefaultPrice.HasValue)
            {
                version.DefaultPrice = product.DefaultPrice;
            }
            if (!version.OriginalPrice.HasValue)
            {
                version.OriginalPrice = product.OriginalPrice;
            }
            if (string.IsNullOrEmpty(version.ImageUrls))
            {
                version.ImageUrls = product.DefaultImageUrls;
            }

            var result = _mapper.Map<ProductVersionViewModel>(version);
            if (version.InventoryNumber.HasValue && version.InventoryNumber.Value == 0)
            {
                result.Condition = ProductCondition.OutOfStock.GetDescription();
            }
            else
            {
                result.Condition = ProductCondition.InStock.GetDescription();
            }
            if (!product.DisplayCurrentStockQuantity)
            {
                result.InventoryNumber = null;
            }
            return Json(new { result = "ok", data = JsonConvert.SerializeObject(result) });

        }
        #endregion

        #region Product comment
        public virtual JsonResult RatingSummary(int id)
        {
            var ratingSummary = _productCommentService.GetSummaryForProduct(id);
            return Json(new { averate = ratingSummary.AverageRating, total = ratingSummary.TotalRating }, JsonRequestBehavior.AllowGet);
        }

        public virtual PartialViewResult ListComment(int id)
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual JsonResult CommentProduct(ProductCommentViewModel comment, int productId = 0, int? parentComment = null)
        {
            if (ModelState.IsValid && productId != 0)
            {
                ProductComment newComment = _mapper.Map<ProductComment>(comment);
                if (comment.Id == 0)
                {
                    newComment.ProductId = productId;
                    newComment.CustomerId = User.Identity.GetUserId<int>();
                    newComment.ParentComment = parentComment;
                    if (_productCommentService.Insert(newComment))
                    {
                        return Json(new { result = "ok" });
                    }
                }
                else
                {
                    newComment.CustomerId = User.Identity.GetUserId<int>();
                    newComment.ProductId = productId;
                    if (_productCommentService.Update(newComment))
                    {
                        return Json(new { result = "ok" });
                    }
                }
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = Resource.UnknowErrorMessage });
            }
            var errorList = (from item in ModelState.Values
                             from error in item.Errors
                             select error.ErrorMessage).ToList();
            return Json(new { errors = errorList });
        }

        public virtual JsonResult GetListComment(int id, int page = 1, CommentSortOrder sortOrder = CommentSortOrder.MostUseful)
        {
            var listComment = _productCommentService.GetCommentForProduct(id, commentPerPage, page, sortOrder).ToList();
            var result = _mapper.Map<List<ProductCommentViewModel>>(listComment);
            var listUpvoteCommentId = _productCommentService.GetRatedCommentForProductAndUser(
                id, User.Identity.GetUserId<int>());
            for (int i = 0; i < listComment.Count; i++)
            {
                var customer = _userManager.FindById(listComment[i].CustomerId);
                if (customer != null)
                {
                    result[i].CustomerName = customer.FullName;
                }
                else
                {
                    result[i].CustomerName = "guest";
                }

                int rate;
                if (listUpvoteCommentId.TryGetValue(result[i].Id, out rate))
                {
                    result[i].Rate = rate;
                }
                else
                {
                    result[i].Rate = 0;
                }
            }
            return Json(new { listComment = result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public virtual JsonResult GetUserComment(int id)
        {
            var userComment = _mapper.Map<ProductCommentViewModel>(
                _productCommentService.GetProductAndUser(id, User.Identity.GetUserId<int>()));
            if (userComment != null)
            {
                return Json(new { userComment = userComment }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { userComment = new ProductCommentViewModel() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Authorize]
        public virtual JsonResult RateComment(int id, bool rateUp)
        {
            if (_productCommentService.Rate(id, User.Identity.GetUserId<int>(), rateUp))
            {
                return Json(new { result = "Success" });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { error = Resource.UnknowErrorMessage });
        }
        #endregion

        #region Product List
        [ChildActionOnly]
        public virtual ActionResult ListProductSameBrand(int id, string viewName = null, int? countItem = null)
        {
            if (!countItem.HasValue)
            {
                int maxProductInSlider;
                if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.MaxProductInSlider), out maxProductInSlider))
                {
                    maxProductInSlider = 0;
                }
                countItem = maxProductInSlider;
            }
            if (countItem.Value < 0)
            {
                countItem = 0;
            }


            var products = _mapper.Map<List<ProductBoxViewModel>>(_productService
                .GetPagedListSameBrand(id, _language, countItem.Value).ToList());
            if (products.Count == 0)
            {
                return new EmptyResult();
            }
            foreach (var product in products)
            {
                Utility.MapProductAditionalAttribute(product);
            }

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "ListProductSameBrand";
            }
            return PartialView(viewName, products);
        }

        [ChildActionOnly]
        public virtual ActionResult ListProductSameGroup(int id, string viewName = null, int? countItem = null)
        {
            if (!countItem.HasValue)
            {
                int maxProductInSlider;
                if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.MaxProductInSlider), out maxProductInSlider))
                {
                    maxProductInSlider = 0;
                }
                countItem = maxProductInSlider;
            }
            if (countItem.Value < 0)
            {
                countItem = 0;
            }

            var p = _productService.Get(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            var model = _mapper.Map<ProductDetailsViewModel>(p);

            var products = _mapper.Map<List<ProductBoxViewModel>>(_productService
                .GetByProductGroupId(model.ProductGroupID).Take(countItem.Value).ToList());
            if (products.Count == 0)
            {
                return new EmptyResult();
            }
            foreach (var product in products)
            {
                Utility.MapProductAditionalAttribute(product);
            }

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "ListProductSameGroup";
            }
            return PartialView(viewName, products);
        }

        [ChildActionOnly]
        public virtual ActionResult ListProduct(int? groupId = null, ProductSortOrder order = ProductSortOrder.Newest, string viewName = null, int? countItem = null)
        {
            if (!countItem.HasValue)
            {
                int maxProductInSlider;
                if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.MaxProductInSlider), out maxProductInSlider))
                {
                    maxProductInSlider = 0;
                }
                countItem = maxProductInSlider;
            }
            if (countItem.Value < 0)
            {
                countItem = 0;
            }

            IEnumerable<ProductBoxViewModel> products;
            if (groupId.HasValue)
            {
                products = _mapper.Map<IEnumerable<ProductBoxViewModel>>(_productService
                                .GetByProductGroupId(groupId.Value, order).Take(countItem.Value));
            }
            else
            {
                products = _mapper.Map<IEnumerable<ProductBoxViewModel>>(_productService
                                .GetList(_language, order).Take(countItem.Value));
            }

            if (products.Count() == 0)
            {
                return new EmptyResult();
            }

            foreach (var product in products)
            {
                Utility.MapProductAditionalAttribute(product);
            }
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "ListProduct";
            }
            return PartialView(viewName, products);
        }
        #endregion

        #region Product Group


        #endregion

        #region Helper

        #endregion
    }
}