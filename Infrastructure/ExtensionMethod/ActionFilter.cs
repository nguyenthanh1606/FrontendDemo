using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Frontend.Infrastructure.ExtensionMethod
{
    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
    }

    public class ExportModelStateToTempData : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Only export when ModelState is not valid
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                //Export if we are redirecting
                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                {
                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class ImportModelStateFromTempData : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

            if (modelState != null)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    //Otherwise remove it.
                    filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        { 
            //dont check if AllowAnonymous 
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }

            var context = filterContext.HttpContext;

            if (context.Session["UserName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped) }));
            }
            else
            {
                if (!string.IsNullOrEmpty(Roles))
                {
                    string[] allowRoles = Roles.Split(',');
                    string userRoles = context.Session["Roles"].ToString();
                    var userIsInRole = allowRoles.Any(role => userRoles.Contains(role));
                    if (!userIsInRole)
                    {
                        filterContext.Result = new RedirectResult("~/AccessDenied");
                    }
                }
            }
            
        }
    }

    //public class LocalizationAttribute : ActionFilterAttribute
    //{
    //    private string _defaultLanguage;
    //    private string[] _supportedLanguage = { "vi-VN", "en-US" };

    //    public LocalizationAttribute()
    //    {
    //        var globalizationSection =
    //        WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
    //        _defaultLanguage = globalizationSection.UICulture;
    //    }

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        string lang = (string)filterContext.RouteData.Values["lang"];
    //        //return to default lang if not supported
    //        if(string.IsNullOrEmpty(lang) || !_supportedLanguage.Contains(lang))
    //        {
    //            lang = _defaultLanguage;
    //            filterContext.ActionParameters["lang"] = lang;
    //            filterContext.RouteData.Values.Remove("lang");
    //            filterContext.RouteData.Values.Add("lang", lang);
    //        }
    //        //set localization
    //        if (lang != _defaultLanguage)
    //        {
    //            try
    //            {
    //                Thread.CurrentThread.CurrentCulture =
    //                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
    //            }
    //            catch (Exception e)
    //            {
    //                throw new NotSupportedException(String.Format("ERROR: Invalid language code '{0}'.", lang));
    //            }
    //        }
    //        base.OnActionExecuting(filterContext);
    //    }
    //}

    public class LocalizationAttribute : ActionFilterAttribute
    {
        private string _defaultLanguage;
        private string[] _supportedLanguage = { "vi-VN", "en-US" };

        public LocalizationAttribute()
        {
            var globalizationSection =
            WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
            _defaultLanguage = globalizationSection.UICulture;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies["lang"];
            if(cookie != null)
            {
                filterContext.ActionParameters["lang"] = cookie.Value;
            }
            else
            {
                filterContext.ActionParameters["lang"] = _defaultLanguage;
            }
            base.OnActionExecuting(filterContext);
        }


    }

    public class EmbededLocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;
        public EmbededLocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager("Resources.Resource", Assembly.Load("App_GlobalResources"));
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string displayName = _resource.GetString(_resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }
    }

    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.HttpContext.Response.Redirect("/");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}