using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        public HostingContext Context { get; set; }

        public ServerController(HostingContext context)
        {
            Context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Index()
        {
            try
            {
                return Ok(
                    await Context
                    .Servers
                    .Include(s => s.Datacenter)
                    .ToListAsync()
                );    
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Show(int id)
        {
            try
            {
                 return Ok(
                    await Context
                    .Servers
                    .Where(s => s.ID == id)
                    .Include(s => s.Datacenter)
                    .ToListAsync()
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] Server server)
        {

            try
            {
                 Context.Servers.Add(server);
                 await Context.SaveChangesAsync();
                 return Ok("server successfully added!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update([FromBody] Server server)
        {

            try
            {
                 Context.Servers.Update(server);
                 await Context.SaveChangesAsync();
                 return Ok($"Server with IP {server.ID} has been changed!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            if(id <= 0) return BadRequest("Wrong ID!");

            try
            {
                 var server = await Context.Servers.FindAsync(id);
                 string serverIP = server.IPAdress;
                 Context.Servers.Remove(server);
                 await Context.SaveChangesAsync();
                 return Ok($"Server on IP {serverIP} has been removed!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
    }
}
