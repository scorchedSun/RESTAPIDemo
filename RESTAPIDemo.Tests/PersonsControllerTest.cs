using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositories;
using RESTAPIDemo.Controllers;
using TestUtils;
using Utils;

namespace RESTAPIDemo.Tests
{
    [TestClass]
    public class PersonsControllerTest
    {
        private readonly TestablePersonDataSource dataSource = TestablePersonDataSource.Create();
        private readonly IPersonRepository repository;
        private readonly PersonsController controller;

        public PersonsControllerTest()
        {
            repository = new PersonRepository(dataSource);
            controller = new PersonsController(repository)
            {
                Logger = new TestableLogger()
            };
        }

        [TestMethod]
        public void PersonsController_GetAllPersons_ReturnsOk()
        {
            ObjectResult result = controller.Get();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PersonsController_GetPersonByValidID_ReturnsOk()
        {
            ObjectResult result = controller.Get(TestablePersonDataSource.ExistingID);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PersonsController_GetPersonByInvalidID_ReturnsNotFound()
        {
            ObjectResult result = controller.Get(TestablePersonDataSource.InvalidID);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void PersonsController_GetPersonByAmbiguousID_ReturnsInternalServerError()
        {
            ObjectResult result = controller.Get(TestablePersonDataSource.AmbiguousID);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.AreEqual(1, ((TestableLogger)controller.Logger).Errors.Count);
        }

        [TestMethod]
        public void PersonsController_GetPersonsByColour_ReturnsOk()
        {
            ObjectResult result = controller.Get(TestablePersonDataSource.ValidColour.Name.ToLower());
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PersonsController_GetPersonsByUnsupportedColour_ReturnsBadRequest()
        {
            ObjectResult result = controller.Get(TestablePersonDataSource.InvalidColour.Name.ToLower());
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
