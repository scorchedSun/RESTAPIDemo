using CSVDataSource.Contracts;
using DataSource;
using Exceptions;
using System;
using System.Collections.Generic;

namespace CSVDataSource.Configuration
{
    /// <summary>
    /// Configuration for a data source that uses a CSV file as storage.
    /// </summary>
    public sealed class CSVDataSourceConfiguration : DataSourceConfiguration, ICSVDataSourceConfiguration
    {
        private const string expectedType = "csv";
        private const string numberOfFieldsExceptionMessage = "The entry NumberOfFields in the appsettings couldn't be parsed as an integer.";
        private const string separatorId = "Separator";
        private const string fieldSequenceFormat = "FieldSequence:{0}";

        private IList<string> fieldSequence;
        private string separator;

        /// <summary>
        /// The <see cref="string"/> separating values on one line
        /// </summary>
        public string Separator => separator ?? (separator = configuration[string.Format(dataSourceSettingsBase, Name, separatorId)]);

        /// <summary>
        /// The sequence of the fields, defining which value is located at which index on one line
        /// </summary>
        public IList<string> FieldSequence => fieldSequence ?? (fieldSequence = new List<string>(DetermineFieldSequenceForDataSource()));

        /// <summary>
        /// Creates a new <see cref="CSVDataSourceConfiguration"/>.
        /// </summary>
        /// <param name="name">The data source's name</param>
        /// <param name="configuration">Contains the data source's settings</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        /// <exception cref="InvalidDataSourceTypeException">If the configured data source type doesn't match the expected type</exception>
        private CSVDataSourceConfiguration(
            string name,
            IDictionary<string, string> configuration)
            : base(name, configuration)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            string configuredType = GetConfiguredType(configuration, Name);
            if (configuredType != expectedType) throw new InvalidDataSourceTypeException(name, configuredType, expectedType);
        }

        /// <summary>
        /// Factory method to create instances of <see cref="CSVDataSourceConfiguration"/>.
        /// </summary>
        /// <param name="configuration">Contains the data source's settings</param>
        /// <param name="name">The data source's name</param>
        /// <returns>A new instance of <see cref="CSVDataSourceConfiguration"/></returns>
        new public static ICSVDataSourceConfiguration LoadFrom(IDictionary<string, string> configuration,
                                                               string name)
            => new CSVDataSourceConfiguration(name, configuration);

        /// <summary>
        /// Determines the field sequence using the given configuration.
        /// </summary>
        /// <returns>The sequence of fields</returns>
        private string[] DetermineFieldSequenceForDataSource()
        {
            if (!int.TryParse(configuration[string.Format(dataSourceSettingsBase, Name, "NumberOfFields")], out int numberOfFields))
                throw new FormatException(numberOfFieldsExceptionMessage);

            string[] fields = new string[numberOfFields];
            for (int i = 0; i < numberOfFields; i++)
                fields[i] = configuration[string.Format(dataSourceSettingsBase, Name, string.Format(fieldSequenceFormat, i))];

            return fields;
        }
    }
}
