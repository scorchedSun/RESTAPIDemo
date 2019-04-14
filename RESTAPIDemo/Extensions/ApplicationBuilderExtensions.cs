using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Ninject;

namespace RESTAPIDemo
{
    // Source: https://dev.to/cwetanow/wiring-up-ninject-with-aspnet-core-20-3hp
    public static class ApplicationBuilderExtensions
    {
        public static void BindToMethod<T>(this IKernel config, Func<T> method)
            => config.Bind<T>().ToMethod(_ => method());

        public static Type[] GetControllerTypes(this IApplicationBuilder builder)
        {
            var manager = builder.ApplicationServices.GetRequiredService<ApplicationPartManager>();

            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            return feature.Controllers.Select(t => t.AsType()).ToArray();
        }

        public static T GetRequestService<T>(this IApplicationBuilder builder) where T : class
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            return GetRequestServiceProvider(builder).GetService<T>();
        }

        private static IServiceProvider GetRequestServiceProvider(IApplicationBuilder builder)
        {
            var accessor = builder.ApplicationServices.GetService<IHttpContextAccessor>()
                ?? throw new InvalidOperationException(typeof(IHttpContextAccessor).FullName);
            var context = accessor.HttpContext
                ?? throw new InvalidOperationException("No HttpContext.");

            return context.RequestServices;
        }
    }
}
