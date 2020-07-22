using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.NishDbFolder
{
    public class NishCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
