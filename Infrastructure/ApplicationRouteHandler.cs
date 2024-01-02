using Autofac;
using Autofac.Integration.Mvc;
using Store.Data;
using Store.Data.Models;
using Store.Service.SystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Frontend.Infrastructure
{
    public class ApplicationRouteHandler : IRouteHandler
    {
        public IRoutingService RoutingService { get; set; }

        public ApplicationRouteHandler()
        {
            RoutingService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IRoutingService>();
        }

        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns>
        /// An object that processes the request.
        /// </returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // attempt to retrieve controller and action for current path and remove '/' character at begin
            var url = requestContext.HttpContext.Request.FilePath.ToString().Split('/').Last();
            Routing routing = RoutingService.GetByUrl(url);

            if (routing != null)
            {
                // Assign route values to current requestContext
                requestContext.RouteData.Values["controller"] = routing.Controller;
                requestContext.RouteData.Values["action"] = routing.Action;
                requestContext.RouteData.Values["id"] = routing.EntityId;
            }
            // Method that returns a 404 error
            else
            {
                requestContext.RouteData.Values["controller"] = "Error";
                requestContext.RouteData.Values["action"] = "NotFound";
            }
            return new MvcHandler(requestContext);
        }
    }
}