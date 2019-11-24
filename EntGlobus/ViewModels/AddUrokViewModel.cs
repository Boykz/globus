using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class AddUrokViewModel
    {
        public int Id { get; set; }
        public string subject { get; set; }
        public int course { get; set; }
        public string text { get; set; }
        public string video { get; set; }
        public string time { get; set; }
        public IFormFile photo { get; set; }
        public IFormFile mat1 { get; set; }
        public IFormFile mat2 { get; set; }
        public IFormFile mat3 { get; set; }
        public IFormFile mat4 { get; set; }
    }
}
