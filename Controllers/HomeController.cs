using AutoMapper;
using Store.Service.Service;
using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Data;
using PagedList;
using Store.Data.Models;
using Store.Service.ProductServices;
using Store.Service.SystemService;
using Frontend.Infrastructure.Mapping;

using Admin.Infrastructure.ExtensionMethod;
using Resources;
using Frontend.Infrastructure.Helpers;
using Store.Service.Helper.ExtensionMethod;
using System.Net;
using System.Xml;
using System.IO;


namespace Frontend.Controllers
{
    [RequireHttps]
    public partial class HomeController : BaseFrontendController
    {
        #region field and constructor
        private readonly ISystemService _systemService;
        private readonly IContentService _contentService;
        private readonly IGroupService _groupService;
        private readonly IProductService _productService;
        private readonly IGeneralService _generalService;
        private readonly IDistributionService _distributionService;
        private readonly IDiscountService _discountService;
        private readonly IAdService _adService;
        private readonly IProductGroupService _productGroupService;
        private readonly IProductSearchService _productSearchService;
        private readonly IProductGroupPropertiesService _productGroupPropertyService;
        private readonly IBrandService _brandService;
        private readonly IProductAdsService _productAdsService;
        private readonly IProductAdsDistributionService _productAdsDistributionService;
        private readonly ILinkService _linkService;
        private readonly ITagService _tagService;
        private readonly IFeedBackService _feedbackService;

        private readonly IMapper _mapper;

        public HomeController(ISystemService systemService, IContentService contentService, IMapper mapper,
            IGroupService groupservice, IDistributionService dstService, IGeneralService generalService,
            IProductService productService, IAdService adService, IDiscountService discountService,
            IProductGroupService productGroupService, IProductSearchService productSearchService,
            IProductGroupPropertiesService productGroupPropertyService, IBrandService brandService,
            IProductAdsService productAdsService, IProductAdsDistributionService productAdsDistributionService,
            ILinkService linkService, ITagService tagService, IFeedBackService feedbackService)
        {
            _systemService = systemService;
            _contentService = contentService;
            _groupService = groupservice;
            _productService = productService;
            _distributionService = dstService;
            _generalService = generalService;
            _adService = adService;
            _discountService = discountService;
            _productSearchService = productSearchService;
            _mapper = mapper;
            _productGroupPropertyService = productGroupPropertyService;
            _brandService = brandService;
            _productGroupService = productGroupService;
            _productAdsService = productAdsService;
            _productAdsDistributionService = productAdsDistributionService;
            _linkService = linkService;
            _tagService = tagService;
            _feedbackService = feedbackService;
        }
        #endregion

        protected void SetupCouponCodeForProduct(IEnumerable<Object> list)
        {
            var products = list as IEnumerable<ProductBoxViewModel>;
            if (products == null) return;

            foreach (var item in products)
            {
                var discounts = _discountService.GetDiscountsAppliedToProduct(item.ProductID);
                if (discounts != null && discounts.Count() != 0)
                {
                    var temp = discounts.First();
                    item.CouponCode = temp.CouponCode;
                    if (temp.UsePercentage)
                        item.DiscountCodeValue = temp.DiscountPercent.ToPercentageString();
                    else
                        item.DiscountCodeValue = temp.DiscountAmount.ToString();
                }
            }
        }

        public virtual ActionResult Index()
        {
            var cookielang = Request.Cookies["lang"].Value;
            var ad = _adService.GetAllDeployedAdForPosition(2, cookielang).FirstOrDefault();

            if (ad != null && !string.IsNullOrEmpty(ad.ImageUrl))
            {
                ViewBag.HomeImage = ad.ImageUrl.ToString();
            }
            else
            {
                ViewBag.HomeImage = "~/Content/UserFiles/Images/default.jpeg";
            }
            return View();
        }

        public virtual ActionResult ChangeLanguage(string lang, string returnUrl)
        {
            var langCookie = new HttpCookie("lang", lang)
            {
                HttpOnly = true
            };
            Response.AppendCookie(langCookie);


            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index");
            }

            return Redirect(returnUrl);
        }

        public virtual ActionResult Search(string query, int? brand = null, string order = null, int page = 1, int? itemPerPage = null)
        {
            if (query == null)
            {
                return RedirectToAction("Index");
            }

            if (query.Length > 128)
            {
                query = query.Substring(0, 128);
            }

            SearchProductResult searchResult;
            //try to parse enum sort order
            ProductSortOrder sortOrder;
            if (Enum.TryParse(order, true, out sortOrder))
            {
                searchResult = _productSearchService.SearchProduct(query, _language, null, brand, sortOrder: sortOrder);
            }
            else
            {
                searchResult = _productSearchService.SearchProduct(query, _language, null, brand);
            }

            //Map search result to list product and list category for filter
            SearchViewModel model = _mapper.Map<SearchViewModel>(searchResult);
            var productGroupIds = searchResult.ListProductProperties.Select(o => o.GroupPropertyID);
            var groupedProductGroupProperty = searchResult.ListProductProperties.GroupBy(o => o.GroupPropertyID);
            foreach (var group in groupedProductGroupProperty)
            {
                ProductGroupProperty groupProperty = _productGroupPropertyService.Get(group.Key);
                model.ListCategory.ProductAttributes.Add(groupProperty, group.ToList());
            }
            model.ListProduct = CustomMapping.ListProductToPagedListProductBox(searchResult.ListProduct, page, itemPerPage);

            //viewbag to check current select filter values
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Path);
            baseUrl = baseUrl + "?query=" + query;
            if (brand.HasValue)
            {
                var selectedBrand = _brandService.Get(brand.Value);
                if (selectedBrand != null)
                {
                    model.ListFilter.Add(new Tuple<string, string>(Resource.Brand + ": " + selectedBrand.Name,
                        Utility.ContructUrl(baseUrl, null, null, null)));
                }
            }
            ViewBag.FilterBrand = brand;
            ViewBag.CurrentQuery = query;
            ViewBag.QueryString = Request.QueryString;
            ViewBag.Order = order;
            var listSortOrder = from ProductSortOrder stt in Enum.GetValues(typeof(ProductSortOrder))
                                select new
                                {
                                    Id = stt.ToString(),
                                    Name = stt.GetDescription()
                                };
            ViewBag.ListSortOrder = new SelectList(listSortOrder, "Id", "Name");
            return View(model);
        }

        public virtual ActionResult GetPostedContents(string viewName, int groupID, int countItem, DistributionOrder order = DistributionOrder.HighestPriority)
        {
            if (countItem < 0)
            {
                countItem = 0;
            }
            var result = _distributionService.GetOrderedForGroupID(groupID, _language, order).Take(countItem);
            IEnumerable<ContentViewModel> model = Enumerable.Empty<ContentViewModel>();
            if (result.Any())
            {
                // update published date
                foreach (var item in result)
                {
                    item.Item2.CreationDate = item.Item2.CreationDate;
                }
                model = _mapper.Map<IEnumerable<ContentViewModel>>(result.Select(o => o.Item2));
            }

            return PartialView(viewName, model);
        }

        public virtual ActionResult GetPartnerLinks(string viewName, int countItem)
        {
            var model = _mapper.Map<IEnumerable<PartnerLinkViewModel>>(_linkService.GetAll(_language).Take(countItem));
            return PartialView(viewName, model);
        }

        /// <summary>
        /// Get list content that in group last layout is news
        /// </summary>
        /// <param name="countItem"></param>
        /// <param name="order"></param>
        /// <param name="getTopGroupInfo"></param>
        /// <returns></returns>
        //public virtual PartialViewResult ListNewsPartial(int countItem, ContentSortOrder order = ContentSortOrder.Lastest, bool getTopGroupInfo = false, string viewName = null)
        //{
        //    if (countItem < 0)
        //    {
        //        countItem = 0;
        //    }
        //    HomePageGroupViewModel model = new HomePageGroupViewModel();

        //    if (getTopGroupInfo)
        //    {
        //        var group = _groupService.GetListOrderedByGroupLayout((int)CurrentGroupLayout.LayoutContentTiles, 1, _language).FirstOrDefault();
        //        if (group != null)
        //        {
        //            model.Group = _mapper.Map<GroupViewModel>(group);
        //        }
        //        else
        //        {
        //            model.Group = new GroupViewModel();
        //        }
        //    }

        //    var c = _contentService
        //        .GetAllOrderedByGroupLayout(_language, (int)CurrentGroupLayout.LayoutContentTiles, ContentStatus.Published, order, countItem);
        //    model.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(_contentService
        //        .GetAllOrderedByGroupLayout(_language, (int)CurrentGroupLayout.LayoutContentTiles, ContentStatus.Published, order, countItem));

        //    return PartialView(viewName ?? "ListNewsPartial", model);
        //}

        //public virtual PartialViewResult GetTopContentByGroupLayout(string viewName, int groupLayout, int countItem, bool getTopGroupInfo = false)
        //{
        //    if (countItem < 0)
        //    {
        //        countItem = 0;
        //    }
        //    HomePageGroupViewModel model = new HomePageGroupViewModel();
        //    //get info of top group by layout if requested
        //    if (getTopGroupInfo)
        //    {
        //        var group = _groupService.GetListOrderedByGroupLayout(groupLayout, 1, _language).FirstOrDefault();
        //        if (group != null)
        //        {
        //            model.Group = _mapper.Map<GroupViewModel>(group);
        //        }
        //        else
        //        {
        //            model.Group = new GroupViewModel();
        //        }
        //    }

        //    model.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(_contentService
        //        .GetAllOrderedByGroupLayout(_language, groupLayout, ContentStatus.Published, ContentSortOrder.TopPriority, countItem));

        //    return PartialView(viewName, model);
        //}

        //public virtual PartialViewResult GetContentForGroupID(string viewName, int groupID, int countItem, bool getTopGroupInfo = false)
        //{
        //    if (countItem < 0)
        //    {
        //        countItem = 0;
        //    }
        //    HomePageGroupViewModel model = new HomePageGroupViewModel();

        //    if (getTopGroupInfo)
        //    {
        //        var group = _groupService.GetGroupForID(groupID);
        //        if (group != null)
        //        {
        //            model.Group = _mapper.Map<GroupViewModel>(group);
        //        }
        //        else
        //        {
        //            model.Group = new GroupViewModel();
        //        }
        //    }

        //    model.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(_distributionService.GetOrderedForGroupID(groupID, _language, DistributionOrder.HighestPriority).Take(countItem));

        //    return PartialView(viewName, model);
        //}

        public virtual PartialViewResult ListTagPartial(string viewName = null, int? groupId = null)
        {
            //if groupId is null -> get all tag
            IEnumerable<TagViewModel> model = groupId.HasValue ?
                _mapper.Map<IEnumerable<TagViewModel>>(_tagService.GetTagForGroup(groupId.Value)) :
                _mapper.Map<IEnumerable<TagViewModel>>(_tagService.GetList());
            return PartialView(viewName ?? "ListTagPartial", model);
        }

        private string GetFormattedXml(string url)
        {
            // Create a web client.
            using (WebClient client = new WebClient())
            {
                // Get the response string from the URL.
                string xml = client.DownloadString(url);

                // Load the response into an XML document.
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(xml);

                // Format the XML.
                using (StringWriter string_writer = new StringWriter())
                {
                    XmlTextWriter xml_text_writer =
                        new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = Formatting.Indented;
                    xml_document.WriteTo(xml_text_writer);

                    // Return the result.
                    return string_writer.ToString();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual JsonResult Contact(ContactViewModel customerContact, bool isContact = true)
        {
            //if (ModelState.IsValid)
            //{
            if (customerContact.DateSchedule != null)
            {
                customerContact.DateSchedule = Convert.ToDateTime(customerContact.DateSchedule);
            }
            var result = _feedbackService.Insert(_mapper.Map<FeedBack>(customerContact), isContact ? FeedbackType.Contact : FeedbackType.Contact);
            //if (result.HasValue)
            //{

            if (isContact == true)
            {

            }
            else
            {
                // SendEmail(customerContact, product);
            }
            return Json(new { result = true, msg = Resource.ContactSuccessMessage });
            //}
            //}

            //Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //return Json(new { result = false, msg = Resource.UnknowErrorMessage });
        }
    }
}