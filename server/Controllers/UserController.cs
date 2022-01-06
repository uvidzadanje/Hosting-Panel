using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.Models;
using Microsoft.Extensions.Configuration;
using server.Helpers;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private Auth auth;
        public HostingContext Context { get; set; }

        public UserController(IConfiguration configuration, HostingContext context)
        {
            this.auth = new Auth(configuration);
            Context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ShowUserFromSession()
        {
            try
            {
                var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

                return Ok(
                    new{
                        user = (await Context
                        .Users
                        .Where(u=> u.ID == token["id"])
                        .ToListAsync())
                        .Select(u => new {
                             ID = u.ID,
                             Username = u.Username,
                             FullName = u.FullName,
                             Priority = u.Priority
                         })
                         .FirstOrDefault()
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

        // [HttpGet]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<ActionResult> Index()
        // {
        //     try
        //     {
        //         return Ok(
        //             new{
        //                 users = (await Context
        //                 .Users
        //                 .ToListAsync())
        //                 .Select(u => new {
        //                      ID = u.ID,
        //                      Username = u.Username,
        //                      FullName = u.FullName,
        //                      Priority = u.Priority
        //                  })
        //             }
        //         );    
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(
        //             new {
        //             error = e.Message
        //         });
        //     }
        // }

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
                         user = (await Context
                         .Users
                         .Where(u => u.ID == id)
                         .ToListAsync())
                         .Select(u => new {
                             ID = u.ID,
                             Username = u.Username,
                             FullName = u.FullName,
                             Priority = u.Priority
                         })
                         .FirstOrDefault()
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
        public async Task<ActionResult> Create([FromBody] User user)
        {
            if(String.IsNullOrEmpty(user.FullName) || String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.Password)) 
                return BadRequest(new {error = "Some of fields are empty!"});  
            if(user.FullName.Length > 50) return BadRequest(new {error = "Fullname is too long"});
            if(user.Username.Length > 25) return BadRequest(new {error = "Username is too long"});

            try
            {
                var users = await Context
                            .Users
                            .Where(u => u.Username == user.Username)
                            .ToListAsync();

                if(users.Count != 0) return BadRequest(new {error = "User with this username already exist!"});

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                Context.Users.Add(user);
                await Context.SaveChangesAsync();

                string token = this.auth.GenerateJwtToken(user);

                return Ok(new {
                    message = "User successfully added!",
                    token = token
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {
                    error = e.Message
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update([FromBody] User user)
        {

            try
            {
                var token = this.auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

                if(token == null) return  Unauthorized(new {error = "You must to login!"});

                User userFromDB = await Context
                                    .Users
                                    .Where(u=> u.ID == token["id"])
                                    .FirstOrDefaultAsync();

                if(!String.IsNullOrEmpty(user.Password))
                {
                    if(user.FullName.Length > 50) return BadRequest("Fullname is too long");
                    userFromDB.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }
                if(!String.IsNullOrEmpty(user.FullName)) userFromDB.FullName = user.FullName;

                Context.Users.Update(userFromDB);
                await Context.SaveChangesAsync();

                return Ok(
                    new{
                        message = $"User with ID {userFromDB.ID} has been changed!"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {
                    error = e.Message
                });
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete()
        {
            var token = this.auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return Unauthorized(new {error = "You must to login!"});

            if(token["id"] <= 0) return BadRequest(new {error = "Wrong ID!"});

            try
            {
                var user = await Context.Users.FindAsync(token["id"]);

                if(user == null) throw new Exception("User doesn't exist!");

                var reports = await Context
                                .Reports
                                .Where(r => r.User.ID == user.ID)
                                .ToListAsync();

                foreach(var report in reports)
                {
                    Context.Reports.Remove(report);
                    await Context.SaveChangesAsync();
                }

                var serverRelations = await Context
                                        .UserServers
                                        .Where(us => us.User.ID == user.ID)
                                        .ToListAsync();

                foreach(var serverRelation in serverRelations)
                {
                    Context.UserServers.Remove(serverRelation);
                    await Context.SaveChangesAsync();
                }

                string userName = user.Username;
                Context.Users.Remove(user);
                await Context.SaveChangesAsync();
                HttpContext.Response.Cookies.Delete("token");
                return Ok(
                    new {
                        message = $"User {userName} has been removed!"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {
                    error = e.Message
                });
            }
        }

        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Login([FromBody] Login loginData)
        {
            if(String.IsNullOrEmpty(loginData.Username) || String.IsNullOrEmpty(loginData.Password)) return BadRequest(new {error = "Username and password are required!"});

            try
            {
                var user = await Context.Users.Where(u => u.Username == loginData.Username).FirstOrDefaultAsync();

                if(user == null) throw new Exception("User doesn't exist!");

                if(!BCrypt.Net.BCrypt.Verify(loginData.Password, user.Password)) throw new Exception("Passowrd doesn't match!");

                string token = this.auth.GenerateJwtToken(user);

                HttpContext.Response.Cookies.Append("token", token);

                return Ok(new {
                    message = "Successfully login!",
                    token = token
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

        [Route("Auth")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsAuthentificated()
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            if(token == null) return BadRequest("Unauthentificated user!");

            return Ok(new {isAuthentificated = true});
        }

        [Route("Logout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("token");
            return Ok(new {message = "Successfully logged out!"});
        }

        [Route("Server")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddServerRent([FromBody] Server server)
        {
            var token = auth.ValidateJwtToken(HttpContext.Request.Cookies["token"]);

            try
            {
                if(token == null) return Unauthorized(new { error = "You must to login!" });
                if(token["priority"] == 0) return StatusCode(403, new {error = "You cannot add rent!"});

                User userFromDB = await Context
                            .Users
                            .Where(u => u.ID == token["id"])
                            .FirstOrDefaultAsync();

                Server serverFromDB = await Context
                                .Servers
                                .Where(s => s.ID == server.ID)
                                .FirstOrDefaultAsync();

                if(serverFromDB == null) return BadRequest(new { error = "Server with this ip address doesn't exist!"});
                
                int count = (await Context
                            .UserServers
                            .Where(us=> us.User == userFromDB && us.Server == serverFromDB)
                            .ToListAsync()).Count;

                if(count != 0) return BadRequest(new { error = "You already rented this server!"});

                UserServer userServer = new UserServer();
                userServer.User = userFromDB;
                userServer.Server = serverFromDB;

                Context.UserServers.Add(userServer);
                await Context.SaveChangesAsync();

                return Ok(new {
                    message = "Rent successfully added!"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {
                    error = e.Message
                });
            }
        }
    }
}
