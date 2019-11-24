using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Tolem
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public AppUsern Identity { get; set; }
        public string type { get; set; }
        public bool success { get; set; }
        public string price { get; set; }
        public DateTime date { get; set; }
    }
}
