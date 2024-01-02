using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class GroupViewModel
    {
        public GroupViewModel()
        {
            ListGroup = new List<GroupViewModel>();
            ListContent = new List<ContentBoxViewModel>();
        }
        public int GroupID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int Status { get; set; }
        public int GroupType { get; set; }
        public Nullable<int> GroupParentID { get; set; }
        public string GroupParentName { get; set; }
        public Nullable<int> Layout { get; set; }
        public int Level { get; set; }
        public int Priority { get; set; }
        public string FriendUrl { get; set; }
        public int FriendUrlId { get; set; }
        public string BannerUrl { get; set; }
        public string IconUrl { get; set; }
        public int Protected { get; set; }
        public List<GroupViewModel> ListGroup { get; set; }
        public List<ContentBoxViewModel> ListContent { get; set; }
        public int LastGroupParentID { get; set; }
    }
}