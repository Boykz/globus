using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels.AnaliticsModel
{
    public class DoPayViewModel
    {
        public List<DoPayQiwi> DoPayQiwis { get; set; }
        public List<DoPayDefault> DoPayDefaults { get; set; }
    }

    public class DoPayQiwi
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Fio { get; set; }

        public bool pay { get; set; }
        public DateTime DateTime { get; set; }

        public string Pan { get; set; }
    }

    public class DoPayDefault
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Fio { get; set; }
    }
}
