using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace EasyAccessMobile
{
    class ApiHelper
    {
        private static readonly string CONTENT_TYPE = "application/json; charset=utf-8";

        public enum Methods
        {
            GET,
            POST
        }

        public static WebResponse NewRequest(string url, Methods method, object body = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = CONTENT_TYPE;
            request.Method = method.ToString("g");

            if (body != null)
            {
                var a = request.GetRequestStream();
                using (var streamWriter = new StreamWriter(a))
                {
                    string json = JsonConvert.SerializeObject(body);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
            }

            return request.GetResponse();
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