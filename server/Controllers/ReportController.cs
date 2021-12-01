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
    public class ReportController : ControllerBase
    {
        public HostingContext Context { get; set; }

        private Auth auth;

        public ReportController(IConfiguration configuration, HostingContext context)
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
                    await Context
                    .Reports
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Show(int id, [FromHeader] string authorization)
        {

            var token = auth.ValidateJwtToken(authorization);

            if(token == null) return Unauthorized("Unauthentificated user");

            try
            {
                var report = await Context
                    .Reports
                    .Include(r => r.User)
                    .Where(r => r.ID == id)
                    .FirstOrDefaultAsync();


                if(token["priority"] != 0 && token["id"] != report.User.ID) return Forbid("You dont have permission to show report!");

                return Ok(
                    new {
                        report = report
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(new {
                    error = e.Message
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Create([FromBody] Report report, [FromHeader] string authorization)
        {

            var token = auth.ValidateJwtToken(authorization);

            if(token == null) return Unauthorized("Unauthentificated user");

            if(token["priority"] != 1) return Forbid("You dont have permission to add report!");

            if(String.IsNullOrEmpty(report.Description)) return BadRequest("Description is required");

            try
            {
                 Context.Reports.Add(report);
                 await Context.SaveChangesAsync();
                 return Ok("Report successfully added!");
            }
            catch (Exception e)
            {
                return BadRequest( new {
                    error = e.Message
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromBody] Report report, [FromHeader] string authorization)
        {


            var token = auth.ValidateJwtToken(authorization);

            if(token == null) return Unauthorized("Unauthentificated user");

            if(token["id"] == report.User.ID) return Forbid("You dont have permission to add datacenter!");

            if(String.IsNullOrEmpty(report.Description)) return BadRequest("Description is required");

            try
            {
                 Context.Reports.Update(report);
                 await Context.SaveChangesAsync();
                 return Ok($"Report with ID {report.ID} has been changed!");
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
        public async Task<ActionResult> Delete(int id, [FromHeader] string authorization)
        {

            var token = auth.ValidateJwtToken(authorization);

            if(token == null) return Unauthorized("Unauthentificated user");

            if(id <= 0) return BadRequest("Wrong ID!");

            try
            {
                var report = await Context.Reports.Include(r=> r.User).FirstOrDefaultAsync();
                
                if(token["id"] != report.User.ID) return Forbid("You don't have permission to delete report!");

                int reportID = report.ID;
                Context.Reports.Remove(report);
                await Context.SaveChangesAsync();
                return Ok($"Report with ID {reportID} has been removed!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
    }
}
