using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Areas.Main.Models;
using EntGlobus.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Areas.Main.Controllers
{
    [Area("Main")]
    public class MainController : Controller
    {
        private entDbContext db;

        public MainController(entDbContext _db)
        {
            this.db = _db;
        }


        
        public IActionResult Index()
        {
            MainViewModel mvm = new MainViewModel();

            return View();
        }
    }
}