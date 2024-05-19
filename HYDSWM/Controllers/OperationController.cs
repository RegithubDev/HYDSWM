using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMMON;
using COMMON.ASSET;
using HYDSWM;
using HYDSWM.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEMOSWMCKC.Controllers
{
    public class OperationController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private TReport _report;
        public OperationController(IWebHostEnvironment host, TReport report)
        {
            this._host = host;
            this._report = report;
        }
        [CustomAuthorize]
        public IActionResult Index()
        {
            return View();
        }
        [CustomPostAuthorize]
        public IActionResult VehicleCollectionNoti()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllCollectionNotification(string ZoneId, string CircleId, string WardId)
        {
            var obj = new
            {
                ZoneId = ZoneId,
                CircleId = CircleId,
                WardId = WardId,
                LoginId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetAllCollectionNotification";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult GetAllCollectionNoti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "1";
            requestModel.ZoneId = !string.IsNullOrEmpty(requestModel.ZoneId) ? requestModel.ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(requestModel.CircleId) ? requestModel.CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllCollectionNoti";
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
        public JsonResult GetAllPrimaryTransaction(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllPrimaryTransaction";
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
        public JsonResult ChangeStatusOfTransaction(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string CTDId = dresult.CTDId;
            string StatusId = dresult.StatusId;
            string OperationTypeId = dresult.OperationTypeId;
            string TId = dresult.TId;
            var obj = new
            {
                CTDId = !string.IsNullOrEmpty(CTDId) ? CTDId : "0",
                StatusId = StatusId,
                OperationTypeId = OperationTypeId,
                TId = TId,
                CreatedBy = this.User.GetUserId(),
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/ChangeStatusOfTransaction";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult GetAllMPrimaryVehicleTransactionByContainer(int CTDId, DateTime TDate)
        {
            var obj = new
            {
                CTDId = CTDId,
                CreatedDate = TDate
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetAllMPrimaryVehicleTransactionByContainer";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [CustomAuthorize]
        public IActionResult RptWeightBridge()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllWeightBridgeInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllWeightBridgeInfo";
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
        public IActionResult RptOperationSummary()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult RptOperationSummaryInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/rptGetOperationSummary";
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
        public FileResult ExportOperationSummaryData(string zid, string cid, string VehicleTypeId, string TSId, DateTime FromDate, DateTime ToDate, string FName)
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

            requestModel.ZoneId = !string.IsNullOrEmpty(zid) ? zid : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(cid) ? cid : "0";
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.Status = !string.IsNullOrEmpty(TSId) ? TSId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/rptGetOperationSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportMAppDataLog(Result, Name);

            return File(filearray, ContentType, filename);
        }

        [HttpPost]
        public ActionResult ExportOperationSummaryCSV(string zid, string cid, string VehicleTypeId, string TSId, DateTime FromDate, DateTime ToDate, string FName)
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

            requestModel.ZoneId = !string.IsNullOrEmpty(zid) ? zid : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(cid) ? cid : "0";
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.Status = !string.IsNullOrEmpty(TSId) ? TSId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/rptGetOperationSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllOperationSummaryCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }

        public IActionResult RptContainerforHKL()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllRptContainerforHKL(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetrptContainerforHKL";
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
            // var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };

            return Json(response);
        }

        [HttpPost]
        public FileResult ExportAllRptContainerforHKL(string TSId, DateTime FromDate, DateTime ToDate, string FName)
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

            // requestModel.ZoneId = !string.IsNullOrEmpty(zid) ? zid : "0";
            // requestModel.CircleId = !string.IsNullOrEmpty(cid) ? cid : "0";
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.Status = !string.IsNullOrEmpty(TSId) ? TSId : "0";
            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetrptContainerforHKL";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllRptContainerforHKL(Result, Name);

            return File(filearray, ContentType, filename);
        }


        public IActionResult PendingContainerforHKL()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllPendingContainerforHKL(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetPendingContainerforHKL";
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
        public FileResult ExportPendingContainerforHKL(string zid, string TSId, DateTime FromDate, DateTime ToDate, string FName)
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

            requestModel.ZoneId = !string.IsNullOrEmpty(zid) ? zid : "0";
            // requestModel.CircleId = !string.IsNullOrEmpty(cid) ? cid : "0";
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.Status = !string.IsNullOrEmpty(TSId) ? TSId : "0";
            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetPendingContainerforHKL";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportPendingContainerforHKL(Result, Name);

            return File(filearray, ContentType, filename);
        }

        public IActionResult AllRptVehicleInfo()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllRptVehicleInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllTotalVehicle";
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
            // var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };

            return Json(response);
        }

        [HttpPost]
        public FileResult ExportGetAllRptVehicleInfo(string Status, DateTime FromDate, DateTime ToDate, string FName)
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

            // requestModel.Status = !string.IsNullOrEmpty(TSId) ? TSId : "0";
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            requestModel.Status = Status;
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllTotalVehicle";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportGetAllRptVehicleInfo(Result, Name);

            return File(filearray, ContentType, filename);
        }



        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllTransferStationByUser()
        {
            string endpoint = "api/Operation/GetAllTransferStationByUser?UserId=" + this.User.GetUserId();
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [CustomAuthorize]
        public IActionResult RptTSWiseSummary()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetMDashboardLst(string zid, string cid, string VehicleTypeId, string TSId, DateTime FromDate, DateTime ToDate, string TStatus)
        {
            var obj = new
            {
                ZoneId = zid,
                CircleId = cid,
                UserId = this.User.GetUserId(),
                VehicleTypeId = VehicleTypeId,
                TSId = this.User.GetTSId(),
                UTsId = TSId,
                FromDate = FromDate,
                ToDate = ToDate,
                TStatus = !string.IsNullOrEmpty(TStatus) ? TStatus : "0"
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetMDashboardLst";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetOpMapDashboardLst(string zid, string cid, string VehicleTypeId, string TSId, DateTime FromDate, DateTime ToDate, string TStatus)
        {
            var obj = new
            {
                ZoneId = zid,
                CircleId = cid,
                UserId = this.User.GetUserId(),
                VehicleTypeId = VehicleTypeId,
                TSId = this.User.GetTSId(),
                UTsId = TSId,
                FromDate = FromDate,
                ToDate = ToDate,
                TStatus = !string.IsNullOrEmpty(TStatus) ? TStatus : "0"
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetOpMapDashboardLst";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public FileResult ExportDataLogByTS(string zid, string cid, string VehicleTypeId, string TSId, DateTime FromDate, DateTime ToDate, string FName)
        {
            byte[] filearray = null;
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string Name = FName;
            string filename = string.Empty;
            var obj = new
            {
                ZoneId = zid,
                CircleId = cid,
                UserId = this.User.GetUserId(),
                VehicleTypeId = VehicleTypeId,
                TSId = TSId,
                FromDate = FromDate,
                ToDate = ToDate
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetExportDataLogByTS";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            Name += " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportDataLogByTS(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [CustomAuthorize]
        public IActionResult AllPendingOperation()
        {
            return View();
        }

        [CustomPostAuthorize]
        public JsonResult GetAllPendingOperation(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = !string.IsNullOrEmpty(requestModel.NotiId) ? requestModel.NotiId : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllPendingOperation";
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
        public FileResult ExportOpenOperationSummaryData(string TSId, string Status, DateTime FromDate, DateTime ToDate,string VehicleTypeId, string FName)
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
            requestModel.NotiId = !string.IsNullOrEmpty(Status) ? Status : "0";
            requestModel.Status = !string.IsNullOrEmpty(TSId) ? TSId : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.VehicleTypeId = VehicleTypeId;// "0";

            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllPendingOperation";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportOpenProcessData(Result, Name);

            return File(filearray, ContentType, filename);
        }

        [CustomAuthorize]
        public IActionResult OperationIndex()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllOperation1Notification(int TsId)
        {
            var obj = new
            {
                LoginId = this.User.GetUserId(),
                TSId = this.User.GetTSId(),
                UTSId = TsId,
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetAllOperation1Notification";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllScannedVehicleForBarChart(int TsId, string SearchType, int OperationTypeId)
        {
            var obj = new
            {
                LoginId = this.User.GetUserId(),
                TSId = this.User.GetTSId(),
                UTSId = TsId,
                OperationTypeId = OperationTypeId,
                SearchType = SearchType
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetAllScannedVehicleForBarChart";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [CustomPostAuthorize]
        public IActionResult GetAllOpt1VehicleNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllVehicleOpt1Noti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllVehicleOpt1Noti";
            //string endpoint = "api/Operation/GetAllVehicleOpt1NotiB64";
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
        [CustomPostAuthorize]
        public FileResult ExportAllOptVehicle(string NotiId, string Status, string VehicleTypeId, string FName)
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
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "0";

            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllVehicleOpt1Noti";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            filearray = _report.ExportAllOptVehicle(Result, Name);

            return File(filearray, ContentType, filename);
        }

        [CustomPostAuthorize]
        public IActionResult GetAllOpt1ArvlEntityNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllArvlOfEntityOpt1Noti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Route = !string.IsNullOrEmpty(requestModel.Route) ? requestModel.Route : "-1";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(requestModel.NotiId) ? requestModel.NotiId : "0";
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllArvlOfEntityOpt1Noti";
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
        public FileResult ExportAllArvlOfEntityOpt1Noti(string Status, string Route, string NotiId, string VehicleTypeId, DateTime FromDate, DateTime ToDate, string FName)
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
            requestModel.Route = !string.IsNullOrEmpty(Route) ? Route : "-1";
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "0";

            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllArvlOfEntityOpt1Noti";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllArvlOfEntityOpt1Noti(Result, Name);

            return File(filearray, ContentType, filename);
        }


        [CustomPostAuthorize]
        public IActionResult AllOpt1ContainerNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllOpt1ContainerNotiInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Route = !string.IsNullOrEmpty(requestModel.Route) ? requestModel.Route : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllOpt1ContainerNoti";
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

        [CustomPostAuthorize]
        public IActionResult AllOpt1HKLNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllOpt1HKLNotiInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            // string endpoint = "api/Operation/GetAllOpt1HKLNoti";
            string endpoint = "api/Operation/GetAllOpt1HKLNotiB64";
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
        [CustomPostAuthorize]
        public IActionResult AllOpt1RCVNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllOpt1RCVNoti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Route = !string.IsNullOrEmpty(requestModel.Route) ? requestModel.Route : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllOpt1RCVNoti";
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
        [CustomPostAuthorize]
        public IActionResult AllOpt1UNQContainerNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllOpt1UNQContainerNoti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Route = !string.IsNullOrEmpty(requestModel.Route) ? requestModel.Route : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllOpt1UNQContainerNoti";
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
        [CustomPostAuthorize]
        public IActionResult AllOpt1UNQHKLNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllOpt1UNQHKLNoti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Route = !string.IsNullOrEmpty(requestModel.Route) ? requestModel.Route : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllOpt1UNQHKLNoti";
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
        [CustomPostAuthorize]
        public IActionResult AllForceTransactionOpt1Noti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllForceTransactionInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Route = !string.IsNullOrEmpty(requestModel.Route) ? requestModel.Route : "0";
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.Shift = this.User.GetTSId();
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.FromDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            requestModel.ToDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllForceTransaction_Paging";
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
        public FileResult ExportWeightBridgeSummaryData(string NotiId, DateTime FromDate, DateTime ToDate, string FName)
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
            requestModel.NotiId = !string.IsNullOrEmpty(requestModel.NotiId) ? NotiId : "0";
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllWeightBridgeInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name;
            filearray = _report.ExportWeightBridgeReportData(Result, Name);

            return File(filearray, ContentType, filename);
        }

        [HttpPost]
        public ActionResult ExportWeightBridgeSummaryCSV(string NotiId, DateTime FromDate, DateTime ToDate, string FName)
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
            requestModel.NotiId = !string.IsNullOrEmpty(requestModel.NotiId) ? NotiId : "0";
            requestModel.FromDate = FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : FromDate;
            requestModel.ToDate = ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllWeightBridgeInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportWeightBridgeSummaryCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }

        [CustomAuthorize]
        public IActionResult AllDeployLocation()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllDeployLocation_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllDeployLocation_Paging";
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
        [CustomPostAuthorize]
        public JsonResult AddDeployLocation(string jobj)
        {

            dynamic dresult = JObject.Parse(jobj);
            //var array = JArray.Parse(JArrayval);
            string msg = string.Empty;
            var obj = new
            {
                DLId = dresult.DLId,
                DeployLocation = dresult.DeployLocation
                           ,
                Radius = dresult.Radius,
                Lat = dresult.Lat,
                Lng = dresult.Lng

                ,
                IsActive = dresult.IsActive
                ,
                CreatedBy = this.User.GetUserId(),
                ZoneId = dresult.ZoneId,
                CircleId = dresult.CircleId,
                WardId = dresult.WardId


                        ,

            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/AddDeployLocation";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetDeployLocationInfoById(int DLId)
        {
            string endpoint = "api/Operation/GetDeployLocationInfoById?DLId=" + DLId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public FileResult ExportAllDeployLocation(string WardId, string ZoneId, string CircleId, string Status, string FName)
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
            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";

            //requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "-1";

            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";


            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllDeployLocation_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllDeployLocation(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [CustomAuthorize]
        public IActionResult AllDeployIncharge()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllDeployIncharge(DataTableAjaxPostModel requestModel)
        {
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Operation/GetAllDeployIncharge";
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
        public JsonResult GetAllDeployLocationAndWardMaster(string loginId)
        {
            var obj = new
            {
                Ccode = this.User.GetCompanyCode(),
                LoginId = loginId
            };
            string endpoint = "api/Operation/GetAllDeployLocationAndWardMaster";
            string input = JsonConvert.SerializeObject(obj);
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult SaveandupdateDeployIncharge(string jobj, string JArrayval)
        {
            dynamic dresult = JObject.Parse(jobj);
            string pddd = dresult.Pwd;

            string output = "";
            //string msg = CommonHelper.ValidatePassword(pddd);


            string EncrptedPWD = PasswordHelper.EncryptPwd(pddd);
            var obj = new
            {
                UserId = dresult.UserId,
                Name = dresult.Name,
                LoginId = dresult.LoginId,
                Pwd = EncrptedPWD,
                ContactNo = dresult.ContactNo,
                IsActive = dresult.IsActive,

                CreatedBy = this.User.GetUserId(),

                JArrayval = JArrayval
            };
            string endpoint = "api/Operation/SaveandupdateDeployIncharge";
            string input = JsonConvert.SerializeObject(obj);
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }



        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetDeployInchargeDataById(string UserId)
        {
            string endpoint = "api/Operation/GetDeployInchargeDataById?UserId=" + UserId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            JObject myjson = JObject.Parse(Result);
            string pwd = (string)myjson["Pwd"];
            myjson["Pwd"] = PasswordHelper.DecryptPwd(pwd);
            return Json(myjson);
        }
    }
}
