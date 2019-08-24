using EAMobile.Models;
using Newtonsoft.Json;
using System.Net;

namespace EAMobile
{
    public static class EasyAccessApi
    {
        static readonly string baseUri = @"https://easyaccess.azurewebsites.net/api";

        public static void Register(string email, string username)
        {
            UserToRegister userToRegister = new UserToRegister()
            {
                Email = email,
                Username = username
            };
            ApiHelper.NewRequest($"{baseUri}/Users/Register", ApiHelper.Methods.POST, userToRegister);
        }

        public static string Login(string username)
        {
            var response = ApiHelper.NewRequest($"{baseUri}/Users/AuthenticateByEmail/{username}?authName=EasyAccessMobile",
                                                ApiHelper.Methods.GET);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                return ApiHelper.GetResponseBody(response);
            }

            return null;
        }

        public static void InsertNewToken(FirebaseToken firebaseToken)
        {
            ApiHelper.NewRequest($"{baseUri}/Tokens", ApiHelper.Methods.POST, firebaseToken);
        }

        public static User GetUser(string username, string token)
        {
            var response = ApiHelper.NewRequest($"{baseUri}/Users/{username}", ApiHelper.Methods.GET, null, token);

            return JsonConvert.DeserializeObject<User>(ApiHelper.GetResponseBody(response));
        }

        public static void AddApplication(string userId, Application app, string token)
        {
            ApiHelper.NewRequest($"{baseUri}/Users/{userId}/applications", ApiHelper.Methods.POST, app, token);
        }

        public static void RemoveApplication(string userId, string appId, string token)
        {
            ApiHelper.NewRequest($"{baseUri}/Users/{userId}/applications/{appId}", ApiHelper.Methods.DELETE, null, token);
        }
    }
}
