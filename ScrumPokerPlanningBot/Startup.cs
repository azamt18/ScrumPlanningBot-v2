using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ScrumPokerPlanningBot.API.Controllers;

[assembly: OwinStartup(typeof(ScrumPokerPlanningBot.API.Startup))]

namespace ScrumPokerPlanningBot.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // New code: Add the error page middleware to the pipeline. 
            app.UseErrorPage();

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.Run(context =>
            {
                // New code: Throw an exception for this URI path.
                if (context.Request.Path.Equals(new PathString("/fail")))
                {
                    throw new Exception("Random exception");
                }

                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });
        }

        protected void Application_Start()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceScopeModule.SetServiceProvider(services.BuildServiceProvider());

            DependencyResolver.SetResolver(new ServiceProviderDependencyResolver());
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ScopedThing>();
            services.AddTransient<HomeController>();
        }
    }

    public class ScopedThing : IDisposable
    {
        public ScopedThing()
        {

        }
        public void Dispose()
        {
        }
    }

    internal class ServiceScopeModule : IHttpModule
    {
        private static ServiceProvider _serviceProvider;

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                scope.Dispose();
            }
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            context.Items[typeof(IServiceScope)] = _serviceProvider.CreateScope();
        }

        public static void SetServiceProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }

    internal class ServiceProviderDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            if (HttpContext.Current?.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                return scope.ServiceProvider.GetService(serviceType);
            }

            throw new InvalidOperationException("IServiceScope not provided");
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (HttpContext.Current?.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                return scope.ServiceProvider.GetServices(serviceType);
            }

            throw new InvalidOperationException("IServiceScope not provided");
        }
    }
}
