﻿using Contracts;
using CSVDataSource.Contracts;
using Exceptions;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace CSVDataSource
{
    public class CSVPersonDataSource : IDataSource<IPerson>
    {
        [Inject]
#pragma warning disable 649
        private readonly ILogger logger;
#pragma warning restore 

        private readonly ICSVDataSourceConfiguration configuration;
        private string FilePath => configuration.Path;

        private readonly IConverter<(int, string), IPerson> converter;
        private readonly IList<(int, string)> toConvert = new List<(int, string)>();

        public CSVPersonDataSource(
            [Named(nameof(IPerson))] ICSVDataSourceConfiguration configuration,
            IConverter<(int, string), IPerson> converter)
        {
            this.configuration = configuration;
            if (!File.Exists(FilePath))
                throw new ArgumentException($"Can't create a data source for the file '{FilePath}' since it doesn't exist.");
            this.converter = converter;
        }

        public IList<IPerson> LoadAll()
        {
            toConvert.Clear();
            try
            {
                return converter.Convert(ProcessLines(File.ReadAllLines(FilePath)));
            }
            catch (Exception ex) when (ex is TooFewFieldsException || ex is TooManyFieldsException)
            {
                logger.Log(ex);
                throw;
            }
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
