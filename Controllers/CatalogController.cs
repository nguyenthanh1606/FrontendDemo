using AutoMapper;
using Store.Service.ProductServices;
using Store.Service.Service;
using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Service.Helper;
using Store.Data.Models;
using PagedList.Mvc;
using PagedList;
using Frontend.Infrastructure.ExtensionMethod;
using Store.Service.SystemService;
using Frontend.Infrastructure.Mapping;
using Frontend.Infrastructure.Helpers;
using Resources;
using Store.Service.Helper.ExtensionMethod;

namespace Frontend.Controllers
{
    public partial class CatalogController : BaseFrontendController
    {
        #region field and constructor
        private readonly ISystemService _systemService;
        private readonly IProductService _productService;
        private readonly IProductGroupService _productGroupService;
        private readonly IProductGroupPropertiesService _productGPService;
        private readonly IProductVersionService _productVersionService;
        private readonly IDiscountService _discountService;
        private readonly IProductSearchService _productSearchService;
        private readonly IBrandService _brandService;
        private readonly IProductGroupPropertiesService _productGroupPropertiesService;
        private readonly IProductAdsDistributionService _productAdsDistributionService;
        private readonly IProductAdsService _productAdsService;
        private readonly IMapper _mapper;
        private readonly List<int> _availableItemPerPage;

        public CatalogController(ISystemService systemService, IMapper mapper, IProductGroupService productGroupService,
            IProductService productService, IProductGroupPropertiesService productGPService, IProductVersionService productVersionService,
            IDiscountService discountService, IProductSearchService productSearchService, IBrandService brandService,
            IProductAdsDistributionService productAdsDistributionService, IProductAdsService productAdsService, IProductGroupPropertiesService productGroupPropertiesService)
        {
            _systemService = systemService;
            _productGroupService = productGroupService;
            _productService = productService;
            _mapper = mapper;
            _productGPService = productGPService;
            _productVersionService = productVersionService;
            _discountService = discountService;
            _productSearchService = productSearchService;
            _brandService = brandService;
            _productAdsDistributionService = productAdsDistributionService;
            _productAdsService = productAdsService;
            _productGroupPropertiesService = productGroupPropertiesService;
        }
        #endregion

        //// Use below ProductGroup action because attr and price filter not required
        //public ActionResult ProductGroup(int id, string name, string query, string attr, string price, 
        //    int? brand = null, string order = null, int page = 1)
        //{
        //    int? minPrice, maxPrice;
        //    minPrice = maxPrice = null;
        //    int min, max;
        //    if (!string.IsNullOrEmpty(price))
        //    {
        //        string[] listPrice = price.Split('.');
        //        //only filter when price can split to exactly 2 string (min and max)
        //        if (listPrice.Count() == 2)
        //        {
        //            if (int.TryParse(listPrice[0], out min) && int.TryParse(listPrice[1], out max))
        //            {
        //                minPrice = min;
        //                maxPrice = max;
        //            }
        //        }
        //    }
        //    SearchProductResult searchResult;
        //    //try to parse enum sort order
        //    ProductSortOrder sortOrder;
        //    if(Enum.TryParse(order, true, out sortOrder))
        //    {
        //        searchResult = _productSearchService.SearchProduct(query, _language, id, brand, minPrice, maxPrice, attr, sortOrder);
        //    }
        //    else
        //    {
        //        searchResult = _productSearchService.SearchProduct(query, _language, id, brand, minPrice, maxPrice, attr);
        //    }
        //    //return page no products found
        //    //if(searchResult.ListProduct.Count() == 0)
        //    //{
        //    //    return View("NoProductFound", id);
        //    //}
        //    //Map search result to list product and list category for filter
        //    SearchViewModel model = _mapper.Map<SearchViewModel>(searchResult);
        //    var productGroupIds = searchResult.ListProductProperties.Select(o => o.GroupPropertyID);
        //    var groupedProductGroupProperty = searchResult.ListProductProperties.GroupBy(o => o.GroupPropertyID);
        //    foreach (var group in groupedProductGroupProperty)
        //    {
        //        ProductGroupProperty groupProperty = _productGPService.Get(group.Key);
        //        model.ListCategory.ProductAttributes.Add(groupProperty, group.ToList());
        //    }
        //    if(!string.IsNullOrEmpty(name))
        //    {
        //        searchResult.ListProduct = searchResult.ListProduct.Where(o => o.Title.StartsWith(name));
        //    }
        //    model.ListProduct = CustomMapping.ListProductToPagedListProductBox(searchResult.ListProduct, page, 
        //        searchResult.ListProductProperties.ToList());
        //    model.ProductGroup = _mapper.Map<ProductGroupViewModel>(_productGroupService.Get(id));

        //    foreach (var productads in _productAdsDistributionService.ProductAdsDistribution_GetAllOrderedForPositionAndProductGroupID(_language, 1, model.ProductGroup.GroupID).ToList())
        //    {
        //        if (model.ProductGroup.ListProductAds == null)
        //        {
        //            model.ProductGroup.ListProductAds  = new List<ProductAds>();
        //        }
        //        else
        //        {
        //            model.ProductGroup.ListProductAds.Add(_productAdsService.Get(productads.ProductAdID));
        //        }

        //    }

        //    model.ListProduct.ToList().ForEach(t =>
        //    {
        //        // setup coupon code
        //        var discounts = _discountService.GetDiscountsAppliedToProduct(t.ProductID);
        //        if (discounts != null && discounts.Count() != 0)
        //        {
        //            var temp = discounts.First();
        //            t.CouponCode = temp.CouponCode;
        //            if (temp.UsePercentage)
        //                t.DiscountCodeValue = temp.DiscountPercent.ToPercentageString();
        //            else
        //                t.DiscountCodeValue = temp.DiscountAmount.ToString();
        //        }
        //    });

        //    //viewbag to check current select filter values
        //    string baseUrl = Request.Url.GetLeftPart(UriPartial.Path);
        //    if (!string.IsNullOrEmpty(query))
        //    {
        //        baseUrl = baseUrl + "?query=" + query;
        //    }
        //    string[] listSelectedProperties = null;
        //    if (attr != null)
        //    {
        //        listSelectedProperties = attr.Split('.');
        //    }

        //    if (brand.HasValue)
        //    {
        //        var selectedBrand = _brandService.Get(brand.Value);
        //        if (selectedBrand != null)
        //        {
        //            model.ListFilter.Add(new Tuple<string, string>(Resource.Brand + ": " + selectedBrand.Name,
        //                Utility.ContructUrl(baseUrl, null, price, listSelectedProperties)));
        //        }
        //    }
        //    if (minPrice.HasValue && maxPrice.HasValue)
        //    {
        //        model.ListFilter.Add(new Tuple<string, string>(Resource.Price + ": từ " +
        //            string.Format("{0:n0}", minPrice.Value) + " đến " + string.Format("{0:n0}", maxPrice.Value),
        //                Utility.ContructUrl(baseUrl, brand, null, listSelectedProperties)));
        //    }

        //    if (attr != null)
        //    {
        //        var listProperties = model.ListCategory.ProductAttributes.SelectMany(o => o.Value).ToList();
        //        var listGroupProperties = model.ListCategory.ProductAttributes.Select(o => o.Key).ToList();
        //        int prop;
        //        foreach(string selectedProperties in listSelectedProperties)
        //        {
        //            if (int.TryParse(selectedProperties, out prop))
        //            {
        //                var property = listProperties.Where(o => o.ProductPropertyID == prop).FirstOrDefault();
        //                if(property != null)
        //                {
        //                    var groupProp = listGroupProperties.Where(o => o.GroupPropertyID == property.GroupPropertyID).FirstOrDefault();
        //                    if(groupProp != null)
        //                    {
        //                        model.ListFilter.Add(new Tuple<string, string>(groupProp.Title + ": " + property.Title,
        //                            Utility.ContructUrl(baseUrl, brand, price, listSelectedProperties.Where(x => x != selectedProperties).ToArray())));
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    ViewBag.MaxPrice = _productService.getBoundaryPrice(id, true);
        //    ViewBag.MinPrice = _productService.getBoundaryPrice(id, false);
        //    ViewBag.FilterName = name;
        //    ViewBag.FilterBrand = brand;
        //    ViewBag.FilterPrice = price;
        //    ViewBag.FilterAttributes = attr;
        //    ViewBag.ListSortOrder = GetListPossibleSortOrder(!string.IsNullOrEmpty(query));
        //    ViewBag.Order = order;
        //    return View(model);
        //}

        //use this because no filter attribute or price
        public virtual ActionResult ProductGroup(int id, string query, int? brand = null, string order = "Newest", string properties = null, int page = 1, int? itemPerPage = null)
        {
            SearchProductResult searchResult;
            //try to parse enum sort order
            ProductSortOrder sortOrder;
            if (order == "Promotion")
            {
                searchResult = _productSearchService.SearchProductPromotion(query, _language, id, brand, null, null, properties);
            }
            else if (Enum.TryParse(order, true, out sortOrder))
            {
                searchResult = _productSearchService.SearchProduct(query, _language, id, brand, null, null, properties, sortOrder: sortOrder);
            }
            else
            {
                searchResult = _productSearchService.SearchProduct(query, _language, id, brand);
            }

            //Map search result to list product and list category for filter
            SearchViewModel model = _mapper.Map<SearchViewModel>(searchResult);
            model.ListProduct = CustomMapping.ListProductToPagedListProductBox(searchResult.ListProduct, page, itemPerPage);
            model.ProductGroup = _mapper.Map<ProductGroupViewModel>(_productGroupService.Get(id));
            model.ProductGroup.LastGroupParentID = _productGroupService.GetLastProductGroupParentId(model.ProductGroup.GroupID);
            model.ProductGroup.GroupParentName = _productGroupService.Get(model.ProductGroup.LastGroupParentID).Title;
            //ad of product group
            model.ProductGroup.ListProductAds = _mapper.Map<IEnumerable<ProductGroupAdViewModel>>(_productAdsService.GetAll(_language, (int)ProductAdStatus.Deploying, id));

            //coupon code for product
            model.ListProduct.ToList().ForEach(t =>
            {
                // setup coupon code
                var discounts = _discountService.GetDiscountsAppliedToProduct(t.ProductID);
                if (discounts != null && discounts.Count() != 0)
                {
                    var temp = discounts.First();
                    t.CouponCode = temp.CouponCode;
                    if (temp.UsePercentage)
                        t.DiscountCodeValue = temp.DiscountPercent.ToPercentageString();
                    else
                        t.DiscountCodeValue = temp.DiscountAmount.ToString();
                }
            });

            model.ProductGroup.ProductGroupProperties = _mapper.Map<List<ProductGroupPropertyViewModel>>(_productGroupPropertiesService.GetByGroupId(model.ProductGroup.GroupID, true, true));

            foreach (var pp in model.ProductGroup.ProductGroupProperties)
            {
                pp.ListProductProperty = _mapper.Map<List<ProductPropertyViewModel>>(_productGroupPropertiesService.GetListProductProperties(pp.GroupPropertyID));
            }

            //viewbag for current filter
            ViewBag.ListSortOrder = GetListPossibleSortOrder(!string.IsNullOrEmpty(query));
            ViewBag.ItemPerPage = itemPerPage;
            ViewBag.FilterBrand = brand;
            ViewBag.Order = order;
            ViewBag.Query = query;
            return View(model);
        }

        //currently sort order when search not including order by rating
        public SelectList GetListPossibleSortOrder(bool isIncludeRelevant = false)
        {
            var listSortOrder = from ProductSortOrder stt in Enum.GetValues(typeof(ProductSortOrder))
                                where (stt!= ProductSortOrder.Relevant && stt != ProductSortOrder.HighestRating && stt != ProductSortOrder.HighestPriority)
                                select new
                                {
                                    Id = stt.ToString(),
                                    Name = stt.GetDescription()
                                };
            return new SelectList(listSortOrder, "Id", "Name");
        }

        //// Use below ProductGroup action because attr and price filter not required
        //public ActionResult Brand(int id, string query, int page = 1, string price = null)
        //{
        //    if (!string.IsNullOrEmpty(query) && query.Length > 128)
        //    {
        //        query = query.Substring(0, 128);
        //    }

        //    int? minPrice, maxPrice;
        //    minPrice = maxPrice = null;
        //    int min, max;
        //    if (!string.IsNullOrEmpty(price))
        //    {
        //        string[] listPrice = price.Split('.');
        //        //only filter when price can split to exactly 2 string (min and max)
        //        if (listPrice.Count() == 2)
        //        {
        //            if (int.TryParse(listPrice[0], out min) && int.TryParse(listPrice[1], out max))
        //            {
        //                minPrice = min;
        //                maxPrice = max;
        //            }
        //        }
        //    }
        //    var searchResult = _productSearchService.SearchProduct(query, _language, null, id, minPrice, maxPrice);

        //    //Map search result to list product and list category for filter
        //    SearchViewModel model = _mapper.Map<SearchViewModel>(searchResult);
        //    model.Brand = _mapper.Map<BrandCategoryViewModel>(_brandService.Get(id));
        //    var productGroupIds = searchResult.ListProductProperties.Select(o => o.GroupPropertyID);
        //    var groupedProductGroupProperty = searchResult.ListProductProperties.GroupBy(o => o.GroupPropertyID);
        //    foreach (var group in groupedProductGroupProperty)
        //    {
        //        ProductGroupProperty groupProperty = _productGPService.Get(group.Key);
        //        model.ListCategory.ProductAttributes.Add(groupProperty, group.ToList());
        //    }
        //    model.ListProduct = CustomMapping.ListProductToPagedListProductBox(searchResult.ListProduct, page,
        //        searchResult.ListProductProperties.ToList());

        //    //viewbag to check current select filter values
        //    string baseUrl = Request.Url.GetLeftPart(UriPartial.Path);
        //    if(!string.IsNullOrEmpty(query))
        //    {
        //        baseUrl = baseUrl + "?query=" + query;
        //    }
        //    if (minPrice.HasValue && maxPrice.HasValue)
        //    {
        //        model.ListFilter.Add(new Tuple<string, string>(Resource.Price + ": từ " +
        //            string.Format("{0:n0}", minPrice.Value) + " đến " + string.Format("{0:n0}", maxPrice.Value),
        //                Utility.ContructUrl(baseUrl, id, null, null)));
        //    }
        //    //viewbag to check current select filter values
        //    ViewBag.FilterBrand = id;
        //    ViewBag.FilterPrice = price;
        //    ViewBag.CurrentQuery = query;
        //    ViewBag.QueryString = "brand=" + id + Request.QueryString;
        //    return View(model);
        //}

        //use this because no filter attribute or price
        public virtual ActionResult Brand(int id, string query, string order = null, int page = 1, int? itemPerPage = null)
        {
            var brand = _brandService.Get(id);
            if (brand == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(query) && query.Length > 128)
            {
                query = query.Substring(0, 128);
            }

            SearchProductResult searchResult;
            ProductSortOrder sortOrder;
            if (Enum.TryParse(order, true, out sortOrder))
            {
                searchResult = _productSearchService.SearchProduct(query, _language, null, id, sortOrder: sortOrder);
            }
            else
            {
                searchResult = _productSearchService.SearchProduct(query, _language, null, id);
            }

            //Map search result to list product and list category for filter
            SearchViewModel model = _mapper.Map<SearchViewModel>(searchResult);
            model.Brand = _mapper.Map<BrandCategoryViewModel>(brand);
            var productGroupIds = searchResult.ListProductProperties.Select(o => o.GroupPropertyID);
            var groupedProductGroupProperty = searchResult.ListProductProperties.GroupBy(o => o.GroupPropertyID);
            foreach (var group in groupedProductGroupProperty)
            {
                ProductGroupProperty groupProperty = _productGPService.Get(group.Key);
                model.ListCategory.ProductAttributes.Add(groupProperty, group.ToList());
            }
            model.ListProduct = CustomMapping.ListProductToPagedListProductBox(searchResult.ListProduct, page, itemPerPage);


            //viewbag to check current select filter values
            ViewBag.ListSortOrder = GetListPossibleSortOrder(!string.IsNullOrEmpty(query));
            ViewBag.FilterBrand = id;
            ViewBag.Order = order;
            ViewBag.QueryString = "brand=" + id + Request.QueryString;
            return View(model);
        }

        public virtual PartialViewResult ProductGroupFilter(int id, string name, string attr, string price, string brand)
        {
            int maxProductPerPage;
            if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.MaxProductInSlider), out maxProductPerPage))
            {
                maxProductPerPage = 0;
            }
            var model = _mapper.Map<List<ProductBoxViewModel>>(
                _productService.GetPagedList(_language, maxProductPerPage).ToList());

            return PartialView("ProductList", model);
        }

        public virtual PartialViewResult ListProductGroupAds(int id, string viewName = null)
        {
            IEnumerable<ProductAdsViewModel> model = _mapper.Map<IEnumerable<ProductAdsViewModel>>(
                _productAdsService.GetDeployedAdForID(id));

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "ListProductGroupAds";
            }
            return PartialView(viewName, model);
        }

        // id: ProductId
        public virtual JsonResult GetPropertiesByProduct(int id, ProductGroupPropertiesType type)
        {
            var result = _productVersionService.GetPropertiesByIDAndType(id, type);
            return Json(result.ToArray());
        }

        public virtual ActionResult ProductGroupInfo(int id, string viewName = null)
        {
            ProductGroupViewModel model = _mapper.Map<ProductGroupViewModel>(_productGroupService.Get(id));
            if (model == null)
            {
                return new EmptyResult();
            }

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "ProductGroupInfo";
            }
            return PartialView(viewName, model);
        }

        public virtual ActionResult ProductGroupOnHome(string viewName = null, int number = 0)
        {
            var model = _mapper.Map<IEnumerable<ProductGroupViewModel>>(_productGroupService.GetListProductGroupOnHomePage(_language));
            if (model == null)
            {
                return new EmptyResult();
            }

            if (number < 0)
            {
                number = 0;
            }

            model = model.Take(number);

            if (model == null || model.Count() == 0)
            {
                return new EmptyResult();
            }

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "ProductGroupOnHome";
            }
            return PartialView(viewName, model);
        }

        public virtual ActionResult GetChildGroup(int id, string viewName = null, int? countItem = null, bool getAllChildGroup = false, bool getProductChildGroup = false)
        {
            if (getProductChildGroup == false)
            {
                if (id == 0)
                {
                    var model = new ProductGroupViewModel();
                    if (getAllChildGroup)
                    {
                        GetProductGroupArchitecture(model, _mapper.Map<List<ProductGroupViewModel>>(_productGroupService.GetGroupsForGroupParentID(id, ProductGroupStatus.Active)));
                    }
                    else
                    {
                        model.ChildGroup = _mapper.Map<List<ProductGroupViewModel>>(_productGroupService.GetGroupsForGroupParentID_OnlyOneLevel(id, ProductGroupStatus.Active));
                        foreach (var childgroup in model.ChildGroup)
                        {
                            childgroup.ChildGroup = _mapper.Map<List<ProductGroupViewModel>>(_productGroupService.GetGroupsForGroupParentID_OnlyOneLevel(childgroup.GroupID, ProductGroupStatus.Active));
                        }
                    }

                    if (countItem.HasValue)
                    {
                        model.ChildGroup = model.ChildGroup.Take(countItem.Value).ToList();
                    }

                    if (string.IsNullOrEmpty(viewName))
                    {
                        viewName = "GetChildGroup";
                    }
                    return PartialView(viewName, model);
                }
                else
                {
                    var model = new ProductGroupViewModel();
                    model = _mapper.Map<ProductGroupViewModel>(_productGroupService.Get(id));
                    if (model == null)
                    {
                        return new EmptyResult();
                    }

                    if (getAllChildGroup)
                    {
                        GetProductGroupArchitecture(model, _mapper.Map<List<ProductGroupViewModel>>(_productGroupService.GetGroupsForGroupParentID(id, ProductGroupStatus.Active)));
                    }
                    else
                    {
                        model.ChildGroup = _mapper.Map<List<ProductGroupViewModel>>(_productGroupService.GetGroupsForGroupParentID_OnlyOneLevel(id, ProductGroupStatus.Active));
                    }

                    if (countItem.HasValue)
                    {
                        model.ChildGroup = model.ChildGroup.Take(countItem.Value).ToList();
                    }

                    if (string.IsNullOrEmpty(viewName))
                    {
                        viewName = "GetChildGroup";
                    }
                    return PartialView(viewName, model);
                }
            }
            else
            {
                var model = _mapper.Map<ProductGroupViewModel>(_productGroupService.Get(id));
                return PartialView(viewName, model);
            }
        }


        private ProductGroupViewModel GetProductGroupArchitecture(ProductGroupViewModel productgroup, List<ProductGroupViewModel> listProductGroups)
        {
            productgroup.ChildGroup = listProductGroups.Where(t => t.GroupParentID == productgroup.GroupID).ToList();
            listProductGroups = listProductGroups.Where(t => !productgroup.ChildGroup.Select(o => o.GroupID).Contains(t.GroupID)).ToList();
            foreach (var item in productgroup.ChildGroup)
                GetProductGroupArchitecture(item, listProductGroups);

            return productgroup;
        }

    }
}