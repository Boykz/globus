using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.ApiServece;
using EntGlobus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    [Route("Live")]
    public class LiveLessonApiController : Controller
    {
        private readonly entDbContext db;
        public LiveLessonApiController(entDbContext _db)
        {
            db = _db;
        }

        [HttpGet("List")]
        public async Task<JsonResult> LiveList()
        {
            var res = await db.liveLessons.ToListAsync();
            return new JsonResult(res);
        }


        [HttpPost("post")]
        public async Task<JsonResult> LivePost([FromBody]LiveIdModel lim)
        {
            PodLiveWithHistoryModel plwhm = new PodLiveWithHistoryModel();

            var res = await db.PodLiveLessons.ToListAsync();
            
            plwhm.Live = (from d in res
                          where d.LiveLessonId == lim.Id
                          where d.Status == true
                          select new ModelPodLiveWithHistoryModel
                          {
                              Id = d.Id,
                              LiveLessonId = lim.Id,
                              StartDate = d.StartDate,
                              Status = d.Status,
                              TypeVideo = d.TypeVideo,
                              UrlPhoto = d.UrlPhoto,
                              UrlVideo = d.UrlVideo,
                              Nuska = d.Nuska,
                          }).FirstOrDefault();

            plwhm.History = (from d in res
                          where d.LiveLessonId == lim.Id
                          where d.Status == false
                          select new ModelPodLiveWithHistoryModel
                          {
                              Id = d.Id,
                              LiveLessonId = lim.Id,
                              StartDate = d.StartDate,
                              Status = d.Status,
                              TypeVideo = d.TypeVideo,
                              UrlPhoto = d.UrlPhoto,
                              UrlVideo = d.UrlVideo,
                              Nuska = d.Nuska,
                          }).ToList();

            return new JsonResult(plwhm);
        }


        [HttpGet("Geto")]
        public JsonResult Geto()
        {
            return new JsonResult("hello...");
        }

        [HttpPost("Posto")]
        public JsonResult Posto()
        {
            return new JsonResult("hello...");
        }

    }



}