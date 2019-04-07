using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVDataSource
{
    public class CSVPersonDataSource : IDataSource<IPerson>
    {
        private readonly IPhysicalDataSource physicalDataSource;
        private string FilePath => physicalDataSource.FilePath;

        private readonly IConverter<(int, string), IPerson> converter;
        private readonly IList<(int, string)> toConvert = new List<(int, string)>();

        public CSVPersonDataSource(
            IPhysicalDataSource physicalDataSource,
            IConverter<(int, string), IPerson> converter)
        {
            this.physicalDataSource = physicalDataSource;
            if (!File.Exists(FilePath))
                throw new ArgumentException($"Can't create a data source for the file '{FilePath}' since it doesn't exist.");
            this.converter = converter;
        }

        public IList<IPerson> LoadAll()
        {
            toConvert.Clear();
            return converter.Convert(ProcessLines(File.ReadAllLines(FilePath)));
        }

        private IList<(int, string)> ProcessLines(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!line.Equals(string.Empty))
                    toConvert.Add((i + 1, line));
            }
            return toConvert;
        }

        public void WriteAll(IList<IPerson> entries) => File.WriteAllLines(FilePath, converter.Convert(entries).Select(entry => entry.Item2));
    }
}
