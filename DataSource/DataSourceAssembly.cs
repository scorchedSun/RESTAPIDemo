using Contracts;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DataSource
{
    /// <summary>
    /// Represents an assembly containing the code for a data source.
    /// Such an assembly needs to fulfill the following criterias to be able to be used as an data source:
    ///     - Be named accoring to the combination of <see cref="dataSourceType"/>, <see cref="assemblyBase"/> and <see cref="fileExtension"/>
    ///     - Exist in the execution directory
    ///     - If necessary contain a public interface extending <see cref="IDataSourceConfiguration"/>
    ///     - If the previous point is met, a public class implementing said interface. The class has to implement a public static factory method 
    ///       named "LoadFrom"
    /// </summary>
    public class DataSourceAssembly
    {
        // Base name for the assembly
        private const string assemblyBase = "DataSource";
        // File extension for assemblies
        private const string fileExtension = ".dll";
        
        // The data source's type, used to identify the underlying assembly
        private readonly string dataSourceType;
        // The actual assembly
        private Assembly underlyingAssembly;

        /// <summary>
        /// The acutal assembly from the execution directory
        /// </summary>
        public Assembly UnderlyingAssembly
            => underlyingAssembly
            ?? (underlyingAssembly = Assembly.LoadFile(Path.Combine(ExecutionPath, FileName)));

        /// <summary>
        /// The interface extending <see cref="IDataSourceConfiguration"/> if the <see cref="UnderlyingAssembly"/>
        /// defines such an interface otherwise <see cref="IDataSourceConfiguration"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">There are more than one interfaces extending <see cref="IDataSourceConfiguration"/></exception>
        public Type TypeOfConfigurationInterface => UnderlyingAssembly
            .ExportedTypes
            .SingleOrDefault(type => IsDataSourceConfigurationInterface(type.GetTypeInfo()))
            ?? typeof(IDataSourceConfiguration);

        /// <summary>
        /// The configuration class implementing the interface <see cref="TypeOfConfigurationInterface"/> 
        /// used in the <see cref="UnderlyingAssembly"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">There isn't exaclty one public class implementing <see cref="TypeOfConfigurationInterface"/></exception>
        public Type TypeOfConfiguration => UnderlyingAssembly
            .ExportedTypes
            .SingleOrDefault(type => type.GetTypeInfo().ImplementedInterfaces.Contains(TypeOfConfigurationInterface))
            ?? typeof(DataSourceConfiguration);

        /// <summary>
        /// Creates a new <see cref="DataSourceAssembly"/> for a specific type of data source.
        /// </summary>
        /// <param name="dataSourceType">The data source's type</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        public DataSourceAssembly(string dataSourceType)
        {
            if (dataSourceType is null) throw new ArgumentNullException(nameof(dataSourceType));
            if (dataSourceType.Equals(string.Empty)) throw new ArgumentException(nameof(dataSourceType));
            this.dataSourceType = dataSourceType;
        }

        /// <summary>
        /// Name of the file containing the actual underlying assembly.
        /// </summary>
        private string FileName => dataSourceType.ToUpper() + assemblyBase + fileExtension;

        /// <summary>
        /// Determines whether a given <see cref="TypeInfo"/> implements the interface
        /// <see cref="IDataSourceConfiguration"/>.
        /// </summary>
        /// <param name="type">The <see cref="TypeInfo"/> to inspect.</param>
        /// <returns><see cref="true"/> if <paramref name="type"/> implements <see cref="IDataSourceConfiguration"/>, <see cref="false"/> otherwise</returns>
        private bool IsDataSourceConfigurationInterface(TypeInfo type) => type.ImplementedInterfaces.Contains(typeof(IDataSourceConfiguration)) && type.IsInterface;

        /// <summary>
        /// Gets the entry assembly's execution path / location
        /// </summary>
        private string ExecutionPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}
