using EntGlobus.Models.NishDbFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels.adminview
{
    public class nishviewmodel
    {
        public IEnumerable<NishPay> NishPays { get; set; }

        public IEnumerable<NishPay> NishPaysIdentity { get; set; }

        public NishCourse NishCourse { get; set; }
    }
}
