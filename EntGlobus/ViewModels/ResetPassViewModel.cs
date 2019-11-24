using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class ResetPassViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string TelNum { get; set; }
        [Required]
        public string NewPassword { get; set; }
        //[Required]
        //public string Code { get; set; }


    }
}
