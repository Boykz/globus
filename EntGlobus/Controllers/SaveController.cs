using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    public class SaveController : Controller
    {

        private readonly IHostingEnvironment _appEnvironment;
        private readonly entDbContext db;
        public SaveController(IHostingEnvironment appEnvironment, entDbContext _db)
        {
            _appEnvironment = appEnvironment;
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Video()
        {
            return View();
        }

        public async Task<IActionResult> Video(IFormFile video)
        {
            var imgname = video.FileName;
            string path_Root = _appEnvironment.WebRootPath;

            string path_to_Images = path_Root + "\\video\\" + imgname;
            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {
                await video.CopyToAsync(stream);
            }

            return View();
        }
    }
}