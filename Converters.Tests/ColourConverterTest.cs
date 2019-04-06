using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Converters.Tests
{
    [TestClass]
    public class ColourConverterTest
    {
        [TestMethod]
        [DataRow("1")]
        [DataRow("2")]
        [DataRow("3")]
        [DataRow("4")]
        [DataRow("5")]
        [DataRow("6")]
        [DataRow("7")]
        public void ColourConverter_ConvertSupportedCode_Succeeds(string code)
        {
            ColourConverter converter = new ColourConverter();

            Color colour = converter.Convert(code);

            Assert.AreEqual(converter.ColourMap[code], colour);
        }

        [TestMethod]
        [DataRow("999")]
        [DataRow("Potato")]
        [ExpectedException(typeof(InvalidColourCodeException))]
        public void ColourConverter_ConvertUnsopportedCode_ThrowsInvalidColourCodeException(string code)
        {
            ColourConverter converter = new ColourConverter();

            converter.Convert(code);
        }

        [TestMethod]
        public void ColourConverter_ConvertSupportedColour_Succeeds()
        {
            Color colour = Color.Blue;
            ColourConverter converter = new ColourConverter();
            string expectedCode = converter.ColourMap.Single(kvp => kvp.Value.Equals(colour)).Key;

            string converted = converter.Convert(colour);

            Assert.AreEqual(expectedCode, converted);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsupportedColourException))]
        public void ColourConverter_ConvertUnsupportedColour_ThrowsUnsupportedColourException()
        {
            ColourConverter converter = new ColourConverter();

            converter.Convert(Color.Beige);
        }
    }
}
