using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.OPERATION
{
    public class PendingWBInfo
    {
        public string EntityNo { get; set; }
        public decimal GrossWt { get; set; }
        public decimal TareWt { get; set; }
        public decimal NetWt { get; set; }
        public DateTime TDate { get; set; }
        public string CreatedBy { get; set; }
        public string WTCode { get; set; }
        public int WBId { get; set; }
        public string TSCode { get; set; }
        public string Status { get; set; }
        public int TSId { get; set; }
        public int SCId { get; set; }
        public int OperationTypeId { get; set; }
    }
}
