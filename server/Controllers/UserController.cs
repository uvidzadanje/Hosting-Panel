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
    public class UserController : ControllerBase
    {
        public HostingContext Context { get; set; }

        public UserController(HostingContext context)
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
                    .Users
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
                    .Users
                    .Where(u => u.ID == id)
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
        public async Task<ActionResult> Create([FromBody] User User)
        {
            if(User.FullName.Length > 50) return BadRequest("Fullname is too long");
            if(User.Username.Length > 25) return BadRequest("Username is too long");

            try
            {
                 Context.Users.Add(User);
                 await Context.SaveChangesAsync();
                 return Ok("User successfully added!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update([FromBody] User user)
        {
            if(user.FullName.Length > 50) return BadRequest("Fullname is too long");
            if(user.Username.Length > 25) return BadRequest("Username is too long");

            try
            {
                 Context.Users.Update(user);
                 await Context.SaveChangesAsync();
                 return Ok($"User with ID {user.ID} has been changed!");
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
                 var user = await Context.Users.FindAsync(id);
                 string userName = user.Username;
                 Context.Users.Remove(user);
                 await Context.SaveChangesAsync();
                 return Ok($"User {userName} has been removed!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
    }
}
