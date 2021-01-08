using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        /// <summary>
        ///  Тест тапсыру тест апи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public JsonResult GetOne()
        {


            List<Ques> q1 = new List<Ques>();
            List<Ques> q2 = new List<Ques>();
            List<Ques> q3 = new List<Ques>();
            List<Ques> q4 = new List<Ques>();
            List<Ques> q5 = new List<Ques>();

            for (int i = 1; i <= 5; i++)
            {
                q1.Add(new Ques { Number = i, Surak = $"Мат Сауаттылық {i}-сұрақ бойынша сқрақ қой???", JauapNumber = 1, Jauap = "C", A = "5cm", B = "12cm", C = "8cm", D = "3cm", E = "10cm" });
            }
            for (int i = 1; i <= 5; i++)
            {
                q2.Add(new Ques { Number = i, Surak = $"Математика {1}-сұрақ бойынша сқрақ қой???", JauapNumber = 1, Jauap = "A", A = "5 m", B = "12 m", C = "8 m", D = "3 m", E = "10 m" });
            }
            for (int i = 1; i <= 5; i++)
            {
                q3.Add(new Ques { Number = i, Surak = $"Физика {1}-сұрақ бойынша сқрақ қой???", JauapNumber = 1, Jauap = "E", A = "5cm", B = "12cm", C = "8cm", D = "3cm", E = "10cm" });
            }
            for (int i = 1; i <= 5; i++)
            {
                q4.Add(new Ques { Number = i, Surak = $"Қазақстан тарихы {1}-сұрақ бойынша сқрақ қой???", JauapNumber = 1, Jauap = "C", A = "5cm", B = "12cm", C = "8cm", D = "3cm", E = "10cm" });
            }
            for (int i = 1; i <= 5; i++)
            {
                q5.Add(new Ques { Number = i, Surak = $"Оқу сауаттылық {1}-сұрақ бойынша сқрақ қой???", JauapNumber = 1, Jauap = "D", A = "5cm", B = "12cm", C = "8cm", D = "3cm", E = "10cm" });
            }

            Pan matsau = new Pan { Name = "math", Ques = q1 };
            Pan matem = new Pan { Name = "matem", Ques = q2 };
            Pan fizika = new Pan { Name = "fizika", Ques = q3 };
            Pan kaztarih = new Pan { Name = "kaztarih", Ques = q4 };
            Pan okusauat = new Pan { Name = "okusauat", Ques = q5 };

            TestTs test = new TestTs { Pans = new List<Pan>() };
            test.Pans.Add(matsau);
            test.Pans.Add(matem);
            test.Pans.Add(fizika);
            test.Pans.Add(kaztarih);
            test.Pans.Add(okusauat);

            return new JsonResult(test);
        }
    }


    public class TestTs
    {
        public List<Pan> Pans { get; set; }
    }

    public class Pan
    {
        public string Name { get; set; }
        public List<Ques> Ques { get; set; }
    }

    public class Ques
    {
        public int Number { get; set; }
        public string Surak { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }

        public int JauapNumber { get; set; }

        public string Jauap { get; set; }
    }
}
