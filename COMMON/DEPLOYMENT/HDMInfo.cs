using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.DEPLOYMENT
{
    public class HDZoneInfo
    {
        public int ZId { get; set; }
        public int Active { get; set; }
        public int InActive { get; set; }
        public string ZoneNo { get; set; }
        public string Zonecode { get; set; }
        public int TotalZRowCount { get; set; }
        public int TotalCircleRowCount { get; set; }
        public int TotalWardRowCount { get; set; }
        public List<HDCircleInfo> Circlelst { get; set; }
        public List<DepOutDataInfo> Outlst { get; set; }
        public int TotalAsset { get; set; }
    }
    public class HDCircleInfo
    {
        //DepCircleInfo()
        //{
        //    Wardlst = new List<DepWardInfo>();
        //}
        public int CircleId { get; set; }
        public string CircleName { get; set; }
        public string CircleCode { get; set; }
        public int ZoneId { get; set; }
        public int TotalCircleRowCount { get; set; }
        public List<HDWardInfo> Wardlst { get; set; }
    }
    public class HDWardInfo
    {
        public int WardId { get; set; }
        public string WardNo { get; set; }
        public string WardName { get; set; }
        public int CirlceId { get; set; }
        public int TotalWardRowCount { get; set; }
        public List<HDVehicleTypeInfo> VehicleData { get; set; }
    }

    public class HDVehicleTypeInfo
    {
        public int VehicleTypeId { get; set; }
        public string VehicleType { get; set; }
        public int Active { get; set; }
        public int InActive { get; set; }
        public int InRepair { get; set; }
        public int Condemed { get; set; }
        public int TotalAsset { get; set; }
        public int Deployed { get; set; }
        public int NotDeployed { get; set; }
        public int NotReported { get; set; }
        public int Reported { get; set; }
        public float DepPercentage { get; set; }

        public int Trips1 { get; set; }
        public int Trips2 { get; set; }
        public int Trips3 { get; set; }
        public int Trips4 { get; set; }
        public int MoreThan4Trips { get; set; }
        public int ReportedUnique { get; set; }
    }
}
