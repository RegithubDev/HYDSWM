using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.COLLECTION
{
    public class GeoLocationSurveyInfo
    {
        public string GeoPointId { get; set; }
        public string GeoPointName { get; set; }
        public int CategoryId { get; set; }
        public int ZoneId { get; set; }
        public int CircleId { get; set; }
        public int WardId { get; set; }
        public string Location { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Remarks { get; set; }
        public decimal Radius { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
    public class EmergencyLocationSurveyInfo
    {
        public string GeoPointId { get; set; }
        public string GeoPointName { get; set; }
        public int CategoryId { get; set; }
        public int PointType { get; set; }
        public int ZoneId { get; set; }
        public int CircleId { get; set; }
        public int WardId { get; set; }
        public string Location { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Remarks { get; set; }
        public decimal Radius { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
}
