using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.AnaliticaDbFolder
{
    public class QiwiAnalitic
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Date { get; set; }
        public string Pan { get; set; }

        public bool? Call { get; set; } = false;

        public string Result { get; set; }
    }
}
