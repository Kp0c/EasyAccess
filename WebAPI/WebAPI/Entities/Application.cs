using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using WebAPI.Helpers;

namespace WebAPI.Entities
{
    public class Application
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string Action { get; set; }


        public bool Authenticate(DataContext context, IHttpContextAccessor httpContextAccessor, string authName)
        {
            string authId = Guid.NewGuid().ToString();

            var pendingAuth = new PendingAuth { Application = this, AuthId = authId };

            context.PendingAuths.Add(pendingAuth);
            context.SaveChanges();

            var request = httpContextAccessor.HttpContext?.Request;
            string baseUri = $"https://{request.Host}/api/applications/{Id}/complete";

            switch (ApplicationType)
            {
                case ApplicationType.Email:
                    EmailHelper.SendAuthEmail(this, authName, baseUri, authId);
                    break;
                case ApplicationType.Android:
                    var firebaseToken = context.FirebaseTokens.Find(Action).Token;
                    SendAuthByAndroid(baseUri, firebaseToken, authName, authId);
                    break;
            }


            const int TOTAL_WAIT_TIME = 60 * 1000;
            const int NAP_TIME = 1000;

            for (int i = 0; i < TOTAL_WAIT_TIME / NAP_TIME; ++i)
            {
                if (context.PendingAuths.Contains(pendingAuth))
                {
                    Thread.Sleep(NAP_TIME);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        void SendAuthByAndroid(string baseUri, string firebaseToken, string authName, string authId)
        {
            var serverKey = string.Format("key={0}", "AAAAo8wQY8E:APA91bF1qH6sGrP6URWnAKvmNTjve3F8JL35DQDtP5zVIMC_d3vDiCGXQBYYuoT2F57QcQwBx9Y-_B3pMb3O47ZyUFLGsxFAu2g0O-DJfd0MI5dLAslcu7bKJPd2LsKXFv1vmPO0xDNJ");
            
            var senderId = string.Format("id={0}", "703503295425");

            var title = $"Allow access for {authName}.";
            var body = $"Allow access for {authName}.";

            var data = new
            {
                to = firebaseToken,
                notification = new { title, body }
            };

            var jsonBody = JsonConvert.SerializeObject(data);

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
            {
                httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var result = httpClient.SendAsync(httpRequest).Result.StatusCode;
                }
            }
        }
    }
}
