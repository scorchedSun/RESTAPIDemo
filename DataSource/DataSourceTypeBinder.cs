using Contracts;
using Exceptions;
using Ninject;
using System;
using System.Collections.Generic;

namespace DataSource
{
    /// <summary>
    /// Provides functionality to load bindings for data sources into the IoC container's <see cref="IKernel"/>.
    /// </summary>
    public class DataSourceTypeBinder
    {
        // Strings to identify entries in appsettings.json regarding data sources
        private readonly IDictionary<Type, string> dataSourceTypeIds = new Dictionary<Type, string>
        {
            [typeof(IPerson)] = "Person"
        };

        // Strings to identify supported data source types (file formats)
        private readonly IList<string> validDataSourceTypes = new List<string>
        {
            "csv"
        };

        private readonly IDictionary<string, string> appConfiguration;

        public DataSourceTypeBinder(IDictionary<string, string> appConfiguration)
        {
            if (appConfiguration is null) throw new ArgumentNullException(nameof(appConfiguration));
            this.appConfiguration = appConfiguration;
        }

        /// <summary>
        /// Creates the Ninject bindings for a data source
        /// </summary>
        /// <typeparam name="T">The type the data source is for</typeparam>
        /// <param name="kernel">The IoC container's <see cref="IKernel"/></param>
        /// <returns>The IoC container's <see cref="IKernel"/></returns>
        /// <exception cref="InvalidDataSourceTypeException">Thrown if the data source type configured in the application settings isn't in the list of supported types</exception>
        public IKernel Bind<T>(IKernel kernel)
        {
            string dataSourceId = dataSourceTypeIds[typeof(T)];
            string dataSourceType = DataSourceConfiguration.GetConfiguredType(appConfiguration, dataSourceId);
            if (!IsDataSourceTypeValid(dataSourceType))
                throw new InvalidDataSourceTypeException(dataSourceId, dataSourceType);

            DataSourceAssembly dataSourceAssembly = new DataSourceAssembly(dataSourceType);
            DataSourceConfigurationCreator configurationCreator = new DataSourceConfigurationCreator(dataSourceAssembly);
            object configuration = configurationCreator.Create(appConfiguration, dataSourceId);

            kernel.Bind(dataSourceAssembly.TypeOfConfigurationInterface).ToMethod(_ => configuration).Named(typeof(T).Name);
            kernel.Load(dataSourceAssembly.UnderlyingAssembly);

            return kernel;
        }

        private bool IsDataSourceTypeValid(string type) => validDataSourceTypes.Contains(type.ToLower());
    }
}
