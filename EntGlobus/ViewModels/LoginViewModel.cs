using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string TelNum { get; set; }
        public string TelTrue { get { return this.TelNum.Remove(0, 1); }
            set { value = TelNum; }
        }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
