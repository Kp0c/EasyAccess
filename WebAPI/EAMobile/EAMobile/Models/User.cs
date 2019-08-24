using System;
using System.Collections.Generic;
using System.Text;

namespace EAMobile.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
