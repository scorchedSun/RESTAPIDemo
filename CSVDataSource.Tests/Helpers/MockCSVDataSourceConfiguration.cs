using System;
using System.Collections.Generic;
using CSVDataSource.Contracts;

namespace CSVDataSource.Tests.Helpers
{
    public class MockCSVDataSourceConfiguration : ICSVDataSourceConfiguration
    {
        public string Separator { get; set; }

        public IList<string> FieldSequence { get; set; }

        public string Path { get; set; }

        public Type Type { get; set; }

        public string Name { get; set; }
    }
}
