using System;
using System.Collections.Generic;
using System.Text;

namespace EAMobile.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
