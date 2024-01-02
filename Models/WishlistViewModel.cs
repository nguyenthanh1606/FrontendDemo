using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class WishlistViewModel
    {
        public  DateTime CreationDate { get; set; }

        public  int Id { get; set; }
        public  int ProductId { get; set; }
        public  int Status { get; set; }
        public  string WishlistId { get; set; }

        public ProductBoxViewModel ProductDetails{get;set;}
    }
}