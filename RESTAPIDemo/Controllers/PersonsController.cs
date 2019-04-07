using System.Collections.Generic;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : Controller
    {
        private readonly IPersonRepository repository;

        public PersonsController(IPersonRepository repository) => this.repository = repository;

        // GET api/values
        [HttpGet]
        public OkObjectResult Get() => Ok(Json(repository.GetAll()));

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
