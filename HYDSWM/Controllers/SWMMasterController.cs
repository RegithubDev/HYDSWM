using COMMON;
using COMMON.SWMENTITY;
using HYDSWM;
using HYDSWM.Helpers;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEMOSWMCKC.Controllers
{
    public class SWMMasterController : Controller
    {
        private readonly IWebHostEnvironment _host;

        public SWMMasterController(IWebHostEnvironment host)
        {
            this._host = host;
        }
        public IActionResult AddHouseHold(string param)
        {
            List<WardInfo> wardlst = new List<WardInfo>();
            List<SectorInfo> sectorlst = new List<SectorInfo>();
            #region owner type
            var bodyparam1 = new
            {
                CCode = this.User.GetCompanyCode(),
            };
            string input1 = JsonConvert.SerializeObject(bodyparam1);
            string endpoint1 = "api/SWMMaster/GetAllOwnerType";
            HttpClientHelper<List<OwnerTypeInfo>> apiobj1 = new HttpClientHelper<List<OwnerTypeInfo>>();
            List<OwnerTypeInfo> _lst = apiobj1.PostRequest(endpoint1, input1);
            #endregion

            #region Property type
            var bodyparam2 = new
            {
                CCode = this.User.GetCompanyCode(),
            };
            string input2 = JsonConvert.SerializeObject(bodyparam2);
            string endpoint2 = "api/SWMMaster/GetAllPropertyType";
            HttpClientHelper<List<PropertyTypeInfo>> apiobj2 = new HttpClientHelper<List<PropertyTypeInfo>>();
            List<PropertyTypeInfo> _lstpropertytype = apiobj2.PostRequest(endpoint2, input2);

            #endregion


            #region Circle
            var bodyparam3 = new
            {
                IsAll = "YES",
                CCode = this.User.GetCompanyCode(),
            };
            string input3 = JsonConvert.SerializeObject(bodyparam3);
            string endpoint3 = "api/SWMMaster/GetAllCircle";
            HttpClientHelper<List<CircleInfo>> apiobj3 = new HttpClientHelper<List<CircleInfo>>();
            List<CircleInfo> _lstcircle = apiobj3.PostRequest(endpoint3, input3);
            #endregion

            #region Identity Type
            var bodyparam4 = new
            {
                IsAll = "YES",
                CCode = this.User.GetCompanyCode(),
            };
            string input4 = JsonConvert.SerializeObject(bodyparam4);
            string endpoint4 = "api/SWMMaster/GetAllIdentityType";
            HttpClientHelper<List<IdentityTypeInfo>> apiobj4 = new HttpClientHelper<List<IdentityTypeInfo>>();
            List<IdentityTypeInfo> _lstIdentityType = apiobj4.PostRequest(endpoint4, input4);

            #endregion

            ViewBag.OwnerType = _lst;
            ViewBag.PropertyType = _lstpropertytype;
            ViewBag.CircleLst = _lstcircle;
            ViewBag.IDentityTypelst = _lstIdentityType;
            if (!string.IsNullOrEmpty(param))
            {
                try
                {

                    string endpoint5 = "api/SWMMaster/GetHouseHoldInfoById?param=" + CommonHelper.Decrypt(param);
                    HttpClientHelper<HouseholdInfo> apiobj5 = new HttpClientHelper<HouseholdInfo>();
                    HouseholdInfo obj = apiobj5.GetSingleItemRequest(endpoint5);

                    var bodyparam6 = new
                    {
                        IsAll = "NO",
                        CircleId = !string.IsNullOrEmpty(obj.CircleId) ? Convert.ToInt32(obj.CircleId) : 0
                    };
                    string input6 = JsonConvert.SerializeObject(bodyparam6);
                    string endpoint6 = "api/SWMMaster/GetWardByCircle";
                    HttpClientHelper<List<WardInfo>> apiobj6 = new HttpClientHelper<List<WardInfo>>();
                    wardlst = apiobj6.PostRequest(endpoint6, input6);

                    var bodyparam7 = new
                    {
                        CCode = this.User.GetCompanyCode(),
                        IsAll = "NO",
                        CircleId = !string.IsNullOrEmpty(obj.CircleId) ? Convert.ToInt32(obj.CircleId) : 0,
                        WardId = obj.WardNo
                    };
                    string input7 = JsonConvert.SerializeObject(bodyparam7);
                    string endpoint7 = "api/SWMMaster/GetAllSector";
                    HttpClientHelper<List<SectorInfo>> apiobj7 = new HttpClientHelper<List<SectorInfo>>();
                    sectorlst = apiobj7.PostRequest(endpoint7, input7);

                    ViewBag.SectorLst = sectorlst;
                    ViewBag.WardLst = wardlst;
                    return PartialView(obj);
                }
                catch (Exception ex)
                {
                    return PartialView();
                }
            }
            else
            {
                ViewBag.WardLst = wardlst;
                ViewBag.SectorLst = sectorlst;
                return PartialView();
            }
        }

        [HttpPost]
        public JsonResult AddHouseHold(HouseholdInfo obj)
        {
            string msg = string.Empty;
            obj.CCode = this.User.GetCompanyCode();
            obj.UserId = this.User.GetUserId();
            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/SWMMaster/AddHouseHold";
            HttpClientHelper<GResposnse> apiobj = new HttpClientHelper<GResposnse>();

            GResposnse Result = apiobj.PostRequest(endpoint, input);
            switch (Result.Result.ToString())
            {

                case "0":
                    msg = "Something Went Wrong!";
                    break;
                case "1":
                    msg = Result.Msg + " Your UID-" + Result.Code;
                    break;
                case "2":
                    msg = Result.Msg + " Your UID-" + Result.Code;
                    break;
                case "3":
                    msg = Result.Msg + " Your UID Is Already Exists-" + Result.Code;
                    break;


            }

            var objresult = new
            {
                data = Result,
                Msg = msg
            };
            return Json(objresult);
        }

        [CustomAuthorize]
        public IActionResult AllHouseHold(string param)
        {
            ViewBag.Type = !string.IsNullOrEmpty(param) ? param : "0";
            return View();
        }
        [HttpPost]
        public JsonResult AllHouseHoldInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.UserId = this.User.GetUserId();
            requestModel.CircleId = !string.IsNullOrEmpty(requestModel.CircleId) ? requestModel.CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.CCode = this.User.GetCompanyCode();
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/SWMMaster/AllHouseHoldInfo";
            HttpClientHelper<List<HouseHold_Paging>> apiobj = new HttpClientHelper<List<HouseHold_Paging>>();

            List<HouseHold_Paging> _lst = apiobj.PostRequest(endpoint, input);
            int tt = 0;
            if (_lst.Count > 0)
            {
                tt = _lst[0].TotalCount.Value;
            }
            var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };
            return Json(response);
        }

        public FileResult DownLoadQR(string UHouseId)
        {

            HouseQrCodeInfo info = new HouseQrCodeInfo();
            info.QrCode = UHouseId;
            //  List<HouseQrCodeInfo> _lst = _dataRepository.GetAllHouseQrCodeInfo(StoredProcedureHelper.spGetAllQrCodeById, parameters);
            string wwwRoot = _host.WebRootPath;


            string compressedFileName = "QrCodeHouse-" + UHouseId;
            var tempOutPutPath = wwwRoot + "/" + Url.Content("/TempFile/") + compressedFileName + ".zip";

            Response.Headers.Add("Content-Disposition", "attachment; filename=" + compressedFileName + ".zip");
            Response.ContentType = "application/zip";
            using (var zipStream = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {

                byte[] fileBytes = QRHelper.RenderQrCode(info.QrCode, wwwRoot + "/otherfiles/global_assets/images/resl.png");

                var fileEntry = new ZipEntry(info.QrCode + ".png")
                {
                    Size = fileBytes.Length
                };

                zipStream.PutNextEntry(fileEntry);
                zipStream.Write(fileBytes, 0, fileBytes.Length);


                zipStream.Flush();
                zipStream.Close();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            return File(finalResult, "application/zip", compressedFileName + ".zip");
        }
        [HttpPost]
        public JsonResult GetAllZone()
        {
            string endpoint = "api/SWMMaster/GetAllZone";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllCircleByZone(int ZoneId)
        {
            string endpoint = "api/SWMMaster/GetAllCircleByZone?ZoneId=" + ZoneId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public IActionResult GetWardByCircle(string CircleId)
        {
            var bodyparam = new
            {
                IsAll = "NO",
                CircleId = CircleId
            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/SWMMaster/GetWardByCircle";
            HttpClientHelper<List<WardInfo>> apiobj = new HttpClientHelper<List<WardInfo>>();

            object[] mparameters = { "NO", Convert.ToInt32(CircleId) };
            List<WardInfo> _lst = apiobj.PostRequest(endpoint, input);


            List<SelectListItem> emplst = new List<SelectListItem>();
            _lst.ForEach(x =>
            {
                emplst.Add(new SelectListItem
                {
                    Text = x.WardNo,
                    Value = x.WardId.ToString()
                });
            });

            return Json(emplst);
        }
        [HttpPost]
        public JsonResult GetAllCircleByUser()
        {
            string endpoint = "api/SWMMaster/GetAllCircleByUser?UserId=" + this.User.GetUserId();
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllWardByUser(string CircleId)
        {
            string endpoint = "api/SWMMaster/GetAllWardByUser?UserId=" + this.User.GetUserId() + "&CircleId=" + CircleId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllWardByCircle(string CircleId)
        {
            var bodyparam = new
            {
                CircleId = CircleId,
                IsAll = "YES"
            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/SWMMaster/GetAllWard";
            HttpClientHelper<List<WardInfo>> apiobj = new HttpClientHelper<List<WardInfo>>();
            List<WardInfo> Result = apiobj.PostRequest(endpoint, input);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllShift()
        {
            var bodyparam = new
            {
                CCode = this.User.GetCompanyCode(),
                IsAll = "YES"
            };
            string input = JsonConvert.SerializeObject(bodyparam);
            string endpoint = "api/SWMMaster/GetAllShift";
            HttpClientHelper<List<ShiftInfo>> apiobj = new HttpClientHelper<List<ShiftInfo>>();
            List<ShiftInfo> _lstShift = apiobj.PostRequest(endpoint, input);
            return Json(_lstShift);
        }
        [HttpPost]
        public JsonResult GetAllContrator()
        {

            string endpoint = "api/SWMMaster/GetAllContrator?CCode=" + this.User.GetCompanyCode() + "&IsAll=YES";
            HttpClientHelper<List<string>> apiobj = new HttpClientHelper<List<string>>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllCircle()
        {

            var bodyparam3 = new
            {
                IsAll = "YES",
                CCode = this.User.GetCompanyCode(),
            };
            string input3 = JsonConvert.SerializeObject(bodyparam3);
            string endpoint3 = "api/SWMMaster/GetAllCircle";
            HttpClientHelper<List<CircleInfo>> apiobj3 = new HttpClientHelper<List<CircleInfo>>();
            List<CircleInfo> _lstcircle = apiobj3.PostRequest(endpoint3, input3);
            return Json(_lstcircle);
        }
        [CustomAuthorize]
        public IActionResult AllShift()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AllShiftInfo()
        {
            string endpoint = "api/SWMMaster/AllShiftInfo?CCode=" + this.User.GetCompanyCode();
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddShift()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult AddShiftInfo(ShiftInfo info)
        {
            info.CCode = this.User.GetCompanyCode();
            info.UserId = this.User.GetUserId();
            string input3 = JsonConvert.SerializeObject(info);
            string endpoint3 = "api/SWMMaster/AddAndUpdateShift";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.PostRequestString(endpoint3, input3);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult ShiftInfoById(int ShiftId)
        {
            string endpoint = "api/SWMMaster/ShiftInfoById?ShiftId=" + ShiftId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [HttpPost]
        public JsonResult GetAllDesignationFromEmpTbl(int ShiftId)
        {
            string endpoint = "api/SWMMaster/GetAllDesignationFromEmpTbl";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }

    }
}
