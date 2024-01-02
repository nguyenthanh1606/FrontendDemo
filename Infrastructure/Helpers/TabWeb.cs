using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Frontend.Models;
using Store.Service.SystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Infrastructure.Helpers
{
    public static class TabWeb
    {
        public static string SystemParameter(SysParaType type)
        {
            ISystemService sysService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<ISystemService>();
            return sysService.GetSystemParameter(type);
        }

        public static string GetCurrentTheme()
        {
            ISystemService sysService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<ISystemService>();
            return sysService.GetAppPropertyValue(AppPropertyString.CurrentTheme);
        }
    }
}