using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class AllcourseViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название курса")]
        public string Name { get; set; }
        public string Decription { get; set; }
        public IFormFile Url_Img { get; set; }
        public string uri { get; set; }
        public DateTime dateTime { get; set; }
    }
}
