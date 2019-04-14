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

        // The configuration containing the data source's settings
        private readonly IDictionary<string, string> configuration;

        /// <summary>
        /// Creates a <see cref="DataSourceConfigurationCreator"/>.
        /// </summary>
        /// <param name="configuration">Contains the data source's settings</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public DataSourceTypeBinder(IDictionary<string, string> configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            this.configuration = configuration;
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
            if (kernel is null) throw new ArgumentNullException(nameof(kernel));

            string dataSourceId = dataSourceTypeIds[typeof(T)];
            string dataSourceType = DataSourceConfiguration.GetConfiguredType(this.configuration, dataSourceId);
            if (!IsDataSourceTypeValid(dataSourceType))
                throw new InvalidDataSourceTypeException(dataSourceId, dataSourceType);

            DataSourceAssembly dataSourceAssembly = new DataSourceAssembly(dataSourceType);
            DataSourceConfigurationCreator configurationCreator = new DataSourceConfigurationCreator(dataSourceAssembly);
            object config = configurationCreator.Create(this.configuration, dataSourceId);

            kernel.Bind(dataSourceAssembly.TypeOfConfigurationInterface).ToConstant(config).Named(typeof(T).Name);
            kernel.Load(dataSourceAssembly.UnderlyingAssembly);

            return kernel;
        }

        /// <summary>
        /// Checks whether a given data source type is in the list of supported data sources.
        /// </summary>
        /// <param name="type">The data source type</param>
        /// <returns><see cref="true"/> if the type is supported, <see cref="false"/> otherwise</returns>
        private bool IsDataSourceTypeValid(string type) => validDataSourceTypes.Contains(type.ToLower());
    }
}
