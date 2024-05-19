using COMMON;
using HYDSWM.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEMOSWMCKC.Controllers
{
    public class Demo1Controller : Controller
    {
        public IActionResult ContainerDetail()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllContainerInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetAllContainer";
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


        public IActionResult AllCircle()
        {
            return View();
        }
        public IActionResult AddCircle()
        {
            return PartialView();
        }
        public JsonResult GetCircleInfoById(int CircleId)
        {
            string endpoint = "api/Master/GetCircleInfoById?CircleId=" + CircleId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public JsonResult SaveAndUpdateCircle(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string CircleId = dresult.CircleId;
            string CircleName = dresult.CircleName;
            string CircleCode = dresult.CircleCode;
            string ZoneId = dresult.ZoneId;
            string IsActive = dresult.IsActive;
            var obj = new
            {
                CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0",
                CircleName = CircleName,
                ZoneId = ZoneId,
                CircleCode = CircleCode,
                CCode = this.User.GetCompanyCode(),
                IsActive = IsActive
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateCircle";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        public IActionResult RouteInfo()
        {
            return View();
        }
    }
}
