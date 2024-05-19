using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace COMMON
{
   public class MailHelper
    {
        public static void SendEmail(List<string> tolst, List<string> cclst, string FromEmailId,string Password,string MailDisplayDesc,string Smtp,bool Ssl,int Port, string txtSubject, string txtBody)
        {
            //List<string> tolst = ToMail.Contains(',') ? ToMail.Split(',').ToList() : (!string.IsNullOrEmpty(ToMail) ? new List<string>() { ToMail } : null);

            //if (tolst.Count > 0)
            //    tolst = tolst.Distinct().ToList();
            //List<string> cclst = CCMail.Contains(',') ? CCMail.Split(',').ToList() : (!string.IsNullOrEmpty(CCMail) ? new List<string>() { CCMail } : null);
            //if (cclst.Count > 0)
            //    cclst = cclst.Distinct().ToList();
            try
            {

                SmtpClient client = new SmtpClient(Smtp);
                client.Port = Port;
                client.EnableSsl = Ssl;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(FromEmailId, Password);
                MailMessage msg = new MailMessage();


                foreach (string item in tolst)
                {
                    msg.To.Add(item);
                }

                foreach (string item in cclst)
                {
                    msg.CC.Add(item);
                }

                msg.From = new MailAddress(FromEmailId, MailDisplayDesc);
                msg.Subject = txtSubject;
                msg.IsBodyHtml = true;
                msg.Body = txtBody;
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
