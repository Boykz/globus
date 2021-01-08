using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EntGlobus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EntGlobus.ViewModels;
using EntGlobus.ApiServece;
using EntGlobus.ViewModels.HomeView;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> role;
        private readonly UserManager<AppUsern> muser;
        private readonly SignInManager<AppUsern> signInManager;
        private readonly entDbContext db;
        public HomeController(RoleManager<IdentityRole> _role, UserManager<AppUsern> _user, entDbContext _db, SignInManager<AppUsern> _signInManager)
        {
            role = _role;
            muser = _user;
            db = _db;
            signInManager = _signInManager;
        }



        public IActionResult Index()
        {
            return View();
        }


        public IActionResult NewLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewLogin(LoginApiModel model)
        {
            var sign = await signInManager.PasswordSignInAsync(model.Number, model.Password, false, false);

            if (sign.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public IActionResult NewRegister(string result)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewRegister(LoginApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var ext = await muser.FindByNameAsync(model.Number);

            if (ext != null)
            {
                ViewBag.Result = "Бұл номер тіркелген";
                return View(model);
            }
            AppUsern appUsern = new AppUsern { UserName = model.Number, FirstName = model.Name };

            var result = await muser.CreateAsync(appUsern, model.Password);

            await signInManager.SignInAsync(appUsern, false);


            await muser.AddToRoleAsync(appUsern, "user");
            await db.SaveChangesAsync();

            return RedirectToAction("Edu", "Home");
        }
















        public async Task<IActionResult> Role(string id)
        {
           AppUsern user = await muser.FindByIdAsync(id);
            //await muser.AddToRoleAsync(user, "Admin");
            var userRoles = await muser.GetRolesAsync(user);
           
            //  var addedRoles = roles.Except(userRoles);
            var allRoles = role.Roles.ToList();
            //ChangeRoleViewModel model = new ChangeRoleViewModel
            //{
            //    UserId = user.Id,
            //    UserEmail = user.UserName,
            //    UserRoles = userRoles,
            //    AllRoles = allRoles
            //};

            //if (user != null)
            //{
            //   var addedrolee = role.RoleExistsAsync("adminstration");
            //    await muser.AddToRolesAsync(user, userRoles);
            //}

           // IdentityResult result = await role.CreateAsync(new IdentityRole(id));


            return Json(userRoles);
        }




        public IActionResult Edu()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<JsonResult> Login([FromBody]LoginApiModel model)
        {
            var sign = await signInManager.PasswordSignInAsync(model.Number, model.Password, false, false);
            if (sign.Succeeded)
            {
                return new JsonResult("true");
            }
            return new JsonResult("false");
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(LoginApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var ext = await muser.FindByNameAsync(model.Number);

            if (ext != null)
            {
                return RedirectToAction("NewRegister", "home", new { result = "Бұл номер тіркелген" });
            }
            AppUsern appUsern = new AppUsern { UserName = model.Number, FirstName = model.Name };

            var result = await muser.CreateAsync(appUsern, model.Password);

            await signInManager.SignInAsync(appUsern, false);


            await muser.AddToRoleAsync(appUsern, "user");
            await db.SaveChangesAsync();

            return RedirectToAction("LiveTest/Index","LiveTest");
        }


        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPage(LoginApiModel model)
        {
            var sign = await signInManager.PasswordSignInAsync(model.Number, model.Password, false, false);

            if (sign.Succeeded)
            {
                return RedirectToAction("LiveTest/Index", "LiveTest");
            }
            return View();
        }
             

        public async Task<IActionResult> Live()
        {
            if(User.Identity.Name != null)
            {
                return RedirectToAction("LiveTest/Index", "LiveTest");
            }
            return RedirectToAction("LoginPage", "Home");
        }


        //    EntGlobus QR - бетінде ашылатын страница. Не удалить!!!
        public IActionResult QrView()
        {
            return View();
        }

    }


}
