using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAPIDemo.Facades;
using Utils;
using Utils.Exceptions;

namespace RESTAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PersonsController : Controller
    {
        private readonly IPersonRepository repository;

        public PersonsController(IPersonRepository repository) => this.repository = repository;

        // GET api/persons
        [HttpGet]
        public OkObjectResult Get() => Ok(ApplyFacade(repository.GetAll()));

        // GET api/persons/5
        [HttpGet("{id}")]
        public ObjectResult Get(int id)
        {
            try
            {
                return Ok(ApplyFacade(repository.Get(id)));
            }
            catch (PersonDoesNotExistException doesntExist)
            {
                return NotFound(doesntExist.Message);
            }
            catch (AmbiguousIDException ambiguousID)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ambiguousID.Message);
            }
        }

        // GET api/persons/color/blue
        [HttpGet("color/{name}")]
        public ObjectResult Get(string name)
        {
            Color colour;
            try
            {
                colour = ColourMap.GetColourByName(name);
            }
            catch (InvalidColourNameException invalidName)
            {
                return BadRequest(invalidName.Message);
            }

            return Ok(ApplyFacade(repository.GetByFavouriteColour(colour)));
        }

        private IList<PersonFacade> ApplyFacade(IList<IPerson> persons) => persons.Select(ApplyFacade).ToList();
        private PersonFacade ApplyFacade(IPerson person) => new PersonFacade(person);
    }
}
