using Contracts;
using CSVDataSource.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Utils.Exceptions;

namespace RESTAPIDemo.Configurations
{
    public sealed class CSVDataSourceConfiguration : DataSourceConfiguration, ICSVDataSourceConfiguration
    {
        private const string expectedType = "csv";

        private IList<string> fieldSequence;
        private string separator;

        public string Separator => separator ?? (separator = configuration[string.Format(dataSourceSettingsBase, Name, "Separator")]);

        public IList<string> FieldSequence => fieldSequence ?? (fieldSequence = new List<string>(DetermineFieldSequenceForDataSource()));

        private CSVDataSourceConfiguration(
            string name,
            IConfiguration configuration,
            IConverter<string, Type> typeConverter)
            : base(name, configuration, typeConverter)
        {
            if (ConfiguredType != expectedType) throw new InvalidDataSourceTypeException(name, ConfiguredType, expectedType);
        }

        new public static ICSVDataSourceConfiguration LoadFrom(IConfiguration configuration,
                                                               string name,
                                                               IConverter<string, Type> typeConverter)
            => new CSVDataSourceConfiguration(name, configuration, typeConverter);

        private string[] DetermineFieldSequenceForDataSource()
        {
            if (!int.TryParse(configuration[string.Format(dataSourceSettingsBase, Name, "NumberOfFields")], out int numberOfFields))
                throw new FormatException("The entry NumberOfFields in the appsettings couldn't be parsed as an integer.");

            string[] fieldSequence = new string[numberOfFields];
            for (int i = 0; i < numberOfFields; i++)
                fieldSequence[i] = configuration[string.Format(dataSourceSettingsBase, Name, $"FieldSequence:{i}")];

            return fieldSequence;
        }
    }
}
