using COMMON;
using COMMON.SWMENTITY;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HYDSWM.Helpers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using COMMON.GENERIC;
using DEMOSWMCKC.Middleware;

namespace HYDSWM.Controllers
{

    public class AccountController : Controller
    {

        public AccountController()
        {

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [RateLimitDecorator(StrategyType = StrategyTypeEnum.IpAddress)]
        public IActionResult Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return View();
            }

            ClaimsIdentity identity = null;
            bool isAuthenticated = false;
            //string endpoint = "api/User/Login?userName=" + userName + "&password=" + password;
            //HttpClientHelper<LoginResponse> apiobj = new HttpClientHelper<LoginResponse>();
            //LoginResponse usr = apiobj.GetSingleItemRequest(endpoint);
            BUserLoginInfo info = new BUserLoginInfo();
            info.UserName = userName;
            info.LoginType = CommonHelper.Application_Type;
            info.Password = password;
            string input = JsonConvert.SerializeObject(info);
            string endpoint = "api/User/ValidateLogin";
            HttpClientHelper<LoginResponse> apiobj = new HttpClientHelper<LoginResponse>();
            LoginResponse usr = apiobj.PostRequest(endpoint, input);

            string endpoint2 = "api/User/GetSubMenuByRolev1?rolename=" + usr.RoleName;
            HttpClientHelper<string> apiobj2 = new HttpClientHelper<string>();
            string Result2 = apiobj2.GetRequest(endpoint2);
            List<RoleMaster> mList = new List<RoleMaster>();
            if (!string.IsNullOrEmpty(Result2))
                mList = JsonConvert.DeserializeObject<List<RoleMaster>>(Result2);
            RoleMaster rminfo = new RoleMaster();
            if (mList.Count > 0)
                rminfo = mList.FirstOrDefault();
            if (usr != null && usr.Result == 1)
            {
                identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, usr.FullName),
                            new Claim(ClaimTypes.NameIdentifier, usr.LoginId),
                            new Claim(ClaimTypes.Role, usr.RoleName),
                            new Claim(ClaimTypes.MobilePhone, usr.RoleId.ToString()),
                            new Claim(ClaimTypes.PrimaryGroupSid, usr.CCode),
                            new Claim(ClaimTypes.HomePhone, Result2),
                            new Claim(ClaimTypes.Gender, usr.TSId.ToString()),
                            new Claim(ClaimTypes.GivenName, !string.IsNullOrEmpty(rminfo.cn)?rminfo.cn:string.Empty),
                            new Claim(ClaimTypes.Surname, !string.IsNullOrEmpty(rminfo.an)?rminfo.an:string.Empty),
                            new Claim(ClaimTypes.Spn, usr.LevelId.ToString()),
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }
            else
                ViewBag.msg = usr.Msg;

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);
                var roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
                var login = HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(8),
                        IsPersistent = true
                    }
                    );
                if (usr.RoleId > 3)
                {
                    //if (!string.IsNullOrEmpty(this.User.GetController()))
                    //{
                    //    return RedirectToAction(this.User.GetAction(), this.User.GetController());
                    //}
                    if (!string.IsNullOrEmpty(rminfo.cn))
                    {
                        return RedirectToAction(rminfo.an, rminfo.cn);
                    }
                    else
                        return RedirectToAction("AccessDenied", "Account");
                }
                else
                    return RedirectToAction("OperationIndex", "operation");

            }
            return View();

        }
        [HttpPost]
        public JsonResult GetAllMenu()
        {
            string RoleName = this.User.GetRoleName();
            string endpoint1 = "api/User/GetMenuByRole?rolename=" + RoleName;
            HttpClientHelper<string> apiobj1 = new HttpClientHelper<string>();
            string Result1 = apiobj1.GetRequest(endpoint1);

            string endpoint2 = "api/User/GetSubMenuByRole?rolename=" + RoleName;
            HttpClientHelper<string> apiobj2 = new HttpClientHelper<string>();
            string Result2 = apiobj2.GetRequest(endpoint2);
            JArray _lst1 = JArray.Parse(Result1);
            JArray _lst2 = JArray.Parse(Result2);
            var response = new { menu = _lst1, submenu = _lst2 };
            return Json(response);
        }
        [HttpPost]
        public JsonResult GetAllSubMenu()
        {
            string RoleName = this.User.GetRoleName();
            string endpoint = "api/User/GetSubMenuByRole?rolename=" + RoleName;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult Logout()
        {
            BUserLoginInfo info = new BUserLoginInfo();
            info.UserName = this.User.GetUserId();
            info.LoginType = CommonHelper.Application_Type;
            string input = JsonConvert.SerializeObject(info);
            string endpoint = "api/User/Logout";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public JsonResult LogOffApp()
        {
            BUserLoginInfo info = new BUserLoginInfo();
            info.UserName = this.User.GetUserId();
            info.LoginType = CommonHelper.Application_Type;
            string input = JsonConvert.SerializeObject(info);
            string endpoint = "api/User/Logout";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json("Login");
        }
        public IActionResult AccessDenied()
        {
            int RoleId = Convert.ToInt32(this.User.GetRoleId());
            if (RoleId >= 22)
            {
                if (!string.IsNullOrEmpty(this.User.GetController()))
                    return RedirectToAction(this.User.GetAction(), this.User.GetController());
                else
                    return View();
            }
            else
                return View();
        }
        public IActionResult ChangePassword()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return PartialView();
        }
        [HttpPost]
        public JsonResult SaveChangePassword(string jobj)
        {
            string Result = "";
            dynamic dresult = JObject.Parse(jobj);
            string NPwd = dresult.NewPwd;
            var obj = new
            {
                LoginId = this.User.GetUserId()
                             ,
                CurrentPwd = dresult.CurrentPwd
                             ,
                NewPwd = dresult.NewPwd
            };

            string msg = CommonHelper.ValidatePassword(NPwd);
            if (msg == "Success")
            {
                string input = JsonConvert.SerializeObject(obj);
                string endpoint3 = "api/User/SaveChangePassword";
                HttpClientHelper<string> apiobj3 = new HttpClientHelper<string>();
                Result = apiobj3.PostRequestString(endpoint3, input);
                return Json(Result);
            }
            else
            {
                var response = new { Result = 0, Msg = msg };
                var Eresult = JsonConvert.SerializeObject(response);
                return Json(Eresult);
            }
            return Json(Result);
        }
    }

}
