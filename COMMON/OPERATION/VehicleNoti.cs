using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COMMON.OPERATION
{
   public class VehicleNoti
    {
        private string _Img1Url;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int CTDId { get; set; }
        public string OperationType { get; set; }
        public string Step1UId { get; set; }
        public string ContainerName { get; set; }
        public string ContainerCode { get; set; }
        public string Step2UId { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string TStationName { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ContactNo { get; set; }
        public bool IsDeviated { get; set; }
        public string ImgUrl
        {
            get
            {
                return _Img1Url;
            }
            set
            {
                _Img1Url = value;
            }
        }
        public string Img1Base64
        {
            get
            {
                try
                {
                    string Path = CommonHelper.Web_Api_Local_Path + "images/";//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\images\";
                    string Result = string.Empty;
                    if (!string.IsNullOrEmpty(_Img1Url))
                    {

                        int pos = _Img1Url.LastIndexOf("/") + 1;
                        string FFile = _Img1Url.Substring(pos, _Img1Url.Length - pos);
                        //string FFile = "348268764.jpg";
                        string FExtension = FFile.Split('.')[1];
                        string FPath = Path + FFile;
                        if (File.Exists(FPath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(FPath);
                            Result = "data:image/" + FExtension + ";base64," + Convert.ToBase64String(imageArray);
                        }
                    }
                    return Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


        }


    }

    public class VehicleInfo
    {
        private string _Img1Url;
        private string _FolderName;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int CTDId { get; set; }
        public string OperationType { get; set; }
        public string Step1UId { get; set; }
        public string ContainerName { get; set; }
        public string ContainerCode { get; set; }
        public string Step2UId { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string TStationName { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ContactNo { get; set; }
        public bool IsDeviated { get; set; }
        public string ImgUrl
        {
            get
            {
                return _Img1Url;
            }
            set
            {
                _Img1Url = value;
            }
        }
        public string FolderName
        {
            get
            {
                return _FolderName;
            }
            set
            {
                _FolderName = value;
            }
        }
        public string Img1Base64
        {
            get
            {
                try
                {
                    string Path = CommonHelper.Web_Api_Local_Path + _FolderName;//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\images\";
                    string Result = string.Empty;
                    if (!string.IsNullOrEmpty(_Img1Url))
                    {

                        int pos = _Img1Url.LastIndexOf("/") + 1;
                        string FFile = _Img1Url.Substring(pos, _Img1Url.Length - pos);
                        //string FFile = "348268764.jpg";
                        string FExtension = FFile.Split('.')[1];
                        string FPath = Path + FFile;
                        if (File.Exists(FPath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(FPath);
                            Result = "data:image/" + FExtension + ";base64," + Convert.ToBase64String(imageArray);
                        }
                    }
                    return Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


        }

    }
    public class VehicleInfoB64
    {
        private string _Img1Url;
        private string _FolderName;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int CTDId { get; set; }
        public string OperationType { get; set; }
        public string Step1UId { get; set; }
        public string ContainerName { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerType { get; set; }
        public string Step1Lat { get; set; }
        public string Step1Lng { get; set; }
        public string Step1CreatedOn { get; set; }
        public string Step2UId { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string Step2Lat { get; set; }
        public string Step2Lng { get; set; }
        public double Step2Filled { get; set; }
        public string Step2CreatedOn { get; set; }
        public string TStationName { get; set; }
        public string TSType { get; set; }
        public string TSLat { get; set; }
        public string TSLng { get; set; }
        public double Radius { get; set; }
        public bool IsClosed { get; set; }
        public double DistanceFromTS { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ContactNo { get; set; }
        public string Remarks { get; set; }
        public int IsRejected { get; set; }
        public string ActionStatus { get; set; }
        public bool IsDeviated { get; set; }
        public string ImgUrl
        {
            get
            {
                return _Img1Url;
            }
            set
            {
                _Img1Url = value;
            }
        }
        public string FolderName
        {
            get
            {
                return _FolderName;
            }
            set
            {
                _FolderName = value;
            }
        }
        public string Img1Base64
        {
            get
            {
                try
                {
                    string Path = CommonHelper.Web_Api_Local_Path + _FolderName;//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\images\";
                    string Result = string.Empty;
                    if (!string.IsNullOrEmpty(_Img1Url))
                    {

                        int pos = _Img1Url.LastIndexOf("/") + 1;
                        string FFile = _Img1Url.Substring(pos, _Img1Url.Length - pos);
                        //string FFile = "348268764.jpg";
                        string FExtension = FFile.Split('.')[1];
                        string FPath = Path + FFile;
                        if (File.Exists(FPath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(FPath);
                            Result = "data:image/" + FExtension + ";base64," + Convert.ToBase64String(imageArray);
                        }
                    }
                    return Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


        }
    }

    public class AllVehicle
    {
        private string _Img1Url;
        private string _Img2Url;
        private string _FolderName;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int SComplaintId { get; set; }
        public string ComplaintType { get; set; }
        public string ComplaintOn { get; set; }
        public string Complaintcode { get; set; }
        public string Location { get; set; }
        public string TStationName { get; set; }
        public string TSType { get; set; }
        public string Remarks { get; set; }
        public string FLat { get; set; }
        public string FLng { get; set; }
        public string FAddress { get; set; }
        public string CLat { get; set; }
        public string CLng { get; set; }
        public string CAddress { get; set; }
        public string ModeOfReporting { get; set; }

        public string PredefinedSLADuration { get; set; }
        public string Status { get; set; }
        public bool IsClosed { get; set; }
        public string CreatedBy { get; set; }
        public string ClosedBy { get; set; }
        public string CRemarks { get; set; }
        public string ClosedOn { get; set; }
        public string TotalTimeTaken { get; set; }
        public string LabelClass { get; set; }
        public string WardName { get; set; }

        public string ZoneName { get;set; }

        public string CircleName { get; set; }

        public string ComplaintName { get; set; }

        public string ComplaintContactNumber { get; set; }

        public string RegistrationDate { get; set; }
        public string RegistrationType { get; set; }

        public string FImgUrl
        {
            get
            {
                return _Img1Url;
            }
            set
            {
                _Img1Url = value;
            }
        }
        public string CImgUrl
        {
            get
            {
                return _Img2Url;
            }
            set
            {
                _Img2Url = value;
            }
        }
        public string FolderName
        {
            get
            {
                return _FolderName;
            }
            set
            {
                _FolderName = value;
            }
        }
        public string Img1Base64
        {
            get
            {
                try
                {
                    string Path = CommonHelper.Web_Api_Local_Path + _FolderName;//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\images\";
                    string Result = string.Empty;
                    if (!string.IsNullOrEmpty(_Img1Url))
                    {

                        int pos = _Img1Url.LastIndexOf("/") + 1;
                        string FFile = _Img1Url.Substring(pos, _Img1Url.Length - pos);
                        //string FFile = "348268764.jpg";
                        string FExtension = FFile.Split('.')[1];
                        string FPath = Path + FFile;
                        if (File.Exists(FPath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(FPath);
                            Result = "data:image/" + FExtension + ";base64," + Convert.ToBase64String(imageArray);
                        }
                    }
                    return Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


        }

        public string Img2Base64
        {
            get
            {
                try
                {
                    string Path = CommonHelper.Web_Api_Local_Path + _FolderName;//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\images\";
                    string Result = string.Empty;
                    if (!string.IsNullOrEmpty(_Img2Url))
                    {

                        int pos = _Img2Url.LastIndexOf("/") + 1;
                        string FFile = _Img2Url.Substring(pos, _Img2Url.Length - pos);
                        //string FFile = "348268764.jpg";
                        string FExtension = FFile.Split('.')[1];
                        string FPath = Path + FFile;
                        if (File.Exists(FPath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(FPath);
                            Result = "data:image/" + FExtension + ";base64," + Convert.ToBase64String(imageArray);
                        }
                    }
                    return Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


        }

    }

    public class AllVehicleB64 
    {
        private string _Img1Url;
        private string _Img2Url;
        private string _FolderName;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int PClId { get; set; }
        public string PointName { get; set; }
        public string RouteCode { get; set; }
        public string VehicleNo { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string PickDTime { get; set; }
        public string TripName { get; set; }
        public string Img1Url
        {
            get
            {
                return _Img1Url;
            }
            set
            {
                _Img1Url = value;
            }
        }
        public string Img2Url
        {
            get
            {
                return _Img2Url;
            }
            set
            {
                _Img2Url = value;
            }
        }

        public string FolderName
        {
            get
            {
                return _FolderName;
            }
            set
            {
                _FolderName = value;
            }
        }
        public string Img1Base64
        {
            get
            {
                try
                {
                    string Path = CommonHelper.Web_Api_Local_Path + _FolderName;//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\images\";
                    string Result = string.Empty;
                    if (!string.IsNullOrEmpty(_Img1Url))
                    {

                        int pos = _Img1Url.LastIndexOf("/") + 1;
                        string FFile = _Img1Url.Substring(pos, _Img1Url.Length - pos);
                        //string FFile = "348268764.jpg";
                        string FExtension = FFile.Split('.')[1];
                        string FPath = Path + FFile;
                        if (File.Exists(FPath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(FPath);
                            Result = "data:image/" + FExtension + ";base64," + Convert.ToBase64String(imageArray);
                        }
                    }
                    return Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


        }

        public string Img2Base64
        {
            get
            {
                try
                {
                    string Path = CommonHelper.Web_Api_Local_Path + _FolderName;//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\images\";
                    string Result = string.Empty;
                    if (!string.IsNullOrEmpty(_Img2Url))
                    {

                        int pos = _Img2Url.LastIndexOf("/") + 1;
                        string FFile = _Img2Url.Substring(pos, _Img2Url.Length - pos);
                        //string FFile = "348268764.jpg";
                        string FExtension = FFile.Split('.')[1];
                        string FPath = Path + FFile;
                        if (File.Exists(FPath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(FPath);
                            Result = "data:image/" + FExtension + ";base64," + Convert.ToBase64String(imageArray);
                        }
                    }
                    return Result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


        }

    }
}
