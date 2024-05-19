using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COMMON.OPERATION
{
    public class QrHelperInfo
    {
        public int Result { get; set; }
        public string Msg { get; set; }
        public int PTDId { get; set; }
    }
    public class ContainerInfo
    {
        public int CMId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public decimal Capacity { get; set; }
        public string ContainerType { get; set; }
        public bool IsClosed { get; set; }
    }
    public class PContainerInfo
    {
        public int PTDId { get; set; }
        public int OperationTypeId { get; set; }
        public string OperationType { get; set; }
        public string UId { get; set; }
        public string ContainerName { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerType { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public bool IsClosed { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string CompletedOn { get; set; }
        public string CreatedBy { get; set; }
        public int TotalVehicleScanned { get; set; }
        public string ImgUrl { get; set; }
    }
    public class PVehicleInfo
    {
        public int PTDId { get; set; }
        public string OperationType { get; set; }
        public string UId { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public string TStationName { get; set; }
        public string TSType { get; set; }
        public string TSLat { get; set; }
        public string TSLng { get; set; }
        public decimal Radius { get; set; }
        public bool IsDeviated { get; set; }
        public float FilledPerc { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string WasteType { get; set; }
        public string ImgUrl { get; set; }
    }


    
}
