using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace COMMON.CITIZEN
{
   public class CLoginResponseInfo
    {
        [Key]
        public string ContactNo { get; set; }
        public string UHouseId { get; set; }
    }
    public class CComplaintInputInfo
    {
        public string Input { get; set; }
        public IFormFile Image { get; set; }
    }
    public class CCOmplaintInfo
    {
        [Key]
        public string Complaint { get; set; }
        public string   CategoryId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string ComplaintTypeId { get; set; }
        public int WardId { get; set; }
    }
    public class CCOmplaintOInfo
    {
        public string Complaint { get; set; }
        public string CategoryId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string ComplaintTypeId { get; set; }
        public int WardId { get; set; }
        public string UserId { get; set; }
    }
}
