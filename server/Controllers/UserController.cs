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
        public async Task<ActionResult> Index()
        {
            try
            {
                return Ok(
                    new{
                        users = await Context
                        .Users
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
                         user = await Context
                         .Users
                         .Where(u => u.ID == id)
                         .ToListAsync()
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
            if(user.FullName.Length > 50) return BadRequest("Fullname is too long");
            if(user.Username.Length > 25) return BadRequest("Username is too long");

            try
            {
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
        public async Task<ActionResult> Update([FromBody] User user, [FromHeader] string authorization)
        {
            if(user.FullName.Length > 50) return BadRequest("Fullname is too long");
            if(user.Username.Length > 25) return BadRequest("Username is too long");

            try
            {
                var token = this.auth.ValidateJwtToken(authorization);

                if(token == null) return  Unauthorized("You must to login!");

                if(token["id"] != user.ID) return Forbid("You dont have permission for that!");

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                Context.Users.Update(user);
                await Context.SaveChangesAsync();

                return Ok(
                    new{
                        message = $"User with ID {user.ID} has been changed!"
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
        public async Task<ActionResult> Delete([FromHeader] string authorization)
        {
            var token = this.auth.ValidateJwtToken(authorization);

            if(token == null) return Unauthorized("You must to login!");

            if(token["id"] <= 0) return BadRequest("Wrong ID!");

            try
            {
                var user = await Context.Users.FindAsync(token["id"]);
                string userName = user.Username;
                Context.Users.Remove(user);
                await Context.SaveChangesAsync();
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

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Login([FromBody] Login loginData)
        {
            if(String.IsNullOrEmpty(loginData.Username) || String.IsNullOrEmpty(loginData.Password)) return BadRequest("Username and password are required!");

            try
            {
                var user = await Context.Users.Where(u => u.Username == loginData.Username).FirstOrDefaultAsync();

                if(user == null) throw new Exception("User doesn't exist!");

                if(!BCrypt.Net.BCrypt.Verify(loginData.Password, user.Password)) throw new Exception("Passowrd doesn't match!");

                return Ok(new {
                    message = "Successfully login!",
                    token = this.auth.GenerateJwtToken(user)
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

        [Route("auth")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsAuthentificated([FromHeader] string authorization)
        {
            var token = auth.ValidateJwtToken(authorization);

            if(token != null) return BadRequest("Unauthentificated user!");

            return Ok(new {isAuthentificated = true});
        }
    }
}
