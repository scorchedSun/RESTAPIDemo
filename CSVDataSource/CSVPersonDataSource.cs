using Contracts;
using CSVDataSource.Contracts;
using Exceptions;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVDataSource
{
    public class CSVPersonDataSource : IDataSource<IPerson>
    {
        private const string creationWithInvalidFileFormat = "Can't create a data source for the file '{0}' since it doesn't exist.";
        private const string invalidEntryFormat = "Couldn't read entry at line {0} due to an error.";

#pragma warning disable 649
        [Inject]
        public ILogger Logger { get; set; }
#pragma warning restore 

        private readonly ICSVDataSourceConfiguration configuration;
        private string FilePath => configuration.Path;

        private readonly IConverter<(uint, string), IPerson> converter;

        public CSVPersonDataSource(
            [Named(nameof(IPerson))] ICSVDataSourceConfiguration configuration,
            IConverter<(uint, string), IPerson> converter)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            if (converter is null) throw new ArgumentNullException(nameof(converter));

            this.configuration = configuration;
            if (!File.Exists(FilePath))
                throw new ArgumentException(string.Format(creationWithInvalidFileFormat, FilePath));
            this.converter = converter;
        }

        public IList<IPerson> LoadAll()
        {
            IList<(uint, string)> valuesToConvert = ProcessLines(File.ReadAllLines(FilePath));
            IList<IPerson> persons = new List<IPerson>();

            foreach ((uint id, string data) value in valuesToConvert)
            {
                try
                {
                    persons.Add(converter.Convert(value));
                }
                catch (Exception ex) when (ex is TooFewFieldsException
                                            || ex is TooManyFieldsException
                                            || ex is InvalidFormattedAddressException)
                {
                    Logger.Log(string.Format(invalidEntryFormat, value.id - 1), ex);
                }
            }
            return persons;
        }

        public void WriteAll(IList<IPerson> entries)
        {
            if (entries is null) throw new ArgumentNullException(nameof(entries));
            File.WriteAllLines(FilePath, converter.Convert(entries).Select(entry => entry.Item2));
        }

        private IList<(uint, string)> ProcessLines(string[] lines)
        {
            IList<(uint, string)> processedLines = new List<(uint, string)>();
            for (uint i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line))
                    processedLines.Add((i + 1, line));
            }
            return processedLines;
        }
    }
}
