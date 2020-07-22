using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.ApiServece;
using EntGlobus.Areas.LiveTest.ViewModels;
using EntGlobus.Models;
using EntGlobus.Models.DbFolder1;
using EntGlobus.ViewModels.LiveLessonViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Areas.LiveTest.Controllers
{

    [Authorize]
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

        [AllowAnonymous]
        public async Task<IActionResult> Index(string Id)
        {

            if (Id != null)
            {
                var user = await userManager.FindByIdAsync(Id);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, false);
                    return View();
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    return View();
                }
            }

            return RedirectToAction("NewLogin","Home");

        }


        public async Task<IActionResult> Lesson()
        {
            var res = (from b in db.liveLessons
                       select new LiveLessonViewModel
                       {
                           Id = b.Id,
                           Name=b.Name,
                           Title = b.Title,
                           Icon=b.Icon,
                           Information = b.Information,
                           Photo = b.Photo,
                           LiveRealTime = false,
                       }).ToList();

            var date = DateTime.Now.AddHours(14);

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            await db.LiveTestVisitor.AddAsync(new LiveTestVisitor { DateTime = date, UserId = user.Id });
            await db.SaveChangesAsync();

            var live = db.PodLiveLessons.Where(p => p.StartDate <= date).Where(p => p.DurationTime >= date).ToList();

            foreach(var l in live)
            {
                foreach(var d in res)
                {
                    if(d.Id == l.LiveLessonId)
                    {
                        d.LiveRealTime = true;
                    }
                }
            }

            return View(res);
        }


        public async Task<IActionResult> Index2(int? Id)
        {
            //var user = await userManager.FindByNameAsync(User.Identity.Name);
            //if(user == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //var name = user.UserName;
            //var email = user.UserName + "@mail.ru";
            //if (Id == 1)
            //{
            //    return Redirect($"https://bahonshymkentbay.easywebinar.live/oneclick-registration-5?attendee_name=" + name + "&attendee_email=" + email);
            //}
            //return RedirectToAction("Index3");

            if (Id != null)
            {
                var res = await db.liveLessons.FirstOrDefaultAsync(p => p.Id == Id);
                ViewBag.Pan = res.Name;
                if (res != null)
                {
                    if (res.OpenClose == true)
                    {

                        #region Open Day
                        
                                var podres = await db.PodLiveLessons.Where(p => p.LiveLessonId == Id).Include(p=>p.LiveLesson).ToListAsync();

                                
                                LiveTestVideoModel ltvm = new LiveTestVideoModel
                                {
                                    PodLiveLesson = podres.Where(p => p.Status == true).FirstOrDefault(),
                                    HistoryPodLiveLesson = podres.Where(p => p.Status == false).ToList()
                                };
                                
                                return View(ltvm);
                        
                        #endregion

                        #region Pay
                        
                        //var paylivetest = await db.PayLiveTests.Where(p => p.LiveLessonId == res.Id & p.UserId == user.Id & p.EndDate > DateTime.Now.AddHours(14)).FirstOrDefaultAsync();
                        ////var if1 = paylivetest.Where(p => p.EndDate > DateTime.Now).FirstOrDefault();
                        //if (paylivetest != null)
                        //{
                        //    var podres = await db.PodLiveLessons.Where(p => p.LiveLessonId == Id).ToListAsync();

                        //    LiveTestVideoModel ltvm = new LiveTestVideoModel
                        //    {
                        //        PodLiveLesson = podres.Where(p => p.Status == true).FirstOrDefault(),
                        //        HistoryPodLiveLesson = podres.Where(p => p.Status == false).ToList()
                        //    };
                        //    return View(ltvm);
                        //}
                        //else
                        //{
                        //    return RedirectToAction("Pay", new { Id });
                        //}
                        //return RedirectToAction("NextPay");

                        
                        #endregion

                    }
                    else
                    {
                        return RedirectToAction("Close", new { pan=res.Name, Id = res.Id });
                    }
                }
            }
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Index3(int Id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            
            if(user == null)
            {
                return RedirectToAction("Lesson");
            }
            if(Id == 1)
            {
                return Redirect($"");
            }

            return View();
            // return Redirect($"https://bahon335.clickmeeting.com/test1");
        }


        public IActionResult HistoryVideo(Guid Id)
        {
            var res = db.PodLiveLessons.Include(p=>p.LiveLesson).FirstOrDefault(p => p.Id == Id);

            if(res != null)
            {
                if(res.TypeVideo == TypeLiveLesson.Live)
                {
                    return Redirect($"{res.UrlVideo}");
                }
                return View(res);
            }

            return RedirectToAction("Lesson");
        }




        public IActionResult Pay(int Id)
        {
            var res = db.liveLessons.FirstOrDefault(p => p.Id == Id);
            if(res != null)
            {
                ViewBag.Name = res.Name;
                return View();
            }

            return BadRequest();
        }

        public IActionResult NextPay()
        {
            return View();
        }

        public IActionResult Close(string pan, int Id)
        {
            ViewBag.Pan = pan;
            ViewBag.Id = Id;
            return View();
        }

        [AllowAnonymous]
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}