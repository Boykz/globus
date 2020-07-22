using EntGlobus.Models;
using EntGlobus.ViewModels.LiveLessonViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Controllers
{
    public class LiveLessonController : Controller
    {

        private readonly entDbContext db;
        private readonly IHostingEnvironment _appEnvironment;
        private UserManager<AppUsern> userManager;
        public LiveLessonController(entDbContext _db, IHostingEnvironment appEnvironment, UserManager<AppUsern> _userManager)
        {
            db = _db;
            _appEnvironment = appEnvironment;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            var list = (from b in db.liveLessons
                        select new CreateLiveLessonModel
                        {
                            Name = b.Name,
                            Information = b.Information,
                            Title = b.Title,
                            Photo = b.Photo,
                            Id = b.Id,
                            Icon = b.Icon,
                        }).ToList();

            return View(list);
        }


        [HttpPost]
        public async Task<IActionResult> LiveCreate(string name, string title, string info, IFormFile img, IFormFile icon, DateTime date)
        {
            string url = "";
            if (img == null || img.Length == 0)
            {
                url = null;
            }
            else
            {
                var imgname = name + "-" + img.FileName;
                string path_Root = _appEnvironment.WebRootPath;

                string path_to_Images = path_Root + "\\LiveImg\\" + imgname;
                url = "/LiveImg/" + imgname;
                using (var stream = new FileStream(path_to_Images, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

            }

            LiveLesson ll = new LiveLesson { Name = name, Information = info, Title = title, Photo = url };
            db.liveLessons.Add(ll);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public IActionResult RemoveLiveLesson(int Id)
        {
            LiveLesson liveLesson = db.liveLessons.Where(p => p.Id == Id).FirstOrDefault();
            if (liveLesson != null)
            {
                CreateLiveLessonModel cllm = new CreateLiveLessonModel { Id = liveLesson.Id, Information = liveLesson.Information, Name = liveLesson.Name, Photo = liveLesson.Photo, Title = liveLesson.Title };
                return View(cllm);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveLiveLesson(CreateLiveLessonModel cllm, IFormFile img, IFormFile icon)
        {
            LiveLesson ll = db.liveLessons.FirstOrDefault(p => p.Id == cllm.Id);
            ll.Name = cllm.Name;
            ll.Information = cllm.Information;
            ll.Title = cllm.Title;
            ll.OpenClose = cllm.OpenClose;
            if (img != null && img.Length != 0)
            {
                var imgname = ll.Name + "-" + img.FileName;
                string path_Root = _appEnvironment.WebRootPath;

                string path_to_Images = path_Root + "\\LiveImg\\" + imgname;
                string url = "/LiveImg/" + imgname;
                using (var stream = new FileStream(path_to_Images, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }
                ll.Photo = url;
            }
            if (icon != null && icon.Length != 0)
            {
                var imgname = ll.Name + "-" + icon.FileName;
                string path_Root = _appEnvironment.WebRootPath;

                string path_to_Images = path_Root + "\\LiveIcon\\" + imgname;
                string url = "/LiveIcon/" + imgname;
                using (var stream = new FileStream(path_to_Images, FileMode.Create))
                {
                    await icon.CopyToAsync(stream);
                }
                ll.Icon = url;
            }

            db.liveLessons.Update(ll);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public IActionResult PodLive(int Id)
        {
            /////////////////////////////////
            ///

            var res = (from b in db.PodLiveLessons
                       where b.LiveLessonId == Id
                       select new PodLiveLessonModel
                       {
                           Id = b.Id,
                           LiveLessonId = Id,
                           StartDate = b.StartDate,
                           Status = b.Status,
                           TypeVideo = b.TypeVideo,
                           UrlPhoto = b.UrlPhoto,
                           UrlVideo = b.UrlVideo,
                           LiveLesson = b.LiveLesson,
                       }).ToList();

            ViewBag.Id = Id;

            return View(res);
        }

        public IActionResult CreatePodLive(int Id)
        {
            PodLiveLessonModel pllm = new PodLiveLessonModel { LiveLessonId = Id, };
            pllm.LiveLesson = db.liveLessons.FirstOrDefault(p => p.Id == Id);
            return View(pllm);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePodLive(PodLiveLessonModel pllm, IFormFile img)
        {
            PodLiveLesson pll = new PodLiveLesson { LiveLessonId = pllm.LiveLessonId, Nuska = pllm.Nuska, StartDate = pllm.StartDate, Status = true, TypeVideo = pllm.TypeVideo, UrlVideo = pllm.UrlVideo, DurationTime = pllm.StartDate.AddMinutes(90), Title = pllm.Title };
            LiveLesson ll = db.liveLessons.FirstOrDefault(p => p.Id == pllm.LiveLessonId);
            if (img == null || img.Length == 0)
            {
                pll.UrlPhoto = null;
            }
            else
            {
                var imgname = ll.Name + "-PodLiveLesson-" + img.FileName;
                string path_Root = _appEnvironment.WebRootPath;

                string path_to_Images = path_Root + "\\LiveLessonImg\\" + imgname;
                string url = "/LiveLessonImg/" + imgname;
                using (var stream = new FileStream(path_to_Images, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }
                pll.UrlPhoto = url;
            }

            var falsedb = await db.PodLiveLessons.Where(p => p.LiveLessonId == pll.LiveLessonId & p.Status == true).FirstOrDefaultAsync();
            if(falsedb != null)
            {
                falsedb.Status = false;
                db.PodLiveLessons.Update(falsedb);
            }
            
            db.PodLiveLessons.Add(pll);

            await db.SaveChangesAsync();

            return RedirectToAction("PodLive", new { ll.Id });
        }

        public IActionResult EditPodLive(Guid Id)
        {
            var res = db.PodLiveLessons.FirstOrDefault(p => p.Id == Id);
            if(res != null)
            {
                return View(res);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> EditPodLive(PodLiveLesson model)
        {
            model.DurationTime = model.StartDate.AddMinutes(90);

            db.PodLiveLessons.Update(model);
            await db.SaveChangesAsync();

            return RedirectToAction("PodLive", new { Id = model.LiveLessonId });
        }







        public IActionResult Delete(Guid Id)
        {
            var res = db.PodLiveLessons.Where(p => p.Id == Id).ToList();
            db.PodLiveLessons.RemoveRange(res);
            db.SaveChanges();
            return RedirectToAction("Index","LiveLesson");
        }


        public IActionResult Time()
        {
            

            return Content("");
        }


        public async Task<IActionResult> OpenLiveTest(string Id, int LessonId)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                PayLiveTest plt = new PayLiveTest { UserId = user.Id, StartDate = DateTime.Now, Price = 5000, EndDate = DateTime.Now.AddDays(30), LiveLessonId = LessonId, PayLiveTestType = PayLiveTestType.Default };
                db.PayLiveTests.Add(plt);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "Admin", new { userid = Id });
            }
            return View();
        }

    }
}