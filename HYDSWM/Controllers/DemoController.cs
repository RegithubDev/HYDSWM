using COMMON;
using COMMON.COLLECTION;
using COMMON.DEPLOYMENT;
using COMMON.SWMENTITY;
using HYDSWM;
using HYDSWM.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace DEMOSWMCKC.Controllers
{
    public class DemoController : Controller
    {
        private PdfReport _pdfQrCode;
        private readonly IWebHostEnvironment HostingEnvironment;
        public DemoController(IWebHostEnvironment hostingEnvironment, PdfReport pdfQrCode)
        {

            this.HostingEnvironment = hostingEnvironment;
            this._pdfQrCode = pdfQrCode;
        }
        public IActionResult VehicleDetail()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllVehicleInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetAllVehicleInfo";
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

        public IActionResult AllZone()
        {
            return View();
        }
        public IActionResult AddZone()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetZoneInfoById(int ZId)
        {
            string endpoint = "api/Master/GetZoneInfoById?ZId=" + ZId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public JsonResult SaveAndUpdateZone(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string ZId = dresult.ZId;
            string ZoneNo = dresult.ZoneNo;
            string Zonecode = dresult.Zonecode;
            string IsActive = dresult.IsActive;
            var obj = new
            {
                ZId = !string.IsNullOrEmpty(ZId) ? ZId : "0",
                ZoneNo = ZoneNo,
                IsActive = IsActive,
                Zonecode = Zonecode
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Master/SaveAndUpdateZone";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        //[CustomAuthorize]
        public IActionResult AllMData()
        {
            return View();
        }

        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetVehicleMasterSummary(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);


            var obj = new
            {
                ZoneId = item.GetValue("ZoneId").ToString(),
                CircleId = item.GetValue("CircleId").ToString(),
                WardId = item.GetValue("WardId").ToString(),
                VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetVehicleMasterSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(output);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;
            //JArray _lstcircle = FResult.Table1;
            //JArray _lstward = FResult.Table2;
            //JArray _lstvehicledata = FResult.Table3;
            //JArray _lstvehicletype = FResult.Table4;

            List<DepZoneInfo> zonelst = _lst1.ToObject<List<DepZoneInfo>>();
            List<DepOutDataInfo> outlst = _lstout.ToObject<List<DepOutDataInfo>>();
            //List<DepCircleInfo> circlelst = _lstcircle.ToObject<List<DepCircleInfo>>();
            //List<DepVehicleTypeInfo> vehicletypelst = _lstvehicletype.ToObject<List<DepVehicleTypeInfo>>();
            //List<DepVehicleDataInfo> vehicledatalst = _lstvehicledata.ToObject<List<DepVehicleDataInfo>>();
            //List<DepWardInfo> Wardlst = _lstward.ToObject<List<DepWardInfo>>();

            // Wardlst.Select(i =>
            //   {

            //       i.VehicleData = vehicledatalst.Where(kk => kk.WardId == i.WardId).ToList();
            //       return i;

            //   }).ToList();

            // circlelst.Select(k =>
            //{

            //    k.Wardlst = Wardlst.Where(j => k.CircleId == j.CirlceId).ToList();
            //    return k;

            //}).ToList();


            // zonelst.Select(i =>
            // {
            //     int rcount = 0;

            //     List<DepCircleInfo> incirclelst = circlelst.Where(k => k.ZoneId == i.ZId).ToList();
            //     i.Circlelst = incirclelst;

            //     rcount = Wardlst.Count(k => incirclelst.Any(m => m.CircleId == k.CirlceId));
            //     i.TotalZRowCount = vehicletypelst.Count() * rcount;
            //     i.TotalCircleRowCount = (vehicletypelst.Count() * rcount) / incirclelst.Count();
            //     i.TotalWardRowCount = vehicletypelst.Count();
            //     return i;
            // }).ToList();

            zonelst.Select(i =>
            {
                i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
                i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Active);
                i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.InActive);
                //i.Outlst = outlst.Where(j => j.ZId == i.ZId).ToList();
                //i.TotalZRowCount = i.Outlst.Count();
                //i.TotalCircleRowCount = i.Outlst.Select(i => i.CircleId).Distinct().Count();
                //i.TotalWardRowCount = i.Outlst.Select(i => i.WardId).Distinct().Count();
                return i;
            }
            ).ToList();

            var Result = new { ZoneData = zonelst, VehicleoutData = outlst };
            return Json(Result);
        }
        [HttpPost]
        public FileResult ExportVehicleSummaryData(string Jobjval)
        {
            byte[] filearray = null;

            string filePath = Path.Combine(HostingEnvironment.WebRootPath + "/REPORTTEMPLATE/Report_Format.html");
            string ContentType = string.Empty;
            DateTime ReportTime = DateTime.Now;
            string Name = "GNIDA Report " + "";
            string filename = string.Empty;
            DataTableAjaxPostModel requestModel = new DataTableAjaxPostModel();
            requestModel.search = new Search();
            requestModel.order = new List<Order>();
            requestModel.order.Add(new Order { dir = "", column = 0 });

            JObject item = JObject.Parse(Jobjval);


            var obj = new
            {
                ZoneId = item.GetValue("ZoneId").ToString(),
                CircleId = item.GetValue("CircleId").ToString(),
                WardId = item.GetValue("WardId").ToString(),
                VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetVehicleMasterSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(output);
            JArray _lst1 = FResult.Table1;



            //JArray jresult = JArray.Parse(Result);
            ContentType = "application/pdf";
            filename = Name + ReportTime.ToShortDateString() + ".pdf";
            int RCount = 1;
            string txttbl = string.Empty;
            txttbl += "<thead><tr><th>Sr. No</th><th>Zone</th><th>Circle</th><th>Ward</th><th>VehicleType</th><th>Active</th><th>InActive</th><th>InRepair</th><th>Condemed</th><th>Total</th></tr></thead>";

            txttbl += "<tbody>";
            foreach (JObject item1 in _lst1)
            {
                txttbl += "<tr><td>" + RCount + "</td>" + "" +
                             //"<td> " + item.GetValue("DeviceID").ToString() + " </td> " +

                             "<td> " + item1.GetValue("Zonecode").ToString() + " </td> " +
                             "<td> " + item1.GetValue("CircleCode").ToString() + " </td> " +
                             "<td> " + item1.GetValue("WardName").ToString() + " </td> " +
                             "<td> " + item1.GetValue("VehicleType").ToString() + " </td> " +
                             "<td> " + item1.GetValue("Active").ToString() + " </td> " +
                             "<td> " + item1.GetValue("InActive").ToString() + " </td> " +
                             "<td> " + item1.GetValue("InRepair").ToString() + " </td> " +
                             "<td> " + item1.GetValue("Condemed").ToString() + " </td> " +
                             "<td> " + item1.GetValue("TotalAsset").ToString() + " </td> " +


                         "</tr>";
                RCount++;
            }
            txttbl += "</tbody>";

            filearray = _pdfQrCode.ExportToPdfData(txttbl, Name, filePath);

            //filearray = _ReportPdf.ExportToPdfData(txttbl, Name, filePath);




            return File(filearray, ContentType, filename);
        }
        [CustomPostAuthorize]
        public JsonResult GetAllVMasterSummary_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.ZoneId = !string.IsNullOrEmpty(requestModel.ZoneId) ? requestModel.ZoneId : "0";
            requestModel.CircleId = !string.IsNullOrEmpty(requestModel.CircleId) ? requestModel.CircleId : "0";
            requestModel.WardId = !string.IsNullOrEmpty(requestModel.WardId) ? requestModel.WardId : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetAllVMasterSummary_Paging";
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
        public IActionResult GetAllData()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetVMasterSummary(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);


            var obj = new
            {
                ZoneId = item.GetValue("ZoneId").ToString(),
                CircleId = item.GetValue("CircleId").ToString(),
                WardId = item.GetValue("WardId").ToString(),
                VehicleTypeId = item.GetValue("VehicleTypeId").ToString(),
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Operation/GetVehicleMasterSummary";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);

            dynamic FResult = JObject.Parse(output);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;
            JArray _lstWard = FResult.Table2;
            JArray _lstVehicleType = FResult.Table3;
            JArray _lstcircle = FResult.Table4;


            List<HDZoneInfo> zonelst = _lst1.ToObject<List<HDZoneInfo>>();
            List<DepOutDataInfo> outlst = _lstout.ToObject<List<DepOutDataInfo>>();
            //List<DepCircleInfo> circlelst = _lstcircle.ToObject<List<DepCircleInfo>>();
            //List<DepVehicleTypeInfo> vehicletypelst = _lstvehicletype.ToObject<List<DepVehicleTypeInfo>>();
            //List<DepVehicleDataInfo> vehicledatalst = _lstvehicledata.ToObject<List<DepVehicleDataInfo>>();
            List<HDWardInfo> Wardlst = _lstWard.ToObject<List<HDWardInfo>>();
            List<HDVehicleTypeInfo> vehicletypelst = _lstVehicleType.ToObject<List<HDVehicleTypeInfo>>();
            List<HDCircleInfo> ciclelst = _lstcircle.ToObject<List<HDCircleInfo>>();

            // Wardlst.Select(i =>
            //   {

            //       i.VehicleData = vehicledatalst.Where(kk => kk.WardId == i.WardId).ToList();
            //       return i;

            //   }).ToList();

            // circlelst.Select(k =>
            //{

            //    k.Wardlst = Wardlst.Where(j => k.CircleId == j.CirlceId).ToList();
            //    return k;

            //}).ToList();


            // zonelst.Select(i =>
            // {
            //     int rcount = 0;

            //     List<DepCircleInfo> incirclelst = circlelst.Where(k => k.ZoneId == i.ZId).ToList();
            //     i.Circlelst = incirclelst;

            //     rcount = Wardlst.Count(k => incirclelst.Any(m => m.CircleId == k.CirlceId));
            //     i.TotalZRowCount = vehicletypelst.Count() * rcount;
            //     i.TotalCircleRowCount = (vehicletypelst.Count() * rcount) / incirclelst.Count();
            //     i.TotalWardRowCount = vehicletypelst.Count();
            //     return i;
            // }).ToList();

            zonelst.Select(i =>
            {
                i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
                i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Active);
                i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.InActive);
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
                            Active = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Active),
                            InActive = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.InActive),
                            InRepair = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.InRepair),
                            Condemed = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.Condemed),
                            TotalAsset = outlst.Where(kk => kk.WardId == k.WardId && kk.VehicleTypeId == mm.VehicleTypeId).Sum(kk => kk.TotalAsset)
                        }).ToList()


                    }).ToList();
                    info.TotalCircleRowCount = outlst.Where(kk => kk.CircleId == info.CircleId && kk.VehicleTypeId > 0).Count();
                    icllst.Add(info);

                    HDCircleInfo iinfo = new HDCircleInfo();
                    List<HDVehicleTypeInfo> hvehicletype = new List<HDVehicleTypeInfo>();
                    HDVehicleTypeInfo vdata = new HDVehicleTypeInfo();
                    vdata.VehicleTypeId = 0;
                    vdata.VehicleType = string.Empty;
                    vdata.Active = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Active);
                    vdata.InActive = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.InActive);
                    vdata.InRepair = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.InRepair);
                    vdata.Condemed = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.Condemed);
                    vdata.TotalAsset = outlst.Where(k => k.CircleId == j.CircleId && k.VehicleTypeId == 0).Sum(k => k.TotalAsset);
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
    }
}
