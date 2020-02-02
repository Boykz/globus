using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.ApiServece;
using EntGlobus.Models;
using EntGlobus.ViewModels.LiveLessonViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Areas.LiveTest.Controllers
{
    [Area("LiveTest")]
    public class LiveTestController : Controller
    {

        private readonly entDbContext db;
        private UserManager<AppUsern> userManager;
        private SignInManager<AppUsern> SignInManager;
        public LiveTestController(entDbContext _db, UserManager<AppUsern> _userManager, SignInManager<AppUsern> _SignInManager)
        {
            db = _db;
            userManager = _userManager;
            SignInManager = _SignInManager;
        }

        public async Task<IActionResult> Index(string Id)
        {
            if (Id != null)
            {
                var res = await userManager.FindByIdAsync(Id);
                if(res != null)
                {
                    await SignInManager.SignInAsync(res, false);
                    return View();
                }
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                return View();
            }
            return RedirectToAction("Close");
        }

        public async Task<IActionResult> Index2(int? Id)
        {
            if (Id != null)
            {
                var res = await db.liveLessons.FirstOrDefaultAsync(p => p.Id == Id);
                if (res != null)
                {
                    if (res.OpenClose == true)
                    {
                        var user = await userManager.FindByNameAsync(User.Identity.Name);
                        var paylivetest = await db.PayLiveTests.Where(p => p.LiveLessonId == res.Id & p.UserId == user.Id & p.EndDate > DateTime.Now).FirstOrDefaultAsync();
                        //var if1 = paylivetest.Where(p => p.EndDate > DateTime.Now).FirstOrDefault();
                        if(paylivetest != null)
                        {
                            var podres = await db.PodLiveLessons.Where(p => p.LiveLessonId == Id).ToListAsync();

                            LiveTestVideoModel ltvm = new LiveTestVideoModel
                            {
                                PodLiveLesson = podres.Where(p => p.Status == true).FirstOrDefault(),
                                HistoryPodLiveLesson = podres.Where(p => p.Status == false).ToList()
                            };
                            return View(ltvm);
                        }
                        else
                        {
                            return RedirectToAction("Pay");
                        }
                        return RedirectToAction("NextPay");
                    }
                    else
                    {
                        return RedirectToAction("Close", new { pan=res.Name });
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index3()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            
            if(user != null)
            {
                return Content($"Hello {user.FirstName} {user.LastName}");
            }

            return Redirect("https://vk.com");
        }







        public IActionResult Pay()
        {
            return View();
        }

        public IActionResult NextPay()
        {
            return View();
        }

        public IActionResult Close(string pan)
        {
            ViewBag.Pan = pan;
            return View();
        }
    }
}