using Contracts;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Factories;
using System.Drawing;
using TestUtils;
using Utils;

namespace CSVDataSource.Converters.Tests
{
    [TestClass]
    public class PersonConverterTest
    {
        private readonly IConverter<(int, string), IPerson> personConverter;
        private readonly IConverter<string, IAddress> addressConverter;
        private readonly IConverter<string, Color> colourConverter;
        private readonly IPersonBuilderFactory personBuilderFactory;

        public PersonConverterTest()
        {
            personBuilderFactory = new PersonBuilderFactory();

            addressConverter = new AddressConverter(new AddressBuilderFactory());
            colourConverter = new ColourConverter();
            personConverter = new PersonConverter(
                colourConverter,
                addressConverter,
                personBuilderFactory,
                CSVFileUtil.CreateMockConfiguration());
        }

        [TestMethod]
        [DataRow(1, "Test", "Tester", "00000", "Test City", "1")]
        [DataRow(2, "Just", "Another-Test", "11111", "Some City", "7")]
        public void PersonConverter_ConvertValidString_Succeeds(int id, string name, string lastName, string zipCode, string city, string colourCode)
        {
            IPerson person = personConverter.Convert((id, $"{lastName}, {name}, {zipCode} {city}, {colourCode}"));

            Assert.AreEqual(id, person.ID);
            Assert.AreEqual(name, person.Name);
            Assert.AreEqual(lastName, person.LastName);
            Assert.AreEqual(zipCode, person.Address.ZipCode);
            Assert.AreEqual(city, person.Address.City);
            Assert.AreEqual(colourCode, ColourMap.GetCodeFor(person.FavouriteColour));
        }

        [TestMethod]
        [ExpectedException(typeof(TooFewFieldsException))]
        [DataRow("")]
        [DataRow("a")]
        [DataRow("a,b")]
        [DataRow("a, b, c")]
        public void PersonConverter_ConvertStringWithTooFewFields_ThrowsTooFewFieldsException(string toConvert) => personConverter.Convert((1, toConvert));

        [TestMethod]
        [ExpectedException(typeof(TooManyFieldsException))]
        [DataRow("a, b, c, d, e")]
        [DataRow("a, b, c, d, e, f")]
        [DataRow("a, b, c, d, e, f, g")]
        public void PersonConverter_ConvertStringWithTooManyFields_ThrowsTooManyFieldsException(string toConvert) => personConverter.Convert((1, toConvert));

        [TestMethod]
        [DataRow(1, "Test", "Tester", "00000", "Test City", "1")]
        [DataRow(2, "Just", "Another-Test", "11111", "Some City", "7")]
        public void PersonConverter_ConvertPerson_ProducesExpectedFormat(int id, string name, string lastName, string zipCode, string city, string colourCode)
        {
            IAddress address = addressConverter.Convert($"{zipCode} {city}");
            Color colour = colourConverter.Convert(colourCode);
            IPerson person = personBuilderFactory.Create()
                .WithID(id)
                .WithName(name)
                .WithLastName(lastName)
                .WithAddress(address)
                .WithFavouriteColour(colour)
                .Build();

            (int id, string data) converted = personConverter.Convert(person);

            Assert.AreEqual(id, converted.id);
            Assert.AreEqual($"{lastName}, {name}, {zipCode} {city}, {colourCode}", converted.data);
        }
    }
}