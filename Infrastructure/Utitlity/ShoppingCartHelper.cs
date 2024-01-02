using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Store.Service.Service;
using Store.Service.SystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Infrastructure.Helpers
{
    public static class ShoppingCartHelper
    {
        public static string CartKey { get
            {
                return "CartId";
            }
        }

        public static string GetCartId()
        {
            var context = HttpContext.Current;
            var systemService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<ISystemService>();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return context.User.Identity.GetUserId();
            }
            else
            {
                var cartCookie = context.Request.Cookies[CartKey];
                //get expire day from app config, if cannot, default is 7 day
                int expireDays;
                if (!int.TryParse(systemService.GetAppPropertyValue(AppPropertyString.ShoppingCartExpireDays), out expireDays))
                {
                    expireDays = 7;
                }

                if (cartCookie == null)
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie, expire in 7 days
                    cartCookie = new HttpCookie(CartKey, tempCartId.ToString())
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(expireDays)
                    };
                    context.Response.AppendCookie(cartCookie);
                }
                return cartCookie.Value;
            }
        }
    }
}