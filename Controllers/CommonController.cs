using Store.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public partial class CommonController : BaseFrontendController
    {
        private readonly IGeneralService _generalService;
        public CommonController(IGeneralService generalService)
        {
            _generalService = generalService;
        }

        // GET: Common
        public virtual JsonResult GetCity()
        {
            var listCity = _generalService.GetAllCity().Where(t => t.Value1.Trim() == "1").Select(o => new { Value = o.Id, Text = o.Type + " " + o.Name });
            return Json(listCity, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetDistrict(int id = 0)
        {
            var listDistrict = _generalService.GetDistrictForCity(id).Select(o => new { Value = o.Id, Text = o.Type + " " + o.Name });
            return Json(listDistrict, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetWard(int id = 0)
        {
            var listWard = _generalService.GetWardForDistrict(id).Select(o => new { Value = o.Id, Text = o.Type + " " + o.Name });
            return Json(listWard, JsonRequestBehavior.AllowGet);
        }
    }
}