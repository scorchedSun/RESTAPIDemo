using Contracts;
using System;
using System.Collections.Generic;

namespace DataSource
{
    /// <summary>
    /// Defines basic configuration values for instances of <see cref="IDataSource{T}"/> implementations
    /// </summary>
    public class DataSourceConfiguration : IDataSourceConfiguration
    {
        // Formatting string to create accessors for data source settings in the configuration
        protected const string dataSourceSettingsBase = "DataSources:{0}:{1}";

        // The configuration containing the data source's settings
        protected readonly IDictionary<string, string> configuration;

        private string path;

        /// <summary>
        /// Path to the physical data source
        /// </summary>
        public string Path => path ?? (path = ResolveDataSourcePath());

        /// <summary>
        /// The data source's name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new <see cref="DataSourceConfiguration"/>.
        /// </summary>
        /// <param name="name">The data source's name</param>
        /// <param name="configuration">Contains the data source's settings</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        protected DataSourceConfiguration(
            string name,
            IDictionary<string, string> configuration)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (name.Equals(string.Empty)) throw new ArgumentException(nameof(name));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            this.configuration = configuration;
            Name = name;
        }

        /// <summary>
        /// Factory method to create instances of <see cref="DataSourceConfiguration"/>.
        /// </summary>
        /// <param name="configuration">Contains the data source's settings</param>
        /// <param name="name">The data source's name</param>
        /// <returns>A new instance of <see cref="DataSourceConfiguration"/></returns>
        public static IDataSourceConfiguration LoadFrom(IDictionary<string, string> configuration,
                                                        string name)
            => new DataSourceConfiguration(name, configuration);

        /// <summary>
        /// Gets the data source's type from the settings.
        /// </summary>
        /// <param name="configuration">Contains the data source's settings</param>
        /// <param name="name">The data source's name</param>
        /// <returns>The data source's configured type.</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        public static string GetConfiguredType(IDictionary<string, string> configuration, string name)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (name.Equals(string.Empty)) throw new ArgumentException(nameof(name));
            return configuration[string.Format(dataSourceSettingsBase, name, "Type")];
        }

        /// <summary>
        /// Resolves the path to the physical representation of a data source using the <see cref="configuration"/>.
        /// Absolute and relative paths are accepted.
        /// </summary>
        /// <returns>The path to the data source's physical representation</returns>
        private string ResolveDataSourcePath()
        {
            string configuredPath = configuration[string.Format(dataSourceSettingsBase, Name, "Path")];
            if (!System.IO.Path.IsPathRooted(configuredPath))
                configuredPath = System.IO.Path.GetFullPath(configuredPath);
            return configuredPath;
        }
    }
}
