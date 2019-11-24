using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    public class AddController : Controller
    {
        private readonly entDbContext db;
        public AddController(entDbContext _db)
        {
            db = _db;
        }

        [Authorize(Roles = "admin")]
        public  IActionResult Audio()
        {
             var users =  db.Usernew;
            return Json(users);
        }
    }
}