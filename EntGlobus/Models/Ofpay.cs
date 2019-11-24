using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Ofpay
    {
        public int Id { get; set; }

        public string IdentityId { get; set; }

        public AppUsern Identity { get; set; }

        public string type { get; set; }

        public string Price { get; set; }

    }
}
