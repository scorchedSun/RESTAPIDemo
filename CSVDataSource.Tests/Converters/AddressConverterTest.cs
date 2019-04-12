using Contracts;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Factories;
using System.Text.RegularExpressions;

namespace CSVDataSource.Converters.Tests
{
    [TestClass]
    public class AddressConverterTest
    {
        private readonly IAddressBuilderFactory addressBuilderFactory;
        private readonly IConverter<string, IAddress> addressConverter;

        public AddressConverterTest()
        {
            addressBuilderFactory = new AddressBuilderFactory();
            addressConverter = new AddressConverter(addressBuilderFactory);
        }

        [TestMethod]
        [DataRow("56784", "City")]
        [DataRow("00000", "Another City")]
        public void AddressConverter_ConvertStringWithTwoPartsSeparatedByWhitespace_Succeeds(string zipCode, string city)
        {
            IAddress address = addressConverter.Convert($"{zipCode} {city}");

            Assert.AreEqual(zipCode, address.ZipCode);
            Assert.AreEqual(city, address.City);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormattedAddressException))]
        [DataRow("232223")]
        [DataRow("Test")]
        [DataRow("")]
        public void AddressConverter_ConvertStringWithOnePart_ThrowsInvalidFormattedAddressException(string value) => addressConverter.Convert(value);

        [TestMethod]
        [DataRow("12313", "City")]
        [DataRow("32232", "Another City")]
        public void AddressConverter_ConvertingAddress_ProducesExpectedFormat(string zipCode, string city)
        {
            IAddress address = addressBuilderFactory.Create()
                .WithZipCode(zipCode)
                .WithCity(city)
                .Build();

            string converted = addressConverter.Convert(address);

            Assert.IsTrue(Regex.IsMatch(converted, @"\w+\s\w[\w\s]+"));
        }
    }
}