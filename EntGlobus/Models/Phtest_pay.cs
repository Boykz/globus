using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Phtest_pay
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public string Type { get; set; }
        public DateTime dateTime { get; set; }
        public double Price { get; set; }
    }
}
