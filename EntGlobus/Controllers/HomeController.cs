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

namespace EntGlobus.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> role;
        private readonly UserManager<AppUsern> muser;
        private readonly entDbContext db;
        public HomeController(RoleManager<IdentityRole> _role, UserManager<AppUsern> _user, entDbContext _db)
        {
            role = _role;
            muser = _user;
            db = _db;
        }
        //[Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            var users = db.Usernew.Count();
            ViewBag.users = users;
            var regdate = db.Usernew.Select(x => x.regdate.ToString()).Distinct().ToList();
            var usr = from x in db.Usernew select x ;
            int i = 0;
            foreach(var a in usr)
            {
                if (a.regdate.Equals(regdate))
                {
                    i++;                  
                }
            }
            return View(regdate);
        }
        [Authorize(Roles = "admin")]
        public IActionResult About()
        {

            return View();
        }

        //[Authorize(Roles = "admin")]
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
    }
}
