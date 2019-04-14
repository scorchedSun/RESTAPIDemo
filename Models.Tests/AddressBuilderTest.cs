using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Factories;
using System;

namespace Models.Tests
{
    [TestClass]
    public class AddressBuilderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddressBuilder_PassNullAsZipCode_ThrowsArgumentNullException()
        {
            new AddressBuilderFactory().Create().WithZipCode(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddressBuilder_PassEmptyStringAsZipCode_ThrowsArgumentException()
        {
            new AddressBuilderFactory().Create().WithZipCode(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddressBuilder_PassNullAsCity_ThrowsArgumentNullException()
        {
            GetAddressBuilderForCity().WithCity(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddressBuilder_PassEmptyStringAsCity_ThrowsArgumentNullException()
        {
            GetAddressBuilderForCity().WithCity(string.Empty);
        }

        [TestMethod]
        [DataRow("23323", "Test City")]
        [DataRow("2323323", "Some other city")]
        [DataRow("There really is no constraint on zip codes in here", "neither is on cities")]
        public void AddressBuilder_WithValidInputs_CanBuildAddress(string zipCode, string city)
        {
            IAddress address = new AddressBuilderFactory().Create().WithZipCode(zipCode).WithCity(city).Build();

            Assert.AreEqual(zipCode, address.ZipCode);
            Assert.AreEqual(city, address.City);
        }

        [TestMethod]
        public void AddressBuilder_Reuse_NotPossible()
        {
            IFinalAddressBuilder addressBuilder = GetFinalAddressBuilder();
            IAddress address = addressBuilder.Build();
            IAddress address2 = addressBuilder.Build();

            Assert.IsNull(address2);
            Assert.AreNotSame(address, address2);
        }

        private IAddressWithCityBuilder GetAddressBuilderForCity() => new AddressBuilderFactory().Create().WithZipCode("93223");
        private IFinalAddressBuilder GetFinalAddressBuilder() => GetAddressBuilderForCity().WithCity("Test");
    }
}
