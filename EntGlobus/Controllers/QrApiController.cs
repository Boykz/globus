using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.Models.QR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    [Route("api/qr")]
    [ApiController]
    public class QrApiController : ControllerBase
    {
        private entDbContext db;
        private readonly UserManager<AppUsern> userManager;

        public QrApiController(entDbContext _db, UserManager<AppUsern> _userManager)
        {
            db = _db;
            userManager = _userManager;

        }


        [HttpGet("get")]
        public JsonResult Index1()
        {
            return new JsonResult("wert");
        }


        [HttpPost("getpost")]
        public async Task<JsonResult> Index2([FromBody] QrService1 model)
        {
                ResClass rc = new ResClass { };
                int code;
                int.TryParse(model.QrCode, out code);

                var book = await db.QrVideos.Where(p => p.QrCode == code).FirstOrDefaultAsync();

                if (book != null)
                {
                    if (book.Stats == true)
                    {
                        VideoResult vr = new VideoResult { Title = book.Title, VideoUrl = book.VideoUrl, Pay = true };

                        return new JsonResult(vr);
                    }
                    else
                    {

                    #region Error Code
                    //var us = await db.Users.FindAsync(model.UserId);
                    //if (us == null)
                    //{
                    //    rc.Error = "not user";
                    //    return new JsonResult(rc);
                    //}
                    #endregion

                    var stats = await db.QrUserIdentities.Where(p => p.UserId == model.UserId/*&& p.QrBookId == book.QrBookId*/).FirstOrDefaultAsync();

                        if (stats != null)
                        {
                            VideoResult vr = new VideoResult { Title = book.Title, VideoUrl = book.VideoUrl, Pay = true };

                            return new JsonResult(vr);
                        }
                        else
                        {
                            VideoResult vr = new VideoResult { Pay = false };

                            return new JsonResult(vr);
                        }
                    }
                }

            rc.Error = "Бұл QR-код жүйеде тіркелмеген";
            return new JsonResult(rc);
        }


        [HttpGet]
        [Route("getPan")]
        public async Task<JsonResult> GetPan()
        {
            var res = await db.QrBooks.ToListAsync();

            return new JsonResult(res);
        }


        [HttpPost]
        [Route("getNuska")]
        public async Task<JsonResult> GetNuska([FromBody] GetNuskaModal modal)
        {
            var res = await db.QrNuskas.Where(o => o.QrBookId == modal.Id).ToListAsync();

            return new JsonResult(res);
        }


        [HttpPost]
        [Route("getVideo")]
        public async Task<JsonResult> GetVideo([FromBody] GetVideoModal modal)
        {
            var res = await db.QrVideos.Where(o => o.QrNuskaId == modal.Id).ToListAsync();

            return new JsonResult(res);
        }

    }



    public class QrService1
    {
        [Required]
        public string UserId { get; set; }
        
        public string QrCode { get; set; }
    }

    public class VideoResult
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }

        public bool Pay { get; set; }
    }

    public class ResClass
    {
        public string Error { get; set; }
    }


    public class GetNuskaModal
    {
        public Guid Id { get; set; }
    }

    public class GetVideoModal
    {
        public int Id { get; set; }
    }
}
