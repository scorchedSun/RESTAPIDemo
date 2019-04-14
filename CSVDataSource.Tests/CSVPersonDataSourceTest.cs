using Contracts;
using CSVDataSource.Converters;
using CSVDataSource.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using TestUtils;

namespace CSVDataSource.Tests
{
    [TestClass]
    public class CSVPersonDataSourceTest
    {
        private readonly IConverter<(uint, string), IPerson> personConverter;

        public CSVPersonDataSourceTest()
        {
            personConverter = new PersonConverter(
                new ColourConverter(),
                new AddressConverter(new AddressBuilderFactory()),
                new PersonBuilderFactory(),
                CSVFileUtil.CreateMockConfiguration());
        }

        [TestMethod]
        public void CSVPersonDataSource_ReadingValidFile_Succeeds()
        {
            CSVPersonDataSource personDataSource = CreateTestableDataSource(CSVFileUtil.CreateValidTestFile());

            IList<IPerson> persons = personDataSource.LoadAll();

            Assert.AreEqual(CSVFileUtil.NumberOfEntries, persons.Count);
            Assert.AreEqual(0, Errors(personDataSource));
            AssertPersonsWereReadCorrectly(persons);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CSVPersonDataSource_CreatingForNonExistantFile_NotPossible()
        {
            string filePath = CSVFileUtil.GetNonExistantFile();
            Assert.IsFalse(File.Exists(filePath));

            CreateTestableDataSource(filePath).LoadAll();
        }

        [TestMethod]
        public void CSVPersonDataSource_EntriesWithTooFewFields_NotLoaded()
        {
            CSVPersonDataSource personDataSource = CreateTestableDataSource(CSVFileUtil.CreateFileWithTooFewFields());

            IList<IPerson> persons = personDataSource.LoadAll();

            Assert.AreEqual(0, persons.Count);
            Assert.AreEqual(CSVFileUtil.NumberOfEntriesWithTooFewFields, Errors(personDataSource));
        }

        [TestMethod]
        public void CSVPersonDataSource_EntriesWithTooManyfields_NotLoaded()
        {
            CSVPersonDataSource personDataSource = CreateTestableDataSource(CSVFileUtil.CreateFileWithTooManyFields());

            IList<IPerson> persons = personDataSource.LoadAll();

            Assert.AreEqual(0, persons.Count);
            Assert.AreEqual(CSVFileUtil.NumberOfEntriesWithTooManyFields, Errors(personDataSource));
        }

        [TestMethod]
        public void CSVPersonDataSource_EntriesWithInvalidFormattedAddresses_NotLoaded()
        {
            CSVPersonDataSource personDataSource = CreateTestableDataSource(CSVFileUtil.CreateFileWithInvalidFormattedAddresses());

            IList<IPerson> persons = personDataSource.LoadAll();

            Assert.AreEqual(0, persons.Count);
            Assert.AreEqual(CSVFileUtil.NumberOfEntriesWithInvalidFormattedAddresses, Errors(personDataSource));
        }

        private int Errors(CSVPersonDataSource personDataSource)
            => TestableLogger(personDataSource).Errors.Count;

        private TestableLogger TestableLogger(CSVPersonDataSource personDataSource)
            => (TestableLogger)personDataSource.Logger;

        private CSVPersonDataSource CreateTestableDataSource(string filePath)
        {
            var mockConfiguration = CSVFileUtil.CreateMockConfiguration();
            mockConfiguration.Path = filePath;
            return new CSVPersonDataSource(mockConfiguration, personConverter) { Logger = new TestableLogger() };
        }

        private void AssertPersonsWereReadCorrectly(IList<IPerson> persons)
        {
            foreach (var person in persons)
            {
                (uint id, string data) = personConverter.Convert(person);
                Assert.IsTrue(CSVFileUtil.EntryExists(data));
                Assert.AreEqual(CSVFileUtil.IndexOf(data) + 1u, id);
            }
        }
    }
}
