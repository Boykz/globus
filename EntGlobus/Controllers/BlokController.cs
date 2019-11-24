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

    [Route("api/[controller]")]
    public class BlokController : Controller
    {
        private readonly entDbContext db;
        private TimeSpan raz;
        public BlokController(entDbContext _db)
        {
            db = _db;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("list")]
        public async Task<IActionResult> List([FromBody] SearchViewModel body)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResult(new { result = "not found" });
            }

        
            var bbb = await db.Bloks.Where(x=>x.IdentityId==body.Id).ToListAsync();
            var sellbloks = await db.SellBloks.ToListAsync();

            var BuyedBloks = await db.Bloks.FirstOrDefaultAsync(x => x.IdentityId == body.Id && x.blok == "all");

            if(BuyedBloks != null)
            {
                DateTime dated = new DateTime();
                DateTime today = new DateTime();
                dated = BuyedBloks.BuyDate;
                today = DateTime.Today;
                raz = today - dated;

                var month = new TimeSpan(744, 0, 0);
                if (raz > month)
                {
                    BuyedBloks.enable = false;
                    await db.SaveChangesAsync();
                }

            }



            for (int i = 0; i < bbb.Count; i++)
            {
                for (int j = 0; j < sellbloks.Count; j++)
                {
                    if (sellbloks[j].Name == bbb[i].blok && bbb[i].enable == true)
                    {
                        sellbloks[j].Enable = true;
                    }
                }
            }

            if (bbb != null)
            {
                return new ObjectResult(new { result = sellbloks });

            }
            return new ObjectResult(new { result = "not found" });
        }

    }
}