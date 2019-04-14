using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Models.Tests
{
    [TestClass]
    public class PersonBuilderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PersonBuilder_PassInvalidID_ThrowsArgumentOutOfRangeException()
        {
            new PersonBuilderFactory().Create().WithID(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PersonBuilder_PassNullAsName_ThrowsArgumentNullException()
        {
            GetBuilderWithValidID().WithName(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PersonBuilder_PassStringEmptyAsName_ThrowsArgumentException()
        {
            GetBuilderWithValidID().WithName(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PersonBuilder_PassNullAsLastName_ThrowsArgumentNullException()
        {
            GetBuilderWithValidName().WithLastName(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PersonBuilder_PassStringEmptyAsLastName_ThrowsArgumentException()
        {
            GetBuilderWithValidName().WithLastName(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PersonBuilder_PassNullAsAddress_ThrowsArgumentNullExcpetion()
        {
            GetBuilderWithValidLastName().WithAddress(null);
        }

        [TestMethod]
        [DataRow(1u, "test", "tester", "00000", "city")]
        [DataRow(2u, "test 2", "mc Test", "000233 ff", "city of cities")]
        public void PersonBuilder_WithValidInputs_CanBuildPerson(uint id, string name, string lastName, string zipCode, string city)
        {
            IAddress address = new AddressBuilderFactory().Create().WithZipCode(zipCode).WithCity(city).Build();
            IPerson person = new PersonBuilderFactory().Create()
                .WithID(id)
                .WithName(name)
                .WithLastName(lastName)
                .WithAddress(address)
                .WithFavouriteColour(Color.Transparent)
                .Build();

            Assert.AreEqual(id, person.ID);
            Assert.AreEqual(name, person.Name);
            Assert.AreEqual(lastName, person.LastName);
            Assert.AreEqual(zipCode, person.Address.ZipCode);
            Assert.AreEqual(city, person.Address.City);
            Assert.AreEqual(Color.Transparent, person.FavouriteColour);
        }

        [TestMethod]
        public void PersonBuilder_Reuse_NotPossible()
        {
            IFinalPersonBuilder personBuilder = GetFinalPersonBuilder();
            IPerson person = personBuilder.Build();
            IPerson person2 = personBuilder.Build();

            Assert.IsNull(person2);
            Assert.AreNotSame(person, person2);
        }

        private IPersonWithNameBuilder GetBuilderWithValidID() => new PersonBuilderFactory().Create().WithID(1);
        private IPersonWithLastNameBuilder GetBuilderWithValidName() => GetBuilderWithValidID().WithName("Test");
        private IPersonWithAddressBuilder GetBuilderWithValidLastName() => GetBuilderWithValidName().WithLastName("Tester");
        private IFinalPersonBuilder GetFinalPersonBuilder() =>
            GetBuilderWithValidLastName().WithAddress(new AddressBuilderFactory().Create().WithZipCode("1").WithCity("T").Build()).WithFavouriteColour(Color.Transparent);
    }
}
