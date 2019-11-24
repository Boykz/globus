using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class OrderQiwiViewModel
    {
        [Required]
        public string num { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public string type { get; set; }
        public bool pan { get; set; }
    }
}
