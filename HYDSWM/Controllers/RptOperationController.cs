using COMMON;
using COMMON.ASSET;
using HYDSWM;
using HYDSWM.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEMOSWMCKC.Controllers
{
    public class RptOperationController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private TReport _report;
        public RptOperationController(IWebHostEnvironment host, TReport report)
        {
            this._host = host;
            this._report = report;
        }
        [CustomAuthorize]
        public IActionResult RptContainerWisePerf()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllContainerWisePerformance_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/RptOperation/GetAllContainerWisePerformance_Paging";
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
        [CustomAuthorize]
        public IActionResult RptHKLWisePerf()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllHKLWisePerformance_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/RptOperation/GetAllHKLWisePerformance_Paging";
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
        public FileResult ExportContainerWisePerfSummaryData(DateTime FromDate, DateTime ToDate, string FName)
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
            string endpoint = "api/RptOperation/GetAllContainerWisePerformance_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportContainerWisePerfData(Result, Name);

            return File(filearray, ContentType, filename);
        }


        [HttpPost]
        public ActionResult ExportContainerWisePerfSummaryCSV(DateTime FromDate, DateTime ToDate, string FName)
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
            string endpoint = "api/RptOperation/GetAllContainerWisePerformance_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllContainerWisePerfSummaryCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }



        [HttpPost]
        public FileResult ExportHKLWisePerfSummaryData(DateTime FromDate, DateTime ToDate, string FName)
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
            string endpoint = "api/RptOperation/GetAllHKLWisePerformance_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportHKLWisePerfData(Result, Name);

            return File(filearray, ContentType, filename);
        }

        [HttpPost]
        public ActionResult ExportHKLWisePerfSummaryCSV(DateTime FromDate, DateTime ToDate, string FName)
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
            string endpoint = "api/RptOperation/GetAllHKLWisePerformance_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllHKLWisePerfSummaryCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }

        [CustomAuthorize]
        public IActionResult RptWBInfo()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllWBData_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "http://59.145.65.98:8484/api/WBOperation/GetAllWBData_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string MAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("ramky_hiswm_hyd:ramky_hiswm_hyd#2021_$#^&*9846@!"));
            string Result = apiobj.PostRequestWithAuthString(endpoint, input, MAuth);
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
        public FileResult ExportMasterWBData(DateTime FromDate, DateTime ToDate, string FName)
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
            string endpoint = "http://59.145.65.98:8484/api/WBOperation/GetAllWBData_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string MAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("ramky_hiswm_hyd:ramky_hiswm_hyd#2021_$#^&*9846@!"));
            string Result = apiobj.PostRequestWithAuthString(endpoint, input, MAuth);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportMasterWBData(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public ActionResult ExportMasterWBCSV(DateTime FromDate, DateTime ToDate, string FName)
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
            string endpoint = "http://59.145.65.98:8484/api/WBOperation/GetAllWBData_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string MAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("ramky_hiswm_hyd:ramky_hiswm_hyd#2021_$#^&*9846@!"));

            string Result = apiobj.PostRequestWithAuthString(endpoint, input, MAuth);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportMasterWBCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }


        [CustomAuthorize]
        public IActionResult RptAllArvl()
        {
            return View();
        }
        [HttpPost]
        public FileResult ExportArvlEntityData(string Status,string Route,string NotiId,string VehicleTypeId, DateTime FromDate, DateTime ToDate, string FName)
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
            requestModel.Status = Status;
            requestModel.Route = Route;
            requestModel.NotiId = NotiId;
            requestModel.VehicleTypeId = VehicleTypeId;
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllArvlOfEntityOpt1Noti";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportRptArvlEntityData(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public ActionResult ExportArvlEntityCSV(string Status, string Route, string NotiId, string VehicleTypeId, DateTime FromDate, DateTime ToDate, string FName)
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
            requestModel.Status = Status;
            requestModel.Route = Route;
            requestModel.NotiId = NotiId;
            requestModel.VehicleTypeId = VehicleTypeId;
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllArvlOfEntityOpt1Noti";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportArvlEntityCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }


        [CustomAuthorize]
        public IActionResult RptPVehicleWiseInfo()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllPVehicleWiseInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Route = !string.IsNullOrEmpty(requestModel.Route) ? requestModel.Route : "0";
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/RptOperation/GetAllPVehicleWiseInfo";
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
        public FileResult ExportPVehicleWiseData(string Status, DateTime FromDate, DateTime ToDate, string FName,string OwnerType)
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
            requestModel.NotiId = this.User.GetTSId();
            requestModel.Status =!string.IsNullOrEmpty(Status)? Status:"0";
            requestModel.Route =!string.IsNullOrEmpty(OwnerType)? OwnerType:"0";
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/RptOperation/GetAllPVehicleWiseInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportRptPVehicleWiseData(Result, Name);

            return File(filearray, ContentType, filename);
        }

        [HttpPost]
        public ActionResult ExportPVehicleWiseCSV(string Status, DateTime FromDate, DateTime ToDate, string FName, string OwnerType)
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
            requestModel.NotiId = this.User.GetTSId();
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "0";
            requestModel.Route = !string.IsNullOrEmpty(OwnerType) ? OwnerType : "0";
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/RptOperation/GetAllPVehicleWiseInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportPVehicleWiseCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }

    }
}
