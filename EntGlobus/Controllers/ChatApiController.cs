using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.ApiServece;
using EntGlobus.ApiServece.Signalr;
using EntGlobus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    [Route("chatapi")]
    [ApiController]
    public class ChatApiController : ControllerBase
    {
        private readonly entDbContext db;
        public ChatApiController(entDbContext _db)
        {
            db = _db;
        }

        [HttpPost]
        [Route("getmess")]
        public async Task<JsonResult> ChatGet([FromBody]LiveChatId modal)
        {
            Guid guid = Guid.Parse(modal.LiveId);
            var res = await db.LiveChats.Where(p => p.PodLiveLessonId == guid).ToListAsync();

            return new JsonResult(res);
        }
        

        [HttpPost]
        [Route("savemess")]
        public async Task<JsonResult> ChatPost([FromBody]SignalrMessageSave smv)
        {
                Guid guid = Guid.Parse(smv.LiveId);
                LiveChat lc = new LiveChat { Number = smv.Number, Message = smv.Message, MessDate = smv.MessDate, PodLiveLessonId = guid, UserName = smv.UserName };
                await db.LiveChats.AddAsync(lc);
                await db.SaveChangesAsync();

            return new JsonResult("Sicces");
        }
    }
}