using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COMMON.ASSET
{
   public class AssetB64Info
    {
        private string _Img1Url;
        private string _FolderName;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public string ChassisNo { get; set; }
        public string ZoneNo { get; set; }
        public string Zonecode { get; set; }
        public string CircleName { get; set; }
        public string CircleCode { get; set; }
        public string WardNo { get; set; }
        public string WardName { get; set; }
        public double GrossWt { get; set; }
        public double TareWt { get; set; }
        public double NetWt { get; set; }
        public bool IsActive { get; set; }
        public string VStatus { get; set; }
        public string UId { get; set; }
        public bool IsManualUId { get; set; }
        public string VehicleType { get; set; }
        public int VehicleTypeId { get; set; }
        public string OperationType { get; set; }
        public int OperationTypeId { get; set; }
        public string LabelClass { get; set; }
        public string AssetStatus { get; set; }
        public string OwnerType { get; set; }
        public string CreatedOn { get; set; }
        public string LastModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ContactNo { get; set; }
        public string DriverName { get; set; }
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

    public class ContainInfo
    {
        private string _Img1Url;
        private string _FolderName;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int CMId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public double Capacity { get; set; }
        public int ContainerTypeId { get; set; }
        public bool IsActive { get; set; }
        public string CStatus { get; set; }
        public string ContainerType { get; set; }
        public string AssetStatus { get; set; }
        public string LabelClass { get; set; }
        public bool IsManualUId { get; set; }
        public string UId { get; set; }
        public string CreatedOn { get; set; }
        public string LastModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
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
}
