using HYDSWM;
using HYDSWM.Helpers;
using HYDSWMAPI;
using COMMON;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using OfficeOpenXml;

using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using COMMON.ASSET;
using Microsoft.AspNetCore.Hosting;
using ICSharpCode.SharpZipLib.Zip;
using System.Drawing;
using System.Security.Policy;
using COMMON.DEPLOYMENT;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO.Compression;

using Grpc.Core;
using Microsoft.Extensions.Hosting;
using System.Data.OleDb;
using System.Data.SqlClient;
using Sylvan.Data.Csv;
using System.Data.Common;
using OfficeOpenXml;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Diagnostics;
using Spire.Xls;
using static QRCoder.PayloadGenerator;


namespace HYDSWM.Controllers
{
    public class AssetController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private TReport _report;
        static string apiBaseUrl = Startup.StaticConfig.GetValue<string>("WebAPIBaseUrl");
        static string BasicAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("patna:patna#2020"));
        private PdfReport _pdfQrCode;

        public AssetController(TReport report, IWebHostEnvironment host, PdfReport pdfQrCode)
        {
            this._report = report;
            this._host = host;
            this._pdfQrCode = pdfQrCode;
        }
        [CustomAuthorize]
        public IActionResult AllContainer()
        {
            ViewBag.UserId = this.User.GetUserId();
            return View();
        }
        [CustomPostAuthorize]
        public IActionResult AllContainerNoti()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllContainerInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            //string endpoint = "api/Asset/GetAllContainer";
            string endpoint = "api/Asset/GetAllContainerB64";
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
        public JsonResult GetTStationInfoById(int CMId)
        {
            string endpoint = "api/Asset/GetContainerActionStatusById?CMId=" + CMId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public IActionResult AddContainer()
        {
            return PartialView();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public async Task<JsonResult> SaveAndUpdateContainer([FromForm] IFormCollection formData)
        {
            var url = apiBaseUrl + "api/Asset/SaveAndUpdateContainer"; ;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", BasicAuth);
            MultipartFormDataContent form = new MultipartFormDataContent();

            if (formData.Files.Count > 0)
            {


                var filePath = formData.Files[0].FileName;

                if (CommonHelper.ExtensionType(Path.GetExtension(filePath)))
                {
                    byte[] filep = ConvertToBytes(formData.Files[0]);
                    Stream frequestStream = new MemoryStream(filep);

                    var imageContent = new ByteArrayContent(filep);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

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
        [CustomPostAuthorize]
        public JsonResult GetContainerInfoById(int CMId)
        {
            string endpoint = "api/Asset/GetContainerInfoById?CMId=" + CMId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        public FileResult DownLoadQR(string UId)
        {

            string wwwRoot = _host.WebRootPath;
            string compressedFileName = "QrCode-" + UId;
            var tempOutPutPath = wwwRoot + "/" + Url.Content("/TempFile/") + compressedFileName + ".zip";

            Response.Headers.Add("Content-Disposition", "attachment; filename=" + compressedFileName + ".zip");
            Response.ContentType = "application/zip";
            using (var zipStream = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {

                byte[] fileBytes = null;//QRHelper.CallQr(UId, wwwRoot + "/otherfiles/global_assets/images/QrLogo.png");

                var fileEntry = new ZipEntry(UId + ".png")
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
        [CustomAuthorize]
        public IActionResult AllVehicle()
        {
            ViewBag.UserId = this.User.GetUserId();
            ViewBag.UserName = this.User.GetUserName();

            HttpContext.Session.SetString("user_name", this.User.GetUserName());

            return View();
        }




        [CustomPostAuthorize]
        public IActionResult AllVehicleDashboard()
        {
            ViewBag.UserId = this.User.GetUserId();
            return View();
        }

        

        [CustomPostAuthorize]
        public IActionResult BIReports()
        {
            ViewBag.UserId = this.User.GetUserId();
            return View();
        }


        [CustomPostAuthorize]
        public IActionResult AllVehicleNoti()
        {
            return View();
        }
        public IActionResult AddVehicle()
        {
            return PartialView();
        }
        [HttpPost]
        public async void Upload_file()
        {


            //uploaded file retrieval
            var file = Request.Form.Files[0];

            //creating path to "Upload" Folder
            string path = Path.Combine(file.FileName);

            //copying uploaded file to Server folder
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //creating a workbook instance
            Workbook workbook = new Workbook();

            //Loading data from the xl File
            workbook.LoadFromFile(file.FileName);


            //Selecting the worksheet
            Worksheet sheet = workbook.Worksheets[0];


            //Generating .csv File
            sheet.SaveToFile("ExcelToCSV.csv", ",", Encoding.UTF8);


            /*Code For Converting .csv file to Database Table*/

            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser("ExcelToCSV.csv"))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                //return null;
            }

            using (SqlConnection dbConnection = new SqlConnection("Data Source=LCOH-NET-1\\SQLEXPRESS;Initial Catalog=dbHYDCKC06;User ID=dotnet7; Password=Resl@12345;Integrated Security=false;MultipleActiveResultSets=True"))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "VehicleMaster2";
                    foreach (var column in csvData.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    s.WriteToServer(csvData);
                }
            }

            //Deleting both the xl and csv files

            System.IO.File.Delete(file.FileName);
            System.IO.File.Delete("ExcelToCSV.csv");



        }

        
        public IActionResult VehicleSummary()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllVehicleInfo(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.Status = !string.IsNullOrEmpty(requestModel.Status) ? requestModel.Status : "0";
            requestModel.VehicleTypeId = !string.IsNullOrEmpty(requestModel.VehicleTypeId) ? requestModel.VehicleTypeId : "0";
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetAllVehicleInfo";
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
        [CustomPostAuthorize]
        public async Task<JsonResult> SaveAndUpdateVehicleInfo([FromForm] IFormCollection formData)
        {
            var url = apiBaseUrl + "api/Asset/SaveAndUpdateVehicleInfo";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", BasicAuth);
            MultipartFormDataContent form = new MultipartFormDataContent();

            if (formData.Files.Count > 0)
            {
                var filePath = formData.Files[0].FileName;
                if (CommonHelper.ExtensionType(Path.GetExtension(filePath)))
                {
                    byte[] filep = ConvertToBytes(formData.Files[0]);
                    Stream frequestStream = new MemoryStream(filep);

                    var imageContent = new ByteArrayContent(filep);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

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
        [CustomPostAuthorize]
        public JsonResult GetVehicleInfoById(int VehicleId)
        {
            string endpoint = "api/Asset/GetVehicleInfoById?VehicleId=" + VehicleId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }


        [HttpPost]
        public JsonResult GetVehicleDepoymentReport(DataTableAjaxPostModel requestModel)
        {
            requestModel.UserId = this.User.GetUserId();
            requestModel.FromDate = requestModel.FromDate != null ? Convert.ToDateTime(requestModel.FromDate) : CommonHelper.IndianStandard(DateTime.UtcNow);


            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetVehicleDepoymentReport";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        public JsonResult GetVehicleActionStatusById(int VehicleId)
        {
            string endpoint = "api/Asset/GetVehicleActionStatusById?VehicleId=" + VehicleId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }
        [CustomAuthorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllAssetNotification(string ZoneId, string CircleId, string WardId, string VehicleTypeId)
        {
            var obj = new
            {
                ZoneId = ZoneId,
                CircleId = CircleId,
                WardId = WardId,
                VehicleTypeId = VehicleTypeId,
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetAllAssetNotification";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetZoneWiseVehicle(string ZoneId)
        {
            var obj = new
            {
                ZoneId = ZoneId,
                UserId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Asset/GetZoneWiseVehicle";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
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
        public FileResult GetMultipleQrCode(string JArrayval)
        {
            JArray JArrayPVal = JArray.Parse(JArrayval);

            string wwwRoot = _host.WebRootPath;
            string compressedFileName = "QrCodes";
            var tempOutPutPath = wwwRoot + "/" + Url.Content("/TempFile/") + compressedFileName + ".zip";

            Response.Headers.Add("Content-Disposition", "attachment; filename=" + compressedFileName + ".zip");
            Response.ContentType = "application/zip";
            using (var zipStream = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                foreach (JObject item in JArrayPVal)
                {
                    string UId = string.IsNullOrEmpty(item.GetValue("UId").ToString()) ? "" : item.GetValue("UId").ToString();
                    string VehicleNo = string.IsNullOrEmpty(item.GetValue("VehicleNo").ToString()) ? "" : item.GetValue("VehicleNo").ToString();
                    string VehicleType = string.IsNullOrEmpty(item.GetValue("VehicleType").ToString()) ? "" : item.GetValue("VehicleType").ToString();
                    int OperationTypeId = string.IsNullOrEmpty(item.GetValue("OperationTypeId").ToString()) ? 0 : Convert.ToInt32(item.GetValue("OperationTypeId").ToString());
                    string Owner = string.IsNullOrEmpty(item.GetValue("OwnerType").ToString()) ? "" : item.GetValue("OwnerType").ToString();
                    string Zone = string.IsNullOrEmpty(item.GetValue("Zone").ToString()) ? "" : item.GetValue("Zone").ToString();
                    string Circle = string.IsNullOrEmpty(item.GetValue("Circle").ToString()) ? "" : item.GetValue("Circle").ToString();
                    string Ward = string.IsNullOrEmpty(item.GetValue("Ward").ToString()) ? "" : item.GetValue("Ward").ToString();
                    string LandMark = string.IsNullOrEmpty(item.GetValue("LandMark").ToString()) ? "" : item.GetValue("LandMark").ToString();
                    byte[] fileBytes = GetQrImage(UId, VehicleNo, VehicleType, Owner, Zone, Circle, Ward, OperationTypeId, LandMark);
                    // byte[] fileBytes = QRHelper.VehicleQrCodeWithJobject(item, wwwRoot + "/otherfiles/global_assets/images/QrLogo.png");
                    var fileEntry = new ZipEntry(VehicleNo + ".pdf")
                    {
                        Size = fileBytes.Length
                    };

                    zipStream.PutNextEntry(fileEntry);
                    zipStream.Write(fileBytes, 0, fileBytes.Length);

                }

                zipStream.Flush();
                zipStream.Close();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            return File(finalResult, "application/zip", compressedFileName + ".zip");
        }
        [HttpPost]
        public JsonResult GetSingleQrCode(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);

            string wwwRoot = _host.WebRootPath;

            string UId = string.IsNullOrEmpty(item.GetValue("UId").ToString()) ? "" : item.GetValue("UId").ToString();
            string VehicleNo = string.IsNullOrEmpty(item.GetValue("VehicleNo").ToString()) ? "" : item.GetValue("VehicleNo").ToString();
            string VehicleType = string.IsNullOrEmpty(item.GetValue("VehicleType").ToString()) ? "" : item.GetValue("VehicleType").ToString();
            int OperationTypeId = string.IsNullOrEmpty(item.GetValue("OperationTypeId").ToString()) ? 0 : Convert.ToInt32(item.GetValue("OperationTypeId").ToString());
            string Owner = string.IsNullOrEmpty(item.GetValue("OwnerType").ToString()) ? "" : item.GetValue("OwnerType").ToString();
            string Zone = string.IsNullOrEmpty(item.GetValue("Zone").ToString()) ? "" : item.GetValue("Zone").ToString();
            string Circle = string.IsNullOrEmpty(item.GetValue("Circle").ToString()) ? "" : item.GetValue("Circle").ToString();
            string Ward = string.IsNullOrEmpty(item.GetValue("Ward").ToString()) ? "" : item.GetValue("Ward").ToString();
            string LandMark = string.IsNullOrEmpty(item.GetValue("LandMark").ToString()) ? "" : item.GetValue("LandMark").ToString();

            byte[] fileBytes = GetQrImage(UId, VehicleNo, VehicleType, Owner, Zone, Circle, Ward, OperationTypeId, LandMark);
            string base64String = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
            //string B64Val = "data:application/pdf;base64," + base64String;
            return Json(base64String);
        }
        [HttpPost]
        public JsonResult GetSingleContainerQrCode(string Jobjval)
        {
            JObject item = JObject.Parse(Jobjval);

            string wwwRoot = _host.WebRootPath;

            string UId = string.IsNullOrEmpty(item.GetValue("UId").ToString()) ? "" : item.GetValue("UId").ToString();
            string ContainerCode = string.IsNullOrEmpty(item.GetValue("ContainerCode").ToString()) ? "" : item.GetValue("ContainerCode").ToString();
            string ContainerType = string.IsNullOrEmpty(item.GetValue("ContainerType").ToString()) ? "" : item.GetValue("ContainerType").ToString();

            byte[] fileBytes = GetContainerQrImage(UId, ContainerCode, ContainerType);
            string base64String = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
            //string B64Val = "data:application/pdf;base64," + base64String;
            return Json(base64String);
        }
        //[HttpPost]
        //public JsonResult GetSingleVehicleQrCode(string Jobjval)
        //{
        //    JObject item = JObject.Parse(Jobjval);

        //    string wwwRoot = _host.WebRootPath;

        //    string UId = item.GetValue("VehicleNo").ToString() + "_" + item.GetValue("UId").ToString();

        //    byte[] fileBytes = QRHelper.VehicleQrCodeTest(item, wwwRoot + "/otherfiles/global_assets/images/QrLogo.png");
        //    string base64String = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
        //    string B64Val = "data:image/png;base64," + base64String;
        //    return Json(B64Val);
        //}
        [HttpPost]
        public FileResult GetMultipleContainerQrCode(string JArrayval)
        {
            JArray JArrayPVal = JArray.Parse(JArrayval);

            string wwwRoot = _host.WebRootPath;
            string compressedFileName = "QrCodes";
            var tempOutPutPath = wwwRoot + "/" + Url.Content("/TempFile/") + compressedFileName + ".zip";

            Response.Headers.Add("Content-Disposition", "attachment; filename=" + compressedFileName + ".zip");
            Response.ContentType = "application/zip";
            using (var zipStream = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                foreach (JObject item in JArrayPVal)
                {
                    string UId = string.IsNullOrEmpty(item.GetValue("UId").ToString()) ? "" : item.GetValue("UId").ToString();
                    string ContainerCode = string.IsNullOrEmpty(item.GetValue("ContainerCode").ToString()) ? "" : item.GetValue("ContainerCode").ToString();
                    string ContainerType = string.IsNullOrEmpty(item.GetValue("ContainerType").ToString()) ? "" : item.GetValue("ContainerType").ToString();


                    byte[] fileBytes = GetContainerQrImage(UId, ContainerCode, ContainerType);

                    var fileEntry = new ZipEntry(ContainerCode + ".pdf")
                    {
                        Size = fileBytes.Length
                    };

                    zipStream.PutNextEntry(fileEntry);
                    zipStream.Write(fileBytes, 0, fileBytes.Length);

                }

                zipStream.Flush();
                zipStream.Close();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            return File(finalResult, "application/zip", compressedFileName + ".zip");
        }
        //[HttpPost]
        //public JsonResult GetSingleContainerQrCode(string Jobjval)
        //{
        //    JObject item = JObject.Parse(Jobjval);

        //    string wwwRoot = _host.WebRootPath;

        //    string UId = item.GetValue("ContainerCode").ToString() + "_" + item.GetValue("UId").ToString();

        //    byte[] fileBytes = QRHelper.ContainerQrCodeWithJobject(item, wwwRoot + "/otherfiles/global_assets/images/QrLogo.png");
        //    string base64String = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
        //    string B64Val = "data:image/png;base64," + base64String;
        //    return Json(B64Val);
        //}
        [HttpGet]
        public FileResult GetSingleVehicleQrCode(string UId)
        {
            string Uid = UId;
            string VehicleNo = ""; string VehicleType = ""; string Owner = ""; string Zone = ""; string Circle = ""; string Ward = ""; int OperationTypeId = 0; ; string LandMark = string.Empty;
            var finalResult = GetQrImage(Uid, VehicleNo, VehicleType, Owner, Zone, Circle, Ward, OperationTypeId, LandMark);

            return File(finalResult, "application/pdf", Uid + ".pdf");
        }

        public byte[] GetQrImage(string Uid, string VehicleNo, string VehicleType, string Owner, string Zone, string Circle, string Ward, int OperationTypeId, string LandMark)
        {
            Bitmap QRCode = QRHelper.VehicleQrCodeTest(Uid);
            var finalResult = _pdfQrCode.GenerateQRCode(QRCode, VehicleNo, VehicleType, Owner, Zone, Circle, Ward, Uid, OperationTypeId, LandMark);
            return finalResult;
        }
        public byte[] GetContainerQrImage(string Uid, string ContainerCode, string ContainerType)
        {
            Bitmap QRCode = QRHelper.VehicleQrCodeTest(Uid);
            var finalResult = _pdfQrCode.GenerateContainerQRCode(QRCode, ContainerCode, ContainerType, Uid);
            return finalResult;
        }

        [HttpPost]
        public FileResult ExportAllVehicleDetails(string FromDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string Status, string FName)
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
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetVehicleDepoymentReport";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);


            string input1 = JsonConvert.SerializeObject(requestModel);
            string endpoint1 = "api/Asset/GetAllVehicleInfo";
            HttpClientHelper<string> apiobj1 = new HttpClientHelper<string>();

            string Result1 = apiobj1.PostRequestString(endpoint1, input1);


            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllVehiclMasterDetail(Result, Result1, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public ActionResult ExportAllVehicleDetailsCSV(string FromDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string Status, string FName)
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
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : string.Empty;

            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetAllVehicleInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            //filearray = _report.ExportAllVDeployment(Result, Name);
            string csvData = _report.ExportAllVehicleDetailsCSV(Result, Name);

            // Return the CSV data as a response
            return Content(csvData, "text/csv");
        }

        [HttpPost]
        public JsonResult GetAllVehicleTypeByLogin()
        {
            string LoginId = this.User.GetUserId();
            string AppType = "Web";
            string endpoint = "api/Asset/GetAllVehicleTypeByLogin?LoginId=" + LoginId + "&AppType=" + AppType;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.GetRequest(endpoint);
            return Json(output);
        }










        [HttpPost]
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

            return Json(output);
        }

        [HttpPost]
        public FileResult ExportAllVehicleSummaryDetails(string FromDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string Status, string FName)
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
            requestModel.Status = !string.IsNullOrEmpty(Status) ? Status : string.Empty;


            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Asset/GetAllVehicleInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);


          


            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllVehicleSummaryDetail(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }


        [HttpPost]
        public JsonResult GetVehicleMasterSummaryData(string Jobjval)
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
        public FileResult ExportAllVehicleDepNotDepSummary(string SDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string Status, string FName,string Typeid)
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
            string endpoint = "api/Asset/GetAllDepVsNotDepInfo";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);





            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportAllVehicleDepNotDepSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }
        [HttpPost]
        public FileResult ExportDeployedVsReportedSummary(string SDate, string ZoneId, string CircleId, string WardId, string VehicleTypeId, string Status, string FName, string Typeid)
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
            string endpoint = "api/Asset/GetVehicleDeployedVsReportedPaging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);





            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            // Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportDeployedVsReportedSummary(Result, Name, Sname);

            return File(filearray, ContentType, filename);
        }


        [HttpPost]
        public void Delete_Vehicle()
        
        {

            string UId = Request.Query["UId"].ToString();
            //string string_veh_id = Request.QueryString["veh_id"];


            string input = UId;

            
            //string endpoint = "api/Asset/GetAllContainer";
            string endpoint = "api/Asset/Delete_Vehicle_ById";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            /*
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


            */

            

        }



    }
}

