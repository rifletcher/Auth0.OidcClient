using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    public class Values2Controller : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "2.0-value1", "2.0-value2" };
        }

        // GET api/values/5
        [Authorize("read:user")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "2.0-value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
