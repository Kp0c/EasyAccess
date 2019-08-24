using System;
using System.Collections.Generic;
using System.Text;

namespace EAMobile.Models
{
    public class Application
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string Action { get; set; }
    }
}
