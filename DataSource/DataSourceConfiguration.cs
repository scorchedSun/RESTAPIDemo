using Contracts;
using System.Collections.Generic;

namespace DataSource
{
    public class DataSourceConfiguration : IDataSourceConfiguration
    {
        // Formatting string to create accessors for data source settings in the appsettings.json
        protected const string dataSourceSettingsBase = "DataSources:{0}:{1}";

        protected readonly IDictionary<string, string> configuration;

        private string path;

        public string Path => path ?? (path = ResolveDataSourcePath());

        public string Name { get; }

        protected DataSourceConfiguration(
            string name,
            IDictionary<string, string> configuration)
        {
            this.configuration = configuration;
            Name = name;
        }

        public static IDataSourceConfiguration LoadFrom(IDictionary<string, string> configuration,
                                                        string name)
            => new DataSourceConfiguration(name, configuration);

        /// <summary>
        /// Gets the string representation of the physical data source's type.
        /// </summary>
        /// <returns>The data source's type as specified in appsettings.json</returns>
        public static string GetConfiguredType(IDictionary<string, string> configuration, string name) => configuration[string.Format(dataSourceSettingsBase, name, "Type")];

        /// <summary>
        /// Resolves the path to the physical representation of a data source using the application settings.
        /// Absolute and relative paths are accepted.
        /// </summary>
        /// <returns>The path to the data source's physical representation</returns>
        private string ResolveDataSourcePath()
        {
            string path = configuration[string.Format(dataSourceSettingsBase, Name, "Path")];
            if (!System.IO.Path.IsPathRooted(path))
                path = System.IO.Path.GetFullPath(path);
            return path;
        }
    }
}
