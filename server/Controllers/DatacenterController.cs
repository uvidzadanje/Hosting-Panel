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
    public class DatacenterController : ControllerBase
    {
        public HostingContext Context { get; set; }

        public DatacenterController(HostingContext context)
        {
            Context = context;
        }

        // [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Index()
        {
            try
            {
                return Ok(
                    await Context
                    .Datacenters
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
                    .Datacenters
                    .Where(dc => dc.ID == id)
                    .ToListAsync()
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] Datacenter datacenter)
        {
            if(datacenter.Name.Length > 70) return BadRequest("Name is too long");

            try
            {
                 Context.Datacenters.Add(datacenter);
                 await Context.SaveChangesAsync();
                 return Ok("Datacenter successfully added!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update([FromBody] Datacenter datacenter)
        {
            if(datacenter.Name.Length > 70) return BadRequest("Name is too long");

            try
            {
                 Context.Datacenters.Update(datacenter);
                 await Context.SaveChangesAsync();
                 return Ok($"Student with ID {datacenter.ID} has been changed!");
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
                 var datacenter = await Context.Datacenters.FindAsync(id);
                 string datacenterName = datacenter.Name;
                 Context.Datacenters.Remove(datacenter);
                 await Context.SaveChangesAsync();
                 return Ok($"Datacenter {datacenterName} has been removed!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
    }
}
