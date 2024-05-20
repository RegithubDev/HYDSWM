using COMMON;
using COMMON.ASSET;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using HYDSWM;
using HYDSWM.Helpers;
using HYDSWMAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
    public class ComplaintController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private TReport _report;
        public ComplaintController(IWebHostEnvironment host, TReport report)
        {
            this._host = host;
            this._report = report;
        }

        static string apiBaseUrl = Startup.StaticConfig.GetValue<string>("WebAPIBaseUrl");
        static string BasicAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("patna:patna#2020"));
        [CustomAuthorize]
        public IActionResult AllComplaint()
        {
            ViewBag.RoleId = this.User.GetRoleId();
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllStaffComplaint_Paging(DataTableAjaxPostModel requestModel)
        {
            requestModel.CCode = this.User.GetCompanyCode();
            requestModel.UserId = this.User.GetUserId();
            requestModel.ContratorId = !string.IsNullOrEmpty(requestModel.ContratorId) ? requestModel.ContratorId : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(requestModel.NotiId) ? requestModel.NotiId : "";
            requestModel.FromDate = requestModel.FromDate.ToString()== "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow): requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow): requestModel.ToDate;
            string input = JsonConvert.SerializeObject(requestModel);
            //string endpoint = "api/Complaint/GetAllStaffComplaint_Paging";
            string endpoint = "api/Complaint/GetAllStaffComplaint_PagingB64";
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
        public JsonResult GetComplaintInfoById(string CId)
        {
            string endpoint = "api/Complaint/GetComplaintInfoById?CId=" + CId;
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string Result = apiobj.GetRequest(endpoint);
            return Json(Result);
        }




        [HttpPost]
        public FileResult ExportGetAllStaffComplaint_Paging(string ContratorId, string NotiId, string FName)
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
            requestModel.ContratorId = !string.IsNullOrEmpty(requestModel.ContratorId) ? requestModel.ContratorId : "0";
            requestModel.NotiId = !string.IsNullOrEmpty(requestModel.NotiId) ? requestModel.NotiId : "";
            requestModel.FromDate = requestModel.FromDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.FromDate;
            requestModel.ToDate = requestModel.ToDate.ToString() == "1/1/0001 12:00:00 AM" ? CommonHelper.IndianStandard(DateTime.UtcNow) : requestModel.ToDate;
            requestModel.length = -1;
            string input = JsonConvert.SerializeObject(requestModel);
            string endpoint = "api/Complaint/GetAllStaffComplaint_Paging";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();

            string Result = apiobj.PostRequestString(endpoint, input);

            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filename = Name + ReportTime.ToShortDateString() + ".xlsx";
            //Name += Name + " From-" + FromDate.ToString("dd-MM-yyyy") + " To-" + ToDate.ToString("dd-MM-yyyy");
            filearray = _report.ExportGetAllStaffComplaint_Paging(Result, Name);

            return File(filearray, ContentType, filename);
        }


        [HttpPost]
        public async Task<JsonResult> UpdateStaffComplaint([FromForm] IFormCollection formData)
        {

            var url = apiBaseUrl + "api/Complaint/UpdateStaffComplaint"; ;
            string msg = string.Empty;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", BasicAuth);
            MultipartFormDataContent form = new MultipartFormDataContent();

            if (formData.Files.Count > 0)
            {
                var filePath = formData.Files[0].FileName;
                byte[] filep = ConvertToBytes(formData.Files[0]); //File.ReadAllBytes(filePath);
                Stream frequestStream = new MemoryStream(filep);
                var streamContent = new StreamContent(frequestStream);

                var imageContent = new ByteArrayContent(filep);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                form.Add(imageContent, "image", filePath);
            }
            form.Add(new StringContent(formData["Input"]), "Input");
            var response = await httpClient.PostAsync(url, form);
            string stringContent = await response.Content.ReadAsStringAsync();


            dynamic Result = JObject.Parse(stringContent);
            switch (Result.Result.ToString())
            {

                case "0":
                    msg = "Something Went Wrong!";
                    break;
                case "1":
                    msg = Result.Msg;
                    break;


            }

            var objresult = new
            {
                data = Result,
                Msg = msg
            };
            return Json(objresult);
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

        [CustomPostAuthorize]
        public IActionResult AllComplaintNoti()
        {
            ViewBag.RoleId = this.User.GetRoleId();
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }
        [CustomAuthorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [CustomPostAuthorize]
        public JsonResult GetAllComplaintNotification()
        {
            var obj = new
            {
                AccessBy = "WEB",
                LoginId = this.User.GetUserId()
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Complaint/GetAllComplaintNotification";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }
        [CustomAuthorize]
        public IActionResult RptAllComplaint()
        {
            ViewBag.RoleId = this.User.GetRoleId();
            ViewBag.LoginId = this.User.GetUserId();
            return View();
        }
        [CustomAuthorize]
        public IActionResult AllComplaintCategory()
        {
            return View();
        }
        public IActionResult AddCCategory()
        {
            return PartialView();
        }
        [CustomPostAuthorize]
        public JsonResult SaveAndUpdateCCategory(string jobj)
        {
            dynamic dresult = JObject.Parse(jobj);
            string ComplaintTypeId = dresult.ComplaintTypeId;
            string ComplaintType = dresult.ComplaintType;
            string IsActive = dresult.IsActive;
            var obj = new
            {
                ComplaintTypeId = !string.IsNullOrEmpty(ComplaintTypeId) ? ComplaintTypeId : "0",
                ComplaintType = ComplaintType,
                IsActive = IsActive,
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Complaint/SaveAndUpdateCCategory";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }

        [CustomPostAuthorize]
        public object SaveAndUpdateComplaintInfo()
        {

            string input1 = Request.Query["Complaintdata"].ToString();
            string formdata = Request.Form.ToString();


            var formFile = HttpContext.Request.Form.Files[0];

            var fileName = formFile.FileName;

            var uploads = "D:\\New folder\\hyd\\HYDSWM\\HYDSWM\\wwwroot\\ViewJs\\Complaint\\uploads";
            string FileName = formFile.FileName;
            using (var fileStream = new FileStream(Path.Combine(uploads, FileName), FileMode.Create))
            {
                formFile.CopyToAsync(fileStream);
            }


           


            dynamic dresult = JObject.Parse(input1);
           
            string complaint_name = dresult.complaint_name;

            string complaint_num = dresult.complaint_num;

            string ddlCategory = dresult.ddlCategory;

            string complaint_add = dresult.complaint_add;

            string ddlWard = dresult.ddlWard;

            string txt_circle = dresult.txt_circle;

            string txt_Zone = dresult.txt_Zone;

            string Complaint_descrip = dresult.Complaint_descrip;

            string add_upd1 = dresult.add_upd1;

            string SComplaintId = dresult.SComplaintId;

            string RegDate = dresult.RegDate;

            string RegType = dresult.RegType;





            var obj = new
            {
                complaint_name = complaint_name,
                complaint_num = complaint_num,
                ddlCategory = ddlCategory,
                ddlWard = ddlWard,
                txt_circle = txt_circle,
                txt_Zone = txt_Zone,
                Complaint_descrip = Complaint_descrip,
                add_upd1 = add_upd1,
                SComplaintId = SComplaintId,
                complaint_add = complaint_add,
                RegDate = RegDate


            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Complaint/SaveAndUpdateCCategory";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }


        [CustomPostAuthorize]
        public object UpdateComplaintInfo()
        {

            string input1 = Request.Query["Complaintdata"].ToString();
            string formdata = Request.Form.ToString();

            /*

            var formFile = HttpContext.Request.Form.Files[0];

            var fileName = formFile.FileName;

            var uploads = "D:\\New folder\\hyd\\HYDSWM\\HYDSWM\\wwwroot\\ViewJs\\Complaint\\uploads";
            string FileName = formFile.FileName;
            using (var fileStream = new FileStream(Path.Combine(uploads, FileName), FileMode.Create))
            {
                formFile.CopyToAsync(fileStream);
            }

            */



            dynamic dresult = JObject.Parse(input1);

            string revised_ward_num = dresult.revised_ward_num;

            string Status = dresult.Status;

            string Action_Remark = dresult.Action_Remark;

            string address = dresult.address;

            string SComplaintId = dresult.SComplaintId;

            string comp_id = dresult.comp_id;

            string complaint_num = dresult.complaint_num;
            var obj = new
            {
                revised_ward_num = revised_ward_num,
                Status = Status,
                Action_Remark = Action_Remark,
                address = address,
                comp_id = comp_id,
                complaint_num = complaint_num
            };

            string input = JsonConvert.SerializeObject(obj);
            string endpoint = "api/Complaint/UpdateCCategory";
            HttpClientHelper<string> apiobj = new HttpClientHelper<string>();
            string output = apiobj.PostRequestString(endpoint, input);
            return Json(output);
        }



        public IActionResult AddComplaint()
        {

            


            return PartialView();
        }

        public IActionResult UpdateComplaint()
        {




            return PartialView();
        }


    }
}
