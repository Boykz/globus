using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        public string Id { get; set; }
    }
}
