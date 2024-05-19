using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace COMMON
{
    public class SMSSenderHelper
    {
        public void SendSms(JArray _lstalrt, JArray _lstcontact)
        {
            string MessageBody = string.Empty;
            int Count = 1;
            int ICount = 1;
            try
            {

                foreach (JObject item in _lstalrt)
                {

                    MessageBody = Count > 1 ? MessageBody + Count + "- " + item.GetValue("VehicleNo").ToString() + " Location-https://maps.google.com/?q=" + item.GetValue("GLocation").ToString() + " Alert Type-" + item.GetValue("DType").ToString() + Environment.NewLine : Count + "- " + item.GetValue("VehicleNo").ToString() + " Location-https://maps.google.com/?q=" + item.GetValue("GLocation").ToString() + " Alert Type-" + item.GetValue("DType").ToString() + Environment.NewLine;

                    Count++;
                }
                foreach (JObject item in _lstcontact)
                {
                    if (ICount == 1)
                        MessageBody = item.GetValue("SubjectDesc").ToString() + Environment.NewLine + MessageBody;

                    string FinalMsg = CommonHelper.SmsApiUrl(item.GetValue("Receiver").ToString(), MessageBody);
                    using (HttpClient client = new HttpClient())
                    {
                        using (var Response = client.GetAsync(FinalMsg))
                        {
                            Response.Wait();
                            var result = Response.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var readTask = result.Content.ReadAsStringAsync();
                                readTask.Wait();
                            }
                        }
                    }

                    ICount++;
                }
                //string p = CommonHelper.SmsApiUrl("7042618366", MessageBody);
                //using (HttpClient client = new HttpClient())
                //{
                //    using (var Response = client.GetAsync(p))
                //    {
                //        Response.Wait();
                //        var result = Response.Result;
                //        if (result.IsSuccessStatusCode)
                //        {
                //            var readTask = result.Content.ReadAsStringAsync();
                //            readTask.Wait();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

            }
        }
        public void SendMessage(string Message, string ContactNo)
        {
            string p = CommonHelper.SmsApiUrl(ContactNo, Message);
            using (HttpClient client = new HttpClient())
            {
                using (var Response = client.GetAsync(p))
                {
                    Response.Wait();
                    var result = Response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                    }
                }
            }
        }
    }
}
