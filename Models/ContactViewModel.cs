using Resources;
using Store.Service.Helper.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class ContactViewModel
    {
        [DisplayResource(Name = "Title", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Title { get; set; }

        [DisplayResource(Name = "Content", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Body { get; set; }

        [DisplayResource(Name = "Name", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string CustomerName { get; set; }

        [DisplayResource(Name = "Email", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? Age { get; set; }
        public int? Gender { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string Address { get; set; }
        public DateTime? DateSchedule { get; set; }
        public int? EntityId { get; set; }
        public int? EntityType { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public int? IntValue1 { get; set; }
        public int? IntValue2 { get; set; }
        public int? IntValue3 { get; set; }
    }
}