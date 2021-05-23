using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaundryTime.Utilities.SignalRHubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LaundryTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        // GET: api/<MachineController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MachineController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MachineController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MachineController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MachineController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        //SignalR Machine controle
        [HttpPost("ReceiveMachineUpdate")]
        [Route("api/[controller]/{status:bool}/{machineId:int}")]
        public async void ReceiveMachineUpdate(bool status, int machineId)
        {
            var machineHub = new MachineHub();

            StringBuilder tekst = new StringBuilder();
            tekst.Append($"{status},{machineId}");

            JsonSerializer seri = new JsonSerializer();
            var jsonmsg = JsonConvert.SerializeObject(tekst);

            await machineHub.Clients.All.SendAsync(jsonmsg);
        }
    }
}
