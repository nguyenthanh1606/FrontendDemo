using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class ProductAdsViewModel
    {
        public virtual string AdKey { get; set; }
        public virtual string Content { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Note { get; set; }
        public virtual bool? OpenInNewTab { get; set; }
        public virtual int ProductGroupID { get; set; }
        public virtual string ThumImage { get; set; }
        public virtual string ThumTitle { get; set; }
        public virtual string Url { get; set; }
    }
}