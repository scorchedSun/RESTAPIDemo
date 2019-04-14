using System;
using System.Collections.Generic;
using System.Threading;
using Contracts;
using DataSource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Factories;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using Repositories;

namespace RESTAPIDemo
{
    /// <summary>
    /// Startup class
    /// </summary>
    /// <remarks>
    /// Wiring up Ninject to the application made possible by this tutorial: https://dev.to/cwetanow/wiring-up-ninject-with-aspnet-core-20-3hp
    /// </remarks>

    public class Startup
    {
        private sealed class Scope : DisposableObject { }

        private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();

        private readonly ILogger logger = new DiagnosticsLogger();

        // The IoC's kernel
        private IKernel Kernel { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Called by the runtime.
        /// Use to add services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> of registered services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRequestScopingMiddleware(() => scopeProvider.Value = new Scope());
            services.AddCustomControllerActivation(Resolve);
            services.AddCustomViewComponentActivation(Resolve);
        }

        /// <summary>
        /// Called by the runtime.
        /// Use to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/></param>
        /// <param name="env">The <see cref="IHostingEnvironment"/></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Kernel = RegisterApplicationComponents(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private object Resolve(Type type) => Kernel.Get(type);
        private object RequestScope(IContext context) => scopeProvider.Value;

        /// <summary>
        /// Registers the application's components.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/></param>
        /// <returns>The IoC container's <see cref="IKernel"/></returns>
        private IKernel RegisterApplicationComponents(IApplicationBuilder app)
        {
            IKernel kernel = new StandardKernel();

            kernel = RegisterApplicationServices(app, kernel);
            kernel = CreateBindings(kernel);
            kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

            return kernel;
        }

        /// <summary>
        /// Registers the application's services.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/></param>
        /// <param name="kernel">The IoC container's <see cref="IKernel"/></param>
        /// <returns>The <paramref name="kernel"/></returns>
        private IKernel RegisterApplicationServices(IApplicationBuilder app, IKernel kernel)
        {
            foreach (var ctrlType in app.GetControllerTypes())
            {
                kernel.Bind(ctrlType).ToSelf().InScope(RequestScope);
            }
            return kernel;
        }

        /// <summary>
        /// Creates the IoC bindings so that the interfaces can be resolved at runtime.
        /// </summary>
        /// <param name="kernel">The IoC container's <see cref="IKernel"/></param>
        /// <returns>The <paramref name="kernel"/></returns>
        private IKernel CreateBindings(IKernel kernel)
        {
            kernel.Bind<ILogger>().ToConstant(logger).InSingletonScope();
            kernel.Bind<IPersonBuilderFactory>().To(typeof(PersonBuilderFactory));
            kernel.Bind<IAddressBuilderFactory>().To(typeof(AddressBuilderFactory));
            kernel.Bind<IPersonRepository>().To(typeof(PersonRepository));

            try
            {
                DataSourceTypeBinder binder = new DataSourceTypeBinder(new Dictionary<string, string>(Configuration.AsEnumerable()));
                return binder.Bind<IPerson>(kernel);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                throw;
            }
        }
    }
}
