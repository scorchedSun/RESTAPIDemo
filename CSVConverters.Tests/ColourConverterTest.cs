using Utils.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Utils;
using Contracts;

namespace CSVConverters.Tests
{
    [TestClass]
    public class ColourConverterTest
    {
        [TestMethod]
        public void ColourConverter_ConvertSupportedCode_Succeeds()
        {
            IConverter<string, Color> converter = new ColourConverter();

            foreach (string code in ColourMap.GetAllCodes())
            {
                Color colour = converter.Convert(code);

                Assert.AreEqual(ColourMap.GetColourFor(code), colour);
            }
        }

        [TestMethod]
        [DataRow("999")]
        [DataRow("Potato")]
        [ExpectedException(typeof(InvalidColourCodeException))]
        public void ColourConverter_ConvertUnsopportedCode_ThrowsInvalidColourCodeException(string code)
        {
            IConverter<string, Color> converter = new ColourConverter();

            converter.Convert(code);
        }

        [TestMethod]
        public void ColourConverter_ConvertSupportedColour_Succeeds()
        {
            Color colour = Color.Blue;
            IConverter<string, Color> converter = new ColourConverter();
            string expectedCode = ColourMap.GetCodeFor(colour);

            string converted = converter.Convert(colour);

            Assert.AreEqual(expectedCode, converted);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsupportedColourException))]
        public void ColourConverter_ConvertUnsupportedColour_ThrowsUnsupportedColourException()
        {
            IConverter<string, Color> converter = new ColourConverter();

            converter.Convert(Color.Beige);
        }
    }
}