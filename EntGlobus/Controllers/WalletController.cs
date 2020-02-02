using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.ApiServece;
using EntGlobus.ApiServece.Wallet;
using EntGlobus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly UserManager<AppUsern> _userManager;
        private readonly entDbContext db;
        public WalletController(RoleManager<IdentityRole> _role, UserManager<AppUsern> _user, entDbContext _db)
        {
            _userManager = _user;
            db = _db;
        }

        [Route("Sum")]
        public async Task<JsonResult> SumInfo([FromBody]UserIdModelService model)
        {
            var res = await _userManager.FindByIdAsync(model.Id);
            if(res == null)
            {
                return new JsonResult("Error");
            }
            WalletPrice wp = new WalletPrice { Price = res.WalletPrice };

            return new JsonResult(wp);
        }

        [Route("supplement")]
        public async Task<JsonResult> Supplement([FromBody]AddWallet model)
        {
            var res = await _userManager.FindByIdAsync(model.Id);
            if (res == null)
            {
                return new JsonResult("Error");
            }
            if(model.Price == null)
            {
                return new JsonResult("Error");
            }

            res.WalletPrice = model.Price;
            await _userManager.UpdateAsync(res);

            WalletPrice wp = new WalletPrice { Price = res.WalletPrice };

            return new JsonResult(wp);
        }


        [Route("date")]
        public JsonResult Hter()
        {
            return new JsonResult(DateTime.Now.ToString("4498"+"MMddHHmmss"));
        }
    }
}