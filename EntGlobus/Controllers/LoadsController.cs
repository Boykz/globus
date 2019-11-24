using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace EntGlobus.Controllers
{
    [Authorize(Roles = "admin")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
    public class LoadsController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly entDbContext db;
        private IMemoryCache cache;
        public LoadsController(IHostingEnvironment appEnvironment, entDbContext _db, IMemoryCache Icache)
        {
            _appEnvironment = appEnvironment;
            db = _db;
            cache = Icache;
        }
        public ActionResult Post()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Post(PostViewModel post)
        {
            if (post.file == null || post.file.Length == 0) return Content("file not found");

            var imgname = DateTime.Now.ToString("MMddHHmmss")+ post.file.FileName;
            string path_Root = _appEnvironment.WebRootPath;
            
            string path_to_Images = path_Root + "\\Postimages\\" +  imgname;
            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {
                await post.file.CopyToAsync(stream);
            }
            await db.Posts.AddAsync(new Post { subject = post.subject,text = post.text,date = DateTime.Now,pathimg = imgname,hashtag = post.hashtag,pathvideo = post.pathvideo  });
            await db.SaveChangesAsync();
            var posts = await db.Posts.OrderByDescending(x => x.Id).ToListAsync();
          //  cache.Set("posts", posts, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24)));
            return RedirectToAction(nameof(List));
        }
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<ActionResult> List()
        {
     

          
               var  posts = await db.Posts.OrderByDescending(x => x.Id).ToListAsync();
            
          
            return View(posts);
        }
        public async Task<ActionResult> Edit(int Id)
        {
      
            var post = await db.Posts.FirstOrDefaultAsync(x=>x.Id == Id);
            
            return View(post);
        }

        public async Task<ActionResult> Delimg(int Id)
        {
            var posts = await db.Posts.FirstOrDefaultAsync(i => i.Id == Id);

            string path_Root = _appEnvironment.WebRootPath;
            string path_to_Image = path_Root + posts.pathimg;
       
            try
            {
                System.IO.File.Delete(path_to_Image);
            }
            catch
            {
                return NotFound();
            }
            posts.pathimg = null;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { Id = Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delimg(PostViewModel post)
        {
            var pst = await db.Posts.FirstOrDefaultAsync(x => x.Id == post.Id);
            if (post.file == null || post.file.Length == 0)
            {
                pst.subject = post.subject;
                pst.text = post.text;
                pst.date = DateTime.Now;
            
                pst.hashtag = post.hashtag;
                pst.pathvideo = post.pathvideo;

                await db.SaveChangesAsync();


                return RedirectToAction(nameof(List));
            }
            else
            {
                var imgname = DateTime.Now.ToString("MMddHHmmss") + post.file.FileName;
                string path_Root = _appEnvironment.WebRootPath;

                string path_to_Images = path_Root + "\\Postimages\\" + imgname;
                using (var stream = new FileStream(path_to_Images, FileMode.Create))
                {
                    await post.file.CopyToAsync(stream);
                }


                pst.subject = post.subject;
                pst.text = post.text;
                pst.date = DateTime.Now;
                pst.pathimg = imgname;
                pst.hashtag = post.hashtag;
                pst.pathvideo = post.pathvideo;

                await db.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

        }





        public async Task<ActionResult> Newdel(int? id)
        {
            var posts = await db.Posts.FirstOrDefaultAsync(i=>i.Id == id);
            db.Posts.Remove(posts);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}