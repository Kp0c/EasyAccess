using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EasyAccessMobile.Models;

namespace EasyAccessMobile
{
    static class EasyAccessApi
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
    }
}