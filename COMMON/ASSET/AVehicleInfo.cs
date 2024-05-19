using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.ASSET
{
    public class AVehicleInfo
    {
        public int VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public string ChassisNo { get; set; }
        public int VehicleTypeId { get; set; }
        public int OwnerTypeId { get; set; }
        public int ZoneId { get; set; }
        public int CircleId { get; set; }
        public int WardId { get; set; }
        public decimal GrossWt { get; set; }
        public decimal TareWt { get; set; }
        public decimal NetWt { get; set; }
        public string UId { get; set; }
        public bool IsManual { get; set; }
        public int StatusId { get; set; }
        public string ARemarks { get; set; }
        public string ExStatusVal { get; set; }
        public string UserId { get; set; }
        public string DriverName { get; set; }
        public string ContactNo { get; set; }
        public int DInchargeId { get; set; }
        public int DLocationId { get; set; }
        public int DTsId { get; set; }
    }
    public class AContainerInfo
    {
        public int CMId { get; set; }
        public string Containercode { get; set; }
        public string ContainerName { get; set; }
        public decimal ContainerCapacity { get; set; }
        public string UId { get; set; }
        public int ContainerTypeId { get; set; }
        public bool IsManual { get; set; }
        public int StatusId { get; set; }
        public string ARemarks { get; set; }
        public string ExStatusVal { get; set; }
        public string UserId { get; set; }
    }
}
