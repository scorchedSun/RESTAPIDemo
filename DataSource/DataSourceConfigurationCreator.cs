using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataSource
{
    /// <summary>
    /// Provides functionality to create configuration objects for data sources
    /// </summary>
    public class DataSourceConfigurationCreator
    {
        // Name of the factory method used to create a configuration
        private const string factoryMethodName = nameof(DataSourceConfiguration.LoadFrom);

        // The assembly containing all implementations regarding the data source
        private readonly DataSourceAssembly assembly;

        /// <summary>
        /// Creates a <see cref="DataSourceConfigurationCreator"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="DataSourceAssembly"/> to use</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public DataSourceConfigurationCreator(DataSourceAssembly assembly)
        {
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));
            this.assembly = assembly;
        }

        /// <summary>
        /// Creates a new configuration object utilizing the configuration type's factory method.
        /// </summary>
        /// <param name="configuration">The application settings</param>
        /// <param name="dataSourceName">Data source's name</param>
        /// <returns>The configuration object</returns>
        /// <exception cref="NoFactoryMethodException">Thrown if the configuration type doesn't define a factory method named according to <see cref="factoryMethodName"/>/></exception>
        /// <exception cref="AmbiguousFactoryMethodException">Thrown if the configuration type defines more than one factory method named according to <see cref="factoryMethodName"/></exception>
        public object Create(IDictionary<string, string> configuration, string dataSourceName)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            if (dataSourceName is null) throw new ArgumentNullException(nameof(dataSourceName));
            IEnumerable<MethodInfo> methods = assembly.TypeOfConfiguration
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(method => method.Name.Equals(factoryMethodName));
            EnsureAmountOfFactoryMethodsIsCorrect(methods);

            return methods.Single().Invoke(null, new object[] { configuration, dataSourceName });
        }

        /// <summary>
        /// Helper method to ensure that the number of methods named after <see cref="factoryMethodName"/> meets
        /// the criterias.
        /// </summary>
        /// <param name="methods">The <see cref="IEnumerable{MethodInfo}"/> to check</param>
        private void EnsureAmountOfFactoryMethodsIsCorrect(IEnumerable<MethodInfo> methods)
        {
            if (!methods.Any())
                throw new NoFactoryMethodException(assembly.TypeOfConfiguration.FullName, factoryMethodName);
            else if (methods.Count() > 1)
                throw new AmbiguousFactoryMethodException(assembly.TypeOfConfiguration, factoryMethodName);
        }
    }
}
