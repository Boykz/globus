using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EntGlobus.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    [Route("api/[controller]")]
    public class OfflineController : Controller
    {

        private readonly entDbContext db;
        private readonly UserManager<AppUsern> userManager;
        public OfflineController(entDbContext _db, UserManager<AppUsern> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpPost("check")]
        public async Task<IActionResult> Check([FromBody] SearchViewModel request)
        {
            var oof = await db.Ofpays.Where(x => x.IdentityId == request.Id).ToListAsync();

            return new OkObjectResult(new { res = oof });
        }

    }
}