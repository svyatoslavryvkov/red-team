﻿using Owin;
using Autofac;
using Microsoft.Owin;
using System.Web.Http;
using RedTeam.Repositories;
using Autofac.Integration.WebApi;
using RedTeam.SurveyMaster.WebApi;
using RedTeam.Repositories.Interfaces;
using RedTeam.SurveyMaster.Foundation;
using RedTeam.SurveyMaster.Repositories;
using RedTeam.SurveyMaster.WebApi.Controllers;
using RedTeam.SurveyMaster.Foundation.Interfaces;
using RedTeam.SurveyMaster.Repositories.Interfaces;

[assembly: OwinStartup(typeof(Startup))]

namespace RedTeam.SurveyMaster.WebApi
{
    public sealed class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            app.Use<GlobalExceptionMiddleware>();
            RegisterRoutes(config);
            ConfigureAutofac(config);
            app.UseWebApi(config);
        }

        private void ConfigureAutofac(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<ValueController>();
            builder.RegisterType<SurveyMasterUnitOfWork>().As<ISurveyMasterUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<SurveyService>().As<ISurveyService>().InstancePerLifetimeScope();
            builder.RegisterType<SurveyMasterDbContext>().AsSelf().InstancePerLifetimeScope();
            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void RegisterRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
