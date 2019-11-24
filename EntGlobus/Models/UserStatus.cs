using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class UserStatus
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public DateTime  CheckDate{ get; set; }
    }
}
