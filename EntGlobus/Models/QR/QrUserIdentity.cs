using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.QR
{
    public class QrUserIdentity
    {
        public Guid Id { get; set; }


        [ForeignKey("AppUsern")]
        public string UserId { get; set; }
        public AppUsern AppUsern { get; set; }


        [ForeignKey("QrBook")]
        public Guid QrBookId { get; set; }
        public QrBook QrBook { get; set; }
    }
}
