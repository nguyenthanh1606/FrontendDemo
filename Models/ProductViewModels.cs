using Store.Data.Models;
using Frontend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Frontend.Infrastructure.Utility;
using Store.Service.ProductServices;

namespace Frontend.Models
{
    public class ProductBoxViewModel
    {
        public ProductBoxViewModel()
        {
            ListVersionProperty = new List<PropertyVersionViewModel>();
            ProductGroup = new ProductGroupViewModel();
        }
        public int ProductID { get; set; }
        public int ProductGroupID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Priority { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string IconUrl { get; set; }
        public double? OriginalPrice { get; set; }
        
        public string CouponCode { get; set; }
        public string DiscountCodeValue { get; set; }

        public string Properties { get; set; }
        public string PromoStatus { get; set; }

        public double AverateRating { get; set; }
        public int RatingCount { get; set; }

        public DateTime? CreationDate { get; set; }

        public IEnumerable<ProductPropertyViewModel> ListColorProperties { get; set; }

        public ProductStatus Status { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public ProductGroupViewModel ProductGroup { get; set; }
        public List<PropertyVersionViewModel> ListVersionProperty { get; set; }
    }

    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {
            ListVersionProperty = new List<PropertyVersionViewModel>();
            ListImages = new List<string>();
            Comment = new ProductCommentViewModel();
            ProductGroup = new ProductGroupViewModel();
        }
        public int ProductID { get; set; }
        public int ProductGroupID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public int Priority { get; set; }
        public string Language { get; set; }
        public string Note { get; set; }
        public int? BrandId { get; set; }
        public double DefaultPrice { get; set; }
        public double? OriginalPrice { get; set; }

        public int? Quantity { get; set; }

        public int? InventoryNumber { get; set; }
        public ProductCondition Condition { get; set; }
        
        public Tuple<string,string> Brand { get; set; }
        public ProductGroupViewModel ProductGroup { get; set; }
        public List<PropertyVersionViewModel> ListVersionProperty { get; set; }

        public string DefaultImageUrls { get; set; }
        public List<string> ListImages { get; set; }
        public ProductCommentSummaryViewModel CommentSummary { get; set; }
        public ProductCommentViewModel Comment { get; set; }

        public string Specification {get;set;}

        public bool ExistInWishList { get; set; }

        public ProductStatus Status { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

        public SysParaViewModel PaymentMethod { get; set; }
    }

    public class PropertyVersionViewModel
    {
        public PropertyVersionViewModel()
        {
            ListProperty = new List<ProductPropertyViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int SelectedProperty { get; set; }
        public List<ProductPropertyViewModel> ListProperty { get; set; }
    }

    public class ProductVersionViewModel
    {
        public ProductVersionViewModel()
        {
            ListImages = new List<string>();
        }
        public int? InventoryNumber { get; set; }
        public string Condition { get; set; }
        public List<string> ListImages { get; set; }

        public string Properties { get; set; }

        public double? DefaultPrice { get; set; }
        public double? OriginalPrice { get; set; }
    }

    //public class ProductPropertyViewModel
    //{
    //    public int Id { get; set; }
    //    public string Title { get; set; }
    //    public string Note { get; set; }
    //}

    public class ProductCommentSummaryViewModel
    {
        public double AverageRating { get; set; }
        public int TotalRating { get; set; }
        public int Rate1 { get; set; }
        public int Rate2 { get; set; }
        public int Rate3 { get; set; }
        public int Rate4 { get; set; }
        public int Rate5 { get; set; }
    }

    public class SearchProductViewModel
    {
        public ProductBoxViewModel product { get; set; }
        public List<ProductVersionViewModel> productVersions { get; set; }
    }
}