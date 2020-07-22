using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.SchoolDbFolder
{
    public class SchoolClass
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public DateTime? StartDate { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }
    }
}
