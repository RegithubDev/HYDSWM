using COMMON;
using HYDSWM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DEMOSWMCKC.Controllers
{
    public class AttendanceController : Controller
    {
        [CustomAuthorize]
        public IActionResult AllShifts()
        {
            ViewBag.UserId = this.User.GetUserId();
            ViewBag.UserName = this.User.GetUserName();

            HttpContext.Session.SetString("user_name", this.User.GetUserName());

            return View();
        }
    }
}
