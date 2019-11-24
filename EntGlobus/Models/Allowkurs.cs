using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Allowkurs
    {
        public int Id { get; set; }
        public string UserPhone { get; set; }
        public int Pan_Id  { get; set; }
        public bool pay { get; set; }
        public int Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}
