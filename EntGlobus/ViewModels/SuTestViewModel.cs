using EntGlobus.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class SuTestViewModel
    {
        public int Blok { get; set; }
        public string V_name  { get; set; }
        public List<Sublok> Subloks { get; set; }
        public List<Satilim> Satilims { get; set; }
        public List<Su_ques_ph> Su_Ques_Phs { get; set; }
        public List<Su_right_ans> Su_Right_Ans { get; set; }
        public SuTestViewModel()
        {
            Su_Ques_Phs = new List<Su_ques_ph>();
            Su_Right_Ans = new List<Su_right_ans>();
            Subloks = new List<Sublok>();
            Satilims = new List<Satilim>();
        }

        public string q1 { get; set; }
        public string q2 { get; set; }
        public string q3 { get; set; }
        public string q4 { get; set; }
        public string q5 { get; set; }
        public string q6 { get; set; }
        public string q7 { get; set; }
        public string q8 { get; set; }
        public string q9 { get; set; }
        public string q10 { get; set; }
        public string q11 { get; set; }
        public string q12 { get; set; }
        public string q13 { get; set; }
        public string q14 { get; set; }
        public string q15 { get; set; }
        public string q16 { get; set; }
        public string q17 { get; set; }
        public string q18 { get; set; }
        public string q19 { get; set; }
        public string q20 { get; set; }
        public string q21 { get; set; }
        public string q22 { get; set; }
        public string q23 { get; set; }
        public string q24{ get; set; }
        public string q25 { get; set; }
        public string q26 { get; set; }
        public string q27 { get; set; }
        public string q28 { get; set; }
        public string q29 { get; set; }
        public string q30 { get; set; }
        public int PanID { get; set; }
        public int v_id { get; set; }
        public IFormFile mat1 { get; set; }
        public IFormFile mat2 { get; set; }
        public IFormFile mat3 { get; set; }
        public IFormFile mat4 { get; set; }
        public IFormFile mat5 { get; set; }
        public IFormFile mat6 { get; set; }

        public IFormFile mat7 { get; set; }

        public IFormFile mat8 { get; set; }
    }
}
