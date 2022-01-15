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
    public class ReportTypeController : ControllerBase
    {
        public HostingContext Context { get; set; }
        private Auth auth;

        public ReportTypeController(IConfiguration configuration, HostingContext context)
        {
            Context = context;
            auth  = new Auth(configuration);
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
                        reportTypes = await Context
                                .ReportTypes
                                .ToListAsync()
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(new {error = e.Message});
            }
        }

        [Route("Stats")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetNumOfReportsByReportType()
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);
            if(token == null) return Unauthorized(new {error = "You must to login!"});

            if(token["priority"] != 0) return StatusCode(403, new {error= "You don't have permission for that!"});

            try
            {
                var reportTypes= await Context
                                .ReportTypes
                                .Include(rt => rt.Reports)
                                .ToListAsync();
                return Ok(
                    new {
                        reportTypes = reportTypes
                                        .Select(rt => new {
                                            Name = rt.Name,
                                            ReportsNum = rt.Reports.Count
                                        })
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(new {error = e.Message});
            }
        }
    }
}
