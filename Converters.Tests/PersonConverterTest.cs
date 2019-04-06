using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Drawing;
using System.Linq;

namespace Converters.Tests
{
    [TestClass]
    public class PersonConverterTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PersonConverter_ConstructWithColourConverterNull_ThrowsArgumentNullException() => new PersonConverter(null, new AddressConverter());

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PersonConverter_ConstructWithAddressConverterNull_ThrowsArgumentNullException() => new PersonConverter(new ColourConverter(), null);

        [TestMethod]
        [DataRow(1, "Test", "Tester", "00000", "Test City", "1")]
        [DataRow(2, "Just", "Another-Test", "11111", "Some City", "7")]
        public void PersonConverter_ConvertValidString_Succeeds(int id, string name, string lastName, string zipCode, string city, string colourCode)
        {
            ColourConverter colourConverter = new ColourConverter();
            PersonConverter converter = new PersonConverter(colourConverter, new AddressConverter());

            IPerson person = converter.Convert((id, $"{name}, {lastName}, {zipCode} {city}, {colourCode}"));

            Assert.AreEqual(id, person.ID);
            Assert.AreEqual(name, person.Name);
            Assert.AreEqual(lastName, person.LastName);
            Assert.AreEqual(zipCode, person.Address.ZipCode);
            Assert.AreEqual(city, person.Address.City);
            Assert.AreEqual(colourCode, colourConverter.ColourMap.Single(kvp => kvp.Value == person.FavouriteColour).Key);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [DataRow("")]
        [DataRow("a")]
        [DataRow("a,b")]
        [DataRow("a, b, c")]
        [DataRow("a, b, c, d, e")]
        public void PersonConverter_ConvertInvalidString_ThrowsFormatException(string toConvert)
        {
            PersonConverter converter = new PersonConverter(new ColourConverter(), new AddressConverter());

            converter.Convert((1, toConvert));
        }

        [TestMethod]
        [DataRow(1, "Test", "Tester", "00000", "Test City", "1")]
        [DataRow(2, "Just", "Another-Test", "11111", "Some City", "7")]
        public void PersonConverter_ConvertPerson_ProducesExpectedFormat(int id, string name, string lastName, string zipCode, string city, string colourCode)
        {
            AddressConverter addressConverter = new AddressConverter();
            ColourConverter colourConverter = new ColourConverter();
            IAddress address = addressConverter.Convert($"{zipCode} {city}");
            Color colour = colourConverter.Convert(colourCode);
            IPerson person = new Person(id, name, lastName, address, colour);
            PersonConverter converter = new PersonConverter(colourConverter, addressConverter);

            (int id, string data) converted = converter.Convert(person);

            Assert.AreEqual(id, converted.id);
            Assert.AreEqual($"{name}, {lastName}, {zipCode} {city}, {colourCode}", converted.data);
        }
    }
}
