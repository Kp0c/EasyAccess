using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Entities
{
    public class PendingAuth
    {
        public virtual Application Application { get; set; }
        [Key]
        public string AuthId { get; set; }
    }
}
