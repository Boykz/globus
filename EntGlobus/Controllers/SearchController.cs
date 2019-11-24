using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]

    public class SearchController : Controller
    {
        private readonly entDbContext db;
        private TimeSpan raz;
        public SearchController(entDbContext _db)
        {
            db = _db;
        }
        [HttpPost("by")]
        public async Task<IActionResult> By([FromBody] SearchViewModel body)
        {
            bool exits, enable, pay;
            exits = true; enable = false; pay = false;
            int count = 10;
            var searcher =await db.Searches.FirstOrDefaultAsync(x => x.IdentityId == body.Id);

            if (searcher != null)
            {
                exits = true;
                enable = searcher.enable;
                pay = searcher.pay;
                count = searcher.count;
                DateTime dated = new DateTime();
                DateTime today = new DateTime();
                dated = searcher.date;
                today = DateTime.Today;
                raz =  today-dated;
               
            }
            else
            {
                exits = false;
            }
            if (exits)
            {
                if (!enable)
                {
                    if (pay)
                    {
                        var month = new TimeSpan(744, 0, 0);                 
                        if (raz > month)
                        {
                            searcher.pay = false;
                            await db.SaveChangesAsync();
                            return new OkObjectResult(new {  searcher.pay });
                        }
                        return new OkObjectResult(new { searcher.pay,searcher.date });
                    }
                    else
                    {
                        return new ObjectResult(new { searcher.pay });
                    }
                }
                else
                if (count != 0 && count <= 200)
                {
                    //icount = count - 1;

                    searcher.count -= 1;
                    await db.SaveChangesAsync();
                    return new OkObjectResult(new { searcher.enable});
                }
                else
                {
                    searcher.count = 0;
                    searcher.enable = false;
                    await db.SaveChangesAsync();
                    return new OkObjectResult(new { searcher.enable });
                }
            }
            else
            {
                return new ObjectResult(new { result = "not found" });
            }
        }
    }
}