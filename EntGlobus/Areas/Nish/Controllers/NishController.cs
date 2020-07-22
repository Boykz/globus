using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.Models.NishDbFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Areas.Nish.Controllers
{ 
    [Area("Nish")]
    [Authorize]
    public class NishController : Controller
    {

        private readonly entDbContext db;
        private UserManager<AppUsern> userManager;
        private SignInManager<AppUsern> SignInManager;
        public NishController(entDbContext _db, UserManager<AppUsern> _userManager, SignInManager<AppUsern> _SignInManager)
        {
            db = _db;
            userManager = _userManager;
            SignInManager = _SignInManager;
        }


        public async Task<IActionResult> Index(int? stats)
        {


            if(stats == 1)
            {
                ViewBag.Pay = 1;
            }
            if (stats == 2)
            {
                ViewBag.Pay = 2;
            }

            var res = db.NishCourses.ToList();

            return View(res);
        }


        public async Task<IActionResult> OpenLesson(int Id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if(user == null)
            {
                return RedirectToAction("LiveTest/Index", "LiveTest");
            }

            //var payvalid = db.NishPays.Where(p => p.UserId == user.Id & p.CloseDate > DateTime.Now.AddHours(14)).FirstOrDefault();
            //if(payvalid == null)
            //{
            //    return RedirectToAction("Index", "Nish", new { stats = 1 });
            //}

            //var lesson = db.NishCourses.FirstOrDefault(p => p.Id == Id);
            //if(lesson.StartDate <= DateTime.Now.AddHours(14))
            //{
            //    return RedirectToAction("Index", "Nish", new { stats = 2 });
            //}

            return Redirect($"");
        }


        //public IActionResult NishAdmin()
        //{
        //    var res = db.NishCourses.ToList();

        //    return View(res);
        //}


        //public IActionResult InNish(int Id)
        //{
        //    var res = db.NishCourses.Where(p => p.Id == Id).FirstOrDefault();

        //    return View(res);
        //}


        //[HttpPost]
        //public IActionResult InNish(NishCourse model)
        //{
        //    db.NishCourses.Update(model);
        //    db.SaveChanges();

        //    return RedirectToAction("NishAdmin");
        //}
    }
}