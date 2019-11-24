using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Search
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public AppUsern Identity { get; set; }
        public int count { get; set; }
        public bool enable { get; set; }
        public bool pay { get; set; }
        public DateTime date { get; set; }

    }
}
