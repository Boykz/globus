using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Kurs
    {
        public int Id { get; set; }
        public string subject { get; set; }
        public int course_id { get; set; }
        public string text { get; set; }
        public string video { get; set; }
        public string time { get; set; }
        public string photo { get; set; }
        public string mat1 { get; set; }
        public string mat2 { get; set; }
        public string mat3 { get; set; }
        public string mat4 { get; set; }
        public string watch { get; set; }
    }
}
