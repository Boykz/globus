using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Qiwipay
    {
        public int id { get; set; }
        public string account { get; set; }
        public string prv_txn { get; set; }
        public string txn_id { get; set; }
        public DateTime txn_date { get; set; }
        public double sum { get; set; }
        public string type { get; set; }
        public bool pay { get; set; }
        public string number { get; set; }
        public bool pan { get; set; }



        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUsern User { get; set; }
    }
}
