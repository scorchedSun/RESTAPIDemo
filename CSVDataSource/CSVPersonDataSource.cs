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
        private readonly IList<(int, string)> toConvert = new List<(int, string)>();

        public CSVPersonDataSource(string filePath, IConverter<(int, string), IPerson> converter)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"Can't create a data source for the file '{filePath}' since it doesn't exist.");
            this.filePath = filePath;
            this.converter = converter;
        }

        public IList<IPerson> LoadAll()
        {
            toConvert.Clear();
            return converter.Convert(ProcessLines(File.ReadAllLines(filePath)));
        }

        private IList<(int, string)> ProcessLines(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim(';');
                if (!line.Equals(string.Empty))
                    toConvert.Add((i + 1, line));
            }
            return toConvert;
        }

        public void WriteAll(IList<IPerson> entries) => File.WriteAllLines(filePath, converter.Convert(entries).Select(entry => entry.Item2 + ";"));
    }
}
