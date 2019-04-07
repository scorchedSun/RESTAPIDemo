using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Converters;
using CSVDataSource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Models.Builders;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using Repositories;

namespace RESTAPIDemo
{
    public class Startup
    {
        private sealed class Scope : DisposableObject { }

        private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();
        private IKernel Kernel { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRequestScopingMiddleware(() => scopeProvider.Value = new Scope());
            services.AddCustomControllerActivation(Resolve);
            services.AddCustomViewComponentActivation(Resolve);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

        private IKernel RegisterApplicationComponents(IApplicationBuilder app)
        {
            var kernel = new StandardKernel();

            // Register application services
            foreach (var ctrlType in app.GetControllerTypes())
            {
                kernel.Bind(ctrlType).ToSelf().InScope(RequestScope);
            }

            Type personDataSource;
            string dataSourceType = Configuration["DataSource:Person:Type"].ToLower();
            string dataSourcePath = Configuration["DataSource:Person:Path"];
            if (!Path.IsPathFullyQualified(dataSourcePath))
                dataSourcePath = Path.GetFullPath(dataSourcePath);

            switch (dataSourceType)
            {
                case "csv":
                    personDataSource = typeof(CSVPersonDataSource);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid configuration in DataSource:type '{dataSourceType}'. Check your application settings.");
            }

            // This is where our bindings are configurated
            kernel.Bind<IPerson>().To(typeof(Person));
            kernel.Bind<IAddress>().To(typeof(Address));
            kernel.Bind<IConverter<string, IAddress>>().To(typeof(AddressConverter));
            kernel.Bind<IConverter<string, Color>>().To(typeof(ColourConverter));
            kernel.Bind<IConverter<(int, string), IPerson>>().To(typeof(PersonConverter));
            kernel.Bind<IPersonBuilder>().ToMethod(_ => PersonBuilder.Create());
            kernel.Bind<IAddressBuilder>().ToMethod(_ => AddressBuilder.Create());
            kernel.Bind<IDataSource<IPerson>>()
                .To(personDataSource)
                .WithConstructorArgument("filePath", dataSourcePath);
            kernel.Bind<IPersonRepository>().To(typeof(PersonRepository));

            // Cross-wire required framework services
            kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

            return kernel;
        }
    }
}
