using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUsern> userManager;
        private readonly SignInManager<AppUsern> signInManager;
        private readonly entDbContext db;
        public UserController(SignInManager<AppUsern> _signInManager,  UserManager<AppUsern> _userManager,entDbContext _db)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }        

        public async Task<IActionResult> Personal()
        {        
            IEnumerable<Ofpay> userp = await db.Ofpays.Where(x=>x.IdentityId== userManager.GetUserId(User)).ToListAsync() ;
            return View(userp);
        }

        public async  Task<IActionResult> Spisok()
        {
            string id = userManager.GetUserId(User);
            var viewmodel = await db.Satilims.ToListAsync();
            ViewBag.length =  db.Satilims.Count();
            ViewBag.user_id = id;
            return View(viewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> Qiwiorder( string pan)
        {
            string id = userManager.GetUserName(User);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id == null)
            {
                return Json(BadRequest());
            }
            string date = DateTime.Now.ToString("MMddHHmmss");
            string nnum = id;
            var aaa = nnum.Substring(nnum.Length - 4);
            string acount = aaa + date;
            string prv = date + aaa;
            var oldorder = await db.Qiwipays.Where(x => x.number == id & x.type == pan & x.pay == false).FirstOrDefaultAsync();
            if (oldorder == null)
            {
                await db.Qiwipays.AddAsync(new Qiwipay { account = acount, txn_date = DateTime.Now, sum = 1000, type = pan, number = id, prv_txn = prv, pan = true});
            }
            else
            {
                return RedirectToAction(nameof(Qiwiorder), new { pan = pan, id = id});
            }

            await db.SaveChangesAsync();
            

            return RedirectToAction(nameof(Qiwiorder), new { pan  = pan, id = id});
        }

        public async Task<IActionResult> Qiwiorder(string pan,string id)
        {       
            var oldorder = await db.Qiwipays.Where(x => x.number == id & x.type == pan).FirstOrDefaultAsync();
            return View(oldorder);
        }
       

    }
}