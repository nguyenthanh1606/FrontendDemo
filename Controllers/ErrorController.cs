using Frontend.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public partial class ErrorController : BaseFrontendController
    {
        // GET: Error
        [Route("AccessDenied")]
        public virtual ActionResult AccessDenied()
        {
            return PartialView("CustomError", new CustomErrorInfo { ErrorMessage = Resource.AccessDeniedError, Title = "AccessDenied" });
        }

        public virtual ActionResult NotFound()
        {
            return HttpNotFound();
        }
    }
}