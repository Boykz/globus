using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ApiServece
{
    public class RegisterAuthQrModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }


    public class LoginAuthQrModel
    {
        public string Number { get; set; }
        public string Password { get; set; }
    }

}
