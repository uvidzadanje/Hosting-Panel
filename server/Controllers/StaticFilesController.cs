using System;
using System.Collections.Generic;
using System.IO;
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
    [Route("")]
    public class StaticFilesController : ControllerBase
    {
        public HostingContext Context { get; set; }
        private Auth auth;

        private readonly string htmlPath;
        public StaticFilesController(IConfiguration configuration, HostingContext context)
        {
            Context = context;
            htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "client" ,"html")+"/";
            auth = new Auth(configuration);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult Index()
        {
            var token = getTokenFromCookie();
            if(token == null) return OpenHtml("login.html");
            return OpenHtml("index.html");
        }

        [Route("login")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult Login()
        {
            return OpenHtml("login.html");
        }

        [Route("register")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult Register()
        {
            return OpenHtml("register.html");
        }

        private PhysicalFileResult OpenHtml(string filename)
        {
            string filePath = htmlPath+filename;
            return PhysicalFile(filePath, "text/html");
        }

        private Dictionary<string, int> getTokenFromCookie()
        {
            string tokenFromCookie = HttpContext.Request.Cookies["token"];
            return auth.ValidateJwtToken("Bearer "+tokenFromCookie);
        }
    }
}
