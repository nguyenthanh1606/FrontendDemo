using Store.Service.Service;
using Store.Service.SystemService;
using Store.Service.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Frontend.Models;
using Store.Service.Helper;
using Frontend.Infrastructure.ExtensionMethod;
using Frontend.Infrastructure.Attributes;
using Store.Data.Models;
using Frontend.Infrastructure.Helpers;
using AutoMapper;
using Resources;
using Store.Service.Models;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Frontend.Controllers
{
    public partial class LayoutController : BaseFrontendController
    {
        private readonly IAccountService _accountService;
        private readonly ISystemService _systemService;
        private readonly IEmailAdsService _emailAdsService;
        private readonly IAdService _adService;
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        private readonly IContentService _contentService;
        private readonly IDistributionService _distributionService;
        private readonly ITagService _tagService;
        private readonly ILinkService _linkService;
        private readonly IRoutingService _routingService;

        public LayoutController(IAccountService accountService, ISystemService systemService, IEmailAdsService emailAdsService, IContentService contentService,
            IGroupService groupService, IAdService adService, IMapper mapper, IDistributionService distributionService,
            ITagService tagService, ILinkService linkService, IRoutingService routingService)
        {
            _accountService = accountService;
            _systemService = systemService;
            _emailAdsService = emailAdsService;
            _adService = adService;
            _groupService = groupService;
            _mapper = mapper;
            _distributionService = distributionService;
            _contentService = contentService;
            _tagService = tagService;
            _linkService = linkService;
            _routingService = routingService;
        }

        public virtual ActionResult Group(int id)
        {
            GroupViewModel group = _mapper.Map<GroupViewModel>(_groupService.GetGroupForID(id));
            if (group == null)
            {
                return HttpNotFound();
            }

            if (!group.Layout.HasValue || group.Layout.Value == 0)
            {
                group.Layout = _groupService.GetClosestGroupParrentLayout(group.GroupParentID.Value);
            }

            switch (group.Layout)
            {
                case 1:
                    return LayoutContentList(group);
                case 2:
                    return LayoutContentTiles(group);
                case 3:
                    return LayoutGroupTiles(group);
                case 4:
                    return LayoutGroupList(group);
                case 5:
                    return LayoutContact(group);
                case 6:
                    return LayoutIntroduction(group);
                case 7:
                    return LayoutOrder(group);
                case 8:
                    return LayoutAboutUs(group);
                case 15:
                    return LayoutCustom1(group);
                default:
                    return LayoutContentList(group);
            }
        }

        #region Layout Content
        private LayoutContentViewModel GetLayoutContentData(GroupViewModel group, int page)
        {
            //number of content per page
            int contentsPerPage;
            if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.ContentPerPage), out contentsPerPage))
            {
                contentsPerPage = 10;
            }

            LayoutContentViewModel result = new LayoutContentViewModel();
            result.Group = group;
            result.LastGroupParentID = _groupService.GetLastGroupParentId(group.GroupID);
            result.Group.GroupParentName = _groupService.GetGroupForID(result.LastGroupParentID).Title;
            var listContent = _distributionService.GetOrderedForGroupID(group.GroupID, _language, DistributionOrder.Lastest);

            result.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent
                .Skip((page - 1) * contentsPerPage).Take(contentsPerPage)).ToList();
            //Get group of content info
            foreach (var content in result.ListContent)
            {
                if (content.PublishedGroupId.HasValue)
                {
                    var publishGroup = _groupService.GetGroupForID(content.PublishedGroupId.Value);
                    if (publishGroup != null)
                    {
                        content.GroupName = publishGroup.Title;
                        content.ListDownload = _mapper.Map<List<DownloadViewModel>>(_contentService.GetListDownloadForContentId(content.ContentID));
                    }
                    var author = _accountService.GetAccountProperty(content.Author, AccountPropertyString.Name);
                    content.AuthorName = author;
                }

            }

            result.Page = page;
            result.TotalItems = listContent.Count();
            result.PageSize = contentsPerPage;

            return result;
        }

        public virtual ActionResult LayoutContentTiles(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutContentTiles", model);
        }

        public virtual ActionResult LayoutAboutUs(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutAboutUs", model);
        }

        public virtual ActionResult LayoutCustom1(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutCustom1", model);
        }

        public virtual ActionResult LayoutContentList(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);
            return View("LayoutContentList", model);
        }

        public virtual ActionResult LayoutContentBlock(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);
            for (int i = 0; i < model.ListContent.Count(); i++)
                model.ListContent[i].Value5 = _routingService.GetFriendlyUrl(RoutingType.Content, model.ListContent[i].ContentID);
            return View("LayoutContentBlock", model);
        }

        public virtual ActionResult LayoutContentTimeline(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutContentTimeline", model);
        }

        public virtual ActionResult LayoutContentFocus(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutContentFocus", model);
        }

        public virtual ActionResult LayoutIntroduction(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutIntroduction", model);
        }

        public virtual ActionResult LayoutNews(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutNews", model);
        }

        public virtual ActionResult LayoutCertificate(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutCertificate", model);
        }

        public virtual ActionResult LayoutProduct(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutProduct", model);
        }
        #endregion

        #region Layout Group
        private LayoutGroupViewModel GetLayoutGroupData(GroupViewModel group, int page)
        {
            //number of content per page
            int contentsPerPage;
            if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.ContentPerPage), out contentsPerPage))
            {
                contentsPerPage = 10;
            }

            LayoutGroupViewModel result = new LayoutGroupViewModel();
            result.Group = group;
            result.LastGroupParentID = _groupService.GetLastGroupParentId(group.GroupID);
            var listGroup = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(group.GroupID, _language, 1);
            if (listGroup != null)
            {
                result.ListGroup = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup.Skip((page - 1) * contentsPerPage).Take(contentsPerPage)).ToList();
            }

            var listContent = _distributionService.GetOrderedForGroupID(group.GroupID, _language, DistributionOrder.Lastest);
            result.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent.Skip((page - 1) * contentsPerPage).Take(contentsPerPage)).ToList();


            result.Page = page;
            result.TotalItems = listGroup.Count();
            result.PageSize = contentsPerPage;

            return result;
        }

        public virtual ActionResult LayoutGroupTiles(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutGroupData(group, page);

            return View("LayoutGroupTiles", model);
        }

        public virtual ActionResult LayoutGroupList(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutGroupData(group, page);
            return View("LayoutGroupList", model);
        }

        public virtual ActionResult LayoutGroupBlock(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutGroupAndContentData(group, page);

            return View("LayoutGroupBlock", model);
        }

        #endregion

        #region Layout Group & Content

        private LayoutGroupViewModel GetLayoutGroupAndContentData(GroupViewModel group, int page)
        {
            //number of content per page
            int contentsPerPage;
            if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.ContentPerPage), out contentsPerPage))
            {
                contentsPerPage = 10;
            }

            LayoutGroupViewModel result = new LayoutGroupViewModel();
            result.Group = group;
            result.LastGroupParentID = _groupService.GetLastGroupParentId(group.GroupID);
            var listGroup = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(group.GroupID, _language, 1);

            result.ListGroup = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup.Skip((page - 1) * contentsPerPage).Take(contentsPerPage)).ToList();

            if (listGroup != null)
            {
                foreach (var g in result.ListGroup)
                {
                    var listGroup1 = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(g.GroupID, _language, 1);
                    var listContent = _distributionService.GetOrderedForGroupIDOnlyOneLevel(g.GroupID, _language, DistributionOrder.Lastest);
                    if ((listGroup1 != null) && (listGroup1.Count() > 0))
                    {
                        g.ListGroup = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup1).ToList();
                    }

                    if ((listContent != null))
                    {
                        g.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent).ToList();

                    }
                }
            }

            var listRContent = _distributionService.GetOrderedForGroupIDOnlyOneLevel(group.GroupID, _language, DistributionOrder.Lastest);

            result.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listRContent).ToList();
            foreach (var content in result.ListContent)
            {
                content.ListDownload = _mapper.Map<List<DownloadViewModel>>(_contentService.GetListDownloadForContentId(content.ContentID));
                var author = _accountService.GetAccountProperty(content.Author, AccountPropertyString.Name);
                content.AuthorName = author;
            }
            result.Page = page;
            result.TotalItems = listGroup.Count();
            result.PageSize = contentsPerPage;

            return result;
        }

        public virtual ActionResult LayoutGroupWithContentTiles(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutGroupAndContentData(group, page);

            return View("LayoutGroupWithContentTiles", model);
        }

        public virtual ActionResult LayoutGroupWithContentList(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutGroupAndContentData(group, page);
            return View("LayoutGroupWithContentList", model);
        }

        public virtual ActionResult LayoutGroupWithContentBlock(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutGroupAndContentData(group, page);

            return View("LayoutGroupWithContentBlock", model);
        }

        #endregion

        #region Search Layout

        public virtual ActionResult Search(string q = null, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return RedirectToAction("Index");
            }

            //number of content per page
            int contentsPerPage;
            if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.ContentPerPage), out contentsPerPage))
            {
                contentsPerPage = 10;
            }

            var model = new LayoutContentViewModel();

            var clang = _language.Substring(0, 2);
            var lang = "";
            if (clang == "vi")
            {
                lang = "vi-VN";
            }
            else
            {
                lang = "en-US";
            }
            var listContent = _contentService.Search(q);
            listContent = listContent.Where(o => o.Language == lang);
            model.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent
                .Skip((page - 1) * contentsPerPage).Take(contentsPerPage)).ToList();

            model.Page = page;
            model.TotalItems = listContent.Count();
            model.PageSize = contentsPerPage;
            ViewBag.Query = q;
            return View(model);
        }

        #endregion

        #region Other Layout
        public virtual ActionResult LayoutOrder(GroupViewModel group)
        {
            LayoutContactViewModel model = new LayoutContactViewModel();
            model.Group = group;
            model.LastGroupParentID = _groupService.GetLastGroupParentId(group.GroupID);
            model.Title = _systemService.GetSystemParameter(SysParaType.OrgName);
            model.Map = _systemService.GetSystemParameter(SysParaType.GoogleMap);
            model.Address = _systemService.GetSystemParameter(SysParaType.Address);
            model.Phone = _systemService.GetSystemParameter(SysParaType.Phone);
            model.Fax = _systemService.GetSystemParameter(SysParaType.Fax);
            model.Email = _systemService.GetSystemParameter(SysParaType.Email);
            model.Facebook = _systemService.GetSystemParameter(SysParaType.FacebookPage);
            model.Twitter = _systemService.GetSystemParameter(SysParaType.Twitter);
            model.Youtube = _systemService.GetSystemParameter(SysParaType.Youtube);
            model.Website = _systemService.GetSystemParameter(SysParaType.Website);
            model.Address2 = _systemService.GetSystemParameter(SysParaType.Yahoo);
            //  model.SalesHotline = _systemService.GetSystemParameter(SysParaType.SalesHotline);
            //model.ServicesHotline = _systemService.GetSystemParameter(SysParaType.ServicesHotline);
            //model.TableSchedule = _systemService.GetForID(29).Description;
            //if (_language.Substring(0, 2) == "en")
            //{
            //    model.Title = _systemService.GetSystemParameter(SysParaType.EnOrg);
            //    model.Address = _systemService.GetSystemParameter(SysParaType.EnAddress);
            //    model.Address2 = _systemService.GetForID(27).ParaValue.ToString().Trim();
            //}

            model.ListAddress = model.Address.Split(',');
            var listContent = _distributionService.GetOrderedForGroupID(group.GroupID, _language, DistributionOrder.HighestPriority);

            model.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent).ToList();

            var listGroup = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(group.GroupID, _language, 1);

            model.ListGroup = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup).ToList();

            if (listGroup != null)
            {
                foreach (var g in model.ListGroup)
                {
                    var listGroup1 = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(g.GroupID, _language, 1);
                    var listContent1 = _distributionService.GetOrderedForGroupIDOnlyOneLevel(g.GroupID, _language, DistributionOrder.Lastest);
                    g.ListGroup = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup1).ToList();
                    g.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent1).ToList();
                }
            }

            return View("LayoutOrder", model);
        }


        public virtual ActionResult LayoutContact(GroupViewModel group)
        {
            LayoutContactViewModel model = new LayoutContactViewModel();
            model.Group = group;
            model.LastGroupParentID = _groupService.GetLastGroupParentId(group.GroupID);
            model.Title = _systemService.GetSystemParameter(SysParaType.OrgName);
            model.Map = _systemService.GetSystemParameter(SysParaType.GoogleMap);
            model.Address = _systemService.GetSystemParameter(SysParaType.Address);
            model.Phone = _systemService.GetSystemParameter(SysParaType.Phone);
            model.Fax = _systemService.GetSystemParameter(SysParaType.Fax);
            model.Email = _systemService.GetSystemParameter(SysParaType.Email);
            model.Facebook = _systemService.GetSystemParameter(SysParaType.FacebookPage);
            model.Twitter = _systemService.GetSystemParameter(SysParaType.Twitter);
            model.Youtube = _systemService.GetSystemParameter(SysParaType.Youtube);
            model.Website = _systemService.GetSystemParameter(SysParaType.Website);
            model.Address2 = _systemService.GetSystemParameter(SysParaType.Yahoo);
            //  model.SalesHotline = _systemService.GetSystemParameter(SysParaType.SalesHotline);
            //model.ServicesHotline = _systemService.GetSystemParameter(SysParaType.ServicesHotline);
            //model.TableSchedule = _systemService.GetForID(29).Description;
            //if (_language.Substring(0, 2) == "en")
            //{
            //    model.Title = _systemService.GetSystemParameter(SysParaType.EnOrg);
            //    model.Address = _systemService.GetSystemParameter(SysParaType.EnAddress);
            //    model.Address2 = _systemService.GetForID(27).ParaValue.ToString().Trim();
            //}

            model.ListAddress = model.Address.Split(',');
            var listContent = _distributionService.GetOrderedForGroupID(group.GroupID, _language, DistributionOrder.HighestPriority);

            model.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent).ToList();

            var listGroup = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(group.GroupID, _language, 1);

            model.ListGroup = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup).ToList();

            if (listGroup != null)
            {
                foreach (var g in model.ListGroup)
                {
                    var listGroup1 = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(g.GroupID, _language, 1);
                    var listContent1 = _distributionService.GetOrderedForGroupIDOnlyOneLevel(g.GroupID, _language, DistributionOrder.Lastest);
                    g.ListGroup = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup1).ToList();
                    g.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(listContent1).ToList();
                }
            }

            return View("LayoutContact", model);
        }

        public virtual ActionResult LayoutLibrary(GroupViewModel group)
        {
            LayoutContentViewModel result = new LayoutContentViewModel();

            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);


            return View("LayoutLibrary", model);
        }

        public virtual ActionResult LayoutPartner(GroupViewModel group)
        {
            int page = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]);
            var model = GetLayoutContentData(group, page);

            return View("LayoutPartner", model);
        }


        public virtual ActionResult LayoutContentDetail(int id)
        {
            var model = new LayoutContentDetailViewModel();

            var dist = _distributionService.GetGroupsForContentID(id, _language).FirstOrDefault();
            if (dist != null)
            {
                model.Group = _mapper.Map<GroupViewModel>(_groupService.GetGroupForID(dist.GroupID));
            }
            else
            {
                return HttpNotFound();
            }


            model.LastGroupParentID = _groupService.GetLastGroupParentId(model.Group.GroupID);
            model.Group.GroupParentName = _groupService.GetGroupForID(model.LastGroupParentID).Title;
            model.Content = _mapper.Map<ContentViewModel>(_contentService.GetContentForID(id, true, true));
            model.Content.PublishDate = dist.CustomPublishDate ?? dist.CreationDate;

            var author = _accountService.GetAccountProperty(model.Content.Author, AccountPropertyString.Name);
            model.Content.AuthorName = author;

            //get another news
            model.AnotherNews = _mapper.Map<List<ContentBoxViewModel>>(_distributionService
                .GetOrderedForGroupID(dist.GroupID, _language, DistributionOrder.Lastest).Where(o => o.Item2.ContentID != id).Take(4));
            if (model.AnotherNews!= null)
            {
                foreach (var item in model.AnotherNews)
                {
                    var author1 = _accountService.GetAccountProperty(item.Author, AccountPropertyString.Name);
                    item.AuthorName = author1;
                }
            }

            if (!model.Group.Layout.HasValue || model.Group.Layout.Value == 0)
            {
                model.Group.Layout = _groupService.GetClosestGroupParrentLayout(model.Group.GroupParentID.Value);
            }

            _contentService.UpdateViewCount(model.Content.ContentID);

            var listGroup = _groupService.GetGroupsForGroupParentID_OnlyOneLevel(model.Group.GroupParentID, _language, 1);
            var groups = _mapper.Map<IEnumerable<GroupViewModel>>(listGroup).ToList();
            for (int i = 0; i < groups.Count(); i++)
                groups[i].FriendUrl = _routingService.GetFriendlyUrl(RoutingType.Group, groups[i].GroupID);

            ViewData["listGroup2"] = groups;
            switch (model.Group.Layout)
            {
                case 2:
                    return View("LayoutContentTilesDetail", model);
                case 3:
                    return View("LayoutNewsDetail", model);
                case 15:
                    return View("LayoutCustom1Detail", model);
                default:
                    return View(model);
            }
        }

        public virtual ActionResult DownloadAttached(int id)
        {
            var download = _contentService.GetDownload(id);
            if (download == null)
            {
                return HttpNotFound();
            }

            string filePath = Server.MapPath(download.Url);
            var fileInfo = new FileInfo(Server.MapPath(download.Url.Trim()));
            if (fileInfo.Exists)
            {
                string extension = fileInfo.Extension;
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(download.Url));
                string fileName = download.Name.Trim();
                //add extension to file name if not has one
                if (!fileName.Contains(extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    fileName = fileName + extension;
                }
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                return HttpNotFound();
            }
        }
        #endregion

        #region Tag Layout

        public virtual ActionResult Tag(int id, int page = 1) 
        {
            TagPageViewModel model = new TagPageViewModel();

            model.Tag = _mapper.Map<TagViewModel>(_tagService.Get(id));
            if (model.Tag == null)
            {
                return HttpNotFound();
            }

            //number of content per page
            int contentsPerPage;
            if (!int.TryParse(_systemService.GetAppPropertyValue(AppPropertyString.ContentPerPage), out contentsPerPage))
            {
                contentsPerPage = 10;
            }

            model.ListContent = _mapper.Map<IEnumerable<ContentBoxViewModel>>(
                _contentService.GetContentByTag(id, _language, ContentStatus.Published, contentsPerPage, page));

            model.PageSize = contentsPerPage;
            model.Page = page;
            return View(model);
        }

        #endregion

        [NonAction]
        public String GetAllImgandVideo(string Pictures)
        {
            string images = "";
            string pattern1 = @"<(img)\b[^>]*>";
            string pattern2 = @"<(iframe)\b[^>]*>";

            Regex rgx1 = new Regex(pattern1, RegexOptions.IgnoreCase);
            Regex rgx2 = new Regex(pattern2, RegexOptions.IgnoreCase);
            MatchCollection matches1 = rgx1.Matches(Pictures);
            MatchCollection matches2 = rgx2.Matches(Pictures);
            for (int i = 0, l = matches1.Count; i < l; i++)
            {
                images += "<div class='item'>" + matches1[i].Value + "</div>";
            }
            for (int i = 0, l = matches2.Count; i < l; i++)
            {
                images += "<div class=\"fixed-video-aspect\"><div class=\"item-video\">" + matches1[i].Value + "</div></div>";
            }
            return images;
        }
    }
}

