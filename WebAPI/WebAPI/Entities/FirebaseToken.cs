using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Entities
{
    public class FirebaseToken
    {
        [Key]
        public string DeviceId { get; set; }
        public string Token { get; set; }
    }
}
