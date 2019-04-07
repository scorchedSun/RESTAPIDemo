using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Builders;
using System;
using System.Drawing;
using Utils;

namespace Converters.Tests
{
    [TestClass]
    public class PersonConverterTest
    {
        private readonly IConverter<(int, string), IPerson> personConverter;
        private readonly IConverter<string, IAddress> addressConverter;
        private readonly IConverter<string, Color> colourConverter;
        private readonly IPersonWithID personBuilder;

        public PersonConverterTest()
        {
            personBuilder = PersonBuilder.Create();

            addressConverter = new AddressConverter(AddressBuilder.Create());
            colourConverter = new ColourConverter();
            personConverter = new PersonConverter(colourConverter, addressConverter, personBuilder);
        }

        [TestMethod]
        [DataRow(1, "Test", "Tester", "00000", "Test City", "1")]
        [DataRow(2, "Just", "Another-Test", "11111", "Some City", "7")]
        public void PersonConverter_ConvertValidString_Succeeds(int id, string name, string lastName, string zipCode, string city, string colourCode)
        {
            IPerson person = personConverter.Convert((id, $"{name}, {lastName}, {zipCode} {city}, {colourCode}"));

            Assert.AreEqual(id, person.ID);
            Assert.AreEqual(name, person.Name);
            Assert.AreEqual(lastName, person.LastName);
            Assert.AreEqual(zipCode, person.Address.ZipCode);
            Assert.AreEqual(city, person.Address.City);
            Assert.AreEqual(colourCode, ColourMap.GetCodeFor(person.FavouriteColour));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [DataRow("")]
        [DataRow("a")]
        [DataRow("a,b")]
        [DataRow("a, b, c")]
        [DataRow("a, b, c, d, e")]
        public void PersonConverter_ConvertInvalidString_ThrowsFormatException(string toConvert) => personConverter.Convert((1, toConvert));

        [TestMethod]
        [DataRow(1, "Test", "Tester", "00000", "Test City", "1")]
        [DataRow(2, "Just", "Another-Test", "11111", "Some City", "7")]
        public void PersonConverter_ConvertPerson_ProducesExpectedFormat(int id, string name, string lastName, string zipCode, string city, string colourCode)
        {
            IAddress address = addressConverter.Convert($"{zipCode} {city}");
            Color colour = colourConverter.Convert(colourCode);
            IPerson person = personBuilder
                .WithID(id)
                .WithName(name)
                .WithLastName(lastName)
                .WithAddress(address)
                .WithFavouriteColour(colour)
                .Build();

            (int id, string data) converted = personConverter.Convert(person);

            Assert.AreEqual(id, converted.id);
            Assert.AreEqual($"{name}, {lastName}, {zipCode} {city}, {colourCode}", converted.data);
        }
    }
}
