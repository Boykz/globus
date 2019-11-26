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
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{   //[Authorize(Roles ="developer")]
    public class DevController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUsern> _userManager;
        private readonly entDbContext db;
        public DevController(RoleManager<IdentityRole> _role, UserManager<AppUsern> _user, entDbContext _db)
        {
            _roleManager = _role;
            _userManager = _user;
            db = _db;
        }
        public ActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UserList(string search)
        {

            IEnumerable<AppUsern> user = from s in db.Usernew where s.regdate < DateTime.Today.AddDays(-7) select s;
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
            return View(user);
        }

        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            AppUsern user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.FirstName,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return PartialView(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            AppUsern user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

        public async Task<IActionResult> Satilim()
        {
            IEnumerable<Satilim> satilim = await db.Satilims.ToListAsync();
            return View(satilim);
        }

        public async Task<IActionResult> AddSatu(SatilimViewModel request)
        {
            if (ModelState.IsValid)
            {
                await db.Satilims.AddAsync(new Satilim { Name = request.Name, Price = request.Price, Type = request.Type });
                await db.SaveChangesAsync();
                return RedirectToAction("Satilim");
            }
            return View();

        }

        public async Task<IActionResult> Satdel(int id)
        {
            var satilim =  await db.Satilims.FirstOrDefaultAsync(x => x.Id == id);
            
            db.Satilims.Remove(satilim);
            await db.SaveChangesAsync();
            return RedirectToAction("Satilim");
        }
        }
    }