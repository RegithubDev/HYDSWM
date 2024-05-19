using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.DEPLOYMENT
{
    public class PVehicleDeployInfo
    {
        public string EntityType { get; set; }
        public string UId { get; set; }
        public string ZoneId { get; set; }
        public string CircleId { get; set; }
        public string WardId { get; set; }
        public string LandmarkId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
    }
    public class AllVehicleDeployInfo
    {
        public string VehicleNo { get; set; }
        public string UId { get; set; }
        public string VehicleType { get; set; }
        public string ZoneNo { get; set; }
        public string CircleName { get; set; }
        public string WardNo { get; set; }
        public string OwnerType { get; set; }
        public string Status { get; set; }
        public string DeployedOn { get; set; }
       
    }
}
