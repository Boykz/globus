using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.QR
{
    public class QrBook
    {

        public Guid Id { get; set; }
        public string BookName { get; set; }
        public string PhotoUrl { get; set; }

        public DateTime DateTime { get; set; }
    }
}
