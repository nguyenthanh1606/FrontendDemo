using Frontend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Frontend
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
              name: "Login",
              url: "Login",
              defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
              namespaces: new[] { "Frontend.Controllers" }
          );

            // contact
            routes.MapRoute(
              name: "ContactSuccess",
              url: "ContactSuccess",
              defaults: new { controller = "Home", action = "ContactSuccess", id = UrlParameter.Optional },
              namespaces: new[] { "Frontend.Controllers" }
          );

            routes.MapRoute(
               name: "Search",
               url: "Search/kw",
               defaults: new { controller = "Catalog", action = "ProductGroup" },
               namespaces: new[] { "Frontend.Controllers" }
           );


            //Custom routing
            routes.MapRoute(
                name: "Custom",
                url: "{slug}",
                defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Frontend.Controllers" }
            ).RouteHandler = new ApplicationRouteHandler();

           
           


            //default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Frontend.Controllers" }
            );



        }
    }
}
