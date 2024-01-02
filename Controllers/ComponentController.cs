using Store.Service.Service;
using Store.Service.SystemService;
using Store.Service.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Frontend.Models;
using Store.Service.Helper;
using Frontend.Infrastructure.ExtensionMethod;
using Frontend.Infrastructure.Attributes;
using Store.Data.Models;
using Store.Service.ProductServices;
using Frontend.Infrastructure.Helpers;
using AutoMapper;
using Resources;
using Store.Service.Models;
using Admin.Infrastructure.Helpers;
using System.Text;
using System.Text.RegularExpressions;

namespace Frontend.Controllers
{
    public partial class ComponentController : BaseFrontendController
    {
        private readonly IAccountService _accountService;
        private readonly IMenuService _menuService;
        private readonly ISystemService _systemService;
        private readonly IEmailAdsService _emailAdsService;
        private readonly IProductService _productService;
        private readonly IProductGroupService _productGroupService;
        private readonly IProductGroupPropertiesService _productGroupProService;
        private readonly IBrandService _brandService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IGroupService _groupService;
        private readonly IContentService _contentService;
        private readonly IAdService _adService;
        private readonly IDistributionService _distributionService;
        private readonly ITagService _tagService;
        private readonly ILinkService _linkService;
        private readonly IRoutingService _routingService;
        private readonly IAgencyService _agencyService;
        private readonly IMapper _mapper;
        private readonly IProductSearchService _productSearchService;

        public ComponentController(IAccountService accountService, IMenuService menuService, ISystemService systemService, IEmailAdsService emailAdsService, IProductGroupService proGroupSer,
            IProductService productService, IProductGroupPropertiesService proGroupProSer, IBrandService brandService,
            IShoppingCartService shoppingCartService, IGroupService groupService, IContentService contentService,
            IAdService adService, IMapper mapper, IDistributionService distributionService, ITagService tagService, ILinkService linkService, IRoutingService routingService, IAgencyService agencyService, IProductSearchService productSearchService)
        {
            _accountService = accountService;
            _menuService = menuService;
            _systemService = systemService;
            _emailAdsService = emailAdsService;
            _productService = productService;
            _productGroupService = proGroupSer;
            _productGroupProService = proGroupProSer;
            _brandService = brandService;
            _shoppingCartService = shoppingCartService;
            _groupService = groupService;
            _contentService = contentService;
            _adService = adService;
            _distributionService = distributionService;
            _tagService = tagService;
            _linkService = linkService;
            _routingService = routingService;
            _agencyService = agencyService;
            _mapper = mapper;
            _productSearchService = productSearchService;

        }

        //[ChildActionOnly, Localization, OutputCache(Duration = 3600, VaryByParam = "lang")]
        [Localization]
        public virtual ActionResult Header(string lang)
        {
            HeaderViewModel result = new HeaderViewModel();
            result.MainMenu = _mapper.Map<TreeNode<MenuItem>>(_menuService.GetMenu(MenuType.MainMenu, lang));
            result.TopMenu = _mapper.Map<TreeNode<MenuItem>>(_menuService.GetMenu(MenuType.TopMenu, lang));
            result.MetroMenu = _mapper.Map<TreeNode<MenuItem>>(_menuService.GetMenu(MenuType.MetroMenu, lang));
            result.MenuMobile = _mapper.Map<TreeNode<MenuItem>>(_menuService.GetMenu(MenuType.MobileMenu, lang));
            ViewBag.CurrentHref = Request.RawUrl;
            result.Hotline = _systemService.GetSystemParameter(SysParaType.Hotline);
            result.SiteTitle = _systemService.GetSystemParameter(SysParaType.SiteTitle);
            result.HeaderTitle = _systemService.GetSystemParameter(SysParaType.HeaderAddress);
            return PartialView(result);
        }

        //[ChildActionOnly, Localization, OutputCache(Duration = 3600, VaryByParam = "lang")]
        [Localization]
        public virtual PartialViewResult MainMenu(string lang)
        {
            MainMenuViewModel result = new MainMenuViewModel();
            result.MainMenu = _mapper.Map<TreeNode<MenuItem>>(_menuService.GetMenu(MenuType.MainMenu, lang));
            return PartialView(result);
        }

        //[ChildActionOnly, Localization, OutputCache(Duration = 3600, VaryByParam = "lang")]
        [Localization]
        public virtual PartialViewResult Footer(string lang)
        {
            FooterViewModel result = new FooterViewModel();
            result.Hotline = _systemService.GetSystemParameter(SysParaType.Hotline);
            result.Phone = _systemService.GetSystemParameter(SysParaType.Phone);
            result.SiteTitle = _systemService.GetSystemParameter(SysParaType.OrgName);
            result.SiteName = _systemService.GetSystemParameter(SysParaType.SiteTitle);
            result.Address = _systemService.GetSystemParameter(SysParaType.Address);
            result.Fax = _systemService.GetSystemParameter(SysParaType.Fax);
            result.CustomFooterHtml = _systemService.GetSystemParameter(SysParaType.Footer);
            result.EmailContact = _systemService.GetSystemParameter(SysParaType.Email);
            result.Facebook = _systemService.GetSystemParameter(SysParaType.FacebookPage);
            result.GooglePlus = _systemService.GetSystemParameter(SysParaType.GooglePlus);
            result.Twitter = _systemService.GetSystemParameter(SysParaType.Twitter);
            result.Youtube = _systemService.GetSystemParameter(SysParaType.Youtube);
            result.Map = _systemService.GetSystemParameter(SysParaType.GoogleMap);
            result.Website = _systemService.GetSystemParameter(SysParaType.Website);
            result.FooterMenu = _menuService.GetMenu(MenuType.FooterMenu, lang);
            result.Copyright = _systemService.GetSystemParameter(SysParaType.CopyRight);
            result.SalesHotline = _systemService.GetForID(24).ParaValue.Trim();
            result.ShippingNote = _mapper.Map<SysParaViewModel>(_systemService.GetForID(26));
            result.TechHotline = _systemService.GetForID(25).ParaValue.Trim();
            result.ListAgency = _mapper.Map<List<AgencyViewModel>>(_agencyService.GetList("", null, null, null, _language));
            return PartialView(result);
        }

        // validate email
        [HttpPost]
        public virtual JsonResult DoesEmailAdsExist(string email)
        {
            return Json(!_emailAdsService.checkExistEmail(email));
        }

        [HttpPost]
        public virtual ActionResult SignupNewsletter(string email)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                if (!_emailAdsService.checkExistEmail(email))
                {
                    _emailAdsService.Insert(email);
                    message = Resource.ThanksForSigningUpToNewsletter;
                }
                else
                    message = Resource.EmailExist;
            }

            return Json(new { message = message });
        }


        [ChildActionOnly, OutputCache(Duration = 3600)]
        public virtual ActionResult GoogleAnalytics()
        {
            ViewBag.GoogleAnalytics = _systemService.GetSystemParameter(SysParaType.GoogleAnalyticsID);
            if (string.IsNullOrWhiteSpace(ViewBag.GoogleAnalytics))
            {
                return new EmptyResult();
            }
            return PartialView();
        }

        [ChildActionOnly]
        public virtual ActionResult HeaderScript()
        {
            ViewBag.HeaderScript = _systemService.GetSystemParameter(SysParaType.ScriptsHeader);
            if (string.IsNullOrWhiteSpace(ViewBag.HeaderScript))
            {
                return new EmptyResult();
            }
            return PartialView();
        }


        [ChildActionOnly]
        public virtual ActionResult BodyScript()
        {
            ViewBag.BodyScript = _systemService.GetSystemParameter(SysParaType.ScriptsFooter);
            if (string.IsNullOrWhiteSpace(ViewBag.BodyScript))
            {
                return new EmptyResult();
            }
            return PartialView();
        }

        #region Contact
        public virtual ActionResult PartialContact(string viewName = null)
        {
            LayoutContactViewModel model = new LayoutContactViewModel();

            //3 is current contact group layout id in database
            var groupContact = _groupService.GetListOrderedByGroupLayout((int)CurrentGroupLayout.LayoutContact, 1, _language).FirstOrDefault();
            if (groupContact != null)
            {
                model.Group = _mapper.Map<GroupViewModel>(groupContact);
            }
            else
            {
                model.Group = new GroupViewModel();
            }

            model.Title = _systemService.GetSystemParameter(SysParaType.SiteTitle);
            model.Map = _systemService.GetSystemParameter(SysParaType.GoogleMap);
            model.Address = _systemService.GetSystemParameter(SysParaType.Address);
            model.Phone = _systemService.GetSystemParameter(SysParaType.Phone);
            model.Hotline = _systemService.GetSystemParameter(SysParaType.Hotline);
            model.Fax = _systemService.GetSystemParameter(SysParaType.Fax);
            model.Email = _systemService.GetSystemParameter(SysParaType.Email);
            model.Facebook = _systemService.GetSystemParameter(SysParaType.FacebookPage);
            model.Twitter = _systemService.GetSystemParameter(SysParaType.Twitter);
            model.Youtube = _systemService.GetSystemParameter(SysParaType.Youtube);
            model.GPlus = _systemService.GetSystemParameter(SysParaType.GooglePlus);
            model.Skype = _systemService.GetSystemParameter(SysParaType.Skype);
            model.RSS = _systemService.GetSystemParameter(SysParaType.ScriptsHeader);

            //    model.SalesHotline = _systemService.GetSystemParameter(SysParaType.SalesHotline);
            //  model.ServicesHotline = _systemService.GetSystemParameter(SysParaType.ServicesHotline);
            //model.TableSchedule = _systemService.GetForID(29).Description;
            model.ListAddress = model.Address.Split(',');
            if (string.IsNullOrEmpty(viewName))
                return PartialView(model);
            else
                return PartialView(viewName, model);
        }

        public virtual ActionResult PartialContact_Footer()
        {
            LayoutContactViewModel model = new LayoutContactViewModel();

            //3 is current contact group layout id in database
            var groupContact = _groupService.GetListOrderedByGroupLayout((int)CurrentGroupLayout.LayoutContact, 1, _language).FirstOrDefault();
            if (groupContact != null)
            {
                model.Group = _mapper.Map<GroupViewModel>(groupContact);
            }
            else
            {
                model.Group = new GroupViewModel();
            }

            model.Title = _systemService.GetSystemParameter(SysParaType.SiteTitle);
            model.Map = _systemService.GetSystemParameter(SysParaType.GoogleMap);
            model.Address = _systemService.GetSystemParameter(SysParaType.Address);
            model.Phone = _systemService.GetSystemParameter(SysParaType.Phone);
            model.Hotline = _systemService.GetSystemParameter(SysParaType.Hotline);
            model.Fax = _systemService.GetSystemParameter(SysParaType.Fax);
            model.Email = _systemService.GetSystemParameter(SysParaType.Email);
            model.Facebook = _systemService.GetSystemParameter(SysParaType.FacebookPage);
            model.Twitter = _systemService.GetSystemParameter(SysParaType.Twitter);
            model.Youtube = _systemService.GetSystemParameter(SysParaType.Youtube);
            model.TableSchedule = _systemService.GetForID(29).Description;
            model.ListAddress = model.Address.Split(',');

            return PartialView(model);
        }
        #endregion

        #region Banner component
        /// <summary>
        /// Show banner from advertisement
        /// </summary>
        /// <param name="position">position when publish ad</param>
        /// <returns></returns>
        [ChildActionOnly]
        public virtual ActionResult Display(int position, string viewName = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "Display";
            }
            if (position != 0)
            {
                var result = _mapper.Map<IEnumerable<Ad>, IEnumerable<BannerViewModel>>(
                    _adService.GetAllDeployedAdForPosition(position, _language)).ToList();
                if (result == null || result.Count == 0)
                {
                    return new EmptyResult();
                }

                return PartialView(viewName, result);
            }
            else
            {
                return new EmptyResult();
            }
        }
        #endregion


        public virtual ActionResult GetPartnerLinks(string viewName, Component component)
        {
            if (component.TotalItems < 0)
            {
                component.TotalItems = 0;
            }
            var model = _mapper.Map<IEnumerable<PartnerLinkViewModel>>(_linkService.GetAll(_language).Take(component.TotalItems));
            if (model == null || model.Count() == 0)
            {
                return new EmptyResult();
            }
            return PartialView(viewName, model);
        }

        public virtual ActionResult GetContentForGroupID(string viewName, Component component, DistributionOrder order = DistributionOrder.Lastest, bool getTopGroupInfo = true, string defaultname = "", int? notInclude = 0)
        {
            if (component.TotalItems < 0)
            {
                component.TotalItems = 0;
            }
            HomePageGroupViewModel model = new HomePageGroupViewModel()
            {
                Component = component
            };

            var group = _groupService.GetGroupForID(component.EntityID.Value);


            if (group == null)
            {
                return new EmptyResult();
            }
            int GroupP = _groupService.GetLastGroupParentId(group.GroupID);
            model.Group = _mapper.Map<GroupViewModel>(group);
            model.Group.GroupParentName = _groupService.GetGroupForID(GroupP).Title;
            model.Group.ListGroup = _mapper.Map<List<GroupViewModel>>(
                    _groupService.GetGroupsForGroupParentID_OnlyOneLevel(component.EntityID.Value, _language, 1));

            if (component.ComponentType == (int)ComponentType.Content)
            {
                //Get content if layout is content
                model.ListItem = _mapper.Map<IEnumerable<HomeItemViewModel>>(_distributionService.GetOrderedForGroupID(component.EntityID.Value, _language, order).Take(component.TotalItems));

                foreach (var item in model.ListItem)
                {
                    //get contain group name
                    if (item.PublishedGroupId.HasValue)
                    {
                        var containGroup = _groupService.GetGroupForID(item.PublishedGroupId.Value);
                        if (group != null)
                        {
                            item.GroupName = containGroup.Title;
                        }
                    }

                    var author = _accountService.GetAccountProperty(item.Author, AccountPropertyString.Name);
                    item.AuthorName = author;
                    //get url
                    item.Url = _routingService.GetFriendlyUrl(RoutingType.Content, item.Id);
                }
            }
            else
            {
                //get child group if layout is group
                model.ListItem = _mapper.Map<IEnumerable<HomeItemViewModel>>(
                    _groupService.GetGroupsForGroupParentID_OnlyOneLevel(component.EntityID.Value, _language, 1)
                    .Take(component.TotalItems));
                foreach (var item in model.ListItem)
                {
                    item.Url = "/" + _routingService.GetFriendlyUrl(RoutingType.Group, item.Id);
                    item.PicAndVideo = GetAllImgandVideo(item.Body);
                    item.PicAndVideo += GetAllImgandVideo(item.Youtube);
                }
            }

            model.DefaultName = component.Description;

            if (defaultname != "")
            {
                model.DefaultName = defaultname;
            }

            if (notInclude != 0)
            {
                model.NotInclude = notInclude ?? default(int);

            }

            return PartialView(viewName, model);
        }

        public virtual ActionResult ListTagPartial(string viewName = null, int? groupId = null)
        {
            //if groupId is null -> get all tag
            IEnumerable<TagViewModel> model = groupId.HasValue ?
                _mapper.Map<IEnumerable<TagViewModel>>(_tagService.GetTagForGroup(groupId.Value)) :
                _mapper.Map<IEnumerable<TagViewModel>>(_tagService.GetList());

            if (model == null || model.Count() == 0)
            {
                return new EmptyResult();
            }

            return PartialView(viewName ?? "ListTagPartial", model);
        }

        public virtual ActionResult SocialContact(string viewName, int? ContentID)
        {
            SocialContactModel model = new SocialContactModel();

            model.Website = _systemService.GetSystemParameter(SysParaType.Website);
            model.Facebook = _systemService.GetSystemParameter(SysParaType.FacebookPage);
            model.GooglePlus = _systemService.GetSystemParameter(SysParaType.GooglePlus);
            model.Pinterest = _systemService.GetSystemParameter(SysParaType.HeaderAddress);
            int CT = 0;
            if (ContentID > 0)
            {
                CT = Convert.ToInt32(ContentID);
            }

            if (CT > 0)
            {
                model.Content = _mapper.Map<ContentBoxViewModel>(_contentService.GetContentForID(CT));
            }
            return PartialView(viewName, model);
        }


        [ChildActionOnly]
        public virtual PartialViewResult CopyRight()
        {

            ViewBag.SiteTitle = _systemService.GetSystemParameter(SysParaType.CopyRight);
            ViewBag.OnlineUserCounter = HttpContext.Application["OnlineUserCounter"];
            ViewBag.SiteVisitedCounter = HttpContext.Application["SiteVisitedCounter"].ToString().SplitToArrayDigit();
            return PartialView();
        }

        #region Group

        private GroupViewModel GetGroupArchitecture(GroupViewModel group, List<GroupViewModel> listGroups)
        {
            group.ListGroup = listGroups.Where(t => t.GroupParentID == group.GroupID).ToList();
            listGroups = listGroups.Where(t => !group.ListGroup.Select(o => o.GroupID).Contains(t.GroupID)).ToList();
            foreach (var item in group.ListGroup)
                GetGroupArchitecture(item, listGroups);

            return group;
        }


        public virtual ActionResult GetChildGroup(int id, string viewName = null, int? countItem = null, bool getAllChildGroup = false, bool getProductChildGroup = false)
        {
            if (getProductChildGroup == false)
            {
                if (id == 0)
                {
                    var model = new GroupViewModel();
                    if (getAllChildGroup)
                    {
                        GetGroupArchitecture(model, _mapper.Map<List<GroupViewModel>>(_groupService.GetGroupsForParentID(id, 1)));
                    }
                    else
                    {
                        model.ListGroup = _mapper.Map<List<GroupViewModel>>(_groupService.GetGroupsForGroupParentID_OnlyOneLevel(id, _language, 1));
                        foreach (var childgroup in model.ListGroup)
                        {
                            childgroup.ListGroup = _mapper.Map<List<GroupViewModel>>(_groupService.GetGroupsForGroupParentID_OnlyOneLevel(childgroup.GroupID, _language, 1));
                        }
                    }

                    if (countItem.HasValue)
                    {
                        model.ListGroup = model.ListGroup.Take(countItem.Value).ToList();
                    }

                    if (string.IsNullOrEmpty(viewName))
                    {
                        viewName = "GetChildGroup";
                    }
                    return PartialView(viewName, model);
                }
                else
                {
                    var model = _mapper.Map<GroupViewModel>(_groupService.GetGroupForID(id));
                    if (model == null)
                    {
                        return new EmptyResult();
                    }

                    if (getAllChildGroup)
                    {
                        GetGroupArchitecture(model, _mapper.Map<List<GroupViewModel>>(_groupService.GetGroupsForParentID(id, 1)));
                    }
                    else
                    {
                        model.ListGroup = _mapper.Map<List<GroupViewModel>>(_groupService.GetGroupsForGroupParentID_OnlyOneLevel(id, _language, 1));
                    }

                    if (countItem.HasValue)
                    {
                        model.ListGroup = model.ListGroup.Take(countItem.Value).ToList();
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
                var model = _mapper.Map<GroupViewModel>(_groupService.GetGroupForID(id));
                return PartialView(viewName, model);
            }
        }

        #endregion

        #region Brand
        #endregion

        #region Product

        public virtual ActionResult GetProductForProductGroupID(string viewName, Component component, ProductSortOrder order = ProductSortOrder.Newest, string properties = null)
        {
            if (component.TotalItems < 0)
            {
                component.TotalItems = 0;
            }
            var model = new ProductGroupViewModel();
            var searchResult = new SearchProductResult();
            var result = new List<ProductBoxViewModel>();


            model = _mapper.Map<ProductGroupViewModel>(_productGroupService.Get(component.EntityID.Value));
            if (properties != null)
            {
                searchResult = _productSearchService.SearchProduct("", _language, component.EntityID, null, null, null, properties, order);
                if(searchResult.ListProduct.Count() > 0)
                {
                    IEnumerable<ProductBoxViewModel> takenItems = _mapper.Map<List<ProductBoxViewModel>>(searchResult.ListProduct).Take(component.TotalItems);
                    result = takenItems.ToList();
                }
            } else {
                result = _mapper.Map<List<ProductBoxViewModel>>(_productService.GetByProductGroupId(component.EntityID.Value, order).Take(component.TotalItems));
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        //var list = _specificProductService.GetList(item.ProductID);
                        //foreach (var sp in list)
                        //{
                        //    sp.Specification = _specificationService.Get(sp.SpecificationId);
                        //}
                        //item.ListSpecificationes = _mapper.Map<IEnumerable<SpecificationViewModel>>(list);
                        //item.ProductGroup = _productGroupService.Get(item.ProductGroupID).Title;
                    }
                }
            }
            

            model.ListProduct = result;
            model.DefaultName = component.Description;
            if (string.IsNullOrEmpty(viewName))
                return PartialView(model);
            else
                return PartialView(viewName, model);
        }

        public virtual ActionResult GetAllProduct(string viewName, Component component, ProductSortOrder order = ProductSortOrder.HighestPriority)
        {
            var result = _mapper.Map<IEnumerable<ProductGroupViewModel>>(_productGroupService.GetList(_language));
            // var result = _mapper.Map<IEnumerable<ProductBoxViewModel>>(_productService.GetByProductGroupId(component.EntityID.Value, order).Take(component.TotalItems));

            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    var list = _mapper.Map<IEnumerable<ProductBoxViewModel>>(_productService.GetByProductGroupId(item.GroupID));
                    if (list != null && list.Any())
                    {
                        foreach (var child in list)
                        {
                            //var cm = _specificProductService.GetList(child.ProductID);
                            //foreach (var sp in cm)
                            //{
                            //    sp.Specification = _specificationService.Get(sp.SpecificationId);
                            //}
                            //child.ListSpecificationes = _mapper.Map<IEnumerable<SpecificationViewModel>>(cm);
                            //child.ProductGroup = _productGroupService.Get(child.ProductGroupID).Title;
                        }
                    }
                    item.ListProduct = _mapper.Map<List<ProductBoxViewModel>>(list);
                }
            }
            if (string.IsNullOrEmpty(viewName))
                return PartialView(result);
            else
                return PartialView(viewName, result);
        }

        #endregion

        [NonAction]
        public String GetAllImgandVideo(string Pictures)
        {
            string images = "";
            string pattern1 = @"<(img)\b[^>]*>";
            string pattern2 = @"<(iframe)\b[^>]*>";

            Regex rgx1 = new Regex(pattern1, RegexOptions.IgnoreCase);
            Regex rgx2 = new Regex(pattern2, RegexOptions.IgnoreCase);
            if (Pictures != null)
            {
                MatchCollection matches1 = rgx1.Matches(Pictures);
                MatchCollection matches2 = rgx2.Matches(Pictures);
                for (int i = 0, l = matches1.Count; i < l; i++)
                {
                    images += "<div class='item'>" + matches1[i].Value + "</div>";
                }
                for (int i = 0, l = matches2.Count; i < l; i++)
                {
                    images += "<div class=\"fixed-video-aspect\"><div class=\"item-video\">" + matches1[i].Value + "</div></div>";
                }
            }
            return images;
        }

        public virtual PartialViewResult ProductCategory(int productGroup)
        {
            var result = _productGroupProService.GetByGroupId(productGroup, true, true).Where(t => t.AllowFilter == true);
            var model = new ProductCategoryViewModel();

            // get product property
            foreach (var item in result)
            {
                var list = _productGroupProService.GetListProductProperties(item.GroupPropertyID);
                model.ProductAttributes.Add(item, list);
            }

            // get brand of product
            model.Brands = _mapper.Map<IEnumerable<BrandCategoryViewModel>>(_brandService.GetBrandsForProductGroup(productGroup));

            // get product in shopping cart
            var shoppingCart = _shoppingCartService.GetShoppingCart(ShoppingCartHelper.GetCartId()).OrderByDescending(t => t.Id).Take(2);
            foreach (var item in shoppingCart)
            {
                model.ShoppingCart.Add(_productService.Get(item.ProductID));
            }
            return PartialView(model);
        }
    }
}