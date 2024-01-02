using AutoMapper;
using Store.Data;
using Store.Data.Models;
using Frontend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Infrastructure.Mapping
{
    /// <summary>
    /// Map from frontend viewmodel to model
    /// </summary>
    public class FrontEndViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "FrontEndViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            CreateMap<ContentViewModel, Content>();

            CreateMap<GroupViewModel, Group>();
            CreateMap<ContactViewModel, FeedBack>();

            CreateMap<CartItemViewModel, ShoppingCart>();

            CreateMap<OrderViewModel, Order>()
                .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.CustomerAddress != null? src.CustomerAddress.CustomerName : ""));

            CreateMap<ProductCommentViewModel, ProductComment>();
        }
    }
}