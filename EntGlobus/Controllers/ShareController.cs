using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EntGlobus.Controllers
{
    [Route("api/[controller]")]
    public class ShareController : Controller
    {
        public class Question{
            public int Id { get; set; }
            public string Number { get; set; }
            public string Subject { get; set; }
            public string Quest { get; set; }
            public IFormFile file { get; set; }
         
        }
        public class Pagee
        {
            public int PageSize { get; set; }
        }
        private readonly entDbContext db;
      //  private readonly UserManager<AppUser> userManager;
        //private readonly SignInManager<AppUser> signInManager;
        private readonly IHostingEnvironment _appEnvironment; private IMemoryCache cache;
        public ShareController(entDbContext _db,IHostingEnvironment hostingEnvironment,IMemoryCache memoryCache)
        {
            db = _db;
            _appEnvironment = hostingEnvironment;
            cache = memoryCache;
        }
        [HttpGet("todayqs")]
        public async Task<IActionResult> Todayqs()
        {
            var ques = await db.Dayliquestions.ToListAsync();

            return Json(ques);
        }

        [HttpGet("news")]
        public async Task<IActionResult> News([FromBody] Pagee page)
        {
               var news = await db.Posts.OrderByDescending(x => x.Id).ToListAsync();
                return Json(news);
           
        }

        [HttpPost("newnews")]
        public async Task<IActionResult> Newnews([FromBody] Pagee page)
        {
            List<Post> posts = null;

            //if (!cache.TryGetValue("news", out posts))
            //{               
                 posts = await db.Posts.OrderByDescending(x => x.Id).Take((page.PageSize) * 10).Skip((page.PageSize - 1) * 10).ToListAsync();
            //    cache.Set("news", posts, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12)));
            //}

          
            return Json(posts);

        }

        [HttpPost("watched")]
        public async Task<IActionResult> Watched([FromBody] WatchViewModel watch)
        {
            var news = await db.Posts.FirstOrDefaultAsync(x => x.Id == watch.Id);
            news.watch ++;
            await db.SaveChangesAsync();

            return Json(Ok());
        }

        [HttpPost("offerqs")]
        public async Task<IActionResult> Offerqs([FromForm] Question userData)
        {
            if (userData.file == null || userData.file.Length == 0) return Json(BadRequest());

            var imgname = DateTime.Now.ToString("MMddHHmmss") + userData.file.FileName;
            string path_Root = _appEnvironment.WebRootPath;

            string path_to_Images = path_Root + "\\Postimages\\" + imgname;


            if (path_to_Images != null)
            {
                using (var stream = new FileStream(path_to_Images, FileMode.Create))
                {
                    await userData.file.CopyToAsync(stream);     
                     
                }
                await db.Newqs.AddAsync(new Newqs { Number = userData.Number, Question = userData.Quest, Offerdate = DateTime.Now, Subject = userData.Subject, Uriphoto = "\\Postimages\\" + imgname });
                await db.SaveChangesAsync();
                return Json(Ok());
            }
            else
            {
                var res = new { res = "ERROR" };
                return Json(res);
            }
        }
        [HttpGet("offeredques")]
        public async Task<IActionResult> Offeredques()
        {
            var qses = await db.Newqs.ToListAsync();
            return Json(qses);
        }
        [HttpGet("dailyques")]
        public async Task<IActionResult> Dailyques()
        {
            var model = await db.Dayliquestions.FirstOrDefaultAsync();
            int sum = model.A1 + model.A2 + model.A3 + model.A4 + model.A5;
            float c1 =(float) model.A1/sum * 100;
            float c2 = (float)model.A2 / sum * 100;
            float c3 = (float)model.A3 / sum * 100;
            float c4 = (float)model.A4 / sum * 100;
            float c5 = (float)model.A5 / sum * 100;
            
            return new OkObjectResult( new   { model ,sum ,c1,c2,c3,c4,c5});
        }

        [HttpPost("dailyans")]
        public async Task<IActionResult> Dailyans([FromBody] DailyAnsViewModel getans)
        {
            var qs = await db.Dayliquestions.FirstOrDefaultAsync(x=>x.Id == getans.Id);
            switch (getans.Answer)
            {
                case "ans1":
                    qs.A1++;
                    break;
                case "ans2":
                    qs.A2++;
                    break;
                case "ans3":
                    qs.A3++;
                    break;
                case "ans4":
                    qs.A4++;
                    break;
                case "ans5":
                    qs.A5++;
                    break;
                default:
                    return Json(BadRequest());
            }
            await db.SaveChangesAsync();
            return Json(Ok());
        }

        [HttpGet("kurses")]
        public async Task<IActionResult> Kurses()
        {
            var model = await db.Kurs.ToListAsync();
            return Json(model);
        }
        [HttpPost("kurseacces")]
        public async Task<IActionResult> Kurseacces([FromBody] CheckViewModel port)
        {

            var model = await db.Allowkurs.FirstOrDefaultAsync(x => x.UserPhone == port.num);

        
            
            if(model == null)
            {
                var mmm = await db.Allowkurs.AddAsync(new Allowkurs
                {
                    UserPhone = port.num
                });
                  await db.SaveChangesAsync();

                var nmodel = await db.Allowkurs.FirstOrDefaultAsync(x => x.UserPhone == port.num);

                return Json(nmodel);
            }

            return Json(model);
        }


        [HttpGet("subloks")]
        public async Task<IActionResult> Subloks()
        {
            var model = await db.Subloks.ToListAsync();
            return Json(model);
        }
        [HttpPost("Suvar")]
        public async Task<IActionResult> Suvar([FromBody] ShareSuTEstViewModel mod)
        {
            var model = await db.Suvariants.Where(x => x.Blok_id == mod.Blok_ID).ToListAsync();
            return Json(model);
        }
        [HttpPost("Sutest")]
        public async Task<IActionResult> Sutest([FromBody] ShareSuTEstViewModel mod)
        {
            var model = await db.Su_Ques_s.Where(x => x.Subject_id == mod.Subject_ID & x.Variant_id == mod.Variant_ID).ToListAsync();
            return Json(model);
        }
        [HttpPost("Suans")]
        public async Task<IActionResult> Suans([FromBody] ShareSuTEstViewModel mod)
        {
            var model = await db.Su_Right_Ans.Where(x => x.Variant_id == mod.Variant_ID & x.Subject_id == mod.Subject_ID).OrderByDescending(x=>x.Ques_id).ToListAsync();
            return Json(model);
        }
    }
    
}