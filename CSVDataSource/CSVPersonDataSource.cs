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
    /// <summary>
    /// Class representing a data source that stores <see cref="IPerson"/>s in a CSV file.
    /// </summary>
    public class CSVPersonDataSource : IDataSource<IPerson>
    {
        private const string creationWithInvalidFileFormat = "Can't create a data source for the file '{0}' since it doesn't exist.";
        private const string invalidEntryFormat = "Couldn't read entry at line {0} due to an error.";

        [Inject]
        public ILogger Logger { get; set; }

        // The data source's configuration
        private readonly ICSVDataSourceConfiguration configuration;
        // File path to the physical representation of the data source
        private string FilePath => configuration.Path;

        private readonly IConverter<(uint, string), IPerson> converter;

        /// <summary>
        /// Create a new <see cref="CSVPersonDataSource"/>.
        /// </summary>
        /// <param name="configuration">The data source's configuration</param>
        /// <param name="converter">The converter to use when reading/writing from/to the physical data source</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
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

        /// <summary>
        /// Loads all persons from the CSV file.
        /// </summary>
        /// <returns>The <see cref="IList{IPerson}"/> contained in the CSV file</returns>
        public IList<IPerson> LoadAll()
        {
            EnsureFileCanBeRead();

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
                    // Entries that aren't formatted correctly should be ignored, but need an entry in the Logs
                    Logger.Log(string.Format(invalidEntryFormat, value.id - 1), ex);
                }
            }
            return persons;
        }

        /// <summary>
        /// Writes a <see cref="IList{IPerson}"/> to the CSV file.
        /// </summary>
        /// <param name="entries">The <see cref="IList{IPerson}"/> to write to the CSV file</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public void WriteAll(IList<IPerson> entries)
        {
            if (entries is null) throw new ArgumentNullException(nameof(entries));

            EnsureFileCanBeWrittenTo();

            File.WriteAllLines(FilePath, converter.Convert(entries).Select(entry => entry.Item2));
        }

        /// <summary>
        /// Lines from the CSV file need additional processing since the linenumber shall be used as 
        /// the <see cref="IPerson"/>'s <see cref="IIdentifyable.ID"/>. 
        /// </summary>
        /// <param name="lines">The lines to process</param>
        /// <returns></returns>
        private IList<(uint, string)> ProcessLines(string[] lines)
        {
            IList<(uint, string)> processedLines = new List<(uint, string)>();
            for (uint i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line))
                    processedLines.Add((i + 1, line));  // IDs start at 1 
            }
            return processedLines;
        }

        private void EnsureFileCanBeRead() => EnsureFileAcces(FileAccess.Read);

        private void EnsureFileCanBeWrittenTo() => EnsureFileAcces(FileAccess.Write);

        private void EnsureFileAcces(FileAccess mode)
        {
            try
            {
                File.Open(FilePath, FileMode.Open, mode).Dispose();
            }
            catch (IOException ex)
            {
                Logger.Log($"Unable to {Enum.GetName(typeof(FileAccess), mode).ToLower()} file '{FilePath}'. ", ex);
                throw;
            }
        }
    }
}
