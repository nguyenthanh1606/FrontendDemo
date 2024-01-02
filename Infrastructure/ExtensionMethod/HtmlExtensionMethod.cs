using Autofac;
using Autofac.Integration.Mvc;
using Frontend.Infrastructure.Utility;
using Resources;
using Store.Service.ProductServices;
using Store.Service.Service;
using Store.Service.SystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace Frontend.Infrastructure.ExtensionMethod
{
    public static class HtmlExtensionMethod
    {
        public static MvcHtmlString DisplayPlaceHolderFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var result = html.DisplayNameFor(expression).ToHtmlString();
            return new MvcHtmlString(System.Web.HttpUtility.HtmlDecode(result.ToString()));
        }

        public static IHtmlString AlertModal(this HtmlHelper helper, string initMessage)
        {
            var modal = new TagBuilder("div");
            modal.MergeAttribute("class", "modal fade");
            modal.MergeAttribute("id", "alertModal");
            modal.MergeAttribute("role", "dialog");

            var dialog = new TagBuilder("div");
            dialog.MergeAttribute("class", "modal-dialog modal-sm");

            var content = new TagBuilder("div");
            content.MergeAttribute("class", "modal-content");

            var header = new TagBuilder("div");
            header.MergeAttribute("class", "modal-header");
            header.InnerHtml += "<button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>";
            var title = new TagBuilder("h4");
            title.MergeAttribute("class", "modal-title");
            title.MergeAttribute("id", "alertMessage");

            header.InnerHtml += title.ToString(TagRenderMode.Normal);
            content.InnerHtml += header.ToString(TagRenderMode.Normal);
            dialog.InnerHtml += content.ToString(TagRenderMode.Normal);
            modal.InnerHtml += dialog.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(modal.ToString(TagRenderMode.Normal));

        }

        private static TagBuilder GenerateImg(string imageUrl, string altText, object htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder;
        }

        public static IHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName, object routeValues, AjaxOptions ajaxOptions, string aditionalText = "", object htmlAttributes = null)
        {
            var img = GenerateImg(imageUrl, altText, htmlAttributes);
            var link = helper.ActionLink("[replaceme]" + aditionalText, actionName, routeValues, ajaxOptions).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", img.ToString(TagRenderMode.SelfClosing)));
        }

        public static IHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, string aditionalText = "", object htmlAttributes = null)
        {
            var img = GenerateImg(imageUrl, altText, htmlAttributes);
            var link = helper.ActionLink("[replaceme] " + aditionalText, actionName, controllerName, routeValues, ajaxOptions).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", img.ToString(TagRenderMode.SelfClosing)));
        }

        public static IHtmlString FontawsomeActionLink(this AjaxHelper helper, string faClass, string text, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
        {
            var builder = new TagBuilder("i");
            builder.MergeAttribute("class", "fa " + faClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var link = helper.ActionLink("[replaceme] " + text, actionName, routeValues, ajaxOptions, htmlAttributes).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.Normal)));
        }

        public static IHtmlString FontawsomeActionLink(this AjaxHelper helper, string faClass, string text, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
        {
            var builder = new TagBuilder("i");
            builder.MergeAttribute("class", "fa " + faClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var link = helper.ActionLink("[replaceme] " + text, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.Normal)));
        }

        public static IHtmlString FilesManagerTextboxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var builder = new TagBuilder("div");
            builder.MergeAttribute("class", "input-group");

            var spanBtn = new TagBuilder("span");
            spanBtn.MergeAttribute("class", "input-group-btn");

            var btn = new TagBuilder("button");
            btn.MergeAttribute("class", "btn btn-default");
            btn.MergeAttribute("type", "button");
            btn.MergeAttribute("Title", Resource.OpenFilesManager);
            btn.MergeAttribute("onclick", string.Format("OpenElf(\"#{0}\")", helper.IdFor(expression)));
            btn.SetInnerText("...");

            spanBtn.InnerHtml = btn.ToString();
            builder.InnerHtml += helper.TextBoxFor(expression, htmlAttributes).ToHtmlString();
            builder.InnerHtml += spanBtn.ToString();

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString RenderDefaultThemeStyles(this HtmlHelper helper, params string[] additionalPaths)
        {
            var page = helper.ViewDataContainer as WebPageExecutingBase;
            var ThemeName = page.VirtualPath.Split('/')[2];
            var virtualPath = string.Format("~/Themes/{0}/css/{1}", ThemeName, ThemeName + "def");

            string cssStr = @"<link href=""{0}"" rel=""stylesheet""/>";
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                string result = string.Empty;
                foreach (string path in additionalPaths)
                {
                    result += string.Format(cssStr, VirtualPathUtility.ToAbsolute(path));
                }

                var defaultPath = String.Format("~/Themes/{0}/css/default.css", ThemeName);
                result += string.Format(cssStr, VirtualPathUtility.ToAbsolute(defaultPath));

                return MvcHtmlString.Create(result);
            }
            else
            {
                if (BundleTable.Bundles.GetBundleFor(virtualPath) == null)
                {
                    var defaultPath = String.Format("~/Themes/{0}/css/default.css", ThemeName);
                    BundleTable.Bundles.Add(new StyleBundle(virtualPath).Include(additionalPaths).Include(defaultPath));
                }
                return MvcHtmlString.Create(string.Format(cssStr, HttpUtility.HtmlAttributeEncode(BundleTable.Bundles.ResolveBundleUrl(virtualPath))));
            }
        }

        public static IHtmlString RenderStyles(this HtmlHelper helper, string bundleName, params string[] additionalPaths)
        {
            var page = helper.ViewDataContainer as WebPageExecutingBase;
            var ThemeName = page.VirtualPath.Split('/')[2];
            var virtualPath = string.Format("~/Themes/{0}/css/{1}", ThemeName, bundleName);

            string cssStr = @"<link href=""{0}"" rel=""stylesheet""/>";
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                string result = string.Empty;
                foreach (string path in additionalPaths)
                {
                    result += string.Format(cssStr, VirtualPathUtility.ToAbsolute(path));
                }
                return MvcHtmlString.Create(result);
            }
            else
            {
                if (BundleTable.Bundles.GetBundleFor(virtualPath) == null)
                {
                    BundleTable.Bundles.Add(new StyleBundle(virtualPath).Include(additionalPaths));
                }
                return MvcHtmlString.Create(string.Format(cssStr, HttpUtility.HtmlAttributeEncode(BundleTable.Bundles.ResolveBundleUrl(virtualPath))));
            }
        }

        public static IHtmlString RenderDefaultThemeScripts(this HtmlHelper helper, params string[] additionalPaths)
        {
            var page = helper.ViewDataContainer as WebPageExecutingBase;
            var ThemeName = page.VirtualPath.Split('/')[2];
            var virtualPath = string.Format("~/Themes/{0}/scripts/{1}", ThemeName, ThemeName + "def");
            string scriptStr = @"<script src=""{0}""></script>";
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                string result = string.Empty;
                foreach (string path in additionalPaths)
                {
                    result += string.Format(scriptStr, VirtualPathUtility.ToAbsolute(path));
                }

                var defaultPath = String.Format("~/Themes/{0}/scripts/default.js", ThemeName);
                result += string.Format(scriptStr, VirtualPathUtility.ToAbsolute(defaultPath));
                return MvcHtmlString.Create(result);
            }
            else
            {
                if (BundleTable.Bundles.GetBundleFor(virtualPath) == null)
                {
                    var defaultPath = String.Format("~/Themes/{0}/scripts/default.js", ThemeName);
                    BundleTable.Bundles.Add(new ScriptBundle(virtualPath).Include(additionalPaths).Include(defaultPath));
                }
                return MvcHtmlString.Create(string.Format(scriptStr, HttpUtility.HtmlAttributeEncode(BundleTable.Bundles.ResolveBundleUrl(virtualPath))));
            }
        }

        public static IHtmlString RenderScripts(this HtmlHelper helper, string bundleName, params string[] additionalPaths)
        {
            var page = helper.ViewDataContainer as WebPageExecutingBase;
            var ThemeName = page.VirtualPath.Split('/')[2];
            var virtualPath = string.Format("~/Themes/{0}/scripts/{1}", ThemeName, bundleName);
            string scriptStr = @"<script src=""{0}""></script>";
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                string result = string.Empty;
                foreach (string path in additionalPaths)
                {
                    result += string.Format(scriptStr, VirtualPathUtility.ToAbsolute(path));
                }
                return MvcHtmlString.Create(result);
            }
            else
            {
                if (BundleTable.Bundles.GetBundleFor(virtualPath) == null)
                {
                    BundleTable.Bundles.Add(new ScriptBundle(virtualPath).Include(additionalPaths));
                }
                return MvcHtmlString.Create(string.Format(scriptStr, HttpUtility.HtmlAttributeEncode(BundleTable.Bundles.ResolveBundleUrl(virtualPath))));
            }
        }

        public static IHtmlString CustomActionLink(this HtmlHelper helper, string linktext, RoutingType type, int objectId, object htmlAttributes = null)
        {
            //if a group, check if use external link before get friendly url
            if (type == RoutingType.Group)
            {
                IGroupService groupService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IGroupService>();
                var group = groupService.GetGroupForID(objectId);
                if (group == null)
                {
                    return MvcHtmlString.Create(string.Empty);
                }
                //if group has ExternalLink -> return link with href = ExternalLink
                if (!string.IsNullOrEmpty(group.ExternalLink))
                {
                    var elink = new TagBuilder("a");
                    elink.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                    elink.MergeAttribute("href", group.ExternalLink);
                    return MvcHtmlString.Create(elink.ToString(TagRenderMode.Normal));
                }
            }
            IRoutingService routingService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IRoutingService>();

            string url = "~/" + routingService.GetFriendlyUrl(type, objectId);

            var link = new TagBuilder("a");
            link.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            link.MergeAttribute("href", VirtualPathUtility.ToAbsolute(url));
            link.InnerHtml = linktext;
            return MvcHtmlString.Create(link.ToString(TagRenderMode.Normal));
        }

        public static string CustomAction(this UrlHelper helper, RoutingType type, int objectId)
        {
            IRoutingService routingService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IRoutingService>();
            //var culture = Thread.CurrentThread.CurrentUICulture.Name;
            //string url;
            //if (culture == "vi-VN")
            //{
            //    url = "~/" + routingService.GetFriendlyUrl(type, objectId);
            //}
            //else
            //{
            //    url = "~/" + culture + "/" + routingService.GetFriendlyUrl(type, objectId);
            //}

            //if a group, check if use external link before get friendly url
            if (type == RoutingType.Group)
            {
                IGroupService groupService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IGroupService>();
                var group = groupService.GetGroupForID(objectId);
                if (group == null)
                {
                    return string.Empty;
                }
                //if group has ExternalLink -> return link with href = ExternalLink
                if (!string.IsNullOrEmpty(group.ExternalLink))
                {
                    return group.ExternalLink;
                }
            }
            string url = "~/" + routingService.GetFriendlyUrl(type, objectId);

            return VirtualPathUtility.ToAbsolute(url);
        }

        /// <summary>
        /// return a ul with breadcrumb of given group
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString Breadcrumb(this HtmlHelper helper, BreadcrumbType type, int? id = null, object htmlAttributes = null, string customMenuName = null, string customUrl = null,bool isIncludeHomepageNode = true, bool isIncludeOwn = true)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var breadcrumb = new TagBuilder("ul");
            //fallback when not use htmlAttributes
            //if (htmlAttributes == null)
            //{
            //    breadcrumb.AddCssClass("breadcrumb");
            //}
            breadcrumb.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            breadcrumb.MergeAttribute("itemscope", "");
            breadcrumb.MergeAttribute("itemtype", "http://schema.org/BreadcrumbList");
            //add home
            if (isIncludeHomepageNode)
            {
                var home = new TagBuilder("li");
                home.InnerHtml += string.Format("<a href=\"/\" itemprop=\"item\"><span itemprop=\"name\">{0}</span></a></li>", Resource.HomePage);
                breadcrumb.InnerHtml += home;
            }

            //add all child
            List<TagBuilder> childLi = new List<TagBuilder>();
            switch (type)
            {
                case BreadcrumbType.Group:
                    if (id.HasValue)
                    {
                        //Add curent group first with no link
                        IGroupService groupService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IGroupService>();
                        var group = groupService.GetGroupForID(id.Value);

                        //if it has parrents, add all to list
                        do
                        {
                            TagBuilder li = new TagBuilder("li");
                            li.AddCssClass("urlgroup");

                            TagBuilder link = new TagBuilder("a");
                            link.MergeAttribute("itemprop", "item");
                            link.MergeAttribute("href", urlHelper.CustomAction(RoutingType.Group, group.GroupID));

                            TagBuilder nameSpan = new TagBuilder("span");
                            nameSpan.MergeAttribute("itemprop", "name");
                            nameSpan.SetInnerText(group.Title);

                            link.InnerHtml += nameSpan;
                            li.InnerHtml += link;
                            childLi.Add(li);
                            if (group.GroupParentID != 0)
                            {
                                group = groupService.GetGroupForID(group.GroupParentID);
                            }
                            else
                            {
                                group = null;
                            }
                        }
                        while (group != null);
                    }

                    break;
                case BreadcrumbType.ProductGroup:
                    if (id.HasValue)
                    {
                        //Add curent group first with no link
                        IProductGroupService groupService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IProductGroupService>();
                        var group = groupService.Get(id.Value);

                        //var own = new TagBuilder("li");
                        //own.InnerHtml += string.Format("<a href=\"{0}\" itemprop=\"item\"><span itemprop=\"name\">{1}</span></a>", urlHelper.CustomAction(RoutingType.ProductGroup, group.GroupID), group.Title);
                        //own.MergeAttribute("itemprop", "itemListElement");
                        //own.MergeAttribute("itemscope", "");
                        //own.MergeAttribute("itemtype", "http://schema.org/ListItem");

                        //if it has parrents, add all to list
                        do
                        {
                            TagBuilder li = new TagBuilder("li");
                            li.AddCssClass("urlgroup");
                            li.MergeAttribute("itemprop", "itemListElement");
                            li.MergeAttribute("itemscope", "");
                            li.MergeAttribute("itemtype", "http://schema.org/ListItem");

                            TagBuilder link = new TagBuilder("a");
                            link.MergeAttribute("itemprop", "item");
                            link.MergeAttribute("href", urlHelper.CustomAction(RoutingType.ProductGroup, group.GroupID));

                            TagBuilder nameSpan = new TagBuilder("span");
                            nameSpan.MergeAttribute("itemprop", "name");
                            nameSpan.SetInnerText(group.Title);

                            link.InnerHtml += nameSpan;
                            li.InnerHtml += link;
                            childLi.Add(li);
                            if (group.GroupParentID != 0)
                            {
                                group = groupService.Get(group.GroupParentID);
                            }
                            else
                            {
                                group = null;
                            }
                        }
                        while (group != null);
                        
                        //if (isIncludeOwn == true)
                        //{
                        //    breadcrumb.InnerHtml += own;
                        //}
                    }

                    

                    break;
                case BreadcrumbType.Brand:
                    if (id.HasValue)
                    {
                        IBrandService brandService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IBrandService>();
                        var brand = brandService.Get(id.Value);

                        TagBuilder li = new TagBuilder("li");
                        li.AddCssClass("urlgroup");
                        li.MergeAttribute("itemprop", "itemListElement");
                        li.MergeAttribute("itemscope", "");
                        li.MergeAttribute("itemtype", "http://schema.org/ListItem");

                        TagBuilder link = new TagBuilder("a");
                        link.MergeAttribute("itemprop", "item");
                        link.MergeAttribute("href", urlHelper.CustomAction(RoutingType.Brand, brand.Id));

                        TagBuilder nameSpan = new TagBuilder("span");
                        nameSpan.MergeAttribute("itemprop", "name");
                        nameSpan.SetInnerText(brand.Name);

                        link.InnerHtml += nameSpan;
                        li.InnerHtml += link;
                        childLi.Add(li);
                    }
                    break;
                case BreadcrumbType.Custom:
                default:
                    TagBuilder customLi = new TagBuilder("li");
                    customLi.AddCssClass("urlgroup");
                    customLi.MergeAttribute("itemprop", "itemListElement");
                    customLi.MergeAttribute("itemtype", "http://schema.org/ListItem");

                    if(string.IsNullOrEmpty(customUrl))
                    {
                        TagBuilder customnameSpan = new TagBuilder("span");
                        customnameSpan.MergeAttribute("itemprop", "name");
                        customnameSpan.SetInnerText(customMenuName);
                        customLi.InnerHtml += customnameSpan;
                    }
                    else
                    {
                        TagBuilder link = new TagBuilder("a");
                        link.MergeAttribute("itemprop", "item");
                        link.MergeAttribute("href", customUrl);

                        TagBuilder nameSpan = new TagBuilder("span");
                        nameSpan.MergeAttribute("itemprop", "name");
                        nameSpan.SetInnerText(customMenuName);
                        link.InnerHtml += nameSpan;
                        customLi.InnerHtml += link;
                    }
                    childLi.Add(customLi);
                    break;
            }

            //now add all li to breadcrumb from bottom to top
            for (int i = childLi.Count - 1; i >= 0; i--)
            {
                breadcrumb.InnerHtml += childLi[i];
            }

            return MvcHtmlString.Create(breadcrumb.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalItemCount">total item</param>
        /// <param name="pageNumber">current page</param>
        /// <param name="pageSize">item in page</param>
        /// <returns></returns>
        public static IHtmlString GetPageList(this HtmlHelper helper, int pageNumber, int pageSize, int totalItemCount,
            Dictionary<string, string> parameters = null, object htmlAttributes = null)
        {
            string url = helper.ViewContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Path);
            string param = null;
            if (parameters != null && parameters.Count != 0)
                param = "&" + string.Join("&", parameters.Select(t => string.Format("{0}={1}", t.Key, t.Value)));


            int PageListNo = 10;
            int StartPage;
            if ((pageNumber % PageListNo) == 0)
                StartPage = (pageNumber / PageListNo - 1) * PageListNo + 1;
            else
                StartPage = (pageNumber / PageListNo) * PageListNo + 1;

            int PageCount;
            if ((totalItemCount % pageSize) == 0)
                PageCount = totalItemCount / pageSize;
            else
                PageCount = totalItemCount / pageSize + 1;

            int EndPage = StartPage + PageListNo - 1;

            if (EndPage > PageCount)
                EndPage = PageCount;

            if ((EndPage == pageNumber) && (EndPage < PageCount))
            {
                StartPage++;
                EndPage++;
            }
            else
            {
                if ((StartPage == pageNumber) && (StartPage > 1))
                {
                    StartPage--;
                    EndPage--;
                }
            }

            if (EndPage - StartPage < (PageListNo - 1))
            {
                StartPage = EndPage - (PageListNo - 1);
                if (StartPage < 1)
                    StartPage = 1;
            }
            var pagination = new TagBuilder("ul");
            //add default because there are themes not use htmlAttributes
            if (htmlAttributes == null)
            {
                pagination.AddCssClass("pagination");
            }
            pagination.MergeAttributes(new RouteValueDictionary(htmlAttributes));


            if (EndPage > StartPage)
            {
                if (pageNumber > 1)
                {
                    var prevLi = new TagBuilder("li");
                    prevLi.AddCssClass("PagedList-skipToPrevious");
                    var prevLiAnchor = new TagBuilder("a");
                    prevLiAnchor.InnerHtml += "<i class=\"fa fa-caret-left\"></i>";
                    prevLiAnchor.MergeAttribute("href", url + "?page=" + Convert.ToString(pageNumber - 1) + param);
                    prevLi.InnerHtml += prevLiAnchor;
                    pagination.InnerHtml += prevLi;
                }
                for (int i = StartPage; (i <= EndPage); i++)
                {
                    var li = new TagBuilder("li");
                    var anchor = new TagBuilder("a");
                    if (i == pageNumber)
                    {
                        li.AddCssClass("active");
                    }
                    else
                    {
                        if (i == 1)
                        {
                            anchor.MergeAttribute("href", url + "?" + param);
                        }
                        else
                        {
                            anchor.MergeAttribute("href", url + "?page=" + i.ToString() + param);
                        }
                    }

                    anchor.SetInnerText(i.ToString());
                    li.InnerHtml += anchor;
                    pagination.InnerHtml += li;
                }
                if (pageNumber < PageCount)
                {
                    var nextLi = new TagBuilder("li");
                    nextLi.AddCssClass("PagedList-skipToNext");
                    var prevLiAnchor = new TagBuilder("a");
                    prevLiAnchor.InnerHtml += "<i class=\"fa fa-caret-right\"></i>";
                    prevLiAnchor.MergeAttribute("href", url + "?page=" + Convert.ToString(pageNumber + 1) + param);
                    nextLi.InnerHtml += prevLiAnchor;
                    pagination.InnerHtml += nextLi;
                }
            }
            return MvcHtmlString.Create(pagination.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString GetPageListCustom(this HtmlHelper helper, int pageNumber, int pageSize, int totalItemCount,
         Dictionary<string, string> parameters = null, object htmlAttributes = null)
        {
            string url = helper.ViewContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Path);
            string param = null;
            if (parameters != null && parameters.Count != 0)
                param = "&" + string.Join("&", parameters.Select(t => string.Format("{0}={1}", t.Key, t.Value)));


            int PageListNo = 10;
            int StartPage;
            if ((pageNumber % PageListNo) == 0)
                StartPage = (pageNumber / PageListNo - 1) * PageListNo + 1;
            else
                StartPage = (pageNumber / PageListNo) * PageListNo + 1;

            int PageCount;
            if ((totalItemCount % pageSize) == 0)
                PageCount = totalItemCount / pageSize;
            else
                PageCount = totalItemCount / pageSize + 1;

            int EndPage = StartPage + PageListNo - 1;

            if (EndPage > PageCount)
                EndPage = PageCount;

            if ((EndPage == pageNumber) && (EndPage < PageCount))
            {
                StartPage++;
                EndPage++;
            }
            else
            {
                if ((StartPage == pageNumber) && (StartPage > 1))
                {
                    StartPage--;
                    EndPage--;
                }
            }

            if (EndPage - StartPage < (PageListNo - 1))
            {
                StartPage = EndPage - (PageListNo - 1);
                if (StartPage < 1)
                    StartPage = 1;
            }

            
            var pagination = new TagBuilder("ul");
            //add default because there are themes not use htmlAttributes
            if (htmlAttributes == null)
            {
                pagination.AddCssClass("pagination justify-content-end");
            }
            pagination.MergeAttributes(new RouteValueDictionary(htmlAttributes));


            if (EndPage > StartPage)
            {
                var firstLi = new TagBuilder("li");
                firstLi.AddCssClass("page-item icc");
                var firstLiAnchor = new TagBuilder("a");
                firstLiAnchor.AddCssClass("page-link");
                firstLiAnchor.InnerHtml += "&laquo;";
                firstLiAnchor.MergeAttribute("href", url);
                firstLi.InnerHtml += firstLiAnchor;
                pagination.InnerHtml += firstLi;

                if (pageNumber > 1)
                {
                    var prevLi = new TagBuilder("li");
                    prevLi.AddCssClass("page-item icc");
                    var prevLiAnchor = new TagBuilder("a");
                    prevLiAnchor.AddCssClass("page-link");
                    prevLiAnchor.InnerHtml += "&rsaquo;";
                    prevLiAnchor.MergeAttribute("href", url + "?page=" + Convert.ToString(pageNumber - 1) + param);
                    prevLi.InnerHtml += prevLiAnchor;
                    pagination.InnerHtml += prevLi;
                }
                for (int i = StartPage; (i <= EndPage); i++)
                {
                    var li = new TagBuilder("li");
                    var anchor = new TagBuilder("a");
                    anchor.AddCssClass("page-link");
                    if (i == pageNumber)
                    {
                        li.AddCssClass("page-item --active");
                    }
                    else
                    {
                        li.AddCssClass("page-item");

                        if (i == 1)
                        {
                            anchor.MergeAttribute("href", url + "?" + param);
                        }
                        else
                        {
                            anchor.MergeAttribute("href", url + "?page=" + i.ToString() + param);
                        }
                    }

                    anchor.SetInnerText(i.ToString());
                    li.InnerHtml += anchor;
                    pagination.InnerHtml += li;
                }
                if (pageNumber < PageCount)
                {
                    var nextLi = new TagBuilder("li");
                    nextLi.AddCssClass("page-item icc");
                    var prevLiAnchor = new TagBuilder("a");
                    prevLiAnchor.AddCssClass("page-link");
                    prevLiAnchor.InnerHtml += "&rsaquo;";
                    prevLiAnchor.MergeAttribute("href", url + "?page=" + Convert.ToString(pageNumber + 1) + param);
                    nextLi.InnerHtml += prevLiAnchor;
                    pagination.InnerHtml += nextLi;
                }
                var lastLi = new TagBuilder("li");
                lastLi.AddCssClass("page-item icc");
                var lastLiAnchor = new TagBuilder("a");
                lastLiAnchor.AddCssClass("page-link");
                lastLiAnchor.InnerHtml += "&raquo;";
                lastLiAnchor.MergeAttribute("href", url + "?page=" + Convert.ToString(PageCount) + param);
                lastLi.InnerHtml += lastLiAnchor;
                pagination.InnerHtml += lastLi;
            }
            return MvcHtmlString.Create(pagination.ToString(TagRenderMode.Normal));
        }

        public static string Content(this UrlHelper urlHelper, string contentPath, bool toAbsolute = false)
        {
            var path = urlHelper.Content(contentPath);
            var url = new Uri(HttpContext.Current.Request.Url, path);

            return toAbsolute ? url.AbsoluteUri : path;
        }

        public static string SystemParameter(this HtmlHelper htmlHelper, SysParaType type)
        {
            ISystemService sysService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<ISystemService>();
            return sysService.GetSystemParameter(type);
        }

        #region Banner
        /// <summary>
        /// Generate responsive image with position in advertisement 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="position"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString BannerImage(this HtmlHelper helper, int position, string language, object htmlAttributes = null)
        {
            IAdService adService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IAdService>();
            var ad = adService.GetAllDeployedAdForPosition(position, language).FirstOrDefault();
            if (ad != null)
            {
                var img = GenerateImg(VirtualPathUtility.ToAbsolute(ad.ImageUrl), ad.AdName.Trim(), htmlAttributes);
                return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
            }
            return MvcHtmlString.Empty;
        }

        public static string BannerImageLinkOnly(this HtmlHelper htmlHelper, int position, string language)
        {
            IAdService adService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IAdService>();
            var ad = adService.GetAllDeployedAdForPosition(position, language).FirstOrDefault();
            if (ad != null)
            {
                return ad.ImageUrl;
            }
            return string.Empty;
        }

        public static string BannerUrlOnly(this HtmlHelper htmlHelper, int position, string language)
        {
            IAdService adService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IAdService>();
            var ad = adService.GetAllDeployedAdForPosition(position, language).FirstOrDefault();
            if (ad != null)
            {
                return ad.Url;
            }
            return string.Empty;
        }
        #endregion
    }
}