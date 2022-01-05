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
        public async Task<ActionResult> Show(int id)
        {

            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "Unauthentificated user"});

            try
            {
                var report = await Context
                    .Reports
                    .Include(r => r.User)
                    .Include(r => r.Server)
                    .Include(r => r.ReportType)
                    .Where(r => r.ID == id)
                    .FirstOrDefaultAsync();

                if(token["priority"] != 0 && token["id"] != report.User.ID) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to show report!"});

                if(report == null) return NotFound(new {error = "File not found"});

                return Ok(
                    new {
                        report = new {
                            ID = report.ID,
                            Description = report.Description,
                            CreatedAt = report.CreatedAt,
                            IsSolved = report.IsSolved,
                            User = new {
                                FullName = report.User.FullName
                            },
                            ReportType = new {
                                ID = report.ReportType.ID,
                                Name = report.ReportType.Name
                            },
                            Server = new {
                                ID = report.Server.ID,
                                IPAddress = report.Server.IPAddress
                            }
                        }
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

        [Route("User")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ShowByUserSession()
        {

            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "Unauthentificated user"});

            try
            {
                var reports = await Context
                    .Reports
                    .Include(r => r.User)
                    .Include(r => r.ReportType)
                    .Include(r => r.Server)
                    .Where(r => r.User.ID == token["id"] || token["priority"]==0)
                    .OrderBy(r => r.IsSolved)
                    .OrderBy(r=> r.CreatedAt)
                    .ToListAsync();
                
                if(reports == null) throw new Exception("Reports is empty");

                return Ok(
                    new {
                        reports = reports.Select(r => new {
                            ID = r.ID,
                            Description = HelperFunctions.TruncateLongString(r.Description, 100),
                            CreatedAt = r.CreatedAt,
                            IsSolved = r.IsSolved,
                            User = new { FullName = r.User.FullName},
                            ReportType = new { Name = r.ReportType.Name},
                            Server = new { IPAddress =  r.Server.IPAddress}
                        })
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
        public async Task<ActionResult> Create([FromBody] Report report)
        {

            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "Unauthentificated user"});

            if(token["priority"] != 1) return StatusCode(StatusCodes.Status403Forbidden, new {error = "You dont have permission to add report!"});

            if(String.IsNullOrEmpty(report.Description)) return BadRequest(new {error = "Description is required"});

            try
            {
                report.CreatedAt = DateTime.Now.Date;
                report.User = await Context
                                .Users
                                .Where(u=> u.ID == token["id"])
                                .FirstOrDefaultAsync();
                report.ReportType = await Context
                                .ReportTypes
                                .Where(rt=> rt.ID == report.ReportType.ID)
                                .FirstOrDefaultAsync();
                report.Server = await Context
                                .Servers
                                .Where(s=> s.ID == report.Server.ID)
                                .FirstOrDefaultAsync();
                                
                Context.Reports.Add(report);
                await Context.SaveChangesAsync();
                return Ok(new {message = "Report successfully added!", id = report.ID});
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
        public async Task<ActionResult> Update([FromBody] Report report)
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "Unauthentificated user"});

            Report reportFromDB = await Context
                                    .Reports
                                    .Include(r => r.User)
                                    .Where(r=> r.ID == report.ID)
                                    .FirstOrDefaultAsync();

            if(reportFromDB == null) return NotFound(new {error = "Report not found"});
            
            if(token["id"] != reportFromDB.User.ID) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You dont have permission to update report!"});

            if(String.IsNullOrEmpty(report.Description)) return BadRequest(new {error = "Description is required"});

            reportFromDB.Description = report.Description;

            if(report.Server != null) 
                reportFromDB.Server = await Context
                                            .Servers
                                            .Where(s => s.ID == report.Server.ID)
                                            .FirstOrDefaultAsync();

            if(report.ReportType != null)
                reportFromDB.ReportType = await Context
                                                .ReportTypes
                                                .Where(r => r.ID == report.ReportType.ID)
                                                .FirstOrDefaultAsync();
            try
            {
                 Context.Reports.Update(reportFromDB);
                 await Context.SaveChangesAsync();
                 return Ok(new {message = $"Report with ID {reportFromDB.ID} has been changed!"});
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

            if(id <= 0) return BadRequest(new {error = "Wrong ID!"});

            try
            {
                var report = await Context
                                    .Reports
                                    .Include(r=> r.User)
                                    .Where(r=> r.ID == id)
                                    .FirstOrDefaultAsync();

                if(report == null) throw new Exception("User doesn't exist!");

                if(token["priority"] != 0 && token["id"] != report.User.ID) return StatusCode(StatusCodes.Status403Forbidden ,new{error = "You don't have permission to delete that report!"});

                int reportID = report.ID;
                Context.Reports.Remove(report);
                await Context.SaveChangesAsync();
                return Ok(new {message = $"Report with ID {reportID} has been removed!"});
            }
            catch (Exception e)
            {
                return BadRequest(new {error = e.Message});
            }
        } 
    }
}
