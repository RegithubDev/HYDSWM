using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.OPERATION
{
    public class QRTransactionInfo
    {
        public int OperationTypeId { get; set; }
        public string Step1UId { get; set; }
        public string Step1Lat { get; set; }
        public string Step1Lng { get; set; }
        public DateTime Step1SyncOn { get; set; }
        public string Step2UId { get; set; }
        public string Step2Lat { get; set; }
        public string Step2Lng { get; set; }
        public DateTime Step2SyncOn { get; set; }
        public bool Is90Perc { get; set; }
        public bool IsClosed { get; set; }
        public float Step2Filled { get; set; }
        public int TSId { get; set; }
        public bool IsDeviated { get; set; }
        public decimal DistanceFromTS { get; set; }
        public string CreatedBy { get; set; }
        public string Remarks { get; set; }
        public int ParentId { get; set; }
        public bool Step2IsDeviated { get; set; }
        public decimal Step2DistanceFromTS { get; set; }
        public int DHLTId { get; set; }
    }
    public class PTransactionInfo
    {
        public int OperationTypeId { get; set; }
        public string UId { get; set; }
        public string UIdType { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public bool IsClosed { get; set; }
        public float FilledPerc { get; set; }
        public int TSId { get; set; }
        public bool IsDeviated { get; set; }
        public decimal DistanceFromTS { get; set; }
        public string CreatedBy { get; set; }
        public string Remarks { get; set; }
        public int ParentId { get; set; }
        public int DHLTId { get; set; }
        public string WasteType { get; set; }
    }
}
