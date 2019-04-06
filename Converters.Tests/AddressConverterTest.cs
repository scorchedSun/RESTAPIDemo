using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Text.RegularExpressions;

namespace Converters.Tests
{
    [TestClass]
    public class AddressConverterTest
    {
        [TestMethod]
        [DataRow("56784", "City")]
        [DataRow("00000", "Another City")]
        public void AddressConverter_ConvertStringWithTwoPartsSeparatedByWhitespace_Succeeds(string zipCode, string city)
        {
            AddressConverter converter = new AddressConverter();

            IAddress address = converter.Convert($"{zipCode} {city}");

            Assert.AreEqual(zipCode, address.ZipCode);
            Assert.AreEqual(city, address.City);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [DataRow("232223")]
        [DataRow("Test")]
        public void AddressConverter_ConvertStringWithOnePart_ThrowsFormatException(string value)
        {
            AddressConverter converter = new AddressConverter();
            converter.Convert(value);
        }

        [TestMethod]
        [DataRow("12313", "City")]
        [DataRow("32232", "Another City")]
        public void AddressConverter_ConvertingAddress_ProducesExpectedFormat(string zipCode, string city)
        {
            AddressConverter converter = new AddressConverter();
            IAddress address = new Address(zipCode, city);

            string converted = converter.Convert(address);

            Assert.IsTrue(Regex.IsMatch(converted, @"\w+\s\w[\w\s]+"));
        }
    }
}
