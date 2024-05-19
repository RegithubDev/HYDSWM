using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.ASSET
{
   public class UHFTInfo
    {
        public string TagId { get; set; }
        public string ImeiNo { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string UserId { get; set; }
    }
    public class UHFAssetData
    {
        public int UhfATId { get; set; }
        public string Tid { get; set; }
        public string readerid { get; set; }
        public int AssetId { get; set; }
        public DateTime Dated { get; set; }
        public int TAssetId { get; set; }
        public string ReaderAssetAreaType { get; set; }
        public string ReaderOtherNo { get; set; }
        public string ReaderAssetName { get; set; }
        public string ReaderZoneNo { get; set; }
        public string ReaderWardNo { get; set; }
    }
    public class UHFAssetData1
    {
        public string GeoPointName { get; set; }
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public string GeoPointCategory { get; set; }
        public string SPickupTime { get; set; }
        public string ZoneNo { get; set; }
       
    }
    public class VDepInfo
    {
        public string TStationName { get; set; }
        public string CreatedOn { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OperationType { get; set; }
        public int VehicleTypeId { get; set; }
        public string UId { get; set; }
        public string UIdType { get; set; }
        public string EntityType { get; set; }
        public string EntityNo { get; set; }
    }
}
