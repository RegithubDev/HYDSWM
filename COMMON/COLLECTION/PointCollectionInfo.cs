using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.COLLECTION
{
    public class PointCollectionInfo
    {
        public int PointId { get; set; }
        public string PointName { get; set; }
        public string RouteCode { get; set; }
        public int RouteId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public int TripId { get; set; }
        public string VehicleUId { get; set; }
        public string LoginId { get; set; }
        public bool IsDeviated { get; set; }
        public int ActPointId { get; set; }
        public string AppVersion { get; set; }
        public DateTime? BPhotoStamp { get; set; }
        public string TRefId { get; set; }
    }
    public class EmergencyPointCollectionInfo
    {
        public int EmrUId { get; set; }
        public int PointId { get; set; }
        public string PointName { get; set; }
        // public string RouteCode { get; set; }
        //public int RouteId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        //  public int TripId { get; set; }
        public string VehicleUId { get; set; }
        public string LoginId { get; set; }
        public bool IsDeviated { get; set; }
        public int ActPointId { get; set; }
        public string AppVersion { get; set; }
        public DateTime BPhotoStamp { get; set; }
        public int TypeId { get; set; }
    }
}
