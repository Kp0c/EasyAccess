using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace EAMobile
{
    class ApiHelper
    {
        private static readonly string CONTENT_TYPE = "application/json; charset=utf-8";

        public enum Methods
        {
            GET,
            POST,
            DELETE
        }

        public static HttpWebResponse NewRequest(string url, Methods method, object body = null, string token = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = CONTENT_TYPE;
            request.Method = method.ToString("g");
            if (token != null)
            {
                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {token}");
            }

            if (body != null)
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(body);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
            }

            return request.GetResponse() as HttpWebResponse;
        }

        public static string GetResponseBody(HttpWebResponse response)
        {
            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
