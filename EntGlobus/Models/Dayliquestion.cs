using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Dayliquestion
    {
        public int Id { get; set; }
        public string subject { get; set; }
        public DateTime qstime { get; set; }
        public string question { get; set; }
        public string ans1 { get; set; }
        public string ans2 { get; set; }
        public string ans3 { get; set; }
        public string ans4 { get; set; }
        public string ans5 { get; set; }
        public int A1 { get; set; }
        public int A2 { get; set; }
        public int A3 { get; set; }
        public int A4 { get; set; }
        public int A5 { get; set; }
        public string Correctans { get; set; }

    }
}
