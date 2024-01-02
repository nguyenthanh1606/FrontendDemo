using Store.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class ProductGroupViewModel
    {
        public ProductGroupViewModel()
        {
            ProductGroupProperties = new List<ProductGroupPropertyViewModel>();
        }
        public int GroupID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int GroupParentID { get; set; }
        public string GroupParentName { get; set; }
        public string BannerUrl { get; set; }
        public int Count { get; set; }
        public int LastGroupParentID { get; set; }
        public List<ProductBoxViewModel> ListProduct { get; set; }
        public List<ProductBoxViewModel> ListProductHot { get; set; }
        public List<ProductBoxViewModel> ListProductBestSeller { get; set; }
        public List<ProductBoxViewModel> ListSaleOff{ get; set; }
        public List<ProductGroupViewModel> ChildGroup { get; set; }
        public IEnumerable<ProductGroupAdViewModel> ListProductAds { get; set; }
        public List<ProductGroupPropertyViewModel> ProductGroupProperties { get; set; }
        public string DefaultName { get; set; }
    }

    public class ProductGroupAdViewModel
    {
        public int AdID { get; set; }
        public string AdName { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Note { get; set; }
        public bool? OpenInNewTab { get; set; }
        public string Content { get; set; }
    }

    public class ProductGroupPropertyViewModel
    {
        public ProductGroupPropertyViewModel()
        {
            ListProductProperty = new List<ProductPropertyViewModel>();
        }

        public int GroupPropertyID { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public string Note { get; set; }
        public bool AllowFilter { get; set; }
        public List<ProductPropertyViewModel> ListProductProperty { get; set; }
    }

    public class ProductPropertyViewModel
    {
        public int ProductPropertyID { get; set; }
        public int GroupPropertyID { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public int Priority { get; set; }
        public string Note { get; set; }
    }
}