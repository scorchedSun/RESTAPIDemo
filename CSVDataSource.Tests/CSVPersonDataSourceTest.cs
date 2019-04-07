using Contracts;
using Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using TestUtils;

namespace CSVDataSource.Tests
{
    [TestClass]
    public class CSVPersonDataSourceTest
    {
        private readonly IConverter<(int, string), IPerson> personConverter;

        public CSVPersonDataSourceTest()
        {
            personConverter = new PersonConverter(
                new ColourConverter(),
                new AddressConverter(AddressBuilder.Create()),
                PersonBuilder.Create());
        }

        [TestMethod]
        public void CSVPersonDataSource_ReadingValidCSV_Succeeds()
        {
            IDataSource<IPerson> personDataSource = new CSVPersonDataSource(CSVFileUtil.CreateTestFile(), personConverter);

            IList<IPerson> persons = personDataSource.LoadAll();

            Assert.AreEqual(CSVFileUtil.NumberOfEntries, persons.Count);
            AssertPersonsWereReadCorrectly(persons);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CSVPersonDataSource_CreatingForNonExistantFile_NotPossible()
        {
            string filePath = Path.GetRandomFileName();
            Assert.IsFalse(File.Exists(filePath));

            new CSVPersonDataSource(filePath, personConverter).LoadAll();
        }

        private void AssertPersonsWereReadCorrectly(IList<IPerson> persons)
        {
            foreach (var person in persons)
            {
                (int id, string data) = personConverter.Convert(person);
                Assert.IsTrue(CSVFileUtil.EntryExists(data));
                Assert.AreEqual(CSVFileUtil.IndexOf(data) + 1, id);
            }
        }
    }
}
