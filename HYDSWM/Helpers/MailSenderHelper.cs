using HYDSWMAPI;
using COMMON;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HYDSWM.Helpers
{
    public class MailSenderHelper
    {
        static string Smtp = Startup.StaticConfig.GetValue<string>("EmailSetting:Smtp");
        static bool SSL = Startup.StaticConfig.GetValue<bool>("EmailSetting:SSL");
        static int Port = Startup.StaticConfig.GetValue<int>("EmailSetting:Port");
        static string FromEmailId = Startup.StaticConfig.GetValue<string>("EmailSetting:EmailId");
        static string Password = Startup.StaticConfig.GetValue<string>("EmailSetting:Password");

        public static void GeofenceAlertEmail(string FPath,string ToMail,string CCMail,string MailDisplayDesc,string Subject)
        {

            string readFile = string.Empty;
            using (StreamReader reader = new StreamReader(FPath))
            {
                readFile = reader.ReadToEnd();
            }
            string StrContent = "";
            StrContent = readFile;
            StrContent = StrContent.Replace("[Device]", "");

           // MailHelper.SendEmail(ToMail,CCMail,FromEmailId,Password, MailDisplayDesc,Smtp,SSL,Port, Subject,StrContent);
        }
    }
}
