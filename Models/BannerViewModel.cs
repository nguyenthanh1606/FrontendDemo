using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class BannerViewModel
    {
        public string AdName { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool OpenInNewTab { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
    }

    public class AdViewModel
    {
        public int AdID { get; set; }
        public string AdName { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Note { get; set; }
        public bool? OpenInNewTab { get; set; }
        public string Content { get; set; }
    }
}