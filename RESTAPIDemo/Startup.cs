using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using Contracts;
using Converters;
using CSVConverters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Builders;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using Repositories;

namespace RESTAPIDemo
{
    public class Startup
    {
        private sealed class Scope : DisposableObject { }

        // Formatting string to create accessors for data source settings in the appsettings.json
        private const string dataSourceSettingsBase = "DataSource:{0}:{1}";
        // String to identify entries in appsettings.json regarding data sources for persons
        private const string personDataSourceID = "Person";

        private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();
        // Path to the physical data source for persons as configured in appsettings.json
        private string personDataSourcePath;
        // Type to be used to represent the data source for persons as configured in appsettings.json
        private Type personDataSource;
        // Separator sequence for csv files as configured in appsettings.json
        private string personCSVSeparator;
        // Sequence of fields in the csv file 
        private string[] personFieldSequence;

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

            personDataSourcePath = ResolveDataSourcePath(personDataSourceID);
            personDataSource = DetermineTypeForDataSource(personDataSourceID, new DataSourceTypeConverter());
            personCSVSeparator = DetermineSeparatorForDataSource(personDataSourceID);
            personFieldSequence = DetermineFieldSequenceForDataSource(personDataSourceID);

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
        /// Determines the type for a data source using the application settings.
        /// </summary>
        /// <param name="dataSourceName">The data source's name</param>
        /// <param name="converter">The type converter</param>
        /// <returns>The type for data source</returns>
        private Type DetermineTypeForDataSource(string dataSourceName, IConverter<string, Type> converter)
        {
            return converter.Convert(GetConfiguredType(dataSourceName));
        }

        /// <summary>
        /// Resolves the path to the physical representation of a data source using the application settings.
        /// Absolute and relative paths are accepted.
        /// </summary>
        /// <param name="dataSourceName">The data source's name</param>
        /// <returns>The path to the data source's physical representation</returns>
        private string ResolveDataSourcePath(string dataSourceName)
        {
            string path = Configuration[string.Format(dataSourceSettingsBase, dataSourceName, "Path")];
            if (!Path.IsPathFullyQualified(path))
                path = Path.GetFullPath(path);
            return path;
        }

        private string DetermineSeparatorForDataSource(string dataSourceName)
        {
            if (GetConfiguredType(dataSourceName) != "csv") return null;
            return Configuration[string.Format(dataSourceSettingsBase, dataSourceName, "Separator")];
        }

        private string[] DetermineFieldSequenceForDataSource(string dataSourceName)
        {
            if (GetConfiguredType(dataSourceName) != "csv") return null;
            if (!int.TryParse(Configuration[string.Format(dataSourceSettingsBase, dataSourceName, "NumberOfFields")], out int numberOfFields))
                return null;

            string[] fieldSequence = new string[numberOfFields];
            for (int i = 0; i < numberOfFields; i++)
                fieldSequence[i] = Configuration[string.Format(dataSourceSettingsBase, dataSourceName, $"FieldSequence:{i}")];

            return fieldSequence;
        }

        /// <summary>
        /// Gets the string representation of the physical data source's type.
        /// </summary>
        /// <param name="dataSourceName">The data source's name</param>
        /// <returns>The data source's type as specified in appsettings.json</returns>
        private string GetConfiguredType(string dataSourceName) => Configuration[string.Format(dataSourceSettingsBase, dataSourceName, "Type")];

        /// <summary>
        /// Creates the IoC bindings so that the interfaces can be resolved at runtime.
        /// </summary>
        /// <param name="kernel">The IoC container's <see cref="IKernel"/></param>
        /// <returns>The <paramref name="kernel"/></returns>
        private IKernel CreateBindings(IKernel kernel)
        {
            kernel.Bind<IConverter<string, IAddress>>().To(typeof(AddressConverter));
            kernel.Bind<IConverter<string, Color>>().To(typeof(ColourConverter));
            kernel.Bind<IConverter<(int, string), IPerson>>().To(typeof(PersonConverter));
            kernel.Bind<IPersonBuilder>().ToMethod(_ => PersonBuilder.Create());
            kernel.Bind<IAddressBuilder>().ToMethod(_ => AddressBuilder.Create());
            kernel.Bind<IPhysicalDataSource>().ToMethod(_ => new PhysicalDataSource(personDataSourcePath));
            kernel.Bind<IDataSource<IPerson>>().To(personDataSource);
            kernel.Bind<IPersonRepository>().To(typeof(PersonRepository));
            kernel.Bind<ISeparatorSequence>().ToMethod(_ => new SeparatorSequence(personCSVSeparator)).Named("csv");
            kernel.Bind<IFieldSequence>().ToMethod(_ => new FieldSequence(personFieldSequence)).Named("csv");
            return kernel;
        }
    }
}
