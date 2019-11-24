using EntGlobus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class SatesViewModel
    {
        public string number { get; set; }
        public int sot  { get; set; }
        public int sdo { get; set; }
        public string sts { get; set; }
        public string blok { get; set; }
        public IList<AppUsern> Users { get; set; }
        public List<UserStatus> UserSatuses { get; set; }
        public List<Search> Searches { get; set; }
        public List<Blok> Bloks { get; set; }

        public SatesViewModel()
        {
            UserSatuses = new List<UserStatus>();
            Users = new List<AppUsern>();
            Searches = new List<Search>();
            Bloks = new List<Blok>();
        }
    }
}
