// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Frontend.Controllers
{
    public partial class ProductController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ProductController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult VersionDetails()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.VersionDetails);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult RatingSummary()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RatingSummary);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.PartialViewResult ListComment()
        {
            return new T4MVC_System_Web_Mvc_PartialViewResult(Area, Name, ActionNames.ListComment);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult CommentProduct()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.CommentProduct);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetListComment()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetListComment);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetUserComment()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetUserComment);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult RateComment()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RateComment);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ListProductSameBrand()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListProductSameBrand);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ListProductSameGroup()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListProductSameGroup);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProductController Actions { get { return MVC.Product; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Product";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Product";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Details = "Details";
            public readonly string VersionDetails = "VersionDetails";
            public readonly string RatingSummary = "RatingSummary";
            public readonly string ListComment = "ListComment";
            public readonly string CommentProduct = "CommentProduct";
            public readonly string GetListComment = "GetListComment";
            public readonly string GetUserComment = "GetUserComment";
            public readonly string RateComment = "RateComment";
            public readonly string ListProductSameBrand = "ListProductSameBrand";
            public readonly string ListProductSameGroup = "ListProductSameGroup";
            public readonly string ListProduct = "ListProduct";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Details = "Details";
            public const string VersionDetails = "VersionDetails";
            public const string RatingSummary = "RatingSummary";
            public const string ListComment = "ListComment";
            public const string CommentProduct = "CommentProduct";
            public const string GetListComment = "GetListComment";
            public const string GetUserComment = "GetUserComment";
            public const string RateComment = "RateComment";
            public const string ListProductSameBrand = "ListProductSameBrand";
            public const string ListProductSameGroup = "ListProductSameGroup";
            public const string ListProduct = "ListProduct";
        }


        static readonly ActionParamsClass_Details s_params_Details = new ActionParamsClass_Details();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Details DetailsParams { get { return s_params_Details; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Details
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_VersionDetails s_params_VersionDetails = new ActionParamsClass_VersionDetails();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_VersionDetails VersionDetailsParams { get { return s_params_VersionDetails; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_VersionDetails
        {
            public readonly string productId = "productId";
            public readonly string properties = "properties";
        }
        static readonly ActionParamsClass_RatingSummary s_params_RatingSummary = new ActionParamsClass_RatingSummary();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RatingSummary RatingSummaryParams { get { return s_params_RatingSummary; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RatingSummary
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_ListComment s_params_ListComment = new ActionParamsClass_ListComment();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListComment ListCommentParams { get { return s_params_ListComment; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListComment
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_CommentProduct s_params_CommentProduct = new ActionParamsClass_CommentProduct();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CommentProduct CommentProductParams { get { return s_params_CommentProduct; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CommentProduct
        {
            public readonly string comment = "comment";
            public readonly string productId = "productId";
            public readonly string parentComment = "parentComment";
        }
        static readonly ActionParamsClass_GetListComment s_params_GetListComment = new ActionParamsClass_GetListComment();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetListComment GetListCommentParams { get { return s_params_GetListComment; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetListComment
        {
            public readonly string id = "id";
            public readonly string page = "page";
            public readonly string sortOrder = "sortOrder";
        }
        static readonly ActionParamsClass_GetUserComment s_params_GetUserComment = new ActionParamsClass_GetUserComment();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetUserComment GetUserCommentParams { get { return s_params_GetUserComment; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetUserComment
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_RateComment s_params_RateComment = new ActionParamsClass_RateComment();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RateComment RateCommentParams { get { return s_params_RateComment; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RateComment
        {
            public readonly string id = "id";
            public readonly string rateUp = "rateUp";
        }
        static readonly ActionParamsClass_ListProductSameBrand s_params_ListProductSameBrand = new ActionParamsClass_ListProductSameBrand();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListProductSameBrand ListProductSameBrandParams { get { return s_params_ListProductSameBrand; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListProductSameBrand
        {
            public readonly string id = "id";
            public readonly string viewName = "viewName";
            public readonly string countItem = "countItem";
        }
        static readonly ActionParamsClass_ListProductSameGroup s_params_ListProductSameGroup = new ActionParamsClass_ListProductSameGroup();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListProductSameGroup ListProductSameGroupParams { get { return s_params_ListProductSameGroup; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListProductSameGroup
        {
            public readonly string id = "id";
            public readonly string viewName = "viewName";
            public readonly string countItem = "countItem";
        }
        static readonly ActionParamsClass_ListProduct s_params_ListProduct = new ActionParamsClass_ListProduct();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListProduct ListProductParams { get { return s_params_ListProduct; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListProduct
        {
            public readonly string groupId = "groupId";
            public readonly string order = "order";
            public readonly string viewName = "viewName";
            public readonly string countItem = "countItem";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Details = "Details";
                public readonly string ListProduct = "ListProduct";
                public readonly string ListProductSameBrand = "ListProductSameBrand";
            }
            public readonly string Details = "~/Views/Product/Details.cshtml";
            public readonly string ListProduct = "~/Views/Product/ListProduct.cshtml";
            public readonly string ListProductSameBrand = "~/Views/Product/ListProductSameBrand.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ProductController : Frontend.Controllers.ProductController
    {
        public T4MVC_ProductController() : base(Dummy.Instance) { }

        [NonAction]
        partial void DetailsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Details(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Details);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DetailsOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void VersionDetailsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int productId, string[] properties);

        [NonAction]
        public override System.Web.Mvc.ActionResult VersionDetails(int productId, string[] properties)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.VersionDetails);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "productId", productId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "properties", properties);
            VersionDetailsOverride(callInfo, productId, properties);
            return callInfo;
        }

        [NonAction]
        partial void RatingSummaryOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.JsonResult RatingSummary(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RatingSummary);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            RatingSummaryOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void ListCommentOverride(T4MVC_System_Web_Mvc_PartialViewResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.PartialViewResult ListComment(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_PartialViewResult(Area, Name, ActionNames.ListComment);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ListCommentOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void CommentProductOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, Frontend.Models.ProductCommentViewModel comment, int productId, int? parentComment);

        [NonAction]
        public override System.Web.Mvc.JsonResult CommentProduct(Frontend.Models.ProductCommentViewModel comment, int productId, int? parentComment)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.CommentProduct);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "comment", comment);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "productId", productId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "parentComment", parentComment);
            CommentProductOverride(callInfo, comment, productId, parentComment);
            return callInfo;
        }

        [NonAction]
        partial void GetListCommentOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int id, int page, Store.Service.ProductServices.CommentSortOrder sortOrder);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetListComment(int id, int page, Store.Service.ProductServices.CommentSortOrder sortOrder)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetListComment);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "page", page);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "sortOrder", sortOrder);
            GetListCommentOverride(callInfo, id, page, sortOrder);
            return callInfo;
        }

        [NonAction]
        partial void GetUserCommentOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetUserComment(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetUserComment);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            GetUserCommentOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void RateCommentOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int id, bool rateUp);

        [NonAction]
        public override System.Web.Mvc.JsonResult RateComment(int id, bool rateUp)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RateComment);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "rateUp", rateUp);
            RateCommentOverride(callInfo, id, rateUp);
            return callInfo;
        }

        [NonAction]
        partial void ListProductSameBrandOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id, string viewName, int? countItem);

        [NonAction]
        public override System.Web.Mvc.ActionResult ListProductSameBrand(int id, string viewName, int? countItem)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListProductSameBrand);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "viewName", viewName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "countItem", countItem);
            ListProductSameBrandOverride(callInfo, id, viewName, countItem);
            return callInfo;
        }

        [NonAction]
        partial void ListProductSameGroupOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id, string viewName, int? countItem);

        [NonAction]
        public override System.Web.Mvc.ActionResult ListProductSameGroup(int id, string viewName, int? countItem)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListProductSameGroup);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "viewName", viewName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "countItem", countItem);
            ListProductSameGroupOverride(callInfo, id, viewName, countItem);
            return callInfo;
        }

        [NonAction]
        partial void ListProductOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int? groupId, Store.Service.ProductServices.ProductSortOrder order, string viewName, int? countItem);

        [NonAction]
        public override System.Web.Mvc.ActionResult ListProduct(int? groupId, Store.Service.ProductServices.ProductSortOrder order, string viewName, int? countItem)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListProduct);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "groupId", groupId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "order", order);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "viewName", viewName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "countItem", countItem);
            ListProductOverride(callInfo, groupId, order, viewName, countItem);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
