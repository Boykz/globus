using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class SellBlok
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Variants { get; set; }
        public string Imgurl { get; set; }
        public string Typi { get; set; }
        public bool Enable { get; set; }
    }
}
