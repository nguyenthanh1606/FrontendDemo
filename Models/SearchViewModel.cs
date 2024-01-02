using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            ListCategory = new ProductCategoryViewModel();
            ProductGroup = new ProductGroupViewModel();
            ListFilter = new List<Tuple<string, string>>();
        }

        public int TotalCount { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public IPagedList<ProductBoxViewModel> ListProduct { get; set; }
        public ProductGroupViewModel ProductGroup { get; set; }
        public BrandCategoryViewModel Brand { get; set; }
        public ProductCategoryViewModel ListCategory { get; set; }
        public List<Tuple<string, string>> ListFilter { get; set; }
        public string properties { get; set; }
    }
}