using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models.Enums;
using Microsoft.AspNetCore.Identity;


namespace EntGlobus.Models
{
    public class AppUsern : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string pan1 { get; set; }
        public string pan2 { get; set; }
        public bool enable { get; set; }
        public bool offenable { get; set; }
        public DateTime regdate { get; set; }

        public int? WalletPrice { get; set; }

        public List<Search> Searches { get; set; }

        public AppUsern()
        {
            Searches = new List<Search>();
        }


        public AuthType? AuthType { get; set; }
    }
}
