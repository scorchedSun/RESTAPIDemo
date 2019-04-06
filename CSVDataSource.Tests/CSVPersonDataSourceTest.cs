using Contracts;
using Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVDataSource.Tests
{
    [TestClass]
    public class CSVPersonDataSourceTest
    {
        private readonly IList<string> testData = new List<string>()
        {
            "Müller, Hans, 67742 Lauterecken, 1",
            "Petersen, Peter, 18439 Stralsund, 2",
            "Johnson, Johnny, 88888 made up, 3",
            "Millenium, Milly, 77777 made up too, 4",
            "Müller, Jonas, 32323 Hansstadt, 5",
            "Fujitsu, Tastatur, 42342 Japan, 6",
            "Andersson, Anders, 32132 Schweden - Bonus, 2",
            "Bart, Bertram, 12313 Wasweißich, 1",
            "Gerber, Gerda, 76535 Woanders, 3",
            "Klaussen, Klaus, 43246 Hierach, 2"
        };

        [TestMethod]
        public void CSVPersonDataSource_ReadingValidCSV_Succeeds()
        {
            string filePath = Path.GetTempFileName() + ".csv";
            File.WriteAllLines(filePath, testData.Select(entry => entry + ";"));
            PersonConverter personConverter = new PersonConverter(new ColourConverter(), new AddressConverter());
            CSVPersonDataSource personDataSource = new CSVPersonDataSource(filePath, personConverter);

            IList<IPerson> persons = personDataSource.LoadAll();

            Assert.AreEqual(testData.Count, persons.Count);
            AssertPersonsWereReadCorrectly(persons);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CSVPersonDataSource_CreatingForNonExistantFile_NotPossible()
        {
            string filePath = Path.GetRandomFileName();
            Assert.IsFalse(File.Exists(filePath));

            new CSVPersonDataSource(filePath, new PersonConverter(new ColourConverter(), new AddressConverter())).LoadAll();
        }

        private void AssertPersonsWereReadCorrectly(IList<IPerson> persons)
        {
            PersonConverter personConverter = new PersonConverter(new ColourConverter(), new AddressConverter());

            foreach (var person in persons)
            {
                (int id, string data) = personConverter.Convert(person);
                Assert.IsTrue(testData.Any(entry => entry.Equals(data)));
                Assert.AreEqual(testData.IndexOf(data), person.ID - 1);
            }
        }
    }
}
