using Resources;
using Store.Service.Helper.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class DownloadViewModel
    {
        public int DownloadID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int ContentID { get; set; }
        public int Counter { get; set; }
        public int Author { get; set; }
        public int ModifiedUser { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }   
}