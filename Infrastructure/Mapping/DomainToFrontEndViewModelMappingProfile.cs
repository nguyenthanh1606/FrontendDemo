using AutoMapper;
using Store.Data;
using Store.Data.Models;
using Frontend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;

namespace Frontend.Infrastructure.Mapping
{
    /// <summary>
    /// Map from viewmodel to model
    /// </summary>
    public class DomainToFrontEndViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            CreateMap<Content, ContentViewModel>();
            CreateMap<Ad, BannerViewModel>();
            CreateMap<Ad, AdViewModel>();
            CreateMap<ProductAds, ProductGroupAdViewModel>();
            CreateMap<Distribution, ContentViewModel>();

            CreateMap<Group, GroupViewModel>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title.Trim()))
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Trim()));

            CreateMap<Product, ProductBoxViewModel>()
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.DefaultPrice));

            CreateMap<Content, ContentBoxViewModel>();
            CreateMap<Agency, AgencyViewModel>();
            CreateMap<ContentByLayout, ContentBoxViewModel>()
                .ForMember(dst => dst.PublishDate, opt => opt.MapFrom(src => src.CustomPublishDate ?? src.PublishDate)); ;

            CreateMap<Tuple<Distribution, Content>, ContentBoxViewModel>()
                .ForMember(dst => dst.Headline, opt => opt.MapFrom(src => src.Item2.Headline))
                .ForMember(dst => dst.PictureUrl, opt => opt.MapFrom(src => src.Item2.PictureUrl))
                .ForMember(dst => dst.IconUrl, opt => opt.MapFrom(src => src.Item2.IconUrl))
                .ForMember(dst => dst.Summary, opt => opt.MapFrom(src => src.Item2.Summary))
                .ForMember(dst => dst.Body, opt => opt.MapFrom(src => src.Item2.Body))
                .ForMember(dst => dst.ContentID, opt => opt.MapFrom(src => src.Item2.ContentID))
                .ForMember(dst => dst.Author, opt => opt.MapFrom(src => src.Item2.Author))
                 .ForMember(dst => dst.IntValue1, opt => opt.MapFrom(src => src.Item2.IntValue1))
                .ForMember(dst => dst.Value1, opt => opt.MapFrom(src => src.Item2.Value1))
                .ForMember(dst => dst.Value2, opt => opt.MapFrom(src => src.Item2.Value2))
                .ForMember(dst => dst.Value3, opt => opt.MapFrom(src => src.Item2.Value3))
                .ForMember(dst => dst.Value4, opt => opt.MapFrom(src => src.Item2.Value3))
                 .ForMember(dst => dst.AllowcommentFB, opt => opt.MapFrom(src => src.Item2.AllowcommentFB))
                .ForMember(dst => dst.Counter, opt => opt.MapFrom(src => src.Item2.Counter))
                .ForMember(dst => dst.Source, opt => opt.MapFrom(src => src.Item2.Source))
                .ForMember(dst => dst.Youtube, opt => opt.MapFrom(src => src.Item2.Youtube))
                .ForMember(dst => dst.PublishDate, opt => opt.MapFrom(src => src.Item1.CustomPublishDate ?? src.Item1.CreationDate))
                .ForMember(dst => dst.PublishedGroupId, opt => opt.MapFrom(src => src.Item1.GroupID));

            CreateMap<Tuple<Distribution, Content>, HomeItemViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Item2.ContentID))
                .ForMember(dst => dst.Headline, opt => opt.MapFrom(src => src.Item2.Headline))
                .ForMember(dst => dst.PictureUrl, opt => opt.MapFrom(src => src.Item2.PictureUrl))
                .ForMember(dst => dst.IconUrl, opt => opt.MapFrom(src => src.Item2.IconUrl))
                .ForMember(dst => dst.Summary, opt => opt.MapFrom(src => src.Item2.Summary))
                .ForMember(dst => dst.Body, opt => opt.MapFrom(src => src.Item2.Body))
                .ForMember(dst => dst.Banner, opt => opt.MapFrom(src => src.Item2.BannerImages))
                .ForMember(dst => dst.Source, opt => opt.MapFrom(src => src.Item2.Source))
                .ForMember(dst => dst.Youtube, opt => opt.MapFrom(src => src.Item2.Youtube))
                .ForMember(dst => dst.Value1, opt => opt.MapFrom(src => src.Item2.Value1))
                .ForMember(dst => dst.Value2, opt => opt.MapFrom(src => src.Item2.Value2))
                .ForMember(dst => dst.Value3, opt => opt.MapFrom(src => src.Item2.Value3))
                 .ForMember(dst => dst.Author, opt => opt.MapFrom(src => src.Item2.Author))
                .ForMember(dst => dst.PublishDate, opt => opt.MapFrom(src => src.Item1.CustomPublishDate ?? src.Item1.CreationDate))
                .ForMember(dst => dst.PublishedGroupId, opt => opt.MapFrom(src => src.Item1.GroupID));

            CreateMap<Group, HomeItemViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.GroupID))
                .ForMember(dst => dst.Headline, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.PictureUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dst => dst.Value1, opt => opt.MapFrom(src => src.Value1))
                .ForMember(dst => dst.Value2, opt => opt.MapFrom(src => src.Value2))
                .ForMember(dst => dst.Value3, opt => opt.MapFrom(src => src.Value3))
                .ForMember(dst => dst.Summary, opt => opt.MapFrom(src => src.Description));

            CreateMap<ShoppingCartItem, CartItemViewModel>()
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<ApplicationUser, RegisterViewModel>()
                .ForMember(dst => dst.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dst => dst.Birthday, opt => opt.MapFrom(src => src.Birthday.ToShortDateString()));
            CreateMap<ApplicationUser, AccountBasicInfo>()
                .ForMember(dst => dst.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dst => dst.Birthday, opt => opt.MapFrom(src => src.Birthday.ToShortDateString()));

            CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(dst => dst.ListImages, opt => opt.MapFrom(src => src.DefaultImageUrls != null ? src.DefaultImageUrls.Split('|').ToList() : new List<string>())); ;
            CreateMap<ProductGroupProperty, ProductGroupPropertyViewModel>();
            CreateMap<ProductProperty, ProductPropertyViewModel>()
                .ForMember(dst => dst.ProductPropertyID, opt => opt.MapFrom(src => src.ProductPropertyID));
            CreateMap<ProductGroupProperty, PropertyVersionViewModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.GroupPropertyID))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Title));
            CreateMap<ProductVersion, ProductVersionViewModel>()
                .ForMember(dst => dst.ListImages, opt => opt.MapFrom(src => src.ImageUrls != null ? src.ImageUrls.Split('|').ToList() : new List<string>()));

            CreateMap<ProductGroup, LayoutProductViewModel>();

            CreateMap<CustomerAddress, CustomerAddressViewModel>();
            CreateMap<CustomerAddressItem, CustomerAddressOverviewViewModel>()
                .ForMember(dst => dst.Address, opt => opt.MapFrom(src => string.Join(", ", src.Address, src.Ward, src.District, src.City)));

            CreateMap<Order, CustomerOrderViewModel>()
                .ForMember(dst => dst.CancelReason, opt => opt.MapFrom(src => src.ChangeStatusReason));
            CreateMap<OrderCartItem, OrderCartViewModel>();
            CreateMap<OrderCart, OrderCartViewModel>();

            CreateMap<ProductComment, ProductCommentViewModel>()
                .ForMember(dst => dst.IsAdding, opt => opt.MapFrom(src => false))
                .ForMember(dst => dst.CommentDate, opt => opt.MapFrom(src => src.CommentDate.ToShortDateString()))
                .ForMember(dst => dst.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToShortDateString() : null));

            CreateMap<ProductCommentSummary, ProductCommentSummaryViewModel>();

            CreateMap<FilterProductResult, SearchProductViewModel>()
                .ForMember(dst => dst.product, opt => opt.MapFrom(src => src.product))
                .ForMember(dst => dst.productVersions, opt => opt.MapFrom(src => src.productVersions));

            CreateMap<Brand, BrandCategoryViewModel>();
            CreateMap<ProductGroup, ProductGroupViewModel>();

            CreateMap<ProductGroupSearch, ProductGroupViewModel>();
            CreateMap<BrandSearch, BrandCategoryViewModel>();
            CreateMap<SearchProductResult, ProductCategoryViewModel>()
                .ForMember(dst => dst.Brands, opt => opt.MapFrom(src => src.ListBrand))
                .ForMember(dst => dst.ListProductGroup, opt => opt.MapFrom(src => src.ListProductGroup));
            CreateMap<SearchProductResult, SearchViewModel>()
                .ForMember(dst => dst.ListCategory, opt => opt.MapFrom(src => src))
                .ForMember(dst => dst.MaxPrice, opt => opt.MapFrom(src => src.ListProduct.Count() > 0 ? src.ListProduct.Max(o => o.DefaultPrice) : 0))
                .ForMember(dst => dst.MinPrice, opt => opt.MapFrom(src => src.ListProduct.Count() > 0 ? src.ListProduct.Min(o => o.DefaultPrice) : 0))
                .ForMember(dst => dst.TotalCount, opt => opt.MapFrom(src => src.ListProduct.Count()))
                .ForMember(dst => dst.ListProduct, opt => opt.Ignore());

            CreateMap<RegisterViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, RegisterViewModel>();
            CreateMap<CustomerAddressViewModel, CustomerAddress>();
            CreateMap<CustomerAddress, CustomerAddressViewModel>();
            CreateMap<OrderViewModel, Order>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<WishlistViewModel, Wishlist>();
            CreateMap<Wishlist, WishlistViewModel>();

            CreateMap<PartnerLink, PartnerLinkViewModel>();
            CreateMap<PartnerLinkViewModel, PartnerLink>();
            CreateMap<BankAccount, BankAccountItemViewModel>();
            CreateMap<SysPara, SysParaViewModel>();
        }
    }
}