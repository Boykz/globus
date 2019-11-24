using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Newqs
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
        public string Uriphoto { get; set; }
        public bool Check { get; set; }
        public DateTime Offerdate  { get; set; }
    }
}
