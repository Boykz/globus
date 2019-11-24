using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class PostViewModel
    {
        public string subject { get; set; }
        public string text { get; set; }
        public string pathimg { get; set; }
        public string pathvideo { get; set; }
        public IFormFile file { get; set; }
        public int watch { get; set; }
        public string hashtag { get; set; }
        public int Id { get; set; }
    }
}
