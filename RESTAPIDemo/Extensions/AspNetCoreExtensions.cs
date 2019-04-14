using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RESTAPIDemo
{
    // Source: https://dev.to/cwetanow/wiring-up-ninject-with-aspnet-core-20-3hp
    public static class AspNetCoreExtensions
    {
        public static void AddRequestScopingMiddleware(
            this IServiceCollection services,
            Func<IDisposable> requestScopeProvider)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (requestScopeProvider is null) throw new ArgumentNullException(nameof(requestScopeProvider));

            services
                .AddSingleton<IStartupFilter>(new RequestScopingStartupFilter(requestScopeProvider));
        }

        public static void AddCustomControllerActivation(
            this IServiceCollection services,
            Func<Type, object> activator)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (activator is null) throw new ArgumentNullException(nameof(activator));

            services
                .AddSingleton<IControllerActivator>(
                    new DelegatingControllerActivator(context => activator(context.ActionDescriptor.ControllerTypeInfo.AsType())));
        }

        public static void AddCustomViewComponentActivation(
            this IServiceCollection services,
            Func<Type, object> activator)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (activator is null) throw new ArgumentNullException(nameof(activator));

            services.AddSingleton<IViewComponentActivator>(new DelegatingViewComponentActivator(activator));
        }
    }
}
