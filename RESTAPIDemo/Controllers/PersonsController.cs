using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Contracts;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ninject;
using RESTAPIDemo.Facades;
using Utils;

namespace RESTAPIDemo.Controllers
{
    /// <summary>
    /// API endpoint regarding <see cref="IPerson"/>s.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PersonsController : Controller
    {
        private readonly IPersonRepository repository;

        [Inject]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Create a new <see cref="PersonsController"/>.
        /// </summary>
        /// <param name="repository">The <see cref="IPersonRepository"/> to use</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed.</exception>
        public PersonsController(IPersonRepository repository)
        {
            if (repository is null) throw new ArgumentNullException(nameof(repository));
            this.repository = repository;
        }

        // GET api/persons
        [HttpGet]
        public ObjectResult Get()
        {
            try
            {
                return Ok(ApplyAdapter(repository.GetAll()));
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/persons/5
        [HttpGet("{id}")]
        public ObjectResult Get(uint id)
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
                Logger.Log(ambiguousID);
                return StatusCode(StatusCodes.Status500InternalServerError, ambiguousID.Message);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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

            try
            {
                return Ok(ApplyAdapter(repository.GetByFavouriteColour(colour)));
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private IList<PersonJSONAdapter> ApplyAdapter(IList<IPerson> persons) => persons.Select(ApplyAdapter).ToList();
        private PersonJSONAdapter ApplyAdapter(IPerson person) => new PersonJSONAdapter(person);
    }
}
