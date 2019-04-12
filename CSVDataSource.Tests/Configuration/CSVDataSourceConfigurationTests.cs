using CSVDataSource.Configuration;
using CSVDataSource.Contracts;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSVDataSource.Tests.Configuration
{
    [TestClass]
    public class CSVDataSourceConfigurationTests
    {
        private const string name = "test";

        [TestMethod]
        [ExpectedException(typeof(InvalidDataSourceTypeException))]
        public void CSVDataSourceConfiguration_CreateWithInvalidType_ThrowsInvalidDataSourceTypeException()
        {
            _ = CSVDataSourceConfiguration.LoadFrom(
                new Dictionary<string, string>() { [$"DataSources:{name}:Type"] = "" },
                name);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [DataRow("")]
        [DataRow("not an integer")]
        public void CSVDataSourceConfiguration_LoadFieldSequenceWithInvalidNumberOfFields_ThrowsFormatException(string numberOfFields)
        {
            _ = CSVDataSourceConfiguration.LoadFrom(
                new Dictionary<string, string>
                {
                    [$"DataSources:{name}:Type"] = "csv",
                    [$"DataSources:{name}:NumberOfFields"] = numberOfFields
                },
                name).FieldSequence;
        }
    }
}
