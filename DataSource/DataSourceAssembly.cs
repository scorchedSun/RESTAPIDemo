using Contracts;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DataSource
{
    public class DataSourceAssembly
    {
        private const string assemblyBase = "DataSource";
        private const string fileExtension = ".dll";
        
        private readonly string dataSourceType;
        private Assembly underlyingAssembly;

        public Assembly UnderlyingAssembly => underlyingAssembly ?? (underlyingAssembly = Assembly.LoadFile(Path.Combine(ExecutionPath, FileName)));

        public Type TypeOfConfigurationInterface => UnderlyingAssembly
            .ExportedTypes
            .Single(type => IsDataSourceConfigurationInterface(type.GetTypeInfo()));

        public Type TypeOfConfiguration => UnderlyingAssembly
            .ExportedTypes
            .Single(type => type.GetTypeInfo().ImplementedInterfaces.Contains(TypeOfConfigurationInterface));

        public DataSourceAssembly(string dataSourceType) => this.dataSourceType = dataSourceType;

        private string FileName => dataSourceType.ToUpper() + assemblyBase + fileExtension;

        private bool IsDataSourceConfigurationInterface(TypeInfo type) => type.ImplementedInterfaces.Contains(typeof(IDataSourceConfiguration)) && type.IsInterface;

        private string ExecutionPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}
