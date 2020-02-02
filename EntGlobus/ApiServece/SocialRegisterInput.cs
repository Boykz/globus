using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ApiServece
{
    public class SocialRegisterInput
    {
        public string Type { get; set; }

        public string Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
