using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Service.Helper;
using Frontend.Models;
using System.ComponentModel.DataAnnotations;
using Resources;
using System.Web.Mvc;
using Store.Data.Models;
using PagedList;
using Store.Service.Models;
using Admin.Infrastructure.Helpers;

namespace Frontend.Models
{
    public class LayoutContactViewModel
    {
        public LayoutContactViewModel()
        {
            ListGroup = new List<GroupViewModel>();
            ListContent = new List<ContentBoxViewModel>();
        }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Tel { get; set; }
        public string Phone { get; set; }
        public string Hotline { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Map { get; set; }
        public string TableSchedule { get; set; }
        public string Title { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }
        public string[] ListAddress { get; set; }
        public string GPlus { get; set; }
        public string Skype { get; set; }
        public string RSS { get; set; }
        public string Address2 { get; set; }
        public string ServicesHotline { get; set; }
        public GroupViewModel Group { get; set; }
        public int LastGroupParentID { get; set; }
        public ContactViewModel CustomerContact { get; set; }
        public List<ContentBoxViewModel> ListContent { get; set; }
        public List<GroupViewModel> ListGroup { get; set; }
    }

    public class LayoutGroupViewModel
    {
        public LayoutGroupViewModel()
        {
            ListGroup = new List<GroupViewModel>();
            ListContent = new List<ContentBoxViewModel>();
        }

        public GroupViewModel Group { get; set; }
        public List<GroupViewModel> ListGroup { get; set; }
        public List<ContentBoxViewModel> ListContent { get; set; }
        public int LastGroupParentID { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public string domain { get; set; }
        public int tax { get; set; }
        public int roadfee { get; set; }
        public int bhtn { get; set; }
        public int numberReg { get; set; }
        public int checkfee { get; set; }
    }

    public class LayoutContentViewModel
    {
        public GroupViewModel Group { get; set; }
        public List<ContentBoxViewModel> ListContent { get; set; }
        public int LastGroupParentID { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<TagViewModel> ListTag { get; set; }
        public virtual List<DownloadViewModel> ListDownload { get; set; }
    }

    public class LayoutContentDetailViewModel
    {
        public GroupViewModel Group { get; set; }
        public ContentViewModel Content { get; set; }
        public int LastGroupParentID { get; set; }
        public List<ContentBoxViewModel> AnotherNews { get; set; }
        public ContentBoxViewModel ContentNext { get; set; }
        public ContentBoxViewModel ContentPrev { get; set; }
    }

    public class LayoutHomeViewModel
    {
        public List<ProductBoxViewModel> ListProduct { get; set; }
        public List<ProductBoxViewModel> ListHotProduct { get; set; }
        public GroupViewModel Introduction { get; set; }
        public List<ContentBoxViewModel> ListContent { get; set; }
        public List<AdViewModel> TopAds { get; set; }
        public Dictionary<ProductGroupViewModel, List<ProductGroupViewModel>> ListProductGroup { get; set; }

        public List<ListProductHotForCatalog> AllListProductHotForCatalog { get; set; }

        public LayoutHomeViewModel()
        {
            ListProduct = new List<ProductBoxViewModel>();
            ListHotProduct = new List<ProductBoxViewModel>();
            ListContent = new List<ContentBoxViewModel>();
            TopAds = new List<AdViewModel>();
            ListProductGroup = new Dictionary<ProductGroupViewModel, List<ProductGroupViewModel>>();
            AllListProductHotForCatalog = new List<ListProductHotForCatalog>();
            
        }
    }

    public class LayoutIntroductionViewModel
    {
        public LayoutIntroductionViewModel()
        {
            ListContent = new List<ContentViewModel>();
        }

        public GroupViewModel Group { get; set; }
        public List<ContentViewModel> ListContent { get; set; }
    }
  
    public class LayoutNewsViewModel
    {
        public GroupViewModel Group { get; set; }
        public List<ContentBoxViewModel> ListContentOfGroup { get; set; }

        public int page;
        public int totalItem;
        public int pageSize;
    }

    public class ListProductHotForCatalog
    {
        public int ProductGroup { get; set; }
        public string ProductGroupName { get; set; }

        public List<ProductBoxViewModel> ListProductHot { get; set; }

        public ListProductHotForCatalog()
        {
            ListProductHot = new List<ProductBoxViewModel>();
        }
    }
    public class LayoutNewsDetailViewModel
    {
        public GroupViewModel Group { get; set; }
        public ContentViewModel content { get; set; }
        public List<ContentBoxViewModel> anotherNews { get; set; }
    }

    public class TagPageViewModel
    {
        public TagViewModel Tag { get; set; }
        public IEnumerable<ContentBoxViewModel> ListContent { get; set; }

        public int Page;
        public int TotalItems;
        public int PageSize;
    }

    public class HeaderViewModel
    {
        public HeaderViewModel()
        {
            MainMenu = new TreeNode<MenuItem>();
            TopMenu = new TreeNode<MenuItem>();
            MetroMenu = new TreeNode<MenuItem>();
            MenuMobile = new TreeNode<MenuItem>();
        }
        public string Hotline { get; set; }
        public string SiteTitle { get; set; }
        public string HeaderTitle { get; set; }
        public TreeNode<MenuItem> MainMenu;
        public TreeNode<MenuItem> TopMenu;
        public TreeNode<MenuItem> MetroMenu;
        public TreeNode<MenuItem> MenuMobile;
        public int ShoppingCartCount { get; set; }
    }

    public class MainMenuViewModel
    {
        public MainMenuViewModel()
        {
            MainMenu = new TreeNode<MenuItem>();
        }

        public TreeNode<MenuItem> MainMenu;
    }

    public class FooterViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "EmailNotValid", ErrorMessageResourceType = typeof(Resource))]
        [Remote("DoesEmailAdsExist", "Layout", System.Web.Mvc.AreaReference.UseRoot, HttpMethod = "POST", ErrorMessageResourceName = "EmailExist", ErrorMessageResourceType = typeof(Resource))]
        public string email { get; set; }

        public FooterViewModel()
        {
            FooterMenu = new TreeNode<MenuItem>();
            ListAgency = new List<AgencyViewModel>();
        }

        public string Hotline { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string SiteTitle { get; set; }
        public string SiteName { get; set; }
        public string EmailContact { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string GooglePlus { get; set; }
        public string Youtube { get; set; }
        public string Map { get; set; }
        public string Website { get; set; }
        public string Copyright { get; set; }
        public string SalesHotline { get; set; }
        public string TechHotline { get; set; }
        public SysParaViewModel ShippingNote { get; set; }
        [AllowHtml]
        public string CustomFooterHtml { get; set; }
        public List<AgencyViewModel> ListAgency { get; set; }

        public TreeNode<MenuItem> FooterMenu;
    }

    public class AgencyViewModel
    {
        public int AgencyID { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Address { get; set; }
        public string ImagesUrl { get; set; }
        [AllowHtml]
        public string ImagesMap { get; set; }
        public int CityID { get; set; }
        [AllowHtml]
        public string Summary { get; set; }
        public int? Type { get; set; }
        public int Priority { get; set; }
        public string CityName { get; set; }
        public string TypeName { get; set; }
    }

    public class ProductCategoryViewModel
    {
        public ProductCategoryViewModel()
        {
            ProductAttributes = new Dictionary<ProductGroupProperty, IEnumerable<ProductProperty>>();
            ShoppingCart = new List<Product>();
            Brands = Enumerable.Empty<BrandCategoryViewModel>();
            ListProductGroup = Enumerable.Empty<ProductGroupViewModel>();
        }

        public Dictionary<ProductGroupProperty, IEnumerable<ProductProperty>> ProductAttributes { get; set; }
        public IEnumerable<BrandCategoryViewModel> Brands { get; set; }
        public IEnumerable<ProductGroupViewModel> ListProductGroup { get; set; }

        public List<Product> ShoppingCart { get; set; }
    }

    public class BrandCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string BannerUrl { get; set; }
        public string Description { get; set; }
        public string Slogan { get; set; }
        public int Priority { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public int? Value4 { get; set; }
        public int Count { get; set; }
    }


    public class LayoutProductViewModel
    {
        public int GroupID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int GroupParentID { get; set; }
        public string BannerUrl { get; set; }

        public IPagedList<SearchProductViewModel> listProduct { get; set; }

        public Dictionary<int, List<ProductProperty>> colors { get; set; }

        public LayoutProductViewModel()
        {
            colors = new Dictionary<int, List<ProductProperty>>();
        }
    }

    public class HomeItemViewModel
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Url { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }

        public string PictureUrl { get; set; }
        public string IconUrl { get; set; }
        public string Source { get; set; }
        public DateTime? PublishDate { get; set; }
        public int Author { get; set; }
        public string AuthorName { get; set; }
        public string Banner { get; set; }

        public int? PublishedGroupId { get; set; }
        public string GroupName { get; set; }

        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Youtube { get; set; }
        public string PicAndVideo { get; set; }
    }

    public class HomePageGroupViewModel
    {
        public HomePageGroupViewModel()
        {
            ResponsiveComlumn = new ResponsiveComlumn();
        }

        public GroupViewModel Group { get; set; }
        public IEnumerable<HomeItemViewModel> ListItem { get; set; }
        public Component Component { get; set; }
        public ResponsiveComlumn ResponsiveComlumn { get; set; }
        public string DefaultName { get; set; }
        public int NotInclude { get; set; }
    }

    public class ResponsiveComlumn
    {
        public int Desktop { get; set; }
        public int Tablet { get; set; }
        public int Mobile { get; set; }
    }

    public class SocialContactModel
    {
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string GooglePlus { get; set; }
        public string ZaloID { get; set; }
        public string Pinterest { get; set; }
        public string Instagram { get; set; }
        public GroupViewModel Group { get; set; }
        public ContentBoxViewModel Content { get; set; }
    }
}