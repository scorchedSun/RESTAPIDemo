using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVDataSource
{
    public class CSVPersonDataSource : IDataSource<IPerson>
    {
        private readonly string filePath;
        private readonly IConverter<(int, string), IPerson> converter;

        public CSVPersonDataSource(string filePath, IConverter<(int, string), IPerson> converter)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"Can't create a data source for the file '{filePath}' since it doesn't exist.");
            this.filePath = filePath;
            this.converter = converter;
        }

        public IList<IPerson> LoadAll()
        {
            IList<(int, string)> toConvert = new List<(int, string)>();

            int lineNumber = 1;
            foreach (string line in File.ReadAllLines(filePath))
            {
                if (line.Equals(string.Empty)) continue;

                string trimmedLine = line.Trim(';');
                if (trimmedLine.Count(c => c == ',') == 3)
                    toConvert.Add((lineNumber, trimmedLine));
                lineNumber++;
            }

            return converter.Convert(toConvert);
        }

        public void WriteAll(IList<IPerson> entries) => File.WriteAllLines(filePath, converter.Convert(entries).Select(entry => entry.Item2 + ";"));
    }
}
