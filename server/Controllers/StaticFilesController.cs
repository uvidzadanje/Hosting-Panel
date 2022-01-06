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
            if(token != null) return OpenHtml("user/dashboard.html");
            return OpenHtml("index.html");
        }

        [Route("login")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult Login()
        {
            var token = getTokenFromCookie();
            if(token != null) return OpenHtml("user/dashboard.html");
            return OpenHtml("login.html");
        }

        [Route("register")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult Register()
        {
            var token = getTokenFromCookie();
            if(token != null) return OpenHtml("user/dashboard.html");
            return OpenHtml("register.html");
        }

        [Route("dashboard")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult Dashboard()
        {
            var token = getTokenFromCookie();
            if(token == null) return OpenHtml("login.html");
            return OpenHtml("user/dashboard.html");
        }

        [Route("settings")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult Settings()
        {
            var token = getTokenFromCookie();
            if(token == null) return OpenHtml("login.html");
            return OpenHtml("user/settings.html");
        }

        [Route("report/edit")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult EditReport()
        {
            var token = getTokenFromCookie();
            if(token == null) return OpenHtml("login.html");
            if(token["priority"] == 0) return OpenHtml("dashboard.html");
            return OpenHtml("report/edit.html");
        }

        [Route("server/edit")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult ServerEdit()
        {
            var token = getTokenFromCookie();
            if(token == null) return OpenHtml("login.html");
            if(token["priority"] == 1) return OpenHtml("dashboard.html");
            return OpenHtml("server/edit.html");
        }

        [Route("servers")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PhysicalFileResult IndexServer()
        {
            var token = getTokenFromCookie();
            if(token == null) return OpenHtml("login.html");
            if(token["priority"] != 0) return OpenHtml("dashboard.html");
            return OpenHtml("server/index.html");
        }

        private PhysicalFileResult OpenHtml(string filename)
        {
            string filePath = htmlPath+filename;
            return PhysicalFile(filePath, "text/html");
        }

        private Dictionary<string, int> getTokenFromCookie()
        {
            string tokenFromCookie = HttpContext.Request.Cookies["token"];
            return auth.ValidateJwtToken(tokenFromCookie);
        }
    }
}
