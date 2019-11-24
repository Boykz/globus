using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    
    public class AcountController : Controller
    {
        private readonly SignInManager<AppUsern> signInManager;
        private readonly UserManager<AppUsern> userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AcountController(RoleManager<IdentityRole> _role, SignInManager<AppUsern> _signInManager,UserManager<AppUsern> _userManager)
        {
            signInManager = _signInManager;
                userManager = _userManager;
            _roleManager = _role;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return PartialView(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
           

                if (ModelState.IsValid)
                {
                var result =
                    await signInManager.PasswordSignInAsync(model.TelNum, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Acount");
                    }
                }
                else
                    {
                        ModelState.AddModelError("", "Неправильный номер и (или) пароль");
                    }
                } else
                    {
                        ModelState.AddModelError("", "Неправильный номер и (или) пароль");
                    }

            
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logoff()
        {
            // удаляем аутентификационные куки
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Acount");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public async  Task<IActionResult> AccessDenied()
        {
            ViewBag.Denied = "Error connection";
            await signInManager.SignOutAsync();
            return PartialView();
        }

    }
}