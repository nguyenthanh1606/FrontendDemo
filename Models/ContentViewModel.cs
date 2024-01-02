using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Models
{
    public class ContentViewModel
    {
        public int ContentID { get; set; }
        public int Version { get; set; }
        public int Protected { get; set; }
        public string Language { get; set; }
        public string Headline { get; set; }
        public string Source { get; set; }
        public int Author { get; set; }
        public string IconUrl { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string TagLine { get; set; }
        public string PictureUrl { get; set; }
        public string PictureNote { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public int Status { get; set; }
        public int Editor { get; set; }
        public int Approver { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int Counter { get; set; }
        public int? IntValue1 { get; set; }
        public int? Allowcomment { get; set; }
        public int? AllowcommentFB { get; set; }
        public int? LayoutStyle { get; set; }
        public string BannerImages { get; set; }
        public string Youtube { get; set; }
        public string CustomizeTitle { get; set; }
        public string CustomizeDescription { get; set; }
        public DateTime PublishDate { get; set; }

        public int? PublishedGroupId { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<TagViewModel> ListTag { get; set; }
        public virtual List<DownloadViewModel> ListDownload { get; set; }
        public string PicAndVideo { get; set; }
        public string AuthorName { get; set; }
    }

    public class ContentBoxViewModel
    {
        public int ContentID { get; set; }
        public string Headline { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public int Author { get; set; }
        public string AuthorName { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string PictureUrl { get; set; }
        public string IconUrl { get; set; }
        public string BannerUrl { get; set; }
        public int Counter { get; set; }
        public string Source { get; set; }
        public DateTime PublishDate { get; set; }
        public string Youtube { get; set; }
        public int? PublishedGroupId { get; set; }
        public string GroupName { get; set; }
        public virtual List<DownloadViewModel> ListDownload { get; set; }
        public string PicAndVideo { get; set; }
        public int? IntValue1 { get; set; }
        public int? Allowcomment { get; set; }
        public int? AllowcommentFB { get; set; }
    }
}