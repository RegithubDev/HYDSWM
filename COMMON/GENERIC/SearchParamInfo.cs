using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.GENERIC
{
    public class SearchParamInfo
    {
        public string ZoneId { get; set; }
        public string CircleId { get; set; }
        public string UserId { get; set; }
        public string VehicleTypeId { get; set; }
        public int TSId { get; set; }
        public int UTsId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TStatus { get; set; }
    }
}
