using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.DbFolder1
{
    public class LiveTestVisitor
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUsern User { get; set; }

        public DateTime DateTime { get; set; }
    }
}
