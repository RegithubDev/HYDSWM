using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMMON.SWMENTITY
{
    public class LoginResponse
    {
        [Key]
        public int Result { get; set; }
        public string Msg { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoginId { get; set; }
        public string FullName { get; set; }
        public string CCode { get; set; }
        public int LevelId { get; set; }
        public int TSId { get; set; }
        public string TSName { get; set; }
        public decimal Radius { get; set; }
        public string TSLat { get; set; }
        public string TSLng { get; set; }
    }
    public class GResposnse
    {
        [Key]
        public int Result { get; set; }
        public string Msg { get; set; }
        public string Code { get; set; }
    }
    public class RamkyResposnse
    {
        [Key]
        public int Result { get; set; }
        public string Msg { get; set; }
    }
}
