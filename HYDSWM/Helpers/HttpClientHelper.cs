using HYDSWMAPI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HYDSWM.Helpers
{

    public class HttpClientHelper<T>
    {
        static string apiBaseUrl = Startup.StaticConfig.GetValue<string>("WebAPIBaseUrl");
        static string BasicAuth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("patna:patna#2020"));
        public T GetSingleItemRequest(string apiUrl)
        {
            string endpoint = apiBaseUrl + apiUrl;
            var result1 = default(T);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                using (var Response = client.GetAsync(endpoint))
                {
                    Response.Wait();
                    var result = Response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        result1 = JsonConvert.DeserializeObject<T>(readTask.Result);

                    }


                }
            }
            return result1;
        }
        public T PostRequest(string apiUrl, string bodyparam)
        {
            string endpoint = apiBaseUrl + apiUrl;
            var result1 = default(T);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                StringContent content = new StringContent(bodyparam, Encoding.UTF8, "application/json");
                using (var Response = client.PostAsync(endpoint, content))
                {
                    Response.Wait();
                    var result = Response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        result1 = JsonConvert.DeserializeObject<T>(readTask.Result);

                    }

                }
            }
            return result1;
        }

        public string PostRequestString(string apiUrl, string bodyparam)
        {
            string endpoint = apiBaseUrl + apiUrl;
            string result1 = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                StringContent content = new StringContent(bodyparam, Encoding.UTF8, "application/json");
                using (var Response = client.PostAsync(endpoint, content))
                {
                    Response.Wait();
                    var result = Response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        result1 = readTask.Result;

                    }

                }
            }
            return result1;
        }
        public string GetRequest(string apiUrl)
        {
            string endpoint = apiBaseUrl + apiUrl;
            string result1 = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                using (var Response = client.GetAsync(endpoint))
                {
                    Response.Wait();
                    var result = Response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        result1 = readTask.Result;

                    }


                }
            }
            return result1;
        }
        public string PostRequestWithAuthString(string apiUrl, string bodyparam, string MBasicAuth)
        {
            string endpoint = apiUrl;
            string result1 = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", MBasicAuth);
                StringContent content = new StringContent(bodyparam, Encoding.UTF8, "application/json");
                using (var Response = client.PostAsync(endpoint, content))
                {
                    Response.Wait();
                    var result = Response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        result1 = readTask.Result;

                    }

                }
            }
            return result1;
        }
    }
}
