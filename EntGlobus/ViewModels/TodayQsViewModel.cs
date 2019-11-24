using EntGlobus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class TodayQsViewModel
    {
        public List<Satilim> Satilims { get; set; }
        public Dayliquestion Dayliquestion { get; set; }
        [Required]
        public string subject { get; set; }
        [Required]
        public string question { get; set; }
        [Required]
        public string ans1 { get; set; }
        [Required]
        public string ans2 { get; set; }
        [Required]
        public string ans3 { get; set; }
        [Required]
        public string ans4 { get; set; }
        public int id { get; set; }

        public string ans5 { get; set; }

        public string cor { get; set; }

        public TodayQsViewModel()
        {
            Satilims = new List<Satilim>();
            Dayliquestion = new Dayliquestion();
        }
    }
}
