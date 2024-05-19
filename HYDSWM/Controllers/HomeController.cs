using COMMON;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEMOSWMCKC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment HostingEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            this.HostingEnvironment = hostingEnvironment;
        }
        public JsonResult GetCurrentUserInfo()
        {
            var obj = new
            {
                UserName = this.User.GetUserName(),
                RoleName = this.User.GetRoleName(),
            };
            return Json(obj);
        }
    }
}
