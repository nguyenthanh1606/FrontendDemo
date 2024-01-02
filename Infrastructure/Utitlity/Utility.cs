using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Frontend.Models;
using Resources;
using Store.Service.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Infrastructure.Helpers
{
    public static class Utility
    {
        public static string ContructUrl(string baseUrl, int? brand, string price, string[] properties)
        {
            List<string> query = new List<string>();
            if (brand.HasValue)
            {
                query.Add("brand=" + brand.Value);
            }
            if (!string.IsNullOrEmpty(price))
            {
                query.Add("price=" + price);
            }
            if (properties != null && properties.Count() > 0)
            {
                query.Add("attr=" + string.Join(".", properties));
            }

            if(baseUrl.Contains("?"))
            {
                return baseUrl + "&" + string.Join("&", query);
            }
            else
            {
                return baseUrl + "?" + string.Join("&", query);
            }
        }

        //Get product addition attribute as available color, rating, promo status
        public static void MapProductAditionalAttribute(ProductBoxViewModel product)
        {
            IMapper mapper = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IMapper>();
            IProductGroupPropertiesService productGroupPropertiesService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IProductGroupPropertiesService>();
            IProductCommentService productCommentService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IProductCommentService>();
            
            //map list available color
            product.ListColorProperties = mapper.Map<IEnumerable<ProductPropertyViewModel>>(
                productGroupPropertiesService.GetListPropertiesByType(product.Properties, ProductGroupPropertiesType.Color));

            //map ratting
            var rating = productCommentService.GetSummaryForProduct(product.ProductID);
            product.RatingCount = rating.TotalRating;
            product.AverateRating = rating.AverageRating;

            //map promo status:
            //if have original price -> on sale
            if (product.OriginalPrice.HasValue)
            {
                product.PromoStatus = Resource.SalePromoSrt;
            }
            else
            {
                // create in 7(?) day -> new
                if (product.CreationDate.HasValue && (DateTime.Now - product.CreationDate.Value).TotalDays < 7)
                {
                    product.PromoStatus = Resource.New;
                }
            }
        }
    }
}