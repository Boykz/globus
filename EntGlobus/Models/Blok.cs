using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Blok
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public AppUsern Identity { get; set; }
        public string blok { get; set; }
        public bool enable { get; set; }
        public DateTime BuyDate { get; set; }

    }
}
