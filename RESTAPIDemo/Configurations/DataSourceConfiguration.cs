using Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPIDemo.Configurations
{
    public class DataSourceConfiguration : IDataSourceConfiguration
    {
        // Formatting string to create accessors for data source settings in the appsettings.json
        protected const string dataSourceSettingsBase = "DataSource:{0}:{1}";

        protected readonly IConfiguration configuration;
        protected readonly IConverter<string, Type> typeConverter;

        private string path;
        private Type type;

        public string Path => path ?? (path = ResolveDataSourcePath());

        public Type Type => type ?? (type = typeConverter.Convert(ConfiguredType));

        public string Name { get; }

        protected DataSourceConfiguration(
            string name,
            IConfiguration configuration,
            IConverter<string, Type> typeConverter)
        {
            this.configuration = configuration;
            this.typeConverter = typeConverter;
            Name = name;
        }

        public static IDataSourceConfiguration LoadFrom(IConfiguration configuration,
                                                        string name,
                                                        IConverter<string, Type> typeConverter)
            => new DataSourceConfiguration(name, configuration, typeConverter);

        /// <summary>
        /// Gets the string representation of the physical data source's type.
        /// </summary>
        /// <returns>The data source's type as specified in appsettings.json</returns>
        public string ConfiguredType => configuration[string.Format(dataSourceSettingsBase, Name, "Type")];

        /// <summary>
        /// Resolves the path to the physical representation of a data source using the application settings.
        /// Absolute and relative paths are accepted.
        /// </summary>
        /// <returns>The path to the data source's physical representation</returns>
        private string ResolveDataSourcePath()
        {
            string path = configuration[string.Format(dataSourceSettingsBase, Name, "Path")];
            if (!System.IO.Path.IsPathFullyQualified(path))
                path = System.IO.Path.GetFullPath(path);
            return path;
        }
    }
}
