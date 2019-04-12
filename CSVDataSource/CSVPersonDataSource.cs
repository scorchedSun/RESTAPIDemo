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

        private readonly IConverter<(int, string), IPerson> converter;

        public CSVPersonDataSource(
            [Named(nameof(IPerson))] ICSVDataSourceConfiguration configuration,
            IConverter<(int, string), IPerson> converter)
        {
            this.configuration = configuration;
            if (!File.Exists(FilePath))
                throw new ArgumentException(string.Format(creationWithInvalidFileFormat, FilePath));
            this.converter = converter;
        }

        public IList<IPerson> LoadAll()
        {
            IList<(int, string)> valuesToConvert = ProcessLines(File.ReadAllLines(FilePath));
            IList<IPerson> persons = new List<IPerson>();

            foreach ((int id, string data) value in valuesToConvert)
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

        private IList<(int, string)> ProcessLines(string[] lines)
        {
            IList<(int, string)> processedLines = new List<(int, string)>();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line))
                    processedLines.Add((i + 1, line));
            }
            return processedLines;
        }

        public void WriteAll(IList<IPerson> entries) => File.WriteAllLines(FilePath, converter.Convert(entries).Select(entry => entry.Item2));
    }
}
