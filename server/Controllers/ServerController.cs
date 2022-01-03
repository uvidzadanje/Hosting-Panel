using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.Models;
using server.Helpers;
using Microsoft.Extensions.Configuration;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        public HostingContext Context { get; set; }
        private Auth auth;

        public ServerController(IConfiguration configuration, HostingContext context)
        {
            this.auth = new Auth(configuration);
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
                    new{
                        servers = await Context
                                .Servers
                                .ToListAsync()
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message});
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
                    new {
                        server = await Context
                                .Servers
                                .Where(s => s.ID == id)
                                .Include(s => s.Datacenter)
                                .FirstOrDefaultAsync()
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(new {error = e.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] Server server)
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized("Unauthentificated user");

            if(token["priority"] != 0) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to add server!"});

            try
            {
                 Context.Servers.Add(server);
                 await Context.SaveChangesAsync();
                 return Ok(new {message = "server successfully added!"});
            }
            catch (Exception e)
            {
                return BadRequest(new {error = e.Message});
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromBody] Server server)
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized("Unauthentificated user");

            if(token["priority"] != 0) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to update server!"});

            try
            {
                 Context.Servers.Update(server);
                 await Context.SaveChangesAsync();
                 return Ok(new {message = $"Server with IP {server.IPAdress} has been changed!"});
            }
            catch (Exception e)
            {
                return BadRequest(new {error = e.Message});
            }
        }

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete(int id)
        {

            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "Unauthentificated user"});

            if(token["priority"] != 0) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to delete datacenter!"});

            if(id <= 0) return BadRequest(new { error = "Wrong ID!"});

            try
            {
                 var server = await Context.Servers.FindAsync(id);
                 string serverIP = server.IPAdress;
                 Context.Servers.Remove(server);
                 await Context.SaveChangesAsync();
                 return Ok(new { message = $"Server on IP {serverIP} has been removed!"});
            }
            catch (Exception e)
            {
                return BadRequest(new {error = e.Message});
            }
        } 
    }
}
