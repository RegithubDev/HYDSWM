using COMMON;
using COMMON.CITIZEN;
using COMMON.OPERATION;
using COMMON.STAFFCOMPLAINT;
using GeoCoordinatePortable;
using GoogleMaps.LocationServices;
using HYDSWMAPI.INTERFACE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace HYDSWMAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private IComplaint<CLoginResponseInfo> _dataRepository;
        private readonly IWebHostEnvironment HostingEnvironment;
        public ComplaintController(IComplaint<CLoginResponseInfo> dataRepository, IWebHostEnvironment hostingEnvironment)
        {
            this._dataRepository = dataRepository;
            this.HostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        [Route("GetAllComplaintCategory")]
        public IActionResult GetAllComplaintCategory(JObject obj)
        {
            string Result = string.Empty;
            int ComplaintTypeId = obj.GetValue("ComplaintTypeId").Value<int>();
            string IsAll = obj.GetValue("IsAll").Value<string>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                  new SqlParameter("@ComplaintTypeId",ComplaintTypeId),
                  new SqlParameter("@IsAll",IsAll),
             };
            if (ComplaintTypeId == 0)
                Result = _dataRepository.ExecuteQueryDynamicSqlParameter(StoredProcedureHelper.spGetAllComplaintCategory, parameters);
            else
                Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spGetAllComplaintCategory, parameters);
            return Ok(Result);
        }
        [HttpPost]
        [Route("AddStaffComplaint")]
        public async Task<IActionResult> AddStaffComplaint([FromForm] IFormCollection value)
        {

            string FName = string.Empty;
            SComplaintInfo obj = JsonConvert.DeserializeObject<SComplaintInfo>(value["Input"]);
            string FolderName = "/content/SComplaint/";
            if (value.Files.Count > 0)
                if (value.Files[0].Length > 0)
                    FName = CommonHelper.generateID() + Path.GetExtension(value.Files[0].FileName);
            /*
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start(); //started watcher
            System.Device.Location.GeoCoordinate coord = watcher.Position.Location;
            if (!watcher.Position.Location.IsUnknown)
            {
                double lat = coord.Latitude; //latitude
                double long = coord.Longitude;  //logitude
            }
            */

            string filePath = Path.Combine(HostingEnvironment.WebRootPath + FolderName, FName);
            DateTime TDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("@ComplaintTypeId",obj.ComplaintTypeId),
                  new SqlParameter("@ComplaintDate",TDate),
                  new SqlParameter("@Complaintcode",string.Empty),
                  new SqlParameter("@TSId",obj.TSId),
                  new SqlParameter("@Location",obj.Location),
                  new SqlParameter("@Remarks",obj.Remarks),
                  new SqlParameter("@FLat",obj.FLat),
                  new SqlParameter("@FLng",obj.FLng),
                  new SqlParameter("@FAddress",obj.FAddress),
                  new SqlParameter("@FImgUrl",FName),
                  new SqlParameter("@ModeOfReporting","APP"),
                  new SqlParameter("@CreatedDate",TDate),
                  new SqlParameter("@CreatedBy",obj.CreatedBy),
                  new SqlParameter("@FolderName",FolderName),
              };

            string Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spAddStaffComplaint, parameters);
            dynamic dresult = JObject.Parse(Result);
            if (dresult.Result == "1" || dresult.Result == "2")
                if (value.Files.Count > 0)
                {
                    if (value.Files[0].Length > 0)
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await value.Files[0].CopyToAsync(fileStream);
                        }
                }
            return Ok(Result);
        }

        [HttpPost]
        [Route("GetAllStaffComplaintInfo")]
        public IActionResult GetAllStaffComplaintInfo(JObject obj)
        {
            string baseUrl = Startup.StaticConfig.GetValue<string>("ApiBaseUrl");
            string PageNumber = obj.GetValue("PageNumber").Value<string>();
            string PageSize = obj.GetValue("PageSize").Value<string>();
            string UserId = obj.GetValue("UserId").Value<string>();
            string SearchTerm = obj.GetValue("SearchTerm").Value<string>();
            string IsClick = obj.GetValue("IsClick").Value<string>();
            string NotiId = obj.GetValue("NotiId").Value<string>();
            DateTime FromDate = obj.GetValue("FromDate").Value<DateTime>();
            DateTime ToDate = obj.GetValue("ToDate").Value<DateTime>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                  new SqlParameter("@SearchTerm",SearchTerm),
                  new SqlParameter("@SortColumn",""),
                  new SqlParameter("@SortOrder",""),
                  new SqlParameter("@PageNumber",PageNumber),
                  new SqlParameter("@PageSize",PageSize),
                  new SqlParameter("@UserId",UserId),
                  new SqlParameter("@NotiId",NotiId),
                  new SqlParameter("@FPath",baseUrl),
                  new SqlParameter("@FromDate",FromDate),
                  new SqlParameter("@ToDate",ToDate),
                  new SqlParameter("@AccessBy","App"),
                  new SqlParameter("@IsClick",IsClick),
             };

            string Result = _dataRepository.ExecuteQueryDynamicSqlParameter(StoredProcedureHelper.spGetAllStaffComplaint_Paging, parameters);

            return Ok(Result);
        }


        [HttpGet]
        [Route("GetComplaintInfoById")]
        public IActionResult GetComplaintInfoById()
        {

            int CId = Convert.ToInt32(Request.Query["CId"]);


            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("@SComplaintId",CId)
              };

            string Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spGetComplaintInfoById, parameters);
            return Ok(Result);
        }


        [HttpPost]
        [Route("GetAllStaffComplaint_Paging")]
        public IActionResult GetAllStaffComplaint_Paging(DataTableAjaxPostModel requestModel)
        {
            string baseUrl = Startup.StaticConfig.GetValue<string>("ApiBaseUrl");
            Order _order = new Order();

            string SearchTXT = requestModel.search.value;
            int draw = requestModel.draw;
            int start = requestModel.start;
            int length = requestModel.length;

            string str = string.Empty;
            if (SearchTXT != null)
                str = SearchTXT.Trim();

            string SortColumn = string.Empty;
            string SortDir = requestModel.order[0].dir;
            switch (requestModel.order[0].column)
            {
                case 2:
                    SortColumn = "AssetStatus";
                    break;
                case 3:
                    SortColumn = "UId";
                    break;
                case 4:
                    SortColumn = "ContainerCode";
                    break;
                case 5:
                    SortColumn = "ContainerName";
                    break;
                case 6:
                    SortColumn = "Capacity";
                    break;
                case 7:
                    SortColumn = "ContainerType";
                    break;
                case 8:
                    SortColumn = "CStatus";
                    break;

                default:
                    SortDir = String.Empty;
                    SortColumn = string.Empty;
                    break;
            }
            SqlParameter[] parameters = new SqlParameter[]
             {
                  new SqlParameter("@SearchTerm",str),
                  new SqlParameter("@SortColumn",SortColumn),
                  new SqlParameter("@SortOrder",SortDir),
                  new SqlParameter("@PageNumber",start),
                  new SqlParameter("@PageSize",length),
                  new SqlParameter("@UserId",requestModel.UserId),
                  new SqlParameter("@FPath",baseUrl),
                  new SqlParameter("@NotiId",requestModel.NotiId),
                  new SqlParameter("@FromDate",requestModel.FromDate),
                  new SqlParameter("@ToDate",requestModel.ToDate),
                  new SqlParameter("@AccessBy","WEB"),
                  new SqlParameter("@IsClick",!string.IsNullOrEmpty(requestModel.ContratorId)?requestModel.ContratorId:"0"),
             };

            
            string Result = _dataRepository.ExecuteQueryDynamicSqlParameter(StoredProcedureHelper.spGetAllStaffComplaint_Paging, parameters);

            return Ok(Result);
        }


        [HttpPost]
        [Route("GetAllStaffComplaint_PagingB64")]
        public IActionResult GetAllStaffComplaint_PagingB64(DataTableAjaxPostModel requestModel)
        {
            string baseUrl = Startup.StaticConfig.GetValue<string>("ApiBaseUrl");
            Order _order = new Order();

            string SearchTXT = requestModel.search.value;
            int draw = requestModel.draw;
            int start = requestModel.start;
            int length = requestModel.length;

            string str = string.Empty;
            if (SearchTXT != null)
                str = SearchTXT.Trim();

            string SortColumn = string.Empty;
            string SortDir = requestModel.order[0].dir;
            switch (requestModel.order[0].column)
            {
                case 2:
                    SortColumn = "AssetStatus";
                    break;
                case 3:
                    SortColumn = "UId";
                    break;
                case 4:
                    SortColumn = "ContainerCode";
                    break;
                case 5:
                    SortColumn = "ContainerName";
                    break;
                case 6:
                    SortColumn = "Capacity";
                    break;
                case 7:
                    SortColumn = "ContainerType";
                    break;
                case 8:
                    SortColumn = "CStatus";
                    break;

                default:
                    SortDir = String.Empty;
                    SortColumn = string.Empty;
                    break;
            }
            SqlParameter[] parameters = new SqlParameter[]
             {
                  new SqlParameter("@SearchTerm",str),
                  new SqlParameter("@SortColumn",SortColumn),
                  new SqlParameter("@SortOrder",SortDir),
                  new SqlParameter("@PageNumber",start),
                  new SqlParameter("@PageSize",length),
                  new SqlParameter("@UserId",requestModel.UserId),
                  new SqlParameter("@FPath",baseUrl),
                  new SqlParameter("@NotiId",requestModel.NotiId),
                  new SqlParameter("@FromDate",requestModel.FromDate),
                  new SqlParameter("@ToDate",requestModel.ToDate),
                  new SqlParameter("@AccessBy","WEB"),
                  new SqlParameter("@IsClick",!string.IsNullOrEmpty(requestModel.ContratorId)?requestModel.ContratorId:"0"),
             };


            string Result = _dataRepository.ExecuteQueryDynamicSqlParameter(StoredProcedureHelper.spGetAllStaffComplaint_Paging, parameters);

            List<AllVehicle> Mhlst = JsonConvert.DeserializeObject<List<AllVehicle>>(Result);
            string strJson = JsonConvert.SerializeObject(Mhlst);
            return Ok(strJson);
        }


        [HttpPost]
        [Route("UpdateStaffComplaint")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStaffComplaint([FromForm] IFormCollection value)
        {
            string FolderName = "/content/SComplaint/";
            string Result = string.Empty;
            string FName = string.Empty;
            dynamic obj = JObject.Parse(value["Input"]);

            string CCId = obj.CCId;
            string ActionRemark = obj.ActionRemark;
            string StatusId = obj.StatusId;
            string UserId = obj.UserId;
            string CAddress = obj.CAddress;
            string CLng = obj.CLng;
            string CLat = obj.CLat;

            if (value.Files.Count > 0)
                if (value.Files[0].Length > 0)
                    FName = CommonHelper.generateID() + Path.GetExtension(value.Files[0].FileName);

            var filePath = Path.Combine(HostingEnvironment.WebRootPath + FolderName, FName);
            DateTime TDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("@CCId",CCId),
                  new SqlParameter("@ActionRemark",ActionRemark),
                  new SqlParameter("@CreatedBy",UserId),
                  new SqlParameter("@CLat",CLat),
                  new SqlParameter("@CLng",CLng),
                  new SqlParameter("@CAddress",CAddress),
                  new SqlParameter("@CFName",FName),
              };
            //object[] mparameters = { obj.GetValue("CCId").Value<string>(), obj.GetValue("ComplaintNo").Value<string>(), obj.GetValue("ActionTaken").Value<string>(), obj.GetValue("UserId").Value<string>() };
            Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spUpdateStaffComplaint, parameters);

            dynamic dresult = JObject.Parse(Result);
            if (dresult.Result == 1)
            {
                if (value.Files.Count > 0)
                {
                    if (value.Files[0].Length > 0)
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await value.Files[0].CopyToAsync(fileStream);
                        }
                    }
                }
            }
            return Ok(Result);
        }

        [HttpPost]
        [Route("GetAllComplaintNotification")]
        public IActionResult GetAllComplaintNotification(JObject obj)
        {
            string LoginId = obj.GetValue("LoginId").Value<string>();
            string AccessBy = obj.GetValue("AccessBy").Value<string>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                  new SqlParameter("@LoginId",LoginId),
                  new SqlParameter("@TDate",CommonHelper.IndianStandard(DateTime.UtcNow)),
                  new SqlParameter("@AccessBy",AccessBy),
             };

            string Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spGetAllComplaintNotification, parameters);

            return Ok(Result);
        }


        [HttpPost]
        [Route("UpdateCCategory")]
        public IActionResult UpdateCCategory(JObject obj)
        {


            //string revised_ward_num = obj.GetValue("revised_ward_num").Value<string>();
            string Status = obj.GetValue("Status").Value<string>();
            string Action_Remark = obj.GetValue("Action_Remark").Value<string>();
            string complaint_address = obj.GetValue("complaint_address").Value<string>();
            string comp_id = obj.GetValue("comp_id").Value<string>();
            string complaint_num = obj.GetValue("complaint_num").Value<string>();
            bool IsActive;




            var uploads = "D:\\New folder\\hyd\\HYDSWM\\HYDSWM\\wwwroot\\ViewJs\\Complaint\\uploads";


            //Path.GetExtension(value.Files[0].FileName);

            /*
            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("@ComplaintTypeId",Complaint_num),
                  new SqlParameter("@ComplaintType",Complaint_add),
                  new SqlParameter("@Complaint_descrip",Complaint_descrip),
                  new SqlParameter("@Transfer_station",Transfer_station),
                  new SqlParameter("@IsActive",IsActive)
              };

            */


            int comp_id1 = 0;

            DateTime TDate = CommonHelper.IndianStandard(DateTime.UtcNow);

            if (comp_id != null)
                comp_id1 = Int32.Parse(comp_id);
            
            string Result;





            SqlParameter[] parameters = new SqlParameter[]
          {
                  new SqlParameter("@CCId",comp_id1),
                  new SqlParameter("@ComplaintDate",TDate),
                  new SqlParameter("@ComplaintCode", complaint_num),
                  new SqlParameter("@TSId",23),
                  //new SqlParameter("@revised_ward_num",revised_ward_num),
                  new SqlParameter("@Remarks",Action_Remark),
                  new SqlParameter("@FLat",22.7),
                  new SqlParameter("@FLng",5.7),
                  new SqlParameter("@FAddress",string.Empty),
                  new SqlParameter("@FImgUrl",string.Empty),
                  new SqlParameter("@ModeOfReporting","APP"),
                  new SqlParameter("@CreatedDate",TDate),
                  new SqlParameter("@CreatedBy",string.Empty),
                  new SqlParameter("@FolderName","uploads/"),
                  new SqlParameter("@Action_Remark",Action_Remark),
                  new SqlParameter("@address",complaint_address)
                  
              };



                Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spUpdateStaffComplaint, parameters);
            
            return Ok(Result);
        }





















        [HttpPost]
        [Route("SaveAndUpdateCCategory")]
        public IActionResult SaveAndUpdateCCategory(JObject obj)
        {


            string RegDate = obj.GetValue("RegDate").Value<string>();
            string ddlRegType = obj.GetValue("ddlRegType").Value<string>();
            string citizen_name = obj.GetValue("citizen_name").Value<string>();
            string citizen_email = obj.GetValue("citizen_email").Value<string>();
            string txtContactNo = obj.GetValue("txtContactNo").Value<string>();
            string ddlComplaint_Type = obj.GetValue("ddlComplaint_Type").Value<string>();
            string ddlWard = obj.GetValue("ddlWard").Value<string>();
            string txt_circle = obj.GetValue("txt_circle").Value<string>();
            string txt_Zone = obj.GetValue("txt_Zone").Value<string>();
            string file = obj.GetValue("file").Value<string>();
            string complaint_add = obj.GetValue("complaint_add").Value<string>();
            string complaint_descrip = obj.GetValue("complaint_descrip").Value<string>();
            
            bool IsActive;


            

            var uploads = "D:\\New folder\\hyd\\HYDSWM\\HYDSWM\\wwwroot\\ViewJs\\Complaint\\uploads";


            //Path.GetExtension(value.Files[0].FileName);

            /*
            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("@ComplaintTypeId",Complaint_num),
                  new SqlParameter("@ComplaintType",Complaint_add),
                  new SqlParameter("@Complaint_descrip",Complaint_descrip),
                  new SqlParameter("@Transfer_station",Transfer_station),
                  new SqlParameter("@IsActive",IsActive)
              };

            */

            //int comp_id = 0;

            DateTime TDate = CommonHelper.IndianStandard(DateTime.UtcNow);

             //if(SComplaintId != null)
           // comp_id = Int32.Parse(SComplaintId);
            
            string Result;





            SqlParameter[] parameters = new SqlParameter[]
          {

                  new SqlParameter("@ComplaintTypeId",1),
                  new SqlParameter("@ComplaintDate",TDate),
                  new SqlParameter("@TSId",23),
                  new SqlParameter("citizen_email",citizen_email),
                  new SqlParameter("@Location",complaint_add),
                  new SqlParameter("@Remarks",complaint_descrip),
                  new SqlParameter("@FLat",22.7),
                  new SqlParameter("@FLng",5.7),
                  new SqlParameter("@FAddress",string.Empty),
                  new SqlParameter("@FImgUrl",string.Empty),
                  new SqlParameter("@ModeOfReporting","WEB"),
                  new SqlParameter("@CreatedDate",TDate),
                  new SqlParameter("@CreatedBy",string.Empty),
                  new SqlParameter("@FolderName","uploads/"),
                  new SqlParameter("@ComplaintName",citizen_name),
                  new SqlParameter("@ComplaintContactNumber",txtContactNo),
                  new SqlParameter("@Wardname",ddlWard),
                  new SqlParameter("@Circlename",txt_circle),
                  new SqlParameter("@zonename",txt_Zone),
                  new SqlParameter("@RegDate",RegDate),
                  new SqlParameter("@RegType",ddlRegType)
                  


         };
                    Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spAddStaffComplaint, parameters);
            
            
            return Ok(Result);
        }

        /*
        [HttpPost]
        [Route("AddStaffComplaint")]
        public async Task<IActionResult> AddStaffComplaint([FromForm] IFormCollection value)
        {

            string FName = string.Empty;
            SComplaintInfo obj = JsonConvert.DeserializeObject<SComplaintInfo>(value["Input"]);
            string FolderName = "/content/SComplaint/";
            if (value.Files.Count > 0)
                if (value.Files[0].Length > 0)
                    FName = CommonHelper.generateID() + Path.GetExtension(value.Files[0].FileName);

            string filePath = Path.Combine(HostingEnvironment.WebRootPath + FolderName, FName);
            DateTime TDate = CommonHelper.IndianStandard(DateTime.UtcNow);
            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("@ComplaintTypeId",obj.ComplaintTypeId),
                  new SqlParameter("@ComplaintDate",TDate),
                  new SqlParameter("@Complaintcode",string.Empty),
                  new SqlParameter("@TSId",obj.TSId),
                  new SqlParameter("@Location",obj.Location),
                  new SqlParameter("@Remarks",obj.Remarks),
                  new SqlParameter("@FLat",obj.FLat),
                  new SqlParameter("@FLng",obj.FLng),
                  new SqlParameter("@FAddress",obj.FAddress),
                  new SqlParameter("@FImgUrl",FName),
                  new SqlParameter("@ModeOfReporting","APP"),
                  new SqlParameter("@CreatedDate",TDate),
                  new SqlParameter("@CreatedBy",obj.CreatedBy),
                  new SqlParameter("@FolderName",FolderName),
              };

            string Result = _dataRepository.ExecuteQuerySingleDataTableDynamic(StoredProcedureHelper.spAddStaffComplaint, parameters);
            dynamic dresult = JObject.Parse(Result);
            if (dresult.Result == "1" || dresult.Result == "2")
                if (value.Files.Count > 0)
                {
                    if (value.Files[0].Length > 0)
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await value.Files[0].CopyToAsync(fileStream);
                        }
                }
            return Ok(Result);
        }

        */



    }
}
