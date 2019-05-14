using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private const string NullIpAddress = "::1";

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            //AccountServiceClient serviceClient = new AccountServiceClient();
            //String pox = await serviceClient.GetAccounts<String>();
            var connection = Request.HttpContext.Connection;
            var address = connection.RemoteIpAddress;
            var remote = address != null && address.ToString() != NullIpAddress;

            var local = connection.LocalIpAddress;

            //return Ok(new string[] { "value1", "value2" });
            //return Ok(address.ToString());
            return Ok(local.ToString());
        }

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
