using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    public class ChatController : Controller
    {

        public IActionResult All()
        {
            return View();
        }


        public IActionResult ApiDoc()
        {
            return View();
        }
    }
}