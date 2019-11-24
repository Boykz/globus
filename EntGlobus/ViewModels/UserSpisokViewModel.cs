using EntGlobus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class UserSpisokViewModel
    {
        public bool pay { get; set; }
        public Ofpay Ofpays { get; set; }
        public Satilim Satilims { get; set; }
    }
}
