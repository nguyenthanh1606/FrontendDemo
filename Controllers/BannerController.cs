using AutoMapper;
using Store.Data;
using Store.Data.Models;
using Store.Service.Service;
using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public partial class BannerController : BaseFrontendController
    {
        #region field and constructor
        private readonly IAdService _adService;
        private readonly IMapper _mapper;

        public BannerController(IAdService adService, IMapper mapper)
        {
            _adService = adService;
            _mapper = mapper;
        }
        #endregion

        /// <summary>
        /// Show banner from advertisement
        /// </summary>
        /// <param name="position">position when publish ad</param>
        /// <returns></returns>
        [ChildActionOnly]
        public virtual ActionResult Display(int position, int? countItem = null, string viewName = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "Display";
            }
            var listAdsDeployed = _adService.GetAllDeployedAdForPosition(position, _language);

            if (listAdsDeployed == null || listAdsDeployed.Count() == 0)
            {
                return new EmptyResult();
            }
            if (countItem.HasValue && countItem.Value >= 0)
            {
                listAdsDeployed = listAdsDeployed.Take(countItem.Value);
            }

            var result = _mapper.Map<IEnumerable<Ad>, IEnumerable<BannerViewModel>>(listAdsDeployed).ToList();
            return PartialView(viewName, result);
        }
    }
}