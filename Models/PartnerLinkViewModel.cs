using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class PartnerLinkViewModel
    {
        public int LinkID { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string LinkHref { get; set; }
        public string ImageUrl { get; set; }
        public string Note { get; set; }
    }
}