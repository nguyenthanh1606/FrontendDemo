using Admin.Infrastructure.Helpers;
using Admin.Infrastructure.Mapping;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Frontend.Infrastructure.Mapping;
using Store.Service.SystemService;
using System.Reflection;

namespace Frontend.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            ContainerBuilder builder = null;
            Admin.App_Start.Bootstrapper.SetAutofacContainer(ref builder);
            SetAutofacContainer(builder);
            //read theme config
            var themService = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IThemeService>();
            ThemeConfig.ReadThemeConfig(themService.CurrentTheme);
        }

        //// implement in Administration
        private static void SetAutofacContainer(ContainerBuilder builder)
        {
            if (builder == null)
                builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //auto mapper
            builder.Register(
                c => new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new DomainToViewModelMappingProfile());
                    cfg.AddProfile(new ViewModelToDomainMappingProfile());
                    cfg.AddProfile(new DomainToFrontEndViewModelMappingProfile());
                    cfg.AddProfile(new FrontEndViewModelToDomainMappingProfile());
                }))
                .AsSelf()
                .SingleInstance();

            builder.Register(
                c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            IContainer container = builder.Build();
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}