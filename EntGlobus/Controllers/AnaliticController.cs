using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.ApiServece;
using EntGlobus.Models;
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

            dpvm.DoPayQiwis = (from b in db.Qiwipays
                               select new DoPayQiwi
                               {
                                   Number = b.number,
                                   pay = b.pay,
                                   DateTime = b.txn_date,
                                   Pan = b.type,
                                   Fio = b.User.FirstName + " " + b.User.LastName,
                               }).ToList();


            return View(dpvm);
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
    }
}