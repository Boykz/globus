using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.ApiServece;
using EntGlobus.Helpers;
using EntGlobus.Models;
using EntGlobus.Models.AnaliticaDbFolder;
using EntGlobus.ViewModels.AnaliticsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    public class AnaliticController : Controller
    {

        private readonly entDbContext db;
        private readonly UserManager<AppUsern> um;

        public AnaliticController(entDbContext _db, UserManager<AppUsern> _um)
        {
            this.db = _db;
            um = _um;
        }
        public async Task<IActionResult> Index()
        {
            var list = db.Qiwipays.ToList();

            foreach(var d in list)
            {
                var user = await um.FindByNameAsync(d.number);
                d.UserId = user.Id;

                db.Qiwipays.Update(d);
                await db.SaveChangesAsync();
            }


            return View();
        }

        public IActionResult PayList()
        {
            return View();
        }

        public async Task<IActionResult> DoPay()
        {
            DoPayViewModel dpvm = new DoPayViewModel();

            dpvm.DoPayQiwis = await (from b in db.Qiwipays
                               where b.pay == false
                               select new DoPayQiwi
                               {
                                   Number = b.number,
                                   pay = b.pay,
                                   DateTime = b.txn_date,
                                   Pan = b.type,
                                   Fio = b.User.FirstName + " " + b.User.LastName,
                               }).OrderBy(p=>p.DateTime).ToListAsync();


            return View(dpvm);
        }


        public IActionResult DoPayAnalitic()
        {
            var res = (from b in db.QiwiAnalitics
                               select b).ToList();


            return View(res);
        }

        public IActionResult DeleteDoPayAnalitic(int Id)
        {
            var res = db.QiwiAnalitics.FirstOrDefault(p => p.Id == Id);

            db.QiwiAnalitics.Remove(res);
            db.SaveChanges();


            return RedirectToAction("DoPayAnalitic");
        }


        public IActionResult EditDoPayAnalitic(int Id, string result, bool call)
        {
            var res = db.QiwiAnalitics.FirstOrDefault(p => p.Id == Id);

            res.Call = call;
            res.Result = result;

            db.QiwiAnalitics.Update(res);
            db.SaveChanges();


            return RedirectToAction("DoPayAnalitic");
        }



        public async Task<IActionResult> PayDate()
        {
            ViewBag.Qiwi = await db.Qiwipays.ToListAsync();

            ViewBag.Paybox = await db.Tolems.ToListAsync();


            return View();
        }

        public IActionResult PayLiveTestList()
        {
            ViewBag.Pay = db.PayLiveTests.ToList();

            return View();
        }

        public IActionResult LiveTestVisitorView()
        {
            var res = db.LiveTestVisitor.ToList();
            return new JsonResult(res);
        }


        public async Task<IActionResult> TandauPanStatic()
        {
            var val = await (from b in db.Users
                       where b.pan1 != null
                       select new UserPanView
                       {
                           Number = b.UserName,
                           Pan1 = b.pan1,
                           Pan2 = b.pan2
                       }).ToListAsync();



            return  View(val);
        }

        public async Task<IActionResult> OnlineUser()
        {
            //var users = await um.Users.Where(p => p.pan1 != null).ToListAsync();

            //List<ForUserCallAnalitics> list = new List<ForUserCallAnalitics>();

            //foreach (var b in users)
            //{
            //    list.Add(new ForUserCallAnalitics { UserId = b.Id, Number = b.UserName, UserName = b.FirstName + " " + b.LastName });
            //}

            //await db.ForUserCallAnalitics.AddRangeAsync(list);
            //await db.SaveChangesAsync();


            //var res = await db.ForUserCallAnalitics.ToListAsync();

            //ViewBag.Count = res.Count();

            return View();
        }


        public async Task<ActionResult> CallUser(int? page)
        {
            var user = from s in db.ForUserCallAnalitics select s;
            ViewBag.count = user.Count();


            int pageSize = 20;

            var md = await PaginatedLists<ForUserCallAnalitics>.CreateAsync(user.AsNoTracking(), page ?? 1, pageSize);
            return View(md);
        }


        public IActionResult RemoveUserCall(string UserId)
        {
            var user = db.ForUserCallAnalitics.FirstOrDefault(p => p.UserId == UserId);
            if(user == null)
            {
                return RedirectToAction("CallUser");
            }
            return View(user);
        }


        [HttpPost]
        public IActionResult RemoveUserCall(ForUserCallAnalitics model, int result)
        {
            var user = db.ForUserCallAnalitics.FirstOrDefault(p => p.Id == model.Id);
            if (user == null)
            {
                return RedirectToAction("CallUser");
            }

            user.Call = model.Call;
            user.Comment = model.Comment;
            if(result == 1)
            {
                user.Result = CallType.Бізде_Оқып_жатыр;
            }
            else if (result == 2)
            {
                user.Result = CallType.Баска_курста_окиды;
            }
            else if (result == 3)
            {
                user.Result = CallType.Тагы_хабарласып_коремын;
            }

            db.ForUserCallAnalitics.Update(user);
            db.SaveChanges();

            return RedirectToAction("CallUser");
        }







        public async Task<IActionResult> AllWhatsappMess()
        {
            var users = await (from b in um.Users
                         select new AllWhatsappList {
                             Name = b.FirstName + " " + b.LastName,
                             Phone = b.UserName,
                         }).ToListAsync();

            return View(users);
        }


        public async Task<IActionResult> RegData()
        {
            return View();
        }

    }


    public class UserPanView
    {
        public string Number { get; set; }

        public string Pan1 { get; set; }
        public string Pan2 { get; set; }
    }

    public class AllWhatsappList
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}