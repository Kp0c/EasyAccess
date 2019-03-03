using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Entities
{
    public class Application
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
