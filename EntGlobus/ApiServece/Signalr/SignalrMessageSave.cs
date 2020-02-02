using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ApiServece.Signalr
{
    public class SignalrMessageSave
    {
        public string UserName { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
        public DateTime? MessDate { get; set; }
        public string LiveId { get; set; }
    }
}
