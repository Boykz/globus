using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class SellBlokViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Variants { get; set; }
        public string Typi { get; set; }
        public IFormFile File { get; set; }
    }
}
