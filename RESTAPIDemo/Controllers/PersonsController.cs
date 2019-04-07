using System.Collections.Generic;
using System.Linq;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAPIDemo.Facades;
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
        public OkObjectResult Get() => Ok(repository.GetAll().Select(person => new PersonFacade(person)));

        // GET api/values/5
        [HttpGet("{id}")]
        public ObjectResult Get(int id)
        {
            try
            {
                return Ok(Json(new PersonFacade(repository.Get(id))));
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
