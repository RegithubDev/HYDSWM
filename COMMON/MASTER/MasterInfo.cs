using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COMMON.MASTER
{
    public class MasterInfo
    {

        private string _Img1Url;
        private string _FolderName;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public int TSId { get; set; }
        public string TStationName { get; set; }
        public bool IsActive { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Location { get; set; }
        public double Radius { get; set; }
        public string ZoneNo { get; set; }
        public string CircleName { get; set; }

        public string CityNo { get; set; }
        public string WardNo { get; set; }
        public string TSType { get; set; }
        public double NoOfContainer { get; set; }
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
