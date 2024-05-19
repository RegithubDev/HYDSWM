using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.GENERIC
{
    public class BUserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class BUserLoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string LoginType { get; set; }
        public string UId { get; set; }
    }
    public class QRUserLoginInfo
    {
        public string UserName { get; set; }
        public string HasPwd { get; set; }
        public string Otp { get; set; }
        public string LoginType { get; set; }
        public string UId { get; set; }
    }
}
