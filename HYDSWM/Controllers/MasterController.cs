using COMMON;
using HYDSWM;
using HYDSWM.Helpers;
using HYDSWMAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DEMOSWMCKC.Controllers
{
    public class MasterController : Controller
    {
        private readonly IWebHostEnvironment _host;
        static string apiBaseUrl = Startup.StaticConfig.GetValue<string>("WebAPIBaseUrl");
        static string BasicAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("patna:patna#2020"));

        public MasterController(IWebHostEnvironment host)
        {
            this._host = host;

        }


        [CustomAuthorize]
        public IActionResult AllCity()
        {
            return View();
        }
        public IActionResult AddCity()
        {
            string add_upd = Request.Query["add_upd"].ToString();

            if(add_upd == "0")
                ViewBag.add_upd = "Add";
            else
                ViewBag.add_upd = "Update";

            return PartialView();
        }

        [CustomPostAuthorize]
        public JsonResult SaveAndUpdateCity(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string CId = dresult.CId;
            string CityNo = dresult.CityNo;
            string Citycode = dresult.Citycode;
            string IsActive = dresult.IsActive;
            string UserId = this.User.GetUserId();
            var obj = new
            {
                CId = !string.IsNullOrEmpty(CId) ? CId : "0",
                CityNo = CityNo,
                IsActive = IsActive,
                Citycode = Citycode,
                UserId = UserId
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateCity";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllCityInfo(string IsAll)
        {
            string endpoint = "api/Master/GetAllCity?IsAll=" + IsAll;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetCityInfoById(string CId)
        {
            string endpoint = "api/Master/GetCityInfoById?CId=" + CId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }


        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetComplaintInfoById(string CId)
        {
            string endpoint = "api/Master/GetComplaintInfoById?CId=" + CId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [CustomAuthorize]
        public IActionResult AllZone()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllZoneInfo(string IsAll)
        {
            var obj = new
            {
                ZCode = this.User.GetCompanyCode(),
                IsAll = IsAll
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/GetAllZone";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);


        }


        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllWardInfo(string ward)
        {
            var obj = new
            {
               ward = ward
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/GetAllWard";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);


        }

        

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult SelectCircleByWard(string selected_ward)
        {
            var obj = new
            {
                ward = selected_ward
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SelectCircleByWard";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);


        }


        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult SelectZoneByWard(string selected_ward)
        {
            var obj = new
            {
                ward = selected_ward
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SelectZoneByWard";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);


        }




        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetZoneInfoById(int ZId)
        {
            string endpoint = "api/Master/GetZoneInfoById?ZId=" + ZId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddZone()
        {

            string add_upd = Request.Query["add_upd"].ToString();

            if (add_upd == "0")
                ViewBag.add_upt = "Add";
            else
                ViewBag.add_upt = "Update";

            return PartialView();
        }
        [CustomPostAuthorize]
        public JsonResult SaveAndUpdateZone(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string ZId = dresult.ZId;
            string ZoneNo = dresult.ZoneNo;
            string Zonecode = dresult.Zonecode;
            string IsActive = dresult.IsActive;
            string UserId = this.User.GetUserId();
            string CId = dresult.CId;
            var obj = new
            {
                ZId = !string.IsNullOrEmpty(ZId) ? ZId : "0",
                ZoneNo = ZoneNo,
                IsActive = IsActive,
                Zonecode = Zonecode,
                UserId = UserId,
                CId = CId
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateZone";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [CustomAuthorize]
        public IActionResult AllCircle()
        {
            return View();
        }
        public IActionResult AddCircle()
        {
            string add_upd = Request.Query["add_upd"].ToString();

            if (add_upd == "0")
                ViewBag.add_upd = "Add";
            else
                ViewBag.add_upd = "Update";
            return PartialView();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllCircle(string IsAll)
        {
            var obj = new
            {
                CCode = this.User.GetCompanyCode(),
                IsAll = IsAll
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/GetAllCircle";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetCircleInfoById(int CircleId)
        {
            string endpoint = "api/Master/GetCircleInfoById?CircleId=" + CircleId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [CustomPostAuthorize]
        public JsonResult SaveAndUpdateCircle(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string CircleId = dresult.CircleId;
            string CircleName = dresult.CircleName;
            string CircleCode = dresult.CircleCode;
            string ZoneId = dresult.ZoneId;
            string IsActive = dresult.IsActive;
            string CId = dresult.CId;
             var obj = new
            {
                CircleId = !string.IsNullOrEmpty(CircleId) ? CircleId : "0",
                CircleName = CircleName,
                ZoneId = ZoneId,
                CircleCode = CircleCode,
                CCode = this.User.GetCompanyCode(),
                IsActive = IsActive,
                CreatedBy = this.User.GetUserId(),
                ModifiedBy = this.User.GetUserId(),
                CId = CId
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateCircle";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult GetAllCircleByZone(int ZoneId)
        {
            string endpoint = "api/Master/GetAllCircleByZone?ZoneId=" + ZoneId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [CustomAuthorize]
        public IActionResult AllWard()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllWard(string IsAll)
        {
            var obj = new
            {
                IsAll = IsAll
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/GetAllWard";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetWardInfoById(int WardId)
        {
            string endpoint = "api/Master/GetWardInfoById?WardId=" + WardId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [CustomPostAuthorize]
        public JsonResult SaveAndUpdateWard(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string WardId = dresult.WardId;
            string WardNo = dresult.WardNo;
            string WardCode = dresult.WardCode;
            string CirlceId = dresult.CirlceId;
            string IsActive = dresult.IsActive;
            string CId = dresult.CId;
            var obj = new
            {
                WardId = !string.IsNullOrEmpty(WardId) ? WardId : "0",
                WardNo = WardNo,
                WardCode = WardCode,
                CirlceId = CirlceId,
                CCode = this.User.GetCompanyCode(),
                IsActive = IsActive,
                CreatedBy = this.User.GetUserId(),
                ModifiedBy = this.User.GetUserId(),
                CId = CId
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateWard";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        public IActionResult AddWard()
        {
            string add_upd = Request.Query["add_upd"].ToString();

            if (add_upd == "0")
                ViewBag.add_upd = "Add";
            else
                ViewBag.add_upd = "Update";
            return PartialView();
        }

        [CustomAuthorize]
        public IActionResult AllTransaferStation()
        {
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllTransferStation(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.ZoneId = !string.IsNullOrEmpty(requestModel.ZoneId) ? requestModel.ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(requestModel.CircleId) ? requestModel.CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
           requestModel.CityId = !string.IsNullOrEmpty(requestModel.CityId) ? requestModel.CityId : "0";


            string input = JsonConvert.SerializeObject(requestModel);
            // string endpoint = "api/Master/GetAllTransferStation";
            string endpoint = "api/Master/GetAllTransferStationB64";
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
        public JsonResult GetTStationInfoById(int TSId)
        {



            string endpoint = "api/Master/GetTStationInfoById?TSId=" + TSId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public async Task<JsonResult> SaveAndUpdateTStationInfo([FromForm] IFormCollection formData)
        {

            var url = apiBaseUrl + "api/Master/SaveAndUpdateTStationInfo";
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
        public JsonResult GetAllContainerTypeInfo()
        {
            string endpoint = "api/Master/GetAllContainerTypeInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllAssetStatus()
        {
            var bodyparam = new
            {
                CCode = this.User.GetCompanyCode(),
                IsAll = "YES"
            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/Master/GetAllAssetStatus";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint, input);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllVehicleType()
        {
            string endpoint = "api/Master/GetAllVehicleType";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllOperationType()
        {
            string endpoint = "api/Master/GetAllOperationType";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllOwnerType()
        {
            string endpoint = "api/Master/GetAllOwnerType";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [CustomAuthorize]
        public IActionResult AllOwnerType()
        {
            return View();
        }
        public IActionResult AddOwnerType()
        {
            return PartialView();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllOwnerTypeInfo(string IsAll, int OwnerTId)
        {
            var obj = new
            {
                IsAll = IsAll,
                OwnerTId = OwnerTId
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/GetAllOwnerTypeInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [CustomPostAuthorize]
        public JsonResult SaveAndUpdateOwnerType(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string OwnerTId = dresult.OwnerTId;
            string OwnerType = dresult.OwnerType;
            string IsActive = dresult.IsActive;
            string UserId = this.User.GetUserId();
            var obj = new
            {
                OwnerTId = !string.IsNullOrEmpty(OwnerTId) ? OwnerTId : "0",
                OwnerType = OwnerType,
                IsActive = IsActive,
                UserId = UserId,
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateOwnerType";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        public IActionResult AllVehicleType()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllVehicleTypeInfo(string IsAll)
        {
            string endpoint = "api/Master/GetAllVehicleType?IsAll=" + IsAll;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllDIncharge(string IsAll)
        {
            string endpoint = "api/Master/GetAllDIncharge?IsAll=" + IsAll;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllDLocation(int WardId)
        {
            string endpoint = "api/Master/GetAllDLocation?WardId=" + WardId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllTransferStationByWardId(int WardId)
        {
            string endpoint = "api/Master/GetAllTransferStationByWardId?WardId=" + WardId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetVehicleTypeInfoById(int VehicleTypeId)
        {
            string endpoint = "api/Master/GetVehicleTypeInfoById?VehicleTypeId=" + VehicleTypeId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddVehicleType()
        {
            return PartialView();
        }
        public JsonResult SaveAndUpdateVehicleType(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string VehicleTypeId = dresult.VehicleTypeId;
            string VehicleType = dresult.VehicleType;
            string Volume = dresult.Volume;
            string Density = dresult.Density;
            string OperationTypeId = dresult.OperationTypeId;

            string IsActive = dresult.IsActive;
            var obj = new
            {
                VehicleTypeId = !string.IsNullOrEmpty(VehicleTypeId) ? VehicleTypeId : "0",
                VehicleType = VehicleType,
                Density = Density,
                OperationTypeId = OperationTypeId,
                IsActive = IsActive,
                Volume = Volume

            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateVehicleType";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        public IActionResult AllGeoPointCategoryDetail()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllGeoPointCategoryDetailInfo(string IsAll)
        {
            string endpoint = "api/Master/GetAllGeoPointCategory?IsAll=" + IsAll;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetGeoPointCategoryDetailInfoById(int GPCId)
        {
            string endpoint = "api/Master/GetGeoPointCategoryInfoById?GPCId=" + GPCId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddGeoPointCategoryDetail()
        {
            return PartialView();
        }
        public JsonResult SaveAndUpdateGeoPointCategoryDetail(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string GPCId = dresult.GPCId;
            string GeoPointCategory = dresult.GeoPointCategory;
            string IsActive = dresult.IsActive;
            var obj = new
            {
                GPCId = !string.IsNullOrEmpty(GPCId) ? GPCId : "0",
                GeoPointCategory = GeoPointCategory,
                IsActive = IsActive,
                CreatedBy = this.User.GetUserId(),
                ModifiedBy = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateGeoPointCategory";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        public IActionResult AllTripMaster()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllTripMasterInfo(string IsAll)
        {
            string endpoint = "api/Master/GetAllTripMaster?IsAll=" + IsAll;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllTripMasterInfoById(int TMId)
        {
            string endpoint = "api/Master/GetTripMasterInfoById?TMId=" + TMId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddTripMaster()
        {
            return PartialView();
        }
        public JsonResult SaveAndUpdateAllTripMaster(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string TMId = dresult.TMId;
            string TripId = dresult.TripId;
            string PrefixName = dresult.PrefixName;
            string IsActive = dresult.IsActive;
            var obj = new
            {
                TMId = !string.IsNullOrEmpty(TMId) ? TMId : "0",
                TripId = TripId,
                PrefixName = !string.IsNullOrEmpty(PrefixName) ? PrefixName : "",
                IsActive = IsActive,
                CreatedBy = this.User.GetUserId(),
                ModifiedBy = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateTripMaster";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllComplaintCategory(string IsAll, int ComplaintTypeId)
        {
            var obj = new
            {
                IsAll = IsAll,
                ComplaintTypeId = ComplaintTypeId
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Complaint/GetAllComplaintCategory";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllGeoPointCategory(string IsAll, int GPCId)
        {
            string endpoint = "api/Collection/GetAllGeoPointCategory?IsAll=" + IsAll + "&GPCId=" + GPCId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult GetAllActiveVehicle()
        {
            string endpoint = "api/Master/GetAllActiveVehicle";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllActiveGeoPoint(int ZoneId, int PointCatId)
        {
            string endpoint = "api/Master/GetAllActiveGeoPoint?ZoneId=" + ZoneId + "&PointCatId=" + PointCatId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllTripInfo(string IsAll)
        {
            string endpoint = "api/Master/GetAllTrip?IsAll=" + IsAll;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllActiveRoute(int ZoneId, int CircleId)
        {
            string endpoint = "api/Master/GetAllActiveRoute?ZoneId=" + ZoneId + "&CircleId=" + CircleId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllActiveRouteTrip(int RouteId)
        {
            string endpoint = "api/Master/GetAllActiveRouteTrip?RouteId=" + RouteId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult GetAllAssignedVehicleNumber()
        {
            string endpoint = "api/Master/GetAllAssignedVehicleNumber";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult GetAllAssignedVehicleNumber1(int RouteId, int TripId)
        {
            // string endpoint = "api/Master/GetAllAssignedVehicleNumber1?RouteId=" + RouteId;
            string endpoint = "api/Master/GetAllAssignedVehicleNumber1?RouteId=" + RouteId + "&TripId=" + TripId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllGreyPoint(string RouteTripCode, DateTime SDate)
        {
            //string endpoint = "api/Master/GetAllGreyPoint";
            //HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            //string Result = apiobj.GetRequest(endpoint);
            //return Json(Result);
            string endpoint = "api/Master/GetAllGreyPoint?RouteTripCode=" + RouteTripCode + "&SDate=" + SDate;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult GetAllVehicleNumber(int VehicleTypeId)
        {
            string endpoint = "api/Master/GetAllVehicleNumber?VehicleTypeId=" + VehicleTypeId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
    }
}
