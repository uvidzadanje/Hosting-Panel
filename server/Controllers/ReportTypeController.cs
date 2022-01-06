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
    public class ReportTypeController : ControllerBase
    {
        public HostingContext Context { get; set; }

        public ReportTypeController(HostingContext context)
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetNumOfReportsByReportType()
        {
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
