using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.STAFFCOMPLAINT
{
   public class SComplaintInfo
    {
        public int ComplaintTypeId { get; set; }
        public int TSId { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
        public string FLat { get; set; }
        public string FLng { get; set; }
        public string FAddress { get; set; }
        public string CreatedBy { get; set; }
    }
}
