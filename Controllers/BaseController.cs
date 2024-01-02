using Store.Service.Service;
using Frontend.Infrastructure.Attributes;
using Frontend.Infrastructure.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using Admin.Infrastructure.Attributes;

namespace Frontend.Controllers
{
    public partial class BaseController : Controller
    {
        protected string _language;
        private string[] _supportedLanguage = { "vi-VN", "en-US" };
        private string _defaultLanguage;

        public BaseController()
        {
            var globalizationSection =
            WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
            _defaultLanguage = globalizationSection.UICulture;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {

            base.Initialize(requestContext);
            var langCookie = requestContext.HttpContext.Request.Cookies["lang"];
            if (langCookie != null)
            {
                _language = langCookie.Value;
                if (string.IsNullOrEmpty(_language) || !_supportedLanguage.Contains(_language))
                {
                    //Sets default culture
                    _language = _defaultLanguage;
                }
            }
            else
            {
                _language = _defaultLanguage;
                langCookie = new HttpCookie("lang", _defaultLanguage)
                {
                    HttpOnly = true
                };

                requestContext.HttpContext.Response.AppendCookie(langCookie);
            }

            var ci = new CultureInfo(_language);
            //Finally setting culture for each request
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = ci;
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }

    [GetGlobalMeta]
    public partial class BaseFrontendController : BaseController
    {

    }
}