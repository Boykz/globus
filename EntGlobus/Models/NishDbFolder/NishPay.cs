using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.NishDbFolder
{
    public class NishPay
    {
        public int Id { get; set; }

        public int NishCourseId { get; set; }
        public NishCourse NishCourse { get; set; }

        [ForeignKey("AppUsern")]
        public string UserId { get; set; }
        public AppUsern AppUsern { get; set; }

        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
