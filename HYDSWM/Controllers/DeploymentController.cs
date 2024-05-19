using COMMON;
using COMMON.ASSET;
using COMMON.DEPLOYMENT;
using HYDSWM;
using HYDSWM.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DEMOSWMCKC.Controllers
{
    public class DeploymentController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private TReport _report;
        public DeploymentController(IWebHostEnvironment host, TReport report)
        {
            this._host = host;
            this._report = report;
        }
        [CustomAuthorize]
        public IActionResult AllNotFilledContainer()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllNotFilledContainer_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllNotFilledContainer_Paging";
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
        public IActionResult AllNotFilledHKL()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllNotFilledHKL_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllNotFilledHKL_Paging";
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
        public JsonResult UpdateContainerLocation(int TSId, int DHLTId)
        {
            var obj = new
            {
                TSId = TSId,
                DHLTId = DHLTId,
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Deployment/UpdateContainerLocation";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult UpdateHKLLocation(int TSId, int DHLTId)
        {
            var obj = new
            {
                TSId = TSId,
                DHLTId = DHLTId,
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Deployment/UpdateHKLLocation";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        [CustomAuthorize]
        public IActionResult AllDeLinkHL()
        {
            return View();
        }



        public IActionResult AllDeployDetail()
        {
            return View();
        }



        [HttpPost]
        public JsonResult GetAllHLForDLink_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllHLForDLink_Paging";
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
        public JsonResult GetAllUnAllocatedHKL()
        {
            string endpoint = "api/Deployment/GetAllUnAllocatedHKL";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult AddDelinkHL(int CTDId, string UId, string Remarks)
        {
            var obj = new
            {
                CTDId = CTDId,
                UId = UId,
                Remarks = Remarks,
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Deployment/AddDelinkHL";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        [HttpPost]
        public JsonResult RemoveArvlContainerById(int DHLTId)
        {
            string endpoint = "api/Deployment/RemoveArvlContainerById?DHLTId=" + DHLTId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.GetRequest(endpoint);
            return Json(output);
        }
        [HttpPost]
        public JsonResult RemoveArvlHKLById(int DHLTId)
        {
            string endpoint = "api/Deployment/RemoveArvlHKLById?DHLTId=" + DHLTId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.GetRequest(endpoint);
            return Json(output);
        }
        [HttpPost]
        public JsonResult GetAllHKLForOperation(int TSId)
        {
            string endpoint = "api/Deployment/GetAllHKLForOperation?TSId=" + TSId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.GetRequest(endpoint);
            return Json(output);
        }
        public JsonResult AddHKL(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string VUId = dresult.VUId;
            string CUId = dresult.CUId;
            string CDHLTId = dresult.CDHLTId;
            string CTSId = dresult.CTSId;
            string PCId = dresult.PCId;
            DateTime LastTDate = Convert.ToDateTime(dresult.LastTDate);
            DateTime TDate = Convert.ToDateTime(dresult.TDate);
            var obj = new
            {
                VUId = VUId,
                CUId = CUId,
                CDHLTId = CDHLTId,
                CTSId = CTSId,
                PCId = PCId,
                LastTDate = LastTDate,
                TDate = TDate,
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Deployment/AddHKL";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        public JsonResult AddManualWBRelease(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string PCId = dresult.PCId;
            string CDHLTId = dresult.CDHLTId;
            string VDHLTId = dresult.VDHLTId;
            string VUID = dresult.VUID;
            string VehicleNo = dresult.VehicleNo;
            string TSId = dresult.TSId;
            string OperationTypeId = dresult.OperationTypeId;
            string TSName = dresult.TSName;
            string GrossWt = dresult.GrossWt;
            string TareWt = dresult.TareWt;
            string NetWt = dresult.NetWt;

            DateTime TDate = Convert.ToDateTime(dresult.TDate);
            var obj = new
            {
                PCId = PCId,
                CDHLTId = CDHLTId,
                VDHLTId = VDHLTId,
                VUID = VUID,
                VehicleNo = VehicleNo,
                TSId = TSId,
                OperationTypeId = OperationTypeId,
                TSName = TSName,
                GrossWt = GrossWt,
                TareWt = TareWt,
                NetWt = NetWt,
                TDate = TDate,
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Deployment/AddManualWBRelease";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [CustomAuthorize]
        public IActionResult AllEntityDeployment()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllDeployedEntity_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllDeployedEntity_Paging";
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
        public JsonResult GetAllEntityInfo()
        {
            string endpoint = "api/Deployment/GetAllEntityInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult GetEntityTrans(DateTime TDate, string UId, string EntityType, int VehicleTypeId)
        {
            var obj = new
            {
                UId = UId,
                EntityType = EntityType,
                TDate = TDate,
                VehicleTypeId = VehicleTypeId
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Deployment/GetEntityTrans";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }


        [HttpPost]
        public JsonResult GetVehicleDepoyment(DataTableAjaxPostModel requestModel)
        {
            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = requestModel.FromDate != null ? Convert.ToDateTime(requestModel.FromDate) : CommonHelper.IndianStandard(DateTime.UtcNow);


            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetVehicleDepoyment";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }


        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetDeploymentTsReport(DataTableAjaxPostModel requestModel)
        {
            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = requestModel.FromDate != null ? Convert.ToDateTime(requestModel.FromDate) : CommonHelper.IndianStandard(DateTime.UtcNow);


            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetDeploymentTsReport";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        [HttpPost]
        public FileResult ExportAllEntityDeployment(DateTime FromDate, DateTime ToDate, string ZoneId, string CircleId, string WardId, string VehicleUid, string NotiId, string FName,string Ownertype,string ShiftId)
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
            requestModel.FromDate = FromDate;
            requestModel.ToDate = ToDate;

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : string.Empty;
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : string.Empty;
            requestModel.Route = !string.IsNullOrEmpty(Ownertype) ? Ownertype : string.Empty;
            requestModel.CategoryId = !string.IsNullOrEmpty(ShiftId) ? ShiftId : string.Empty;
            requestModel.ContratorId = "1";
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllDeployedEntity_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllEntityDeployment(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public ActionResult ExportAllEntityDeploymentCSV(DateTime FromDate, DateTime ToDate, string ZoneId, string CircleId, string WardId, string VehicleUid, string NotiId, string FName)
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
            requestModel.FromDate = FromDate;
            requestModel.ToDate = ToDate;

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : string.Empty;
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : string.Empty;
            requestModel.ContratorId = "1";
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllDeployedEntity_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllEntityDeploymentCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }


        [HttpPost]
        public FileResult ExportAllDeployment(string FromDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string NotiId, string FName)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = FromDate != null ? Convert.ToDateTime(FromDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetVehicleDepoyment";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllVehicleDeploySummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }

        // [CustomAuthorize]  
        public IActionResult AllVDeployment()
        {
            return View();
        }


        //[CustomAuthorize]
        public IActionResult DepAtRpt()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllVDeployment_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllVDeployment_Paging";
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
        public JsonResult GetAllSATTrips_Paging(DataTableAjaxPostModel requestModel)  // Add function for seprate sat trips report
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = this.User.GetTSId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllSATTrips_Paging";
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
        public FileResult ExportAllVDeployment(DateTime FromDate, DateTime ToDate, string ZoneId, string CircleId, string WardId, string VehicleUid, string NotiId, string FName, string VehicleTypeId)
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
            requestModel.FromDate = FromDate;
            requestModel.ToDate = ToDate;

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : string.Empty;
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.ContratorId = "1";
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllVDeployment_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllVDeployment(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public ActionResult ExportCSV(DateTime FromDate, DateTime ToDate, string ZoneId, string CircleId, string WardId, string VehicleUid, string NotiId, string FName, string VehicleTypeId)
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
            requestModel.FromDate = FromDate;
            requestModel.ToDate = ToDate;

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : string.Empty;
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.ContratorId = "1";
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllVDeployment_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllVDeploymentToCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }
        [HttpPost]
        public ActionResult ExportSATCSV(DateTime FromDate, DateTime ToDate, string ZoneId, string CircleId, string WardId, string VehicleUid, string NotiId, string FName, string VehicleTypeId)
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
            requestModel.FromDate = FromDate;
            requestModel.ToDate = ToDate;

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : string.Empty;
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.ContratorId = "1";
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllSATTrips_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllSATTripsDataToCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }
        [HttpPost]
        public FileResult ExportAllSATTrips(DateTime FromDate, DateTime ToDate, string ZoneId, string CircleId, string WardId, string VehicleUid, string NotiId, string FName, string VehicleTypeId)
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
            requestModel.FromDate = FromDate;
            requestModel.ToDate = ToDate;

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : string.Empty;
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.ContratorId = "1";
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllSATTrips_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllSATTripsData(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public JsonResult GetAllVehicleTypeByLogin()
        {
            string LoginId = this.User.GetUserId();
            string AppType = "Web";
            string endpoint = "api/Deployment/GetAllVehicleTypeByLogin?LoginId=" + LoginId + "&AppType=" + AppType;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.GetRequest(endpoint);
            return Json(output);
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllAttendencePaging(DataTableAjaxPostModel requestModel)
        {

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = requestModel.FromDate != null ? Convert.ToDateTime(requestModel.FromDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetAllAttendencePaging";
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
        public FileResult ExportAllATDSummary(string FromDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string Trip, string FName, string TSId)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = FromDate != null ? Convert.ToDateTime(FromDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.TripCount = !string.IsNullOrEmpty(Trip) ? Trip : "0";
            requestModel.TSId = !string.IsNullOrEmpty(TSId) ? TSId : "0";


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Deployment/GetDeploymentTsReport";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            string input1 = JsonConvert.SerializeObject(requestModel);
            string endpoint1 = "api/Deployment/GetAllAttendencePaging";
            HttpClientHelper<string> apiobj1 = new HttpClientHelper<string>();

            string Result1 = apiobj1.PostRequestString(endpoint1, input1);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";

            filearray = _report.ExportAllAttendenceDeploymentSummary(Result, Name, Sname, Result1);

            return File(filearray, ContentType, filename);
        }

        //[CustomAuthorize]
        public IActionResult AllDeployedNotDeployedSummary()
        {
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetVehicleDeployedSummary(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);


            var obj = new
            {
                ZoneId = item.GetValue("ZoneId").ToString(),
                CircleId = item.GetValue("CircleId").ToString(),
                WardId = item.GetValue("WardId").ToString(),
                VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
                SDate = item.GetValue("SDate").ToString(),
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetVehicleDeployedSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(output);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;
            JArray _lstWard = FResult.Table2;
            JArray _lstVehicleType = FResult.Table3;
            JArray _lstcircle = FResult.Table4;


            List<HDZoneInfo> zonelst = _lst1.ToObject<List<HDZoneInfo>>();
            List<DepOutDataInfo_New> outlst = _lstout.ToObject<List<DepOutDataInfo_New>>();

            List<HDWardInfo> Wardlst = _lstWard.ToObject<List<HDWardInfo>>();
            List<HDVehicleTypeInfo> vehicletypelst = _lstVehicleType.ToObject<List<HDVehicleTypeInfo>>();
            List<HDCircleInfo> ciclelst = _lstcircle.ToObject<List<HDCircleInfo>>();


            zonelst.Select(i =>
            {
                i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
                i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
                i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.NotDeployed);
                //i.Outlst = outlst.Where(j => j.ZId == i.ZId).ToList();
                //i.TotalZRowCount = i.Outlst.Count();
                //i.TotalCircleRowCount = i.Outlst.Select(i => i.CircleId).Distinct().Count();
                //i.TotalWardRowCount = i.Outlst.Select(i => i.WardId).Distinct().Count();
                List<HDCircleInfo> circllst = ciclelst.Where(j => j.ZoneId == i.ZId).ToList();
                List<HDCircleInfo> icllst = new List<HDCircleInfo>();
                foreach (HDCircleInfo j in circllst)
                {
                    HDCircleInfo info = new HDCircleInfo();

                    info.CircleId = j.CircleId;
                    info.CircleCode = j.CircleCode;
                    info.CircleName = j.CircleName;
                    info.ZoneId = j.ZoneId;
                    info.Wardlst = Wardlst.Where(k => j.CircleId == k.CirlceId).Select(k => new HDWardInfo()
                    {
                        WardId = k.WardId,
                        WardNo = k.WardNo,
                        WardName = k.WardName,
                        CirlceId = k.CirlceId,
                        TotalWardRowCount = vehicletypelst.Count(),
                        VehicleData = vehicletypelst.Select(mm => new HDVehicleTypeInfo()
                        {
                            VehicleType = mm.VehicleType,
                            VehicleTypeId = mm.VehicleTypeId,
                            Deployed = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Deployed),
                            NotDeployed = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.NotDeployed),
                            DepPercentage = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.DepPercentage),
                            //Condemed = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Condemed),
                            //TotalAsset = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.TotalAsset)
                        }).ToList()


                    }).ToList();
                    info.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId > 0).Count();
                    icllst.Add(info);

                    HDCircleInfo iinfo = new HDCircleInfo();
                    List<HDVehicleTypeInfo> hvehicletype = new List<HDVehicleTypeInfo>();
                    HDVehicleTypeInfo vdata = new HDVehicleTypeInfo();
                    vdata.VehicleTypeId = 0;
                    vdata.VehicleType = string.Empty;
                    vdata.Deployed = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Deployed);
                    vdata.NotDeployed = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.NotDeployed);
                    vdata.DepPercentage = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.DepPercentage);
                    //vdata.Condemed = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Condemed);
                    //vdata.TotalAsset = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.TotalAsset);
                    hvehicletype.Add(vdata);

                    iinfo.CircleId = j.CircleId;
                    iinfo.CircleName = j.CircleName;
                    iinfo.ZoneId = j.ZoneId;
                    iinfo.CircleCode = info.CircleCode + " Total";
                    iinfo.Wardlst = new List<HDWardInfo>();
                    iinfo.Wardlst.Add(new HDWardInfo { WardId = 0, WardNo = string.Empty, WardName = string.Empty, CirlceId = j.CircleId, TotalWardRowCount = 1, VehicleData = hvehicletype });
                    iinfo.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId == 0).Count();
                    icllst.Add(iinfo);
                }

                i.Circlelst = icllst;
                i.TotalZRowCount = outlst.Where(mm => mm.ZId == i.ZId).Count();//i.Circlelst.Count();
                return i;
            }
            ).ToList();

            var Result = new { ZoneData = zonelst, VehicleoutData = outlst };
            return Json(Result);
        }//Deployed Vs Not Deployed Summary
        //[HttpPost]
        //[CustomPostAuthorize]
        //public JsonResult GetVehicleDeployedSummary(string Jobjval)
        //{
        //    JObject item = JObject.Parse(Jobjval);


        //    var obj = new
        //    {
        //        ZoneId = item.GetValue("ZoneId").ToString(),
        //        CircleId = item.GetValue("CircleId").ToString(),
        //        WardId = item.GetValue("WardId").ToString(),
        //        VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
        //        SDate = item.GetValue("SDate").ToString(),
        //        UserId = this.User.GetUserId()
        //    };

        //    string input = JsonConvert.SerializeObject(obj);
        //    string endpoint = "api/Asset/GetVehicleDeployedSummary";
        //    HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
        //    string output = apiobj.PostRequestString(endpoint, input);

        //    dynamic FResult = JObject.Parse(output);
        //    JArray _lst1 = FResult.Table;
        //    JArray _lstout = FResult.Table1;
        //    //JArray _lstcircle = FResult.Table1;
        //    //JArray _lstward = FResult.Table2;
        //    //JArray _lstvehicledata = FResult.Table3;
        //    //JArray _lstvehicletype = FResult.Table4;

        //    List<DepZoneInfo> zonelst = _lst1.ToObject<List<DepZoneInfo>>();
        //    List<DepOutDataInfo_New> outlst = _lstout.ToObject<List<DepOutDataInfo_New>>();
        //    //List<DepCircleInfo> circlelst = _lstcircle.ToObject<List<DepCircleInfo>>();
        //    //List<DepVehicleTypeInfo> vehicletypelst = _lstvehicletype.ToObject<List<DepVehicleTypeInfo>>();
        //    //List<DepVehicleDataInfo> vehicledatalst = _lstvehicledata.ToObject<List<DepVehicleDataInfo>>();
        //    //List<DepWardInfo> Wardlst = _lstward.ToObject<List<DepWardInfo>>();

        //    // Wardlst.Select(i =>
        //    //   {

        //    //       i.VehicleData = vehicledatalst.Where(kk => kk.WardId == i.WardId).ToList();
        //    //       return i;

        //    //   }).ToList();

        //    // circlelst.Select(k =>
        //    //{

        //    //    k.Wardlst = Wardlst.Where(j => k.CircleId == j.CirlceId).ToList();
        //    //    return k;

        //    //}).ToList();


        //    // zonelst.Select(i =>
        //    // {
        //    //     int rcount = 0;

        //    //     List<DepCircleInfo> incirclelst = circlelst.Where(k => k.ZoneId == i.ZId).ToList();
        //    //     i.Circlelst = incirclelst;

        //    //     rcount = Wardlst.Count(k => incirclelst.Any(m => m.CircleId == k.CirlceId));
        //    //     i.TotalZRowCount = vehicletypelst.Count() * rcount;
        //    //     i.TotalCircleRowCount = (vehicletypelst.Count() * rcount) / incirclelst.Count();
        //    //     i.TotalWardRowCount = vehicletypelst.Count();
        //    //     return i;
        //    // }).ToList();

        //    zonelst.Select(i =>
        //    {
        //        i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
        //        i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
        //        i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.NotDeployed);
        //        //i.Outlst = outlst.Where(j => j.ZId == i.ZId).ToList();
        //        //i.TotalZRowCount = i.Outlst.Count();
        //        //i.TotalCircleRowCount = i.Outlst.Select(i => i.CircleId).Distinct().Count();
        //        //i.TotalWardRowCount = i.Outlst.Select(i => i.WardId).Distinct().Count();
        //        return i;
        //    }
        //    ).ToList();

        //    var Result = new { ZoneData = zonelst, VehicleoutData = outlst };
        //    return Json(Result);
        //}


        public IActionResult VehicleDepNotDepSummary()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllDepVsNotDepInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            requestModel.CategoryId = !string.IsNullOrEmpty(requestModel.CategoryId) ? requestModel.CategoryId : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetAllDepVsNotDepInfo";
            //string endpoint = "api/Asset/GetAllVehicleInfoB64";
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
        public FileResult ExportAllVehicleDepNotDepSummary(string VehicleTypeId, string ZoneId, string CircleId, string WardId, string SDate, string FName)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = SDate != null ? Convert.ToDateTime(SDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetVehicleDeployedSummaryExcel";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(Result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportDeployedVsNotDeployedSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        [CustomAuthorize]
        public IActionResult AllDeployedVsReportedSummary()
        {
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetVehicleDeployedVsReportedSummary(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);


            var obj = new
            {
                ZoneId = item.GetValue("ZoneId").ToString(),
                CircleId = item.GetValue("CircleId").ToString(),
                WardId = item.GetValue("WardId").ToString(),
                VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
                SDate = item.GetValue("SDate").ToString(),
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetVehicleDeployedVsReportedSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(output);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;
            JArray _lstWard = FResult.Table2;
            JArray _lstVehicleType = FResult.Table3;
            JArray _lstcircle = FResult.Table4;


            List<HDZoneInfo> zonelst = _lst1.ToObject<List<HDZoneInfo>>();
            List<RepDataInfo> outlst = _lstout.ToObject<List<RepDataInfo>>();
            //List<DepCircleInfo> circlelst = _lstcircle.ToObject<List<DepCircleInfo>>();
            //List<DepVehicleTypeInfo> vehicletypelst = _lstvehicletype.ToObject<List<DepVehicleTypeInfo>>();
            //List<DepVehicleDataInfo> vehicledatalst = _lstvehicledata.ToObject<List<DepVehicleDataInfo>>();
            List<HDWardInfo> Wardlst = _lstWard.ToObject<List<HDWardInfo>>();
            List<HDVehicleTypeInfo> vehicletypelst = _lstVehicleType.ToObject<List<HDVehicleTypeInfo>>();
            List<HDCircleInfo> ciclelst = _lstcircle.ToObject<List<HDCircleInfo>>();


            zonelst.Select(i =>
            {
                i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
                i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
                i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.ReportedUnique);

                List<HDCircleInfo> circllst = ciclelst.Where(j => j.ZoneId == i.ZId).ToList();
                List<HDCircleInfo> icllst = new List<HDCircleInfo>();
                foreach (HDCircleInfo j in circllst)
                {
                    HDCircleInfo info = new HDCircleInfo();

                    info.CircleId = j.CircleId;
                    info.CircleCode = j.CircleCode;
                    info.CircleName = j.CircleName;
                    info.ZoneId = j.ZoneId;
                    info.Wardlst = Wardlst.Where(k => j.CircleId == k.CirlceId).Select(k => new HDWardInfo()
                    {
                        WardId = k.WardId,
                        WardNo = k.WardNo,
                        WardName = k.WardName,
                        CirlceId = k.CirlceId,
                        TotalWardRowCount = vehicletypelst.Count(),
                        VehicleData = vehicletypelst.Select(mm => new HDVehicleTypeInfo()
                        {
                            VehicleType = mm.VehicleType,
                            VehicleTypeId = mm.VehicleTypeId,
                            Deployed = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Deployed),
                            ReportedUnique = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.ReportedUnique),
                            Trips1 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips1),
                            Trips2 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips2),
                            Trips3 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips3),
                            Trips4 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips4),
                            MoreThan4Trips = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.MoreThan4Trips)
                        }).ToList()


                    }).ToList();
                    info.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId > 0).Count();
                    icllst.Add(info);

                    HDCircleInfo iinfo = new HDCircleInfo();
                    List<HDVehicleTypeInfo> hvehicletype = new List<HDVehicleTypeInfo>();
                    HDVehicleTypeInfo vdata = new HDVehicleTypeInfo();
                    vdata.VehicleTypeId = 0;
                    vdata.VehicleType = string.Empty;
                    vdata.Deployed = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Deployed);
                    vdata.ReportedUnique = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.ReportedUnique);
                    vdata.Trips1 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips1);
                    vdata.Trips2 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips2);
                    vdata.Trips3 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips3);
                    vdata.Trips4 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips4);
                    vdata.Trips3 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.MoreThan4Trips);
                    hvehicletype.Add(vdata);

                    iinfo.CircleId = j.CircleId;
                    iinfo.CircleName = j.CircleName;
                    iinfo.ZoneId = j.ZoneId;
                    iinfo.CircleCode = info.CircleCode + " Total";
                    iinfo.Wardlst = new List<HDWardInfo>();
                    iinfo.Wardlst.Add(new HDWardInfo { WardId = 0, WardNo = string.Empty, WardName = string.Empty, CirlceId = j.CircleId, TotalWardRowCount = 1, VehicleData = hvehicletype });
                    iinfo.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId == 0).Count();
                    icllst.Add(iinfo);
                }

                i.Circlelst = icllst;
                i.TotalZRowCount = outlst.Where(mm => mm.ZId == i.ZId).Count();//i.Circlelst.Count();
                return i;
            }
            ).ToList();

            var Result = new { ZoneData = zonelst, VehicleoutData = outlst };
            return Json(Result);
        }
        public IActionResult VehicleDeployedVsReportedPaging()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetVehicleDeployedVsReportedPaging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            requestModel.CategoryId = !string.IsNullOrEmpty(requestModel.CategoryId) ? requestModel.CategoryId : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetVehicleDeployedVsReportedPaging";
            //string endpoint = "api/Asset/GetAllVehicleInfoB64";
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
        public FileResult ExportAllDeployedVsReportedSummary(string VehicleTypeId, string ZoneId, string CircleId, string WardId, string SDate, string FName)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = SDate != null ? Convert.ToDateTime(SDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/ExportDeployedVsReportedSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(Result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportDeployedVsNotDeployedSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public FileResult ExportDeployedVsReportedSummary(string VehicleTypeId, string ZoneId, string CircleId, string WardId, string SDate, string FName)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = SDate != null ? Convert.ToDateTime(SDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetDeployedVsReportedSummaryExcel";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(Result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportDeployedVsReportedVehicleSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }

        // Deployed Vs Not Reported
        [CustomAuthorize]
        public IActionResult DeployVsNotReportedSummary()
        {
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetDeployedVsNotReportedSummary(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);
            var obj = new
            {
                ZoneId = item.GetValue("ZoneId").ToString(),
                CircleId = item.GetValue("CircleId").ToString(),
                WardId = item.GetValue("WardId").ToString(),
                VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
                SDate = item.GetValue("SDate").ToString(),
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetDeployedVsNotReportedSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(output);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;
            JArray _lstWard = FResult.Table2;
            JArray _lstVehicleType = FResult.Table3;
            JArray _lstcircle = FResult.Table4;


            List<HDZoneInfo> zonelst = _lst1.ToObject<List<HDZoneInfo>>();
            List<DepOutDataInfo_New> outlst = _lstout.ToObject<List<DepOutDataInfo_New>>();
            //List<DepCircleInfo> circlelst = _lstcircle.ToObject<List<DepCircleInfo>>();
            //List<DepVehicleTypeInfo> vehicletypelst = _lstvehicletype.ToObject<List<DepVehicleTypeInfo>>();
            //List<DepVehicleDataInfo> vehicledatalst = _lstvehicledata.ToObject<List<DepVehicleDataInfo>>();
            List<HDWardInfo> Wardlst = _lstWard.ToObject<List<HDWardInfo>>();
            List<HDVehicleTypeInfo> vehicletypelst = _lstVehicleType.ToObject<List<HDVehicleTypeInfo>>();
            List<HDCircleInfo> ciclelst = _lstcircle.ToObject<List<HDCircleInfo>>();



            zonelst.Select(i =>
            {
                i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
                i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
                i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.NotReported);
                //i.Outlst = outlst.Where(j => j.ZId == i.ZId).ToList();
                //i.TotalZRowCount = i.Outlst.Count();
                //i.TotalCircleRowCount = i.Outlst.Select(i => i.CircleId).Distinct().Count();
                //i.TotalWardRowCount = i.Outlst.Select(i => i.WardId).Distinct().Count();
                List<HDCircleInfo> circllst = ciclelst.Where(j => j.ZoneId == i.ZId).ToList();
                List<HDCircleInfo> icllst = new List<HDCircleInfo>();
                foreach (HDCircleInfo j in circllst)
                {
                    HDCircleInfo info = new HDCircleInfo();

                    info.CircleId = j.CircleId;
                    info.CircleCode = j.CircleCode;
                    info.CircleName = j.CircleName;
                    info.ZoneId = j.ZoneId;
                    info.Wardlst = Wardlst.Where(k => j.CircleId == k.CirlceId).Select(k => new HDWardInfo()
                    {
                        WardId = k.WardId,
                        WardNo = k.WardNo,
                        WardName = k.WardName,
                        CirlceId = k.CirlceId,
                        TotalWardRowCount = vehicletypelst.Count(),
                        VehicleData = vehicletypelst.Select(mm => new HDVehicleTypeInfo()
                        {
                            VehicleType = mm.VehicleType,
                            VehicleTypeId = mm.VehicleTypeId,
                            Deployed = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Deployed),
                            NotReported = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.NotReported)

                        }).ToList()


                    }).ToList();
                    info.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId > 0).Count();
                    icllst.Add(info);

                    HDCircleInfo iinfo = new HDCircleInfo();
                    List<HDVehicleTypeInfo> hvehicletype = new List<HDVehicleTypeInfo>();
                    HDVehicleTypeInfo vdata = new HDVehicleTypeInfo();
                    vdata.VehicleTypeId = 0;
                    vdata.VehicleType = string.Empty;
                    vdata.Deployed = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Deployed);
                    vdata.NotReported = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.NotReported);

                    hvehicletype.Add(vdata);

                    iinfo.CircleId = j.CircleId;
                    iinfo.CircleName = j.CircleName;
                    iinfo.ZoneId = j.ZoneId;
                    iinfo.CircleCode = info.CircleCode + " Total";
                    iinfo.Wardlst = new List<HDWardInfo>();
                    iinfo.Wardlst.Add(new HDWardInfo { WardId = 0, WardNo = string.Empty, WardName = string.Empty, CirlceId = j.CircleId, TotalWardRowCount = 1, VehicleData = hvehicletype });
                    iinfo.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId == 0).Count();
                    icllst.Add(iinfo);
                }

                i.Circlelst = icllst;
                i.TotalZRowCount = outlst.Where(mm => mm.ZId == i.ZId).Count();//i.Circlelst.Count();
                return i;
            }
            ).ToList();

            var Result = new { ZoneData = zonelst, VehicleoutData = outlst };
            return Json(Result);
        }


        public IActionResult DeployedVsNotReported_Paging()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetDeployedVsNotReported(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            requestModel.CategoryId = !string.IsNullOrEmpty(requestModel.CategoryId) ? requestModel.CategoryId : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetDeployedVsNotReported_Paging";
            //string endpoint = "api/Asset/GetAllVehicleInfoB64";
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
        public FileResult ExportDeployedVsNotReported_Paging(string SDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string Status, string FName, string Typeid)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = SDate != null ? Convert.ToDateTime(SDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : string.Empty;
            requestModel.CategoryId = !string.IsNullOrEmpty(Typeid) ? Typeid : string.Empty;




            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetDeployedVsNotReported_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);





            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportDeployedVsNotReported_Paging(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public FileResult ExportDeployedVsNotReported(string VehicleTypeId, string ZoneId, string CircleId, string WardId, string SDate, string FName)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = SDate != null ? Convert.ToDateTime(SDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetDeployedVsNotReportedExcel";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(Result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportDeployedVsNotReportedSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        //Deployed Vs Not Reported
        //Not Deployed Vs Reported
        [CustomAuthorize]
        public IActionResult NotDeployedVsReportedSummary()
        {
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetNotDeployedVsReportedSummary(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);


            var obj = new
            {
                ZoneId = item.GetValue("ZoneId").ToString(),
                CircleId = item.GetValue("CircleId").ToString(),
                WardId = item.GetValue("WardId").ToString(),
                VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
                SDate = item.GetValue("SDate").ToString(),
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetNotDeployedVsReportedSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(output);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;
            JArray _lstWard = FResult.Table2;
            JArray _lstVehicleType = FResult.Table3;
            JArray _lstcircle = FResult.Table4;


            List<HDZoneInfo> zonelst = _lst1.ToObject<List<HDZoneInfo>>();
            List<RepDataInfo> outlst = _lstout.ToObject<List<RepDataInfo>>();
            //List<DepCircleInfo> circlelst = _lstcircle.ToObject<List<DepCircleInfo>>();
            //List<DepVehicleTypeInfo> vehicletypelst = _lstvehicletype.ToObject<List<DepVehicleTypeInfo>>();
            //List<DepVehicleDataInfo> vehicledatalst = _lstvehicledata.ToObject<List<DepVehicleDataInfo>>();
            List<HDWardInfo> Wardlst = _lstWard.ToObject<List<HDWardInfo>>();
            List<HDVehicleTypeInfo> vehicletypelst = _lstVehicleType.ToObject<List<HDVehicleTypeInfo>>();
            List<HDCircleInfo> ciclelst = _lstcircle.ToObject<List<HDCircleInfo>>();


            zonelst.Select(i =>
            {
                i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
                i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
                i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.ReportedUnique);

                List<HDCircleInfo> circllst = ciclelst.Where(j => j.ZoneId == i.ZId).ToList();
                List<HDCircleInfo> icllst = new List<HDCircleInfo>();
                foreach (HDCircleInfo j in circllst)
                {
                    HDCircleInfo info = new HDCircleInfo();

                    info.CircleId = j.CircleId;
                    info.CircleCode = j.CircleCode;
                    info.CircleName = j.CircleName;
                    info.ZoneId = j.ZoneId;
                    info.Wardlst = Wardlst.Where(k => j.CircleId == k.CirlceId).Select(k => new HDWardInfo()
                    {
                        WardId = k.WardId,
                        WardNo = k.WardNo,
                        WardName = k.WardName,
                        CirlceId = k.CirlceId,
                        TotalWardRowCount = vehicletypelst.Count(),
                        VehicleData = vehicletypelst.Select(mm => new HDVehicleTypeInfo()
                        {
                            VehicleType = mm.VehicleType,
                            VehicleTypeId = mm.VehicleTypeId,
                            NotDeployed = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.NotDeployed),
                            ReportedUnique = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.ReportedUnique),
                            Trips1 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips1),
                            Trips2 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips2),
                            Trips3 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips3),
                            Trips4 = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Trips4),
                            MoreThan4Trips = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.MoreThan4Trips)
                        }).ToList()


                    }).ToList();
                    info.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId > 0).Count();
                    icllst.Add(info);

                    HDCircleInfo iinfo = new HDCircleInfo();
                    List<HDVehicleTypeInfo> hvehicletype = new List<HDVehicleTypeInfo>();
                    HDVehicleTypeInfo vdata = new HDVehicleTypeInfo();
                    vdata.VehicleTypeId = 0;
                    vdata.VehicleType = string.Empty;
                    vdata.NotDeployed = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.NotDeployed);
                    vdata.ReportedUnique = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.ReportedUnique);
                    vdata.Trips1 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips1);
                    vdata.Trips2 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips2);
                    vdata.Trips3 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips3);
                    vdata.Trips4 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Trips4);
                    vdata.Trips3 = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.MoreThan4Trips);
                    hvehicletype.Add(vdata);

                    iinfo.CircleId = j.CircleId;
                    iinfo.CircleName = j.CircleName;
                    iinfo.ZoneId = j.ZoneId;
                    iinfo.CircleCode = info.CircleCode + " Total";
                    iinfo.Wardlst = new List<HDWardInfo>();
                    iinfo.Wardlst.Add(new HDWardInfo { WardId = 0, WardNo = string.Empty, WardName = string.Empty, CirlceId = j.CircleId, TotalWardRowCount = 1, VehicleData = hvehicletype });
                    iinfo.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId == 0).Count();
                    icllst.Add(iinfo);
                }

                i.Circlelst = icllst;
                i.TotalZRowCount = outlst.Where(mm => mm.ZId == i.ZId).Count();//i.Circlelst.Count();
                return i;
            }
            ).ToList();

            var Result = new { ZoneData = zonelst, VehicleoutData = outlst };
            return Json(Result);
        }
        //[CustomAuthorize]
        public IActionResult NotDeployedVsReportedPaging()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetNoDeployedVsReportedPaging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            requestModel.CategoryId = !string.IsNullOrEmpty(requestModel.CategoryId) ? requestModel.CategoryId : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetNotDeployedVsReportedPaging";
            //string endpoint = "api/Asset/GetAllVehicleInfoB64";
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
        public FileResult ExportNotDeployedVsReportedSummary(string VehicleTypeId, string ZoneId, string CircleId, string WardId, string SDate, string FName)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = SDate != null ? Convert.ToDateTime(SDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetNotDeployedVsReportedSummaryExcel";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(Result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportNotDeployedVsReportedSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        public FileResult ExportNotDeployedVsReported_Paging(string VehicleTypeId, string ZoneId, string CircleId, string WardId, string SDate, string Status, string Typeid, string FName)
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

            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = SDate != null ? Convert.ToDateTime(SDate) : CommonHelper.IndianStandard(DateTime.UtcNow);

            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : string.Empty;
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : string.Empty;
            requestModel.CategoryId = !string.IsNullOrEmpty(Typeid) ? Typeid : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetNotDeployedVsReportedPaging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportNotDeployedVsReported_Paging(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }

        // Not Deployed Vs  Reported
    }
}
