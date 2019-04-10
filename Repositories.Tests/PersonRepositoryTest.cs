using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TestUtils;
using Utils.Exceptions;

namespace Repositories.Tests
{
    [TestClass]
    public class PersonRepositoryTest
    {
        private readonly TestablePersonDataSource dataSource = TestablePersonDataSource.Create();

        [TestMethod]
        public void PersonRepository_LoadFromCSVDataSource_Succeeds()
        {
            IPersonRepository repository = new PersonRepository(dataSource);

            IList<IPerson> persons = repository.GetAll();

            Assert.AreEqual(dataSource.Persons, persons);
        }

        [TestMethod]
        public void PersonRepository_GetPersonByValidID_Succeeds()
        {
            IPersonRepository repository = new PersonRepository(dataSource);

            IPerson person = repository.Get(TestablePersonDataSource.ExistingID);

            Assert.IsNotNull(person);
        }

        [TestMethod]
        [ExpectedException(typeof(PersonDoesNotExistException))]
        public void PersonRepository_GetPersonByInvalidID_ThrowsPersonDoesNotExistException()
        {
            IPersonRepository repository = new PersonRepository(dataSource);
            repository.Get(TestablePersonDataSource.InvalidID);
        }

        [TestMethod]
        public void PersonRepository_GetPersonsByFavouriteColour_Succeeds()
        {
            IPersonRepository repository = new PersonRepository(dataSource);

            IList<IPerson> persons = repository.GetByFavouriteColour(TestablePersonDataSource.ValidColour);

            Assert.AreEqual(1, persons.Count);
            Assert.AreEqual(TestablePersonDataSource.ExistingID, persons[0].ID);
        }
    }
}
