using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace EntGlobus.Models.QR
{
    public class QrVideo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }

        [ForeignKey("QrBook")]
        public Guid QrBookId { get; set; }
        public QrBook QrBook { get; set; }
    }
}
