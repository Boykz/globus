using EntGlobus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class AdminBuyViewModel
    {
        public List<Satilim> Satilims { get; set; }
        public AppUsern IUser { get; set; }
        public IList<string> Bought { get; set; }
        public AdminBuyViewModel()
        {
            Satilims = new List<Satilim>();
            IUser = new AppUsern();
            Bought = new List<string>();
        }
    }
}
