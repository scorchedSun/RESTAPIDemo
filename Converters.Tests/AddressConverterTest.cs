using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Builders;
using System;
using System.Text.RegularExpressions;

namespace Converters.Tests
{
    [TestClass]
    public class AddressConverterTest
    {
        private readonly IWithZipCodeAddressBuilder addressBuilder;
        private readonly IConverter<string, IAddress> addressConverter;

        public AddressConverterTest()
        {
            addressBuilder = AddressBuilder.Create();
            addressConverter = new AddressConverter(addressBuilder);
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
        [ExpectedException(typeof(FormatException))]
        [DataRow("232223")]
        [DataRow("Test")]
        public void AddressConverter_ConvertStringWithOnePart_ThrowsFormatException(string value) => addressConverter.Convert(value);

        [TestMethod]
        [DataRow("12313", "City")]
        [DataRow("32232", "Another City")]
        public void AddressConverter_ConvertingAddress_ProducesExpectedFormat(string zipCode, string city)
        {
            IAddress address = addressBuilder
                .WithZipCode(zipCode)
                .WithCity(city)
                .Build();

            string converted = addressConverter.Convert(address);

            Assert.IsTrue(Regex.IsMatch(converted, @"\w+\s\w[\w\s]+"));
        }
    }
}
