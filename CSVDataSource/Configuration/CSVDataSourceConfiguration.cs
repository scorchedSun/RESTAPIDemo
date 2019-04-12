using CSVDataSource.Contracts;
using DataSource;
using Exceptions;
using System;
using System.Collections.Generic;

namespace CSVDataSource.Configuration
{
    public sealed class CSVDataSourceConfiguration : DataSourceConfiguration, ICSVDataSourceConfiguration
    {
        private const string expectedType = "csv";
        private const string numberOfFieldsExceptionMessage = "The entry NumberOfFields in the appsettings couldn't be parsed as an integer.";
        private const string separatorId = "Separator";
        private const string fieldSequenceFormat = "FieldSequence:{0}";

        private IList<string> fieldSequence;
        private string separator;

        public string Separator => separator ?? (separator = configuration[string.Format(dataSourceSettingsBase, Name, separatorId)]);

        public IList<string> FieldSequence => fieldSequence ?? (fieldSequence = new List<string>(DetermineFieldSequenceForDataSource()));

        private CSVDataSourceConfiguration(
            string name,
            IDictionary<string, string> configuration)
            : base(name, configuration)
        {
            string configuredType = GetConfiguredType(configuration, Name);
            if (configuredType != expectedType) throw new InvalidDataSourceTypeException(name, configuredType, expectedType);
        }

        new public static ICSVDataSourceConfiguration LoadFrom(IDictionary<string, string> configuration,
                                                               string name)
            => new CSVDataSourceConfiguration(name, configuration);

        private string[] DetermineFieldSequenceForDataSource()
        {
            if (!int.TryParse(configuration[string.Format(dataSourceSettingsBase, Name, "NumberOfFields")], out int numberOfFields))
                throw new FormatException(numberOfFieldsExceptionMessage);

            string[] fieldSequence = new string[numberOfFields];
            for (int i = 0; i < numberOfFields; i++)
                fieldSequence[i] = configuration[string.Format(dataSourceSettingsBase, Name, string.Format(fieldSequenceFormat, i))];

            return fieldSequence;
        }
    }
}
