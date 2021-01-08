using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.QR
{
    public class QrNuska
    {
        public int Id { get; set; }
        public int NuskaNumber { get; set; }



        [ForeignKey("QrBook")]
        public Guid QrBookId { get; set; }
        public QrBook QrBook { get; set; }
    }
}
