using Resources;
using Store.Service.Helper.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class ProductCommentViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }

        [Range(1, 5, ErrorMessage = null, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "Rate", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int? Rating { get; set; }

        [DisplayResource(Name = "Title", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(64, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        [MinLength(5, ErrorMessage = null, ErrorMessageResourceName = "MinLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Title { get; set; }

        [DisplayResource(Name = "Comment", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(2024, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        [MinLength(5, ErrorMessage = null, ErrorMessageResourceName = "MinLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Body { get; set; }
        public string CommentDate { get; set; }
        public string UpdateDate { get; set; }
        public int? Upvote { get; set; }
        public int? Downvote { get; set; }
        public IEnumerable<ProductCommentViewModel> ChildComment { get; set; }
        public bool? IsAdding { get; set; }
        public int Rate { get; set; }
    }
}