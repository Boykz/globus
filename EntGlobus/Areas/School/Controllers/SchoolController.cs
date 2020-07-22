using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Areas.LiveTest.ViewModels;
using EntGlobus.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Areas.School.Controllers
{

    [Area("School")]
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
        {
            var res = (from b in db.liveLessons
                       select new LiveLessonViewModel
                       {
                           Id = b.Id,
                           Name = b.Name,
                           Title = b.Title,
                           Icon = b.Icon,
                           Information = b.Information,
                           Photo = b.Photo,
                           LiveRealTime = false,
                       }).ToList();

            var date = DateTime.Now.AddHours(14);


            return View(res);
        }
    }
}