using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.Models.SchoolDbFolder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    public class SchoolController : Controller
    {

        private readonly entDbContext db;
        private UserManager<AppUsern> userManager;
        private SignInManager<AppUsern> SignInManager;
        public SchoolController(entDbContext _db, UserManager<AppUsern> _userManager, SignInManager<AppUsern> _SignInManager)
        {
            db = _db;
            userManager = _userManager;
            SignInManager = _SignInManager;
        }



        public IActionResult Index(int Id)
        {
            var cl = db.SchoolClasses.Where(p => p.SchoolId == Id).ToList();
            return View(cl);
        }


        public IActionResult Index2(int Id)
        {
            var lis = db.ClassLessons.Where(p => p.SchoolClassId == Id).ToList();

            return View(lis);
        }

        public IActionResult Res()
        {
            //List<ClassLesson> cl = new List<ClassLesson>();
            //cl.Add(new ClassLesson { LessonName = "математика", SchoolClassId = 12 });

            ////cl.Add(new ClassLesson { LessonName = "география", SchoolClassId = 1 });
            ////cl.Add(new ClassLesson { LessonName = "биология", SchoolClassId = 1 });
            ////cl.Add(new ClassLesson { LessonName = "қазақстан тарихы", SchoolClassId = 1 });
            ////cl.Add(new ClassLesson { LessonName = "дүние жүзі тарихы", SchoolClassId = 1 });
            ////cl.Add(new ClassLesson { LessonName = "қазақ әдебиеті", SchoolClassId = 1 });
            ////cl.Add(new ClassLesson { LessonName = "қазақ тілі", SchoolClassId = 1 });
            ////cl.Add(new ClassLesson { LessonName = "аққ", SchoolClassId = 1 });
            ////cl.Add(new ClassLesson { LessonName = "ағылшын тілі", SchoolClassId = 1 });


            //db.ClassLessons.AddRange(cl);
            //db.SaveChanges();

            return View();
        }


        public IActionResult Prob ()
        {
            return View();
        }


        public async Task<IActionResult> Method()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if(user.UserName == "1717")
            {
                return Redirect("https://bahonshymkentbay.easywebinar.live/live-event-10");
            }
            return View();
        }
    }
}