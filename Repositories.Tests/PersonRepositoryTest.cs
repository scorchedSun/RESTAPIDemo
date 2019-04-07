using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Builders;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TestUtils;
using Utils.Exceptions;

namespace Repositories.Tests
{
    [TestClass]
    public class PersonRepositoryTest
    {
        private const int personID = 1;
        private const int invalidPersonID = -1;
        private readonly Color favouriteColour = Color.Blue;
        private readonly TestablePersonDataSource dataSource = new TestablePersonDataSource();

        public IPersonRepository CreateRepository()
        {
            dataSource.WriteAll(new List<IPerson> { CreateTestPerson(personID, favouriteColour), CreateTestPerson(2, Color.Black) });
            return new PersonRepository(dataSource);
        }

        [TestMethod]
        public void PersonRepository_LoadFromCSVDataSource_Succeeds()
        {
            IPersonRepository repository = CreateRepository();

            IList<IPerson> persons = repository.GetAll();

            Assert.AreEqual(dataSource.Persons, persons);
        }

        [TestMethod]
        public void PersonRepository_GetPersonByValidID_Succeeds()
        {
            IPersonRepository repository = CreateRepository();

            IPerson person = repository.Get(personID);

            Assert.IsNotNull(person);
        }

        [TestMethod]
        [ExpectedException(typeof(PersonDoesNotExistException))]
        public void PersonRepository_GetPersonByInvalidID_ThrowsPersonDoesNotExistException()
        {
            IPersonRepository repository = CreateRepository();
            repository.Get(invalidPersonID);
        }

        [TestMethod]
        public void PersonRepository_GetPersonsByFavouriteColour_Succeeds()
        {
            IPersonRepository repository = CreateRepository();

            IList<IPerson> persons = repository.GetByFavouriteColour(favouriteColour);

            Assert.AreEqual(1, persons.Count);
            Assert.AreEqual(personID, persons.First().ID);
        }

        private IAddress CreateTestAddress()
        {
            IAddressBuilder addressBuilder = AddressBuilder.Create();
            return addressBuilder
                .WithZipCode("32423")
                .WithCity("Test City")
                .Build();
        }

        private IPerson CreateTestPerson(int id, Color colour)
        {
            IPersonBuilder personBuilder = PersonBuilder.Create();
            return personBuilder
                .WithID(id)
                .WithName("test")
                .WithLastName("tester")
                .WithAddress(CreateTestAddress())
                .WithFavouriteColour(colour)
                .Build();
        }
    }
}
