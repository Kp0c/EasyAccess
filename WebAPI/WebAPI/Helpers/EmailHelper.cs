using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebAPI.Entities;

namespace WebAPI.Helpers
{
    public static class EmailHelper
    {
        private static void SendEmail(string recipient, string header, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("andriy.yushchenko97@gmail.com", "axowjbeaepwcvdfx"),
                EnableSsl = true
            };

            client.Send("andriy.yushchenko97@gmail.com", recipient, header, body);
        }

        public static void SendRegistrationEmail(UserToRegister user, string baseUri)
        {
            string body = $"Welcome to EasyAccess! Navigate this link to finish registration: {baseUri}/{user.Id}.";

            SendEmail(user.Email, "EasyAccess registarion", body);
        }

        public static void SendAuthEmail(Application email, string authName, string baseUri, string authId)
        {
            string body = $"If you want to access resource {authName}, navigate this link: {baseUri}/{authId}";

            SendEmail(email.Action, $"Allow access for {authName}", body);
        }
    }
}
