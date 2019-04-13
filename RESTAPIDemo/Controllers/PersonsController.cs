using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Contracts;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAPIDemo.Facades;
using Utils;

namespace RESTAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PersonsController : Controller
    {
        private readonly IPersonRepository repository;

        public PersonsController(IPersonRepository repository)
        {
            if (repository is null) throw new ArgumentNullException(nameof(repository));
            this.repository = repository;
        }

        // GET api/persons
        [HttpGet]
        public OkObjectResult Get() => Ok(ApplyAdapter(repository.GetAll()));

        // GET api/persons/5
        [HttpGet("{id}")]
        public ObjectResult Get(int id)
        {
            try
            {
                return Ok(ApplyAdapter(repository.Get(id)));
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

            return Ok(ApplyAdapter(repository.GetByFavouriteColour(colour)));
        }

        private IList<PersonJSONAdapter> ApplyAdapter(IList<IPerson> persons) => persons.Select(ApplyAdapter).ToList();
        private PersonJSONAdapter ApplyAdapter(IPerson person) => new PersonJSONAdapter(person);
    }
}
