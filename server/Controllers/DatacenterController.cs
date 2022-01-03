using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using server.Models;
using server.Helpers;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatacenterController : ControllerBase
    {
        public HostingContext Context { get; set; }

        private Auth auth;

        public DatacenterController(IConfiguration configuration, HostingContext context)
        {
            auth = new Auth(configuration);
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
                    new {
                        datacenters = await Context
                        .Datacenters
                        .ToListAsync()
                    }
                );    
            }
            catch (Exception e)
            {
                return BadRequest(
                    new {
                        error = e.Message
                    });
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
                        datacenter = await Context
                        .Datacenters
                        .Where(dc => dc.ID == id)
                        .ToListAsync()
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(
                    new {
                        error = e.Message
                    });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] Datacenter datacenter)
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "Unauthentificated user"});

            if(token["priority"] != 0) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to add datacenter!"});

            if(datacenter.Name.Length > 70) return BadRequest(new {error = "Name is too long"});

            try
            {
                 Context.Datacenters.Add(datacenter);
                 await Context.SaveChangesAsync();
                 return Ok(new {
                     message = "Datacenter successfully added!"
                 });
            }
            catch (Exception e)
            {
                return BadRequest(
                    new {
                        error = e.Message
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromBody] Datacenter datacenter)
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "Unauthentificated user"});

            if(token["priority"] != 0) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to add datacenter!"});

            if(datacenter.Name.Length > 70) return BadRequest(new {error = "Name is too long"});

            try
            {
                 Context.Datacenters.Update(datacenter);
                 await Context.SaveChangesAsync();
                 return Ok(new {
                     message = $"Datacenter with ID {datacenter.ID} has been changed!"
                 });
            }
            catch (Exception e)
            {
                return BadRequest(new {
                    error = e.Message
                });
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

            if(token["priority"] != 0) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to add datacenter!"});

            if(id <= 0) return BadRequest(new {error = "Wrong ID!"});

            try
            {
                var datacenter = await Context.Datacenters.FindAsync(id);
                string datacenterName = datacenter.Name;
                Context.Datacenters.Remove(datacenter);
                await Context.SaveChangesAsync();
                return Ok(
                    new {
                        message = $"Datacenter {datacenterName} has been removed!" 
                });
            }
            catch (Exception e)
            {
                return BadRequest(
                    new{
                        error = e.Message
                });
            }
        } 
    }
}
