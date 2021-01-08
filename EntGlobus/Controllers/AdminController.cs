using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Helpers;
using EntGlobus.ModelFolder;
using EntGlobus.Models;
using EntGlobus.Models.NishDbFolder;
using EntGlobus.Models.QR;
using EntGlobus.ViewModels;
using EntGlobus.ViewModels.adminview;
using EntGlobus.ViewModels.LiveLessonViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUsern> _userManager;
        private readonly entDbContext db;
  
        private readonly IHostingEnvironment _appEnvironment;
        public AdminController(RoleManager<IdentityRole> _role, UserManager<AppUsern> _user, entDbContext _db, IHostingEnvironment appEnvironment)
        {
            _userManager = _user;
            db = _db;
            _appEnvironment = appEnvironment;
        }


        public async Task<ActionResult> Index(string search, int? page)
        {
            if (search != null)
            {
                page = 1;
            }
            var user = from s in db.Usernew select s;
            var count = user.Count();
            ViewBag.count = count;
            ViewBag.str = search;
            if (!String.IsNullOrEmpty(search))
            {

                user = user.Where(s => s.UserName.Contains(search));
                if (user == null)
                {

                    ViewBag.no = "fgsdaf";
                }
            }
            int pageSize = 20;
            var md = await PaginatedLists<AppUsern>.CreateAsync(user.AsNoTracking(), page ?? 1, pageSize);
            return View(md);
        }


        public async Task<IActionResult> RegDateUser()
        {
            var users = await _userManager.Users.Where(p => p.regdate >= Convert.ToDateTime("2020-06-01")).ToListAsync();

            ViewBag.Users = users;

            return View();
        }
        


        public async Task<IActionResult> Edit(string userId, string change)
        {
           
            // получаем пользователя
            AppUsern user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ViewBag.user_id = user.Id;
               
                ViewBag.num = user.UserName;
                var satim = await db.Satilims.ToListAsync();
                List<Ofpay> ofpay = db.Ofpays.Where(x => x.IdentityId == userId).ToList();
                var a = from o in db.Ofpays where o.IdentityId == userId select o.type.ToString();
                var search = await db.Searches.Where(x => x.IdentityId == userId & x.pay == true).FirstOrDefaultAsync();
                var blok = await db.Bloks.Where(x => x.IdentityId == userId & x.enable == true).FirstOrDefaultAsync();

                List<string> openItems = new List<string>();
                if (search != null)
                {
                    openItems.Add("search");
                }
                if (blok != null)
                {
                    openItems.Add("bloks");
                }
                foreach (string arrItem in a)
                {
                    openItems.Add(arrItem);
                }

                AdminBuyViewModel model = new AdminBuyViewModel
                {
                    Satilims = satim,
                    IUser = user,
                    Bought = openItems
                };
                ViewBag.change = change;

                var live = db.liveLessons.ToList();
                var livepay = db.PayLiveTests.Where(p => p.UserId == userId & p.EndDate > DateTime.Now).ToList();

                List<PayAndBuyLiveTestViewModel> pabltv = new List<PayAndBuyLiveTestViewModel>();
                foreach (var d in live)
                {
                    pabltv.Add( new PayAndBuyLiveTestViewModel { PanId = d.Id, OpenClose = false, Name = d.Name });
                }
                foreach (var d in pabltv)
                {
                    foreach(var f in livepay)
                    {
                        if(d.PanId == f.LiveLessonId)
                        {
                            d.OpenClose = true;
                        }
                    }
                }
                ViewBag.Live = pabltv;


                return View(model);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Entry(EntryViewModel model)
        {
            AppUsern user = await _userManager.FindByIdAsync(model.userId);
            if (user != null)
            {
                user.enable = true;
                user.offenable = true;
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { userId = model.userId });

        }


        [HttpPost]
        public async Task<IActionResult> Edit(string userId, string pans,string fg)
        {
            AppUsern user = await _userManager.FindByIdAsync(userId);

            var admin = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                #region
                //if (pans == "search")
                //{
                //    var serch = await db.Searches.FirstOrDefaultAsync(x => x.IdentityId == userId);
                //    serch.pay = true;
                //    serch.enable = false;
                //    serch.count = 0;
                //    serch.date = DateTime.Now;

                //    await db.SaveChangesAsync();

                //    return RedirectToAction("Edit", new { userId = userId });
                //}

                //if (pans == "bloks")
                //{
                //    var bloks = await db.Bloks.FirstOrDefaultAsync(x => x.IdentityId == userId);
                //    if (bloks != null)
                //    {
                //        bloks.enable = true;
                //        bloks.blok = "all";
                //        bloks.BuyDate = DateTime.Now;
                //        await db.SaveChangesAsync();
                //        return RedirectToAction("Edit", new { userId = userId });
                //    }
                //    else
                //    {
                //        Blok blok = new Blok
                //        {
                //            blok = "all",
                //            enable = true,
                //            BuyDate = DateTime.Now,
                //            IdentityId = userId
                //        };
                //        db.Bloks.Add(blok);
                //        await db.SaveChangesAsync();

                //    }

                //    return RedirectToAction("Edit", new { userId = userId });
                //}


                #endregion

                var sum = await db.Satilims.FirstOrDefaultAsync();

                Ofpay addof = new Ofpay
                {
                    IdentityId = userId,
                    type = pans,
                    Price = String.Format("{0:0.##}", sum.Price),
                    Time = DateTime.Now,
                    AdminNumber = admin.UserName,
                };
                db.Ofpays.Add(addof);
                await db.SaveChangesAsync();

                return RedirectToAction("Edit", new { userId = userId });
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Resetpass(ResetPassViewModel model)
        {
            AppUsern user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ObjectResult(new { result = "not found" });
            }

            string text = user.UserName;

            model.NewPassword = text.Substring(7);

            var _passwordValidator =
               HttpContext.RequestServices.GetService(typeof(IPasswordValidator<AppUsern>)) as IPasswordValidator<AppUsern>;
            var _passwordHasher =
                HttpContext.RequestServices.GetService(typeof(IPasswordHasher<AppUsern>)) as IPasswordHasher<AppUsern>;

            IdentityResult result =
                await _passwordValidator.ValidateAsync(_userManager, user, user.PasswordHash);
            if (result.Succeeded)
            {
               var aa =   "Пароль сәтті ауысты";
                user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                if(user.ResetCount >= 1)
                {
                    user.ResetCount += 1;
                }
                else
                {
                    user.ResetCount = 1;
                }
                
                await _userManager.UpdateAsync(user);
                await db.SaveChangesAsync();
               return RedirectToAction(nameof(Edit), new { userId = model.Id , change = aa });
            }
            return BadRequest();
        }




        public async Task<IActionResult> Money()
        {
            var ofpay = await db.Ofpays.Where(x => x.Id > 8544).ToListAsync();
            ViewBag.sat = ofpay.Count();
            ViewBag.o1000 = ofpay.Where(x => x.Price == "1000").Count()*1000;

            ViewBag.May = db.Ofpays.Where(p => p.Time >= Convert.ToDateTime("01-06-2020")).ToList().Count();

            var WhenAdmin = ofpay.Where(p => p.AdminNumber != null).ToList();

            ViewBag.Medey = WhenAdmin.Where(p => p.AdminNumber == "77472400089").ToList().Count();
            ViewBag.Adik = WhenAdmin.Where(p => p.AdminNumber == "77006406004").ToList().Count();
            ViewBag.Baha = WhenAdmin.Where(p => p.AdminNumber == "77089274498").ToList().Count();


            var usercount = db.Ofpays.Select(x => x.IdentityId.ToString()).Distinct().ToList();
            ViewBag.usercount = usercount.Count();
            ViewBag.usercount6 = usercount.Count() - 216;
            var user6 = usercount.Count - 216;
            ViewBag.usercount4 = usercount.Count - user6;

            IQueryable<PanSatilimViewModel> data =
                from Ofpay in db.Ofpays
                group Ofpay by Ofpay.type into paygroup

                select new PanSatilimViewModel()
                {
                    Pan = paygroup.Key,
                    Buycount = paygroup.Count()
                };
            return View(await data.AsNoTracking().OrderByDescending(x => x.Buycount).ToListAsync());
        }




        public async Task<IActionResult> Delete(string pan, string userId)
        {
            if (pan == null || userId == null)
            {
                return View();
            }

            if (pan == "search")
            {
                var serch = await db.Searches.FirstOrDefaultAsync(x => x.IdentityId == userId);
                serch.pay = false;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { userId = userId });


            }
            if (pan == "bloks")
            {
                var bloks = await db.Bloks.FirstOrDefaultAsync(x => x.IdentityId == userId);
                bloks.enable = false;
                await db.SaveChangesAsync();

                return RedirectToAction("Edit", new { userId = userId });
            }

            var userpan = await db.Ofpays.Where(x => x.IdentityId == userId).Where(x => x.type == pan).FirstOrDefaultAsync();
            db.Ofpays.Remove(userpan);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { userId = userId });
        }



        public async Task<IActionResult> Todayqs()
        {
            var stilim = await db.Satilims.ToListAsync();
            TodayQsViewModel tq = new TodayQsViewModel()
            {
                Satilims = stilim
            };
            return View(tq);
        }


        [HttpPost]
        public async Task<IActionResult> Todayqs(TodayQsViewModel request)
        {
            if (ModelState.IsValid)
            {
                List<string> answers = new List<string>();
                answers.Add(request.ans1);
                answers.Add(request.ans2);
                answers.Add(request.ans3);
                answers.Add(request.ans4);
                answers.Add(request.ans5);
                List<string> randans = new List<string>();
                Random rand = new Random(DateTime.Now.ToString().GetHashCode());
                while (answers.Count > 0)
                {
                    int index = rand.Next(0, answers.Count);
                    randans.Add(answers[index]);
                    answers.RemoveAt(index);
                }
                await db.Dayliquestions.AddAsync(new Dayliquestion
                {
                    subject = request.subject,
                    question = request.question,
                    ans1 = randans[0],
                    ans2 = randans[1],
                    ans3 = randans[2],
                    ans4 = randans[3],
                    ans5 = randans[4],
                    Correctans = request.ans1,
                    qstime = DateTime.Now
                });
                await db.SaveChangesAsync();
                return RedirectToAction("Listqs");
            }
            return View();
        }

        public async Task<IActionResult> Listqs()
        {
            var model = await db.Dayliquestions.OrderByDescending(x => x.qstime).ToListAsync();
            ViewBag.count = model.Count();
            return View(model);
        }

        public async Task<IActionResult> Delqs(int? id)
        {
            var model = await db.Dayliquestions.FirstOrDefaultAsync(x => x.Id == id);
            db.Dayliquestions.Remove(model);
            await db.SaveChangesAsync();
            return RedirectToAction("Listqs");
        }

        public async Task<IActionResult> Editqs(int? id)
        {
            var stilim = await db.Satilims.ToListAsync();
            var qs = await db.Dayliquestions.FirstOrDefaultAsync(x => x.Id == id);
            TodayQsViewModel tq = new TodayQsViewModel()
            {
                Satilims = stilim,
                Dayliquestion = qs
            };

            return View(tq);
        }

        [HttpPost]
        public async Task<IActionResult> Editqs(TodayQsViewModel request)
        {
            var ID = request.id;
            var dqs = await db.Dayliquestions.FirstOrDefaultAsync(x => x.Id == ID);
            dqs.question = request.question;
            dqs.subject = request.subject;
            dqs.ans1 = request.ans1;
            dqs.ans2 = request.ans2;
            dqs.ans3 = request.ans3;
            dqs.ans4 = request.ans4;
            dqs.ans5 = request.ans5;
            dqs.Correctans = request.cor;
            await db.SaveChangesAsync();
            return RedirectToAction("Listqs");

        }

        public async Task<IActionResult> Offers()
        {

            var offers = await db.Newqs.ToListAsync();
            ViewBag.count = offers.Count;
            return View(offers);
        }

        public async Task<IActionResult> Deloffer(int? id)
        {
            var offers = await db.Newqs.FirstOrDefaultAsync(x => x.Id == id);
            string path_Root = _appEnvironment.WebRootPath;
            string path_to_Image = path_Root + offers.Uriphoto;
            db.Newqs.Remove(offers);
            await db.SaveChangesAsync();
            try
            {
                System.IO.File.Delete(path_to_Image);
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Offers");
        }

        public async Task<IActionResult> Addblok()
        {
            var model = await db.SellBloks.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Addblok(SellBlokViewModel request)
        {
            if (request.File == null || request.File.Length == 0) return Content("file not found");

            var imgname = DateTime.Now.ToString("MMddHHmmss") + request.File.FileName;
            string path_Root = _appEnvironment.WebRootPath;

            string path_to_Images = path_Root + "\\Postimages\\" + imgname;
            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            await db.SellBloks.AddAsync(new SellBlok
            {
                Name = request.Name,
                Date = DateTime.Today,
                Typi = request.Typi,
                Imgurl = imgname,
                Variants = request.Variants
            });
            await db.SaveChangesAsync();
            return RedirectToAction("Addblok");
        }

        public async Task<IActionResult> Delblok(int? id)
        {
            var blok = await db.Bloks.FirstOrDefaultAsync(x => x.Id == id);
            db.Remove(blok);
            return RedirectToAction("addblok");
        }

        public async Task<IActionResult> Users(int pageSize, string number)
        {
            List<AppUsern> model = await db.Users.Where(x => x.FirstName != "null").Take(pageSize).ToListAsync();
            var statuses = await db.UserStatuses.ToListAsync();
            var serch = await db.Searches.ToListAsync();
            var bloks = await db.Bloks.ToListAsync();


            if (number != null)
            {
                model = await db.Users.Where(x => x.FirstName != "null" && x.UserName.Contains(number)).Take(pageSize).ToListAsync();
            }
            List<AppUsern> users = new List<AppUsern>();


            ViewBag.serches = db.Searches.Where(x => x.pay == true).Count() - 1;
            ViewBag.bloks = db.Bloks.Count() - 2;
            ViewBag.priceb = (db.Bloks.Count() - 2) * 500;
            ViewBag.pr = ViewBag.serches * 200;
            ViewBag.onuser = db.Users.Where(x => x.FirstName != "null").Count();
            ViewBag.offuser = db.Users.Where(x => x.FirstName == "null").Count();


            SatesViewModel fulmodel = new SatesViewModel()
            {
                Users = model,
                UserSatuses = statuses,
                Searches = serch,
                Bloks = bloks
            };

            ViewBag.pagesize = pageSize;
            ViewBag.cnt = model.Count;
            return View(fulmodel);
        }


        [HttpPost]
        public async Task<IActionResult> Users(SatesViewModel body)
        {

            var model = await db.Users.Where(x => x.FirstName != "null" && x.UserName == body.number).ToListAsync();


            var serch = await db.Searches.ToListAsync();


            var statuses = await db.UserStatuses.ToListAsync();

            var bloks = await db.Bloks.ToListAsync();
            var AllUsers = from s in db.Usernew where s.FirstName != "null" select s;


            SatesViewModel fulmodel = new SatesViewModel()
            {
                Users = model,
                UserSatuses = statuses,
                Searches = serch,
                Bloks = bloks
            };


            return View(fulmodel);
        }






        public ActionResult Status()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Status(UserStatus body)
        {
            var stses = await db.UserStatuses.FirstOrDefaultAsync(x => x.Number == body.Number);

            if (stses != null)
            {
                stses.Status = body.Status;
                stses.Comment = body.Comment;
                stses.CheckDate = DateTime.Now;
                await db.SaveChangesAsync();

            }
            else
            {
                await db.UserStatuses.AddAsync(new UserStatus
                {
                    Number = body.Number,
                    Comment = body.Comment,
                    Status = body.Status,
                    CheckDate = DateTime.Now
                });

                await db.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Users));
        }


        public async Task<ActionResult> Sutest()
        {
            var model = await db.Suvariants.ToListAsync();
            return View(model);
        }

        public async Task<ActionResult> AddVariant()
        {
            var model = await db.Subloks.ToListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> AddVariant(SuTestViewModel suTestView)
        {
            await db.Suvariants.AddAsync(new Suvariant
            {
                Name = suTestView.V_name,
                Blok_id = suTestView.Blok

            });
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Sutest));
        }

        public async Task<ActionResult> AddSuQs(int Id, string Name, int PanID)
        {
            ViewBag.Name = Name;
            ViewBag.v_id = Id;
            ViewBag.PanID = PanID;
            var pr = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Variant_id == Id);
            var prp = await db.Su_Ques_s.FirstOrDefaultAsync(x => x.Variant_id == Id);
            //if (pr != null || prp != null)
            //{
            //    return RedirectToAction(nameof(SuTestView), new { v_id = Id,PanID = 4 });
            //}

            var model = await db.Satilims.ToListAsync();
            ViewBag.s_name = db.Satilims.FirstOrDefault(x => x.Id == PanID).Name;
            var answ = await db.Su_Right_Ans.Where(x => x.Variant_id == Id & x.Subject_id == PanID).OrderBy(x=>x.Ques_id).ToListAsync();
            var ansph = await db.Su_Ques_s.Where(x => x.Variant_id == Id & x.Subject_id == PanID).ToListAsync();
            SuTestViewModel md = new SuTestViewModel
            {
                Satilims = model,
                Su_Right_Ans = answ,
                Su_Ques_Phs = ansph
            };

            return View(md);
        }


        [HttpPost]
        public async Task<ActionResult> Checktest(SuTestViewModel model)
        {
            return RedirectToAction(nameof(AddSuQs), new { Id = model.v_id, Name = model.V_name,PanId = model.PanID });
        }

        public async Task<ActionResult> SuTestView(int v_id,int PanID)
        {
            var rans = await db.Su_Right_Ans.Where(x => x.Variant_id == v_id && x.Subject_id == PanID).ToListAsync();
            var grph = await db.Su_Ques_s.Where(y => y.Variant_id == v_id && y.Subject_id == PanID).ToListAsync();
            ViewBag.vd = v_id;
            ViewBag.s_name =  db.Satilims.FirstOrDefault(x => x.Id == PanID).Name;
            SuTestViewModel mod = new SuTestViewModel()
            {
                Su_Right_Ans = rans,
                Su_Ques_Phs = grph,
                Satilims = await db.Satilims.ToListAsync()
            };

            return View(mod);
        }

        [HttpPost]
        public async Task<ActionResult> SuTestView(SuTestViewModel model)
        {
            var rans = await db.Su_Right_Ans.Where(x => x.Variant_id == model.v_id & x.Subject_id == model.PanID).ToListAsync();
            var grph = await db.Su_Ques_s.Where(y => y.Variant_id == model.v_id & y.Subject_id == model.PanID).ToListAsync();

            SuTestViewModel mod = new SuTestViewModel()
            { 
                Su_Right_Ans = rans,
                Su_Ques_Phs = grph,
                Satilims = await db.Satilims.ToListAsync()
            };


            return View(mod);
        }

       
        [HttpPost]
        public async Task<ActionResult> AddSuQs(SuTestViewModel model)
        {
            string path_Root = _appEnvironment.WebRootPath;
            string mat1 = null;
            string mat2 = null;
            string mat3 = null;
            string mat4 = null;
            string mat5 = null;
            string mat6 = null;
            string mat7 = null;
            string mat8 = null;
            string pppp = model.PanID.ToString() + model.V_name;
            if (model.mat1 != null && model.mat1.Length != 0)
            {
                mat1 = pppp + "m1" + model.mat1.FileName;
                string path_to_mat1 = path_Root + "\\SuTESt_photos\\" + mat1;
                using (var stream = new FileStream(path_to_mat1, FileMode.Create))
                {
                    await model.mat1.CopyToAsync(stream);
                }

               
            }
            if (model.mat2 != null && model.mat2.Length != 0)
            {
                mat2 = pppp + "m2" + model.mat2.FileName;
                string path_to_mat2 = path_Root + "\\SuTESt_photos\\" + mat2;
                using (var stream = new FileStream(path_to_mat2, FileMode.Create))
                {
                    await model.mat2.CopyToAsync(stream);
                }


            }
            if (model.mat3 != null && model.mat3.Length != 0)
            {
                mat3 = pppp + "m3" + model.mat3.FileName;
                string path_to_mat3 = path_Root + "\\SuTESt_photos\\" + mat3;
                using (var stream = new FileStream(path_to_mat3, FileMode.Create))
                {
                    await model.mat3.CopyToAsync(stream);
                }


            }

            if (model.mat4 != null && model.mat4.Length != 0)
            {
                mat4 = pppp + "m4" + model.mat4.FileName;
                string path_to_mat4 = path_Root + "\\SuTESt_photos\\" + mat4;
                using (var stream = new FileStream(path_to_mat4, FileMode.Create))
                {
                    await model.mat4.CopyToAsync(stream);
                }


            }
            if (model.mat5 != null && model.mat5.Length != 0)
            { 
                mat5 = pppp + "m5" + model.mat5.FileName;
                string path_to_mat5 = path_Root + "\\SuTESt_photos\\" + mat5;
                using (var stream = new FileStream(path_to_mat5, FileMode.Create))
                {
                    await model.mat5.CopyToAsync(stream);
                }


            }
            if (model.mat6 != null && model.mat6.Length != 0)
            {
                mat6 = pppp + "m6" + model.mat6.FileName;
                string path_to_mat6 = path_Root + "\\SuTESt_photos\\" + mat6;
                using (var stream = new FileStream(path_to_mat6, FileMode.Create))
                {
                    await model.mat6.CopyToAsync(stream);
                }


            }
            if (model.mat7 != null && model.mat7.Length != 0)
            {
                mat7 = pppp + "m7" + model.mat7.FileName;
                string path_to_mat7 = path_Root + "\\SuTESt_photos\\" + mat7;
                using (var stream = new FileStream(path_to_mat7, FileMode.Create))
                {
                    await model.mat7.CopyToAsync(stream);
                }


            }
            if (model.mat8 != null && model.mat8.Length != 0)
            {
                mat8 = pppp + "m8" + model.mat8.FileName;
                string path_to_mat8 = path_Root + "\\SuTESt_photos\\" + mat8;
                using (var stream = new FileStream(path_to_mat8, FileMode.Create))
                {
                    await model.mat8.CopyToAsync(stream);
                }
            }

            var phphp = await db.Su_Ques_s.FirstOrDefaultAsync(x => x.Variant_id == model.v_id && x.Subject_id == model.PanID);

            if(phphp == null)
            {
                await db.Su_Ques_s.AddAsync(new Su_ques_ph
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Path_1 = mat1,
                    Path_2 = mat2,
                    Path_3 = mat3,
                    Path_4 = mat4,
                    Path_5 = mat5,
                    Path_6 = mat6,
                    Path_7 = mat7
                });
            }
            else
            {
                if(mat1 != null)
                {
                    phphp.Path_1 = mat1;
                }
                if(mat2 != null)
                {
                    phphp.Path_2 = mat2;
                }
               if(mat3 != null)
                {
                    phphp.Path_3 = mat3;
                }
              if(mat4 != null)
                {
                    phphp.Path_4 = mat4;
                }
             if(mat5 != null)
                {
                    phphp.Path_5 = mat5;
                }
             if(mat6 != null)
                {
                    phphp.Path_6 = mat6;
                }
                if(mat7 != null)
                {
                    phphp.Path_7 = mat7;
                }
   
            }


            var q1 = await db.Su_Right_Ans.FirstOrDefaultAsync(x=>x.Ques_id == 1 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);

            if(q1 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 1,
                    Right_ans = model.q1
                });
            }
            else
            {
                if(model.q1 != null)
                {
                    q1.Right_ans = model.q1;
                }
            }

            var q2 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 2 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q2 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 2,
                    Right_ans = model.q2
                });
            }
            else
            {
                if(model.q2 != null)
                q2.Right_ans = model.q2;
            }
            var q3 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 3 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);

            if(q3 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 3,
                    Right_ans = model.q3
                });
            }
            else
            {
                if (model.q3 != null)
                    q3.Right_ans = model.q3;
            }

            var q4 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 4 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q4 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 4,
                    Right_ans = model.q4
                });
            }
            else
            {
                if (model.q4 != null)
                    q4.Right_ans = model.q4;
            }
            var q5 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 5 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q5 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 5,
                    Right_ans = model.q5
                });
            }
            else
            {
                if (model.q5 != null)
                    q5.Right_ans = model.q5;
            }
            var q6 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id ==6 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q6 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 6,
                    Right_ans = model.q6
                });
            }
            else
            {
                if (model.q6 != null)
                    q6.Right_ans = model.q6;
            }
            var q7 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 7 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q7 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 7,
                    Right_ans = model.q7
                });
            }
            else
            {
                if (model.q7 != null)
                    q7.Right_ans = model.q7;
            }

            var q8 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 8 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q8 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 8,
                    Right_ans = model.q8
                });
            }
            else
            {
                if (model.q8 != null)
                    q8.Right_ans = model.q8;
            }
            var q9 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 9 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q9 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 9,
                    Right_ans = model.q9
                });
            }
            else
            {
                if (model.q9 != null)
                    q9.Right_ans = model.q9;
            }

            var q10 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 10 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if (q10 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 10,
                    Right_ans = model.q10
                });
            }
            else
            {
                if (model.q10 != null)
                    q10.Right_ans = model.q10;
            }

            var q11 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 11 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q11 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 11,
                    Right_ans = model.q11
                });
            }
            else
            {
                if (model.q11 != null)
                    q11.Right_ans = model.q11;
            }

            var q12 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 12 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q12 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 12,
                    Right_ans = model.q12
                });
            }
            else
            {
                if (model.q12 != null)
                    q12.Right_ans = model.q12;
            }
            var q13 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 13 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q13 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 13,
                    Right_ans = model.q13
                });
            }
            else
            {
                if (model.q13 != null)
                    q13.Right_ans = model.q13;
            }
            var q14 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 14 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q14 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 14,
                    Right_ans = model.q14
                });
            }
            else
            {
                if (model.q14 != null)
                    q14.Right_ans = model.q14;
            }
            var q15 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 15 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q15 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 15,
                    Right_ans = model.q15
                });
            }
            else
            {
                if (model.q15 != null)
                    q15.Right_ans = model.q15;
            }
            var q16 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 16 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q16 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 16,
                    Right_ans = model.q16
                });
            }
            else
            {
                if (model.q16 != null)
                    q16.Right_ans = model.q16;
            }

            var q17 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 17 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q17 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 17,
                    Right_ans = model.q17
                });
            }
            else
            {
                if (model.q17 != null)
                    q17.Right_ans = model.q17;
            }

            var q18 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 18 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q18 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 18,
                    Right_ans = model.q18
                });
            }
            else
            {
                if (model.q18 != null)
                    q18.Right_ans = model.q18;
            }

            var q19 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 19 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q19 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 19,
                    Right_ans = model.q19
                });
            }
            else
            {
                if (model.q19 != null)
                    q19.Right_ans = model.q19;
            }

            var q20 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 20 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
            if(q20 == null)
            {
                await db.Su_Right_Ans.AddAsync(new Su_right_ans
                {
                    Variant_id = model.v_id,
                    Subject_id = model.PanID,
                    Ques_id = 20,
                    Right_ans = model.q20

                });
            }
            else
            {
                if (model.q20 != null)
                    q20.Right_ans = model.q20;
            }

            if(model.q21 != null)
            {
                var q21 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 21 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q21 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 21,
                        Right_ans = model.q21

                    });
                }
                else
                {
                    q21.Right_ans = model.q21;
                }
             
            }
            if(model.q22 != null)
            {
                var q22 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 22 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q22 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 22,
                        Right_ans = model.q22

                    });
                }
                else
                {
                    q22.Right_ans = model.q22;
                }
              
            }
            if(model.q23 != null)
            {
                var q23 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 23 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q23 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 23,
                        Right_ans = model.q23

                    });
                }
                else
                {
                    q23.Right_ans = model.q23;
                }
            
            }
            if(model.q24 != null)
            {
                var q24 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 24 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q24 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 24,
                        Right_ans = model.q24

                    });
                }
                else
                {
                    q24.Right_ans = model.q24;
                }

            }
            if(model.q25 != null)
            {
                var q25 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 25 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q25 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 25,
                        Right_ans = model.q25

                    });
                }
                else
                {
                    q25.Right_ans = model.q25;
                }
      
            }
            if(model.q26 != null)
            {
                var q26 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 26 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q26 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 26,
                        Right_ans = model.q26

                    });
                }
                else
                {
                    q26.Right_ans = model.q26;
                }
    
            }
            if (model.q27 != null)
            {
                var q27 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 27 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q27 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 27,
                        Right_ans = model.q27

                    });
                }
                else
                {
                    q27.Right_ans = model.q27;
                }
   
            }
            if (model.q28 != null)
            {
                var q28 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 28 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q28 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 28,
                        Right_ans = model.q28

                    });
                }
                else
                {
                    q28.Right_ans = model.q28;
                }

            }

            if (model.q29 != null)
            {
                var q29 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 29 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q29 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 29,
                        Right_ans = model.q29

                    });
                }
                else
                {
                    q29.Right_ans = model.q29;
                }

            }

            if (model.q30 != null)
            {
                var q30 = await db.Su_Right_Ans.FirstOrDefaultAsync(x => x.Ques_id == 30 && x.Subject_id == model.PanID && x.Variant_id == model.v_id);
                if(q30 == null)
                {
                    await db.Su_Right_Ans.AddAsync(new Su_right_ans
                    {
                        Variant_id = model.v_id,
                        Subject_id = model.PanID,
                        Ques_id = 30,
                        Right_ans = model.q30

                    });
                }
                else
                {
                    q30.Right_ans = model.q30;
                }

            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(AddSuQs), new { Id = model.v_id, Name = model.V_name, PanID = model.PanID });
        }

        public async Task<IActionResult> Delsuph(int Id,int PanID,string Name,int Path)
        {
            var model = await db.Su_Ques_s.FirstOrDefaultAsync(x => x.Subject_id == PanID & x.Variant_id == Id);

            if(Path == 1)
            {
               
                string path_Root = _appEnvironment.WebRootPath;
                string path_to_Image = path_Root + "\\SuTESt_photos\\" + model.Path_1;
                model.Path_1 = null;
                await db.SaveChangesAsync();
                try
                {
                    System.IO.File.Delete(path_to_Image);
                }
                catch
                {
                    return NotFound();
                }
            }
            if (Path == 2)
            {

                string path_Root = _appEnvironment.WebRootPath;
                string path_to_Image = path_Root + "\\SuTESt_photos\\" + model.Path_2;
                model.Path_2 = null;
                await db.SaveChangesAsync();
                try
                {
                    System.IO.File.Delete(path_to_Image);
                }
                catch
                {
                    return NotFound();
                }
            }
            if (Path == 3)
            {

                string path_Root = _appEnvironment.WebRootPath;
                string path_to_Image = path_Root + "\\SuTESt_photos\\" + model.Path_3;
                model.Path_3 = null;
                await db.SaveChangesAsync();
                try
                {
                    System.IO.File.Delete(path_to_Image);
                }
                catch
                {
                    return NotFound();
                }
            }
            if (Path == 4)
            {

                string path_Root = _appEnvironment.WebRootPath;
                string path_to_Image = path_Root + "\\SuTESt_photos\\" + model.Path_4;
                model.Path_4 = null;
                await db.SaveChangesAsync();
                try
                {
                    System.IO.File.Delete(path_to_Image);
                }
                catch
                {
                    return NotFound();
                }
            }
            if (Path == 5)
            {

                string path_Root = _appEnvironment.WebRootPath;
                string path_to_Image = path_Root + "\\SuTESt_photos\\" + model.Path_5;
                model.Path_5 = null;
                await db.SaveChangesAsync();
                try
                {
                    System.IO.File.Delete(path_to_Image);
                }
                catch
                {
                    return NotFound();
                }
            }
            if (Path == 6)
            {

                string path_Root = _appEnvironment.WebRootPath;
                string path_to_Image = path_Root + "\\SuTESt_photos\\" + model.Path_6;
                model.Path_6 = null;
                await db.SaveChangesAsync();
                try
                {
                    System.IO.File.Delete(path_to_Image);
                }
                catch
                {
                    return NotFound();
                }
            }
            if (Path == 7)
            {

                string path_Root = _appEnvironment.WebRootPath;
                string path_to_Image = path_Root + "\\SuTESt_photos\\" + model.Path_7;
                model.Path_7 = null;
                await db.SaveChangesAsync();
                try
                {
                    System.IO.File.Delete(path_to_Image);
                }
                catch
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(AddSuQs), new { Id = Id, Name = Name, PanID = PanID });
        }

        public async Task<ActionResult> Changevar(int Id)
        {
            var model = await db.Subloks.ToListAsync();
            var varnt = await db.Suvariants.FirstOrDefaultAsync(x => x.Id == Id);
            ViewBag.vr = varnt.Name;
            ViewBag.vId = varnt.Id;
            var blok = await db.Subloks.FirstOrDefaultAsync(x => x.Id == varnt.Blok_id);
            ViewBag.bl = blok.Name;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Changevar(SuTestViewModel suTestView)
        {
            var model = await db.Suvariants.FirstOrDefaultAsync(x => x.Id == suTestView.v_id);
            model.Name = suTestView.V_name;
            model.Blok_id = suTestView.Blok;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Sutest));
        }






        public IActionResult NishAdmin()
        {
            var res = db.NishCourses.ToList();

            return View(res);
        }


        public IActionResult InNish(int Id)
        {
            nishviewmodel nvm = new nishviewmodel();

            nvm.NishCourse = db.NishCourses.Where(p => p.Id == Id).FirstOrDefault();
            nvm.NishPays = db.NishPays.Where(p => p.NishCourseId == Id).Include(p=>p.AppUsern).ToList();

            return View(nvm);
        }


        [HttpPost]
        public IActionResult InNish(nishviewmodel model)
        {
            db.NishCourses.Update(model.NishCourse);
            db.SaveChanges();

            return RedirectToAction("NishAdmin","Admin");
        }


        [HttpPost]
        public async Task<IActionResult> NishUser(int id, string userid)
        {
            var curs = db.NishCourses.FirstOrDefault(p => p.Id == id);

            NishPay np = new NishPay { NishCourseId = curs.Id, UserId = userid, OpenDate=DateTime.Now, CloseDate=DateTime.Now.AddDays(30) };
            await db.NishPays.AddAsync(np);
            await db.SaveChangesAsync();

            return RedirectToAction("InNish", "Admin", new { Id = id });
        }


        public IActionResult OpenNish()
        {
            var res = db.NishPays.Include(p=>p.AppUsern).Include(p=>p.NishCourse).ToList();

            List<NishPay> np = new List<NishPay>();

            

            foreach (var d in res)
            {
                    foreach (var t in np)
                    {

                        if (t.UserId != d.UserId)
                        {
                            np.Add(new NishPay { UserId = d.UserId, NishCourseId = d.NishCourseId, CloseDate = d.CloseDate, OpenDate = d.OpenDate, AppUsern = d.AppUsern, NishCourse = d.NishCourse });
                        }
                    }                
            }

            nishviewmodel nvm = new nishviewmodel { NishPays = res, NishPaysIdentity = np };



            return View(nvm);
        }

        [HttpPost]
        public async Task<IActionResult> OpenNish(string number, int Id, DateTime date)
        {
            var res = await _userManager.FindByNameAsync(number);
            if(res == null)
            {
                ViewBag.Error = "такой номера нет";
                return View();
            }

            NishPay nishPay = new NishPay { UserId = res.Id, NishCourseId = Id, CloseDate = DateTime.Now.AddHours(30), OpenDate = date };
            db.NishPays.Add(nishPay);
            await db.SaveChangesAsync();

            return RedirectToAction("NishAdmin", "Admin");

        }


















        public IActionResult QrAdmin()
        {
            var res = db.QrBooks.ToList();

            return View(res);
        }
       
        public IActionResult CreateQrBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateQrBook(string name)
        {
            QrBook qb = new QrBook { BookName=name, DateTime=DateTime.Now, PhotoUrl = "/Pan/math.png" };
            db.QrBooks.Add(qb);
            db.SaveChanges();
            return RedirectToAction("QrAdmin","Admin");
        }

        public async Task<IActionResult> InQrBook(Guid Id)
        {
            ViewBag.BookId = Id;

            var res = await db.QrNuskas.Where(o => o.QrBookId == Id).ToListAsync();

            return View(res);
        }

        public IActionResult AddNuska(Guid Id)
        {
            return View(new QrNuska { QrBookId = Id });
        }

        [HttpPost]
        public IActionResult AddNuska(QrNuska modal)
        {
            db.QrNuskas.Add(modal);
            db.SaveChanges();

            QrBook book = db.QrBooks.FirstOrDefault(p => p.Id == modal.QrBookId);
            AdminWork aw = new AdminWork();
            if (book != null)
            {
                if(book.BookName == "Математикалық Сауаттылық")
                {
                    var tt = aw.AddQrVideo("Мат Сау | ", 20, modal.Id);
                    db.QrVideos.AddRange(tt);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("InQrBook", new { Id = modal.QrBookId });
        }




        public async Task<IActionResult> NuskaVideo(int Id)
        {
            var list = await db.QrVideos.Where(o => o.QrNuskaId == Id).ToListAsync();
            ViewBag.NuskaId = Id;
            ViewBag.BookId = db.QrNuskas.Where(o => o.Id == Id).Select(o => o.QrBookId).FirstOrDefault();
            return View(list);
        }




        public async Task<IActionResult> AddQrVideo(int Id)
        {
            QrVideo qv = new QrVideo { QrNuskaId = Id };

            return View(qv);
        }

        [HttpPost]
        public IActionResult AddQrVideo(QrVideo model)
        {
            QrVideo qv = new QrVideo { Stats = model.Stats, Title = model.Title, VideoUrl = model.VideoUrl, QrCode = model.QrCode, QrNuskaId = model.QrNuskaId };

            db.QrVideos.Add(qv);
            db.SaveChanges();

            return RedirectToAction("NuskaVideo", new { Id = model.QrNuskaId });

        }


        public IActionResult UpdateQrVideo(int Id)
        {
            QrVideo qv = db.QrVideos.FirstOrDefault(p => p.Id == Id);

            if(qv != null)
            {
                return View(qv);
            }

            return RedirectToAction("QrAdmin");
        }


        [HttpPost]
        public IActionResult UpdateQrVideo(QrVideo model)
        {
            QrVideo qv = db.QrVideos.FirstOrDefault(p => p.Id == model.Id);

            qv.QrCode = model.QrCode;
            qv.Stats = model.Stats;
            qv.VideoUrl = model.VideoUrl;
            qv.Title = model.Title;

            db.QrVideos.Update(qv);
            db.SaveChanges();

            return RedirectToAction("NuskaVideo", new { Id = model.QrNuskaId });
        }



        public IActionResult DeleteQr(int Id)
        {
            var d = db.QrVideos.FirstOrDefault(p => p.Id == Id);
            if(d != null)
            {
                db.QrVideos.Remove(d);
                db.SaveChanges();
            }
            return RedirectToAction("InQrBook");
        }





        public async Task<IActionResult> UserPostRegister()
        {
            var users = await _userManager.Users.Where(o => o.regdate >= Convert.ToDateTime("01/09/2020")).OrderBy(o=>o.regdate).ToListAsync();

            //  var list = (from b in users select new PostRegisterView{ Fio = b.FirstName + b.LastName, Id = b.Id, Phone = b.UserName, Time = b.regdate }).ToList();

            List<PostRegisterView> prv = new List<PostRegisterView>();
            foreach(var b in users)
            {
                prv.Add(new PostRegisterView { Fio = b.FirstName + " " + b.LastName, Id = b.Id, Phone = b.UserName, Time = b.regdate });
            }

            return View(prv);

        }

    }


    public class AdminWork
    {
        public List<QrVideo> AddQrVideo(string Pan, int count, int nuskaId)
        {
            List<QrVideo> qrlist = new List<QrVideo>();

            for (int i = 1; i <= count; i++)
            {
                qrlist.Add(new QrVideo { QrNuskaId = nuskaId, Stats = false, Title = Pan + i });
            }

            return qrlist;
        }
    }
}