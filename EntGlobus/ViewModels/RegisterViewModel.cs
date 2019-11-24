using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string TelNum { get; set; }          
       
        public string TelTrue {
            get { return this.TelNum.Remove(0, 1); }
            set { value = this.TelNum; }
        }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
