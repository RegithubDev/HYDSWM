using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMMON;
using COMMON.GENERIC;
using COMMON.SWMENTITY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HYDSWM.Helpers;
using COMMON.ASSET;

namespace HYDSWM.Controllers
{
    public class UserController : Controller
    {
        private TReport _report;
        public UserController(TReport report)
        {
            this._report = report;
        }
        [CustomAuthorize]
        public IActionResult Index()
        {
            return View();
        }
        [CustomAuthorize]
        public IActionResult AllRoles()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllUserData(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/User/GetAllUser";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);
            JArray _lst = JArray.Parse(Result);
            IList<JToken> t = _lst.SelectTokens("$..TotalCount").ToList();
            int tt = 0;
            if (t.Count > 0)
            {
                tt = (int)t[0];
            }
            var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };
            return Json(response);
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetUserDataById(String UserId)
        {
            string endpoint = "api/User/GetUserDataById?UserId=" + UserId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            JObject myjson = JObject.Parse(Result);
            string pwd = (string)myjson["Pwd"];
            myjson["Pwd"] = PasswordHelper.DecryptPwd(pwd);
            return Json(myjson);
        }
        public IActionResult AddUser()
        {
            return PartialView();
        }
        public IActionResult AddRoles()
        {
            return PartialView();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllRoles()
        {
            string endpoint = "api/User/GetAllRole?CCode=" + this.User.GetCompanyCode();
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllMenuMaster(int roleId)
        {
            string endpoint1 = "api/User/GetAllMenuMaster?roleId=" + roleId;
            HttpClientHelper<string> apiobj1 = new HttpClientHelper<string>();
            string Result1 = apiobj1.GetRequest(endpoint1);

            return Json(Result1);
        }
        [HttpPost]
        public JsonResult GetAllCircleAndWardMaster(string loginId)
        {
            var obj = new
            {
                Ccode = this.User.GetCompanyCode(),
                LoginId = loginId
            };
            string endpoint = "api/User/GetAllCircleAndWardMaster";
            string input = JsonConvert.SerializeObject(obj);
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }


        [HttpPost]
       // [CustomPostAuthorize]
        public JsonResult SaveandupdateMenu(string roleName, string JArrayval, string roleId, string IsActive, string VehicleTypeId)
        {
            string Vehicle = VehicleTypeId.Replace("[", "").Replace("]", "").Replace("\"","");


            var obj = new
            {
                RoleName = roleName,
                RoleId = roleId,
                IsActive = IsActive,
                VehicleTypeId = Vehicle,
                CCode = this.User.GetCompanyCode(),
                JArrayval = JArrayval
            };
            string endpoint = "api/User/SaveandupdateMenu";
            string input = JsonConvert.SerializeObject(obj);
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult SaveandupdateUser(string jobj, string JArrayval)
        {
            dynamic dresult = JObject.Parse(jobj);
            string pddd = dresult.Pwd;

            string output = "";
            string msg = CommonHelper.ValidatePassword(pddd);
            if (msg == "Success")
            {

                string EncrptedPWD = PasswordHelper.EncryptPwd(pddd);
                var obj = new
                {
                    UserId = dresult.UserId,
                    FullName = dresult.FullName,
                    EmailId = dresult.EmailId,
                    Pwd = EncrptedPWD,
                    Mobile = dresult.Mobile,
                    IsActive = dresult.IsActive,
                    RoleId = dresult.RoleId,
                    CCode = this.User.GetCompanyCode(),
                    EmpCode = dresult.EmpCode,
                    JArrayval = JArrayval
                };
                string endpoint = "api/User/SaveandupdateUser";
                string input = JsonConvert.SerializeObject(obj);
                HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
                output = apiobj.PostRequestString(endpoint, input);
                return Json(output);
            }
            else
            {
                var response = new { Result = 0, Msg = msg };
                var Eresult = JsonConvert.SerializeObject(response);
                return Json(Eresult);
            }

            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllActiveUser()
        {
            string endpoint = "api/User/GetAllActiveUser";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [CustomAuthorize]
        public IActionResult AllMobileRole()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllMobileMenuMaster(int roleId)
        {
            string endpoint1 = "api/User/GetAllMobileMenuMaster?roleId=" + roleId;
            HttpClientHelper<string> apiobj1 = new HttpClientHelper<string>();
            string Result1 = apiobj1.GetRequest(endpoint1);

            return Json(Result1);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult SaveandupdateMobileMenu(string JArrayval)
        {

            var obj = new
            {
                LoginId = this.User.GetUserId(),
                JArrayval = JArrayval
            };
            string endpoint = "api/User/SaveandupdateMobileMenu";
            string input = JsonConvert.SerializeObject(obj);
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [CustomAuthorize]
        public IActionResult RptUserLog()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllUserLogInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();

            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/User/GetAllUserLog";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);
            JArray _lst = JArray.Parse(Result);
            IList<JToken> t = _lst.SelectTokens("$..TotalCount").ToList();
            int tt = 0;
            if (t.Count > 0)
            {
                tt = (int)t[0];
            }
            //var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };
            var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };
            return Json(response);
        }

        [HttpPost]
        public FileResult ExportUserLogSummaryData(DateTime FromDate, DateTime ToDate, string FName)
        {
            byte[] filearray = null;
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string Name = FName;
            string filename = string.Empty;

            DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
            requestModel.search = new Search();
            requestModel.order = new List<Order>();
            requestModel.order.Add(new Order { dir = "", column = 0 });

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Shift = this.User.GetTSId();
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/User/GetAllUserLog";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportUserLogData(Result, Name);

            return File(filearray, ContentType, filename);
        }

        [HttpPost]
        public ActionResult ExportUserLogSummaryCSV(DateTime FromDate, DateTime ToDate, string FName)
        {
            byte[] filearray = null;
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string Name = FName;
            string Sname = FName;
            string filename = string.Empty;
            DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
            requestModel.search = new Search();
            requestModel.order = new List<Order>();
            requestModel.order.Add(new Order { dir = "", column = 0 });

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Shift = this.User.GetTSId();
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/User/GetAllUserLog";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportUserLogSummaryCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }
    }
}
    
