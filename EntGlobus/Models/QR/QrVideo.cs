using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace EntGlobus.Models.QR
{
    public class QrVideo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }

        public bool Stats { get; set; } = false;
        public int QrCode { get; set; }




        [ForeignKey("QrNuska")]
        public int? QrNuskaId { get; set; }
        public QrNuska QrNuska { get; set; }
    }
}
