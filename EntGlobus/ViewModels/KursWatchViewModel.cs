using EntGlobus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class KursWatchViewModel
    {
        public List<Kurs> Kurses { get; set; }
        public Kurs Kurs { get; set; }
        public bool acces {get;set;}
       
     
        public KursWatchViewModel()
        {
            Kurses = new List<Kurs>();
            Kurs  = new Kurs();
        }
    }
}
