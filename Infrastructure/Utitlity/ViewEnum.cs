using Resources;
using Store.Service.Helper;
using Store.Service.Helper.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Frontend.Infrastructure.ExtensionMethod;

namespace Frontend.Infrastructure.Utility
{
    public enum BreadcrumbType
    {
        Group = 0,
        ProductGroup = 1,
        Brand,
        Custom
    }


    public enum ProductCondition
    {
        [EmbededLocalizedDescription("OutOfStock", typeof(Resource))]
        OutOfStock = 0,

        [EmbededLocalizedDescription("InStock", typeof(Resource))]
        InStock = 1
    }
}