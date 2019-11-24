using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{

    public class MatsauController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly SignInManager<AppUsern> signInManager;
        private readonly UserManager<AppUsern> _userManager;
        private readonly entDbContext db;
        public MatsauController(SignInManager<AppUsern> _signInManager, IHostingEnvironment appEnvironment, entDbContext _db, UserManager<AppUsern> userManager)
        {
            signInManager = _signInManager;
            _appEnvironment = appEnvironment;
            db = _db;
            _userManager = userManager;
        }



        // GET: Matsau/Details/5
        public async Task<ActionResult> Korsetilim()
        {
            var model = await db.Kurs.ToListAsync();
            return PartialView(model);
        }


        public async Task<ActionResult> Sabak(int id)
        {
            var models = await db.Kurs.ToListAsync();
            var model = await db.Kurs.FirstOrDefaultAsync(x => x.Id == id);
            var user = _userManager.GetUserName(User);
            ViewBag.num = user;
            bool acces = false;
            try
            {
                var Allow = await db.Allowkurs.FirstOrDefaultAsync(x => x.UserPhone == user);
                if (Allow.pay)
                {
                    acces = true;
                }

            }
            catch
            {
                acces = false;
            }


            KursWatchViewModel mod = new KursWatchViewModel()
            {
                Kurses = models,
                Kurs = model,
                acces = acces

            };
            return PartialView(mod);
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            return PartialView(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.TelNum, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        var mod = await db.Allowkurs.FirstOrDefaultAsync(x => x.UserPhone == model.TelNum);

                        if (mod == null)
                        {
                            await db.Allowkurs.AddAsync(new Allowkurs
                            {
                                Pan_Id = 1,
                                DateTime = DateTime.Now,
                                UserPhone = model.TelNum
                            });
                            await db.SaveChangesAsync();
                        }
                        return RedirectToAction("Korsetilim", "Matsau");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Номер немесе пароліңіз қате!!");
                }
            }
            return PartialView(model);
        }
        public ActionResult Addkurs()
        {
            ViewBag.allcurses = db.AllCourses.ToList();
            return View();
        }
        public async Task<IActionResult> Resept(UserStatusViewModel model)
        {
            await db.Reseptions.AddAsync(new Reseption
            {
                DateTime = DateTime.Now,
                Name = model.Comment,
                Phone = model.Phone
            });
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [RequestSizeLimit(200 * 1024 * 1024)]
        [HttpPost]
        public async Task<IActionResult> AddKurs(AddUrokViewModel model)
        {
            string path_Root = _appEnvironment.WebRootPath;
            if (model.video == null || model.video.Length == 0) return Content("video not found");
            if (model.photo == null || model.photo.Length == 0) return Content("photo not found");
            string mat1 = null;
            string mat2 = null;
            string mat3 = null;
            string mat4 = null;
            if (model.mat1 != null && model.mat1.Length != 0)
            {
                mat1 = model.mat1.FileName;
                string path_to_mat1 = path_Root + "\\Kurs\\" + mat1;
                using (var stream = new FileStream(path_to_mat1, FileMode.Create))
                {
                    await model.mat1.CopyToAsync(stream);
                }
            }

            if (model.mat2 != null && model.mat2.Length != 0)
            {
                mat2 = model.mat2.FileName;
                string path_to_mat2 = path_Root + "\\Kurs\\" + mat2;
                using (var stream = new FileStream(path_to_mat2, FileMode.Create))
                {
                    await model.mat2.CopyToAsync(stream);
                }
            }

            if (model.mat3 != null && model.mat3.Length != 0)
            {
                mat3 = model.mat3.FileName;
                string path_to_mat3 = path_Root + "\\Kurs\\" + mat3;
                using (var stream = new FileStream(path_to_mat3, FileMode.Create))
                {
                    await model.mat3.CopyToAsync(stream);
                }
            }

            if (model.mat4 != null && model.mat4.Length != 0)
            {
                mat4 = model.mat4.FileName;
                string path_to_mat4 = path_Root + "\\Kurs\\" + mat4;
                using (var stream = new FileStream(path_to_mat4, FileMode.Create))
                {
                    await model.mat4.CopyToAsync(stream);
                }
            }

            // var videoname = model.video.FileName;
            var imgname = model.photo.FileName;

            string path_to_Images = path_Root + "\\Kurs\\" + imgname;
            // string path_to_Videos = path_Root + "\\Kurs\\" + videoname;
            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {
                await model.photo.CopyToAsync(stream);
            }
            //using (var stream = new FileStream(path_to_Videos, FileMode.Create))
            //{
            //    await model.video.CopyToAsync(stream);
            //}

            await db.Kurs.AddAsync(new Kurs
            {            
                course_id = model.course,
                subject = model.subject,
                text = model.text,
                video = model.video,
                time = model.time,
                photo = imgname,
                mat1 = mat1,
                mat2 = mat2,
                mat3 = mat3,
                mat4 = mat4
            });
            await db.SaveChangesAsync();


            return RedirectToAction(nameof(Kurses));
        }

        public async Task<IActionResult> Allow(string search)
        {
            var model = from s in db.Allowkurs select s;
            ViewBag.str = search;
            if (!String.IsNullOrEmpty(search))
            {
                model = db.Allowkurs.Where(x => x.UserPhone.Contains(search));
            }


            var op = db.Allowkurs.Count();
            var za = db.Allowkurs.Where(x => x.pay == true).Count();
            ViewBag.see = op;
            ViewBag.pay = za;
            return View(model);
        }

        public async Task<IActionResult> Dostup(int id)
        {
            var model = await db.Allowkurs.FirstOrDefaultAsync(x => x.Id == id);
            model.Price = 1000;
            model.pay = true;
            model.DateTime = DateTime.Now;
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Allow));
        }

        public async Task<IActionResult> Resepts()
        {
            var model = await db.Reseptions.ToListAsync();

            ViewBag.see = model.Count();
            return View(model);
        }


        public async Task<IActionResult> Kurses()
        {
            var model = await db.Kurs.ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> Kurs(int id)
        {
            
            var model = await db.Kurs.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Course = model.course_id == 0;
            ViewBag.Courses =  db.AllCourses.ToList();
            return View(model);
        }
        public async Task<IActionResult> Del(string name, int id, string type)
        {
            var model = await db.Kurs.FirstOrDefaultAsync(x => x.Id == id);

            string path_Root = _appEnvironment.WebRootPath;
            string path_to_file = path_Root + "\\Kurs\\" + name;

            try
            {
                if (type != "video")
                {
                    System.IO.File.Delete(path_to_file);
                }

            }
            catch
            {
                return NotFound();
            }
            if (type == "video")
            {
                model.video = null;
                await db.SaveChangesAsync();
            }
            if (type == "photo")
            {
                model.photo = null;
                await db.SaveChangesAsync();
            }
            if (type == "mat1")
            {
                model.mat1 = null;
                await db.SaveChangesAsync();
            }
            if (type == "mat2")
            {
                model.mat2 = null;
                await db.SaveChangesAsync();
            }
            if (type == "mat3")
            {
                model.mat3 = null;
                await db.SaveChangesAsync();
            }
            if (type == "mat4")
            {
                model.mat4 = null;
                await db.SaveChangesAsync();
            }


            return RedirectToAction("Kurs", new { id = id });
        }
        // [DisableRequestSizeLimit]
        // [RequestSizeLimit(200 * 1024 * 1024)]
        [HttpPost]
        public async Task<IActionResult> Editkurs(AddUrokViewModel model)
        {
            string path_Root = _appEnvironment.WebRootPath;

            var kur = await db.Kurs.FirstOrDefaultAsync(x => x.Id == model.Id);
            string mat1;
            string mat2;
            string mat3;
            string mat4;
            if (model.mat1 != null && model.mat1.Length != 0)
            {
                mat1 = model.mat1.FileName;
                kur.mat1 = mat1;
                string path_to_mat1 = path_Root + "\\Kurs\\" + mat1;
                using (var stream = new FileStream(path_to_mat1, FileMode.Create))
                {
                    await model.mat1.CopyToAsync(stream);
                }
            }


            if (model.mat2 != null && model.mat2.Length != 0)
            {
                mat2 = model.mat2.FileName;
                kur.mat2 = mat2;
                string path_to_mat2 = path_Root + "\\Kurs\\" + mat2;
                using (var stream = new FileStream(path_to_mat2, FileMode.Create))
                {
                    await model.mat2.CopyToAsync(stream);
                }
            }

            if (model.mat3 != null && model.mat3.Length != 0)
            {
                mat3 = model.mat3.FileName;
                kur.mat3 = mat3;
                string path_to_mat3 = path_Root + "\\Kurs\\" + mat3;
                using (var stream = new FileStream(path_to_mat3, FileMode.Create))
                {
                    await model.mat3.CopyToAsync(stream);
                }
            }

            if (model.mat4 != null && model.mat4.Length != 0)
            {
                mat4 = model.mat4.FileName;
                kur.mat4 = mat4;
                string path_to_mat4 = path_Root + "\\Kurs\\" + mat4;
                using (var stream = new FileStream(path_to_mat4, FileMode.Create))
                {
                    await model.mat4.CopyToAsync(stream);
                }
            }
            if (model.video != null && model.video.Length != 0)
            {
                //var videoname = model.video.FileName;
                //string path_to_Videos = path_Root + "\\Kurs\\" + videoname;
                //using (var stream = new FileStream(path_to_Videos, FileMode.Create))
                //{
                //    await model.video.CopyToAsync(stream);
                //}
                kur.video = model.video;
            }

            if (model.photo != null && model.photo.Length != 0)
            {
                var imgname = model.photo.FileName;

                string path_to_Images = path_Root + "\\Kurs\\" + imgname;

                using (var stream = new FileStream(path_to_Images, FileMode.Create))
                {
                    await model.photo.CopyToAsync(stream);
                }
                kur.photo = imgname;
            }
            kur.subject = model.subject;
            kur.text = model.text;

            kur.course_id = model.course;

            kur.time = model.time;
            await db.SaveChangesAsync();


            return RedirectToAction("Kurs", new { id = model.Id });
        }

        public async Task<IActionResult> Addcourse()
        {           
            return View(await db.AllCourses.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Addcourse(AllCourses course)
        {
            await db.AllCourses.AddAsync(new AllCourses { Name = course.Name, dateTime = DateTime.Now });
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Addcourse));
        }



        }
}