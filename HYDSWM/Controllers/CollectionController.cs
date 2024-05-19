using COMMON;
using COMMON.ASSET;
using COMMON.COLLECTION;
using HYDSWM;
using HYDSWM.Helpers;
using HYDSWMAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DEMOSWMCKC.Controllers
{
    public class CollectionController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private TReport _report;
        static string apiBaseUrl = Startup.StaticConfig.GetValue<string>("WebAPIBaseUrl");
        static string BasicAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("patna:patna#2020"));
        public CollectionController(IWebHostEnvironment host, TReport report)
        {
            this._host = host;
            this._report = report;
        }
        [CustomAuthorize]
        public IActionResult AllGeoPoint()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllGeoPoint_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPoint_Paging";
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
        public IActionResult AllEmergencyPoint()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }
        [HttpPost]
        public JsonResult GetAllEmergencyPoint_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllEmergencyPoint_Paging";
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
        public JsonResult GetGeoPointInfoById(int GeoPointId)
        {
            string endpoint = "api/Collection/GetGeoPointInfoById?GeoPointId=" + GeoPointId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetEmergencyPointMasterInfoById(int GeoPointId)
        {
            string endpoint = "api/Collection/GetEmergencyPointMasterInfoById?GeoPointId=" + GeoPointId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        private byte[] ConvertToBytes(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        [HttpPost]
        [CustomPostAuthorize]
        public async Task<JsonResult> AddGeoLocationSurvey([FromForm] IFormCollection formData)
        {

            var url = apiBaseUrl + "api/Collection/AddGeoLocationSurvey"; ;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", BasicAuth);
            MultipartFormDataContent form = new MultipartFormDataContent();

            if (formData.Files.Count > 0)
            {
                var filePath = formData.Files[0].FileName;



                if (CommonHelper.ExtensionType(Path.GetExtension(filePath)))
                {
                    byte[] filep = ConvertToBytes(formData.Files[0]); //File.ReadAllBytes(filePath);
                    Stream frequestStream = new MemoryStream(filep);
                    var streamContent = new StreamContent(frequestStream);


                    //  var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
                    var imageContent = new ByteArrayContent(filep);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    // imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                    form.Add(imageContent, "image", filePath);
                }
                else
                {
                    var Response = new { Result = 0, Msg = "Invalid File Extension" };
                    var EResult = JsonConvert.SerializeObject(Response);
                    return Json(EResult);
                }



            }
            form.Add(new StringContent(formData["Input"]), "Input");

            var response = await httpClient.PostAsync(url, form);
            string stringContent = await response.Content.ReadAsStringAsync();
            return Json(stringContent);
        }
        [HttpPost]
        public async Task<JsonResult> AddEmergencyLocationSurvey([FromForm] IFormCollection formData)
        {

            var url = apiBaseUrl + "api/Collection/AddEmergencyLocationSurvey"; ;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", BasicAuth);
            MultipartFormDataContent form = new MultipartFormDataContent();

            if (formData.Files.Count > 0)
            {
                var filePath = formData.Files[0].FileName;
                byte[] filep = ConvertToBytes(formData.Files[0]); //File.ReadAllBytes(filePath);
                Stream frequestStream = new MemoryStream(filep);
                var streamContent = new StreamContent(frequestStream);


                //  var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
                var imageContent = new ByteArrayContent(filep);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                // imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                form.Add(imageContent, "image", filePath);
            }
            form.Add(new StringContent(formData["Input"]), "Input");

            var response = await httpClient.PostAsync(url, form);
            string stringContent = await response.Content.ReadAsStringAsync();
            return Json(stringContent);
        }

        [CustomAuthorize]
        public IActionResult AllNRoute()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AllNRouteInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllNRouteInfo";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult GetAllNStopByRouteCode(string RouteCode)
        {
            string endpoint = "api/Collection/GetAllNStopByRouteCode?RouteCode=" + RouteCode;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllNStopByTripId(int RTDId)
        {
            string endpoint = "api/Collection/GetAllNStopByTripId?RTDId=" + RTDId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddNRoute()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult AddNRouteInfo(string jobj, string JArrayval)
        {

            dynamic dresult = JObject.Parse(jobj);
            //var array = JArray.Parse(JArrayval);
            string msg = string.Empty;
            var obj = new
            {
                RouteId = dresult.RouteId
                            ,
                RouteName = dresult.RouteName
                            ,
                RouteCode = dresult.RouteCode
                ,
                IsActive = dresult.IsActive
                            ,
                UserId = this.User.GetUserId()
                            ,
                CCode = this.User.GetCompanyCode()
                            ,
                JArrayval = JArrayval
            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/AddNRouteInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }

        [HttpPost]
        public JsonResult GetNRouteInfoById(string RouteId)
        {
            string endpoint = "api/Collection/GetNRouteInfoById?RouteId=" + RouteId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddRoute()
        {
            return View();
        }

        [CustomAuthorize]
        public IActionResult AllPointCollectDetail()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllPointCollectionDetail_Paging(DataTableAjaxPostModel requestModel)
        {
            try
            {
                requestModel.CCode = this.User.GetCompanyCode();
                requestModel.UserId = this.User.GetUserId();
                string input = JsonConvert.SerializeObject(requestModel);
                string endpoint = "api/Collection/GetAllPointCollectionDetail_Paging";
                // string endpoint = "api/Collection/GetAllPointCollectionDetail_PagingB64";
                HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
            catch (Exception ex)
            {
                throw ex;

            }

        }
        // [CustomAuthorize]
        public IActionResult AllRouteWiseCollectDetail()
        {
            return View();
        }
        public IActionResult AllVehicleWiseEmergencyCollectDetail()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllRouteWiseCollection_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllRouteWiseCollection_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult GetAllColPointByRouteDate(string RouteId, DateTime TDate)
        {
            var obj = new
            {
                RouteId = RouteId,
                TDate = TDate
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/GetAllColPointByRouteDate";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult GetAllPointForTimeline(DateTime FromDate, DateTime ToDate, int RouteId, int ZoneId, int CircleId, int TripId, int PageSize)
        {
            var obj = new
            {
                FromDate = FromDate,
                ToDate = ToDate,
                UserId = this.User.GetUserId(),
                RouteId = RouteId,
                ZoneId = ZoneId,
                CircleId = CircleId,
                TripId = TripId,
                PageSize = PageSize,
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/GetAllPointForTimeline";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            List<NPointTimelineInfo> _lst = new List<NPointTimelineInfo>();

            if (!string.IsNullOrEmpty(output))
                _lst = JsonConvert.DeserializeObject<List<NPointTimelineInfo>>(output);
            return Json(_lst);
        }
        [HttpPost]
        public JsonResult GetAllEmerPointForTimeline(DateTime FromDate, DateTime ToDate, int ZoneId, int CircleId, int PageSize)
        {
            var obj = new
            {
                FromDate = FromDate,
                ToDate = ToDate,
                UserId = this.User.GetUserId(),

                ZoneId = ZoneId,
                CircleId = CircleId,

                PageSize = PageSize,
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/GetAllEmerPointForTimeline";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            List<NEmrPointTimelineInfo> _lst = new List<NEmrPointTimelineInfo>();

            if (!string.IsNullOrEmpty(output))
                _lst = JsonConvert.DeserializeObject<List<NEmrPointTimelineInfo>>(output);
            return Json(_lst);
        }

        //New Change for route wise collection
        public IActionResult AllRouteWiseCollectDetail_Copy()
        {
            return View();
        }
        //New Change for route wise collection
        [CustomAuthorize]
        public IActionResult AllSRoute()
        {
            return View();
        }
        public IActionResult AddSRoute()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetSRouteInfoById(string RouteId)
        {
            string endpoint = "api/Collection/GetSRouteInfoById?RouteId=" + RouteId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult AddSRouteInfo(string jobj)
        {

            dynamic dresult = JObject.Parse(jobj);
            //var array = JArray.Parse(JArrayval);
            string msg = string.Empty;
            var obj = new
            {
                RouteId = dresult.RouteId
                            ,
                //RouteName = dresult.RouteName
                //            ,
                RouteCode = dresult.RouteCode
                ,
                IsActive = dresult.IsActive
                            ,
                UserId = this.User.GetUserId()
                            ,
                CCode = this.User.GetCompanyCode()
                            ,
                ZoneId = dresult.ZoneId,
                CircleId = dresult.CircleId,
                ShiftId = dresult.ShiftId
            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/AddSRouteInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult AllSRouteInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllSRouteInfo";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        [CustomAuthorize]
        public IActionResult AllRouteTrip()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllRouteTripByRoute(string RouteId)
        {
            string endpoint = "api/Collection/GetAllRouteTripByRoute?RouteId=" + RouteId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddRouteTrip()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddRouteTrip(string RouteId, string JArrayval)
        {
            string msg = string.Empty;
            var obj = new
            {
                RouteId = RouteId
                            ,
                JArrayval = JArrayval
                         ,
                UserId = this.User.GetUserId()

            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/AddRouteTrip";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        [CustomAuthorize]
        public IActionResult AllTripPoint()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllTripPoint_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllTripPoint_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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

        //[HttpPost]
        //public FileResult ExportAllTripPoint( string FName)
        //{
        //    byte[] filearray = null;
        //    string ContentType = string.Empty;
        //    DateTime ReportTime = DateTime.Now;
        //    string Name = FName;
        //    string Sname = FName;
        //    string filename = string.Empty;
        //    DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
        //    requestModel.search = new Search();
        //    requestModel.order = new List<Order>();
        //    requestModel.order.Add(new Order { dir = "", column = 0 });

        //    requestModel.CCode = this.User.GetCompanyCode();
        //    requestModel.UserId = this.User.GetUserId();
        //    //requestModel.FromDate = FromDate;
        //    //requestModel.ToDate = ToDate;

        //    //requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
        //    //requestModel.NotiId = !string.IsNullOrEmpty(PointId) ? PointId : "0";
        //    requestModel.IsReport = 1;
        //    // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

        //    requestModel.length = -1;
        //    string input = JsonConvert.SerializeObject(requestModel);
        //    string endpoint = "api/Collection/GetAllTripPoint_Paging";
        //    HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

        //    string Result = apiobj.PostRequestString(endpoint, input);

        //    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    filename = Name + ReportTime.ToShortDateString() + ".xlsx";
        //    // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
        //    filearray = _report.ExportAllTripPoint(Result, Name);

        //    return File(filearray, ContentType, filename);
        //}
        [HttpPost]
        public FileResult ExportAllTripPoint(string FName)
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
            //requestModel.FromDate = FromDate;
            //requestModel.ToDate = ToDate;

            //requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            //requestModel.NotiId = !string.IsNullOrEmpty(PointId) ? PointId : "0";
            requestModel.IsReport = 1;
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllTripPoint_Paging_New";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllTripPoint(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public JsonResult GetPointsInfoByTripId(string RTDId)
        {
            string endpoint = "api/Collection/GetPointsInfoByTripId?RTDId=" + RTDId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddSTripPoint()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddSTripPointInfo(string jobj, string JArrayval)//KHSB
        {

            dynamic dresult = JObject.Parse(jobj);
            //var array = JArray.Parse(JArrayval);
            string msg = string.Empty;
            var obj = new
            {
                RouteId = dresult.RouteId
                            ,
                TripId = dresult.TripId
                            ,
                ShiftId= dresult.ShiftId,
                RouteCode = dresult.RouteCode
              ,
                JArrayval = JArrayval
            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/AddSTripPointInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }

        //[CustomAuthorize]
        public IActionResult AddVehicleToTrip()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddVehicleToTrip(string RTDId, string VehicleUId)
        {

            var obj = new
            {
                RTDId = RTDId
                            ,
                VehicleUId = VehicleUId

            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/AddVehicleToTrip";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        public IActionResult Timeline()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetNewRouteCode(string ZoneId, string CircleId)
        {
            string endpoint = "api/Collection/GetNewRouteCode?ZoneId=" + ZoneId + "&CircleId=" + CircleId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllRouteWiseCollectionSummary_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllRouteWiseCollectionSummary_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public IActionResult AllGeoPointsMapInfo(string ZoneId, string CircleId, string WardId, string NotiId, string Status)
        {
            byte[] filearray = null;
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string filename = string.Empty;

            DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
            requestModel.search = new Search();
            requestModel.order = new List<Order>();
            requestModel.order.Add(new Order { dir = "", column = 0 });

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "-1";
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPoint_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);
            return Json(Result);
        }
        public IActionResult AllEmergencyPointsMapInfo(string ZoneId, string CircleId, string WardId, string NotiId, string Status)
        {
            byte[] filearray = null;
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string filename = string.Empty;

            DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
            requestModel.search = new Search();
            requestModel.order = new List<Order>();
            requestModel.order.Add(new Order { dir = "", column = 0 });

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "-1";
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllEmergencyPoint_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);
            return Json(Result);
        }
        [CustomAuthorize]
        public IActionResult GeoPointsVisitSummary()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllGeoPointsVisitSummary(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPointsVisitSummary";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult GetAllGeoPointVisitByPointId(int Pointid, string ZoneId, string CircleId, string WardId, string RouteId, string TripId, int Visitingtype, DateTime FromDate, DateTime ToDate)
        {
            //if(FromDate.Date!=ToDate.Date)
            //{
            //    FromDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day,0,0,0);
            //}
            var bodyparam = new
            {
                Pointid = Pointid,
                ZoneId = ZoneId,
                CircleId = CircleId,
                WardId = WardId,
                RouteId = RouteId,
                TripId = TripId,
                Visitingtype = 0,
                FromDate = FromDate,
                ToDate = ToDate

            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/Collection/GetAllGeoPointVisitByPointId";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
            string Result = apiobj.PostRequestString(endpoint, input);

            return Json(Result);
        }
        [HttpPost]
        public FileResult ExportAllGeoPointsVisitSummary(DateTime FromDate, DateTime ToDate, string Route, string WardId, string ZoneId, string CircleId, string NotiId, string IsReport, int VisitingType, string Status, string CategoryId, string FName, string VehicleUid)
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
            requestModel.VisitingType = VisitingType;
            requestModel.Route = !string.IsNullOrEmpty(Route) ? Route : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.IsReport = 1;
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.CategoryId = !string.IsNullOrEmpty(CategoryId) ? CategoryId : "0";


            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPointsVisitSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllGeoPointsVisitSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public ActionResult ExportAllGeoPointsVisitCSV(DateTime FromDate, DateTime ToDate, string Route, string WardId, string ZoneId, string CircleId, string NotiId, string IsReport, int VisitingType, string Status, string CategoryId, string FName, string VehicleUid)
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
            requestModel.VisitingType = VisitingType;
            requestModel.Route = !string.IsNullOrEmpty(Route) ? Route : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "0";
            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.IsReport = 1;
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.CategoryId = !string.IsNullOrEmpty(CategoryId) ? CategoryId : "0";


            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPointsVisitSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllGeoPointsVisitCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }


        [CustomAuthorize]
        public IActionResult CollectionIndex()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllZoneandRouteWiseCollectionNoti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllZoneWiseCollectionNoti";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult GetAllPointNoti(string ZoneId, string CircleId, string WardId, string RouteId, string TripId, string fromDate)
        {
            var bodyparam = new
            {
                LoginId = this.User.GetUserId(),
                ZoneId = ZoneId,
                CircleId = CircleId,
                WardId = WardId,
                RouteId = RouteId,
                TripId = TripId,
                fromDate = fromDate,

            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/Collection/GetAllPointNoti";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
            string Result = apiobj.PostRequestString(endpoint, input);

            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllEmerGencyPointNoti(string ZoneId, string CircleId, string WardId, string fromDate)
        {
            var bodyparam = new
            {
                LoginId = this.User.GetUserId(),
                ZoneId = ZoneId,
                CircleId = CircleId,
                WardId = WardId,

                fromDate = fromDate,

            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/Collection/GetAllEmerGencyPointNoti";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
            string Result = apiobj.PostRequestString(endpoint, input);

            return Json(Result);
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllZoneWiseCltnNoti(string ZoneId, string CircleId, string WardId, string RouteId, string TripId, string fromDate)
        {
            var bodyparam = new
            {
                LoginId = this.User.GetUserId(),
                ZoneId = ZoneId,
                CircleId = CircleId,
                WardId = WardId,
                RouteId = RouteId,
                TripId = TripId,
                fromDate = fromDate,
            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/Collection/GetAllZoneWiseCltnNoti";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);

            return Json(Result);
        }
        public IActionResult AllGeoPointNoti()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllGeoPointNoti(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPointNoti_paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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

        public IActionResult AllRouteNoti()
        {
            return View();
        }
        public IActionResult AllRouteTripNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult AllRouteTripNotiInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllRouteTripNotiInfo";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public IActionResult AllPointCltnNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult AllPointCltNoti_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllPointCltNoti_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public IActionResult AllEmergencyPointCltnNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult AllEmergencyPointCltNoti_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllEmergencyPointCltNoti_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public IActionResult EarlyCompletion()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult AllPointEarlyCompletion_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllPointEarlyCompletion_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult GetAllEarlyCompletionByRouteId(string RouteId, string TypeId, string TripId, DateTime SDate)//SonuS
        {


            string msg = string.Empty;
            var obj = new
            {
                RouteId = RouteId,
                TypeId = TypeId,
                TripId = TripId,
                SDate = SDate
            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/GetAllEarlyCompletionByRouteId";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);

        }
        [CustomAuthorize]
        public IActionResult AllGeoPointNotCollected()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult AllGeoPointNotCollected_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllGeoPointNotCollected_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public FileResult ExportAllGeoPointNotCollected_Paging(DateTime FromDate, DateTime ToDate, string WardId, string ZoneId, string CircleId, string NotiId, string Status, string CategoryId, string FName)
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
            requestModel.FromDate = FromDate;
            requestModel.ToDate = ToDate;
            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";

            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "0";

            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.CategoryId = !string.IsNullOrEmpty(CategoryId) ? CategoryId : "0";


            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllGeoPointNotCollected_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllGeoPointNotCollected_Paging(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public ActionResult ExportAllGeoPointNotCollected_PagingCSV(DateTime FromDate, DateTime ToDate, string WardId, string ZoneId, string CircleId, string NotiId, string Status, string CategoryId, string FName)
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

            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "0";

            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.CategoryId = !string.IsNullOrEmpty(CategoryId) ? CategoryId : "0";


            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllGeoPointNotCollected_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllGeoPointNotCollected_PagingCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }



        [HttpPost]
        public IActionResult GetAllVisitSummaryForMap_Paging(string ZoneId, string CircleId, string WardId, string Status, string routeId, string TripId, string VisitCount, DateTime SDate)
        {
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string filename = string.Empty;

            DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
            requestModel.search = new Search();
            requestModel.order = new List<Order>();
            requestModel.order.Add(new Order { dir = "", column = 0 });

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            // requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "-1";
            //requestModel.Route = "0";
            //requestModel.NotiId = "0";
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.Route = !string.IsNullOrEmpty(routeId) ? routeId : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(TripId) ? TripId : "0";
            requestModel.ContratorId = !string.IsNullOrEmpty(VisitCount) ? VisitCount : "-1";

            requestModel.length = -1;
            requestModel.FromDate = SDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllVisitSummaryForMap_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);
            return Json(Result);
        }
        [HttpPost]
        public IActionResult GetAllVisitSummaryForEmergencyMap_Paging(string ZoneId, string CircleId, string WardId, string Status, string VisitCount, DateTime SDate)
        {
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string filename = string.Empty;

            DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
            requestModel.search = new Search();
            requestModel.order = new List<Order>();
            requestModel.order.Add(new Order { dir = "", column = 0 });

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            // requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "-1";
            //requestModel.Route = "0";
            //requestModel.NotiId = "0";
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";

            requestModel.ContratorId = !string.IsNullOrEmpty(VisitCount) ? VisitCount : "-1";

            requestModel.length = -1;
            requestModel.FromDate = SDate;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllVisitSummaryForEmergencyMap_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);
            return Json(Result);
        }

        public IActionResult TotalUniqueVehicle()
        {
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllUniqueVehicle(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetTotalUniqueVehicle";
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
        public IActionResult TotalAssignedVehicle()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetTotalAssignedMinitrippers(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetTotalAssignedMinitrippers";
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

        public IActionResult AllZoneWiseSummary()
        {
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllZoneWiseSummary(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllZoneWiseSummary_paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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

        public IActionResult AllPointSummaryNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult AllPointSummaryNoti_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/AllPointSummaryNoti_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult SaveActualGreyPoint(int PointId, string PointName, int PClId)
        {

            //string endpoint = "api/Collection/SaveActualGreyPoint?PointId=" + PointId + "&PointName=" + PointName + "  &PClId=" + PClId;
            //HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            //string Result = apiobj.GetRequest(endpoint);
            //return Json(Result);


            string msg = string.Empty;
            var obj = new
            {
                PointId = PointId,
                PointName = PointName,
                PClId = PClId

            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/SaveActualGreyPoint";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);

        }

        [HttpPost]
        public JsonResult DeletegreypointintoActual(int PClId)
        {
            string msg = string.Empty;
            var obj = new
            {
                PClId = PClId

            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/DeletegreypointintoActual";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        public IActionResult EmergencyPointDetail()//AllTripPoint()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllEmergencyPointDetail_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllEmergencyPointDetail_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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

        //Add Point

        public IActionResult AddEmergencyPoint()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetEmergencyPointsInfo(DataTableAjaxPostModel requestModel)
        {


            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetEmergencyPointsInfo";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult GetEmergencyPointsInfo1(int EmrUid)
        {
            string endpoint = "api/Collection/GetEmergencyPointsInfo1?EmrUid=" + EmrUid;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);



        }

        [HttpPost]
        public JsonResult GetAllDPointsName1(int WardId)
        {

            //requestModel.CCode = this.User.GetCompanyCode();
            //requestModel.UserId = this.User.GetUserId();

            //string input = JsonConvert.SerializeObject(requestModel);
            //string endpoint = "api/Collection/GetAllDPointsName";
            //HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
            //string Result = apiobj.PostRequestString(endpoint, input);
            //JArray _lst = JArray.Parse(Result);

            //IList<JToken> t = _lst.SelectTokens("$..TotalCount").ToList();
            //int tt = 0;
            //if (t.Count > 0)
            //{
            //    tt = (int)t[0];
            //}
            //var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };
            //return Json(response);
            string msg = string.Empty;
            var obj = new
            {
                WardId = WardId

            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/GetAllDPointsName1";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllDPointsName(DataTableAjaxPostModel requestModel)
        {

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();

            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllDPointsName";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult GetAllDEmergencyPointsName(DataTableAjaxPostModel requestModel)
        {

            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();

            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllDEmergencyPointsName";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public JsonResult InsertEmrPointdetail(string jobjS, string JArrayval)
        {

            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(jobjS);//KHBS
            //DataTable dt1 = new DataTable();

            ////dt1.Columns.Add(new DataColumn("PointId", typeof(int)));
            //dt1.Columns.Add(new DataColumn("PointName", typeof(string)));
            //dt1.Columns.Add(new DataColumn("Lat", typeof(string)));
            //dt1.Columns.Add(new DataColumn("Lng", typeof(string)));
            //dt1.Columns.Add(new DataColumn("TypeId", typeof(int)));



            //for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //{

            //    dt1.Rows.Add(
            //        dt.Rows[i]["PointName"].ToString(),
            //        dt.Rows[i]["Lat"].ToString(),
            //        dt.Rows[i]["Lng"].ToString(),
            //        Convert.ToInt32(dt.Rows[i]["TypeId"])

            //         );
            //}

            string UserId = this.User.GetUserId();
            dynamic dresult = JObject.Parse(JArrayval);
            //var array = JArray.Parse(JArrayval);
            string msg = string.Empty;
            var obj = new
            {
                VehicleUid = dresult.VehicleUid
                            ,
                SheuleTime = dresult.SheuleTime
                            ,
                Remarks = dresult.Remarks
              ,
                DDLId = dresult.DDLId
                ,
                ZoneId = dresult.ZoneId
              ,

                CircleId = dresult.CircleId,
                WardId = dresult.WardId,
                CategoryId = dresult.CategoryId,
                PickupTime = dresult.PickupTime,
                isactive = dresult.isactive,
                UserId = UserId
               ,
                jobjS = jobjS
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/InsertEmrPointdetail";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public FileResult ExportAllGeoPoint(string WardId, string ZoneId, string CircleId, string NotiId, string Status, string FName)
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

            requestModel.NotiId = !string.IsNullOrEmpty(NotiId) ? NotiId : "-1";

            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";


            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPoint_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllGeoPoint(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [CustomAuthorize]
        public IActionResult EmerAllPointCollectDetail()
        {
            return View();
        }
        [HttpPost]
        public JsonResult EmerGetAllPointCollectionDetail_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllEmergencyPointCollectionDetail_Paging";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
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
        public FileResult ExportAllEmerGeoPointsVisitSummary(DateTime FromDate, DateTime ToDate, string FName, string VehicleUid)
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

            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.IsReport = 1;
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllEmergencyPointCollectionDetail_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.EmergencyExportAllPoint(Result, Name);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public FileResult ExportAllCollectionPoints(DateTime FromDate, DateTime ToDate, string FName, string PointId, string VehicleUid)
        {
            string Logfilepath = Path.Combine(_host.WebRootPath + "/content/Logs/");
            try
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

                requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
                requestModel.NotiId = !string.IsNullOrEmpty(PointId) ? PointId : "0";
                requestModel.IsReport = 1;
                // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
                //string FolderName = @"\content\Point_Collection\";
                //string filePath = Path.Combine(_host.WebRootPath + FolderName);
                requestModel.length = -1;
                string input = JsonConvert.SerializeObject(requestModel);
                string endpoint = "api/Collection/GetAllPointCollectionDetail_Paging";
                HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

                string Result = apiobj.PostRequestString(endpoint, input);

                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                filename = Name + ReportTime.ToShortDateString() + ".xlsx";
                // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
                filearray = _report.ExportAllCollectdPoint(Result, Name);

                return File(filearray, ContentType, filename);
            }
            catch (Exception ex)
            {
                //t

                CommonHelper.WriteToJsonFile("Pdffile", "Error-" + ex.Message, Logfilepath);

                throw ex;
                //(ex.Message);
            }

        }
        [HttpPost]
        public ActionResult ExportAllCollectionCSV(DateTime FromDate, DateTime ToDate, string FName, string PointId, string VehicleUid)
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

            requestModel.VehicleUid = !string.IsNullOrEmpty(VehicleUid) ? VehicleUid : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(PointId) ? PointId : "0";
            requestModel.IsReport = 1;
            // requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : "-1";
            //string FolderName = @"\content\Point_Collection\";
            //string filePath = Path.Combine(_host.WebRootPath + FolderName);
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllPointCollectionDetail_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllCollectionCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }


        [HttpPost]
        public FileResult ExportAllVehicleDeploy (DateTime FromDate, string WardId, string ZoneId, string CircleId, string VehicleTypeId, string FName)
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
            requestModel.FromDate = FromDate;          
            requestModel.ZoneId = !string.IsNullOrEmpty(ZoneId) ? ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0";           
       
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "-1";
            


            // requestModel.VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0";
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Collection/GetAllGeoPointsVisitSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllGeoPointsVisitSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }

        [HttpPost]
        public JsonResult GetEmergencyPointsInfoById(int EmrUid)
        {
            var bodyparam = new
            {
                EmrUid = EmrUid,


            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/Collection/GetEmergencyPointsInfoById?EmrUid=" + EmrUid;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);
            return Json(Result);



        }
        [HttpPost]
        public JsonResult GetAllDPointsNameByZoneCircle(int WardId)
        {

            string msg = string.Empty;
            var obj = new
            {
                WardId = WardId

            };
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/GetAllDPointsNameByZoneCircle";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string output = apiobj.PostRequestString(endpoint, input);

            return Json(output);
        }
        [HttpPost]
        public JsonResult GetMapDataForGrey(string PointId)
        {
            var obj = new
            {
                PointId = PointId
                
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Collection/GetMapDataForGrey";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult InsertAndUpdateGVP(string PointId,string Lat, string Lng, int Flag)
        {
            var obj = new
            {
                PointId = PointId,
                Lat = Lat,
                Lng = Lng,
                Flag = Flag,
                UserId=this.User.GetUserId()

            };

            string input = JsonConvert.SerializeObject(obj); 
            string endpoint = "api/Collection/InsertAndUpdateGVP";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult GetGeoPointHistory(int GeoPointId)
        {
            string endpoint = "api/Collection/GetGeoPointHistory?GeoPointId=" + GeoPointId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
    }


}
