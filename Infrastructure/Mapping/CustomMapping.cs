using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using PagedList;
using Store.Data.Models;
using Store.Service.SystemService;
using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Service.ProductServices;

namespace Frontend.Infrastructure.Mapping
{
    public static class CustomMapping
    {
        public static IPagedList<ProductBoxViewModel> ListProductToPagedListProductBox(IEnumerable<Product> source, int page, 
            int? itemPerPage = null)
        {
            var systemService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<ISystemService>();
            var productGroupService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IProductGroupService>();
            var productService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IProductService>();
            var mapper = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IMapper>();

            if (!itemPerPage.HasValue)
            {
                int productsPerPage;
                if (!int.TryParse(systemService.GetAppPropertyValue(AppPropertyString.MaxProductPerPage), out productsPerPage))
                {
                    productsPerPage = 10;
                }

                itemPerPage = productsPerPage;
            }

            
            IEnumerable <ProductBoxViewModel> listProductBox = mapper.Map<IEnumerable<ProductBoxViewModel>>(source);
            var result = listProductBox.ToPagedList(page, itemPerPage.Value);

            foreach(var product in result)
            {
                Helpers.Utility.MapProductAditionalAttribute(product);
                product.ProductGroup = mapper.Map<ProductGroupViewModel>(productGroupService.Get(product.ProductGroupID));
                var selectedProperties = productService.GetAllPropertiesForProduct(product.ProductID);

                foreach (ProductGroupProperty item in selectedProperties)
                {
                    //get display name and type for group property
                    PropertyVersionViewModel propertyVersion = new PropertyVersionViewModel
                    {
                        Id = item.GroupPropertyID,
                        Name = item.Title,
                        Type = item.Type
                    };
                    //add list of selection to each group
                    foreach (ProductProperty property in item.ListProductProperty)
                    {
                        propertyVersion.ListProperty.Add(mapper.Map<ProductPropertyViewModel>(property));
                    }
                    //save the info about fist version

                    product.ListVersionProperty.Add(propertyVersion);
                }
            }

            return result;
        }
    }
}