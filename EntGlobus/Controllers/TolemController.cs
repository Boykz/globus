using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Xml;
using EntGlobus.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EntGlobus.Controllers
{
    [Route("api/[controller]")]
    public class TolemController : Controller
    {
        private readonly entDbContext db;
        private readonly UserManager<AppUsern> userManager;
        public TolemController(entDbContext _db, UserManager<AppUsern> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Tolem(string user_id, string type, int pg_result, string pg_amount, DateTime pg_payment_date, string pg_user_phone, string ptype)
        {
            AppUsern user = await userManager.FindByIdAsync(user_id);
          
          
            if (pg_result == 1)
            {
                if (user == null)
                {
                    return new ObjectResult(new { result = "user not found" });
                }
                if (ptype == "pan")
                {
                    await db.AddAsync(new Ofpay { IdentityId = user_id, type = type,Price = pg_amount.Split(".")[0] });
                    await db.Tolems.AddAsync(new Tolem { IdentityId = user_id, type = type, success = true, price = pg_amount, date = pg_payment_date });
                    await db.SaveChangesAsync();
                    return new OkObjectResult(new { result = type, bl = "pann" });
                }

                if (type == "search")
                {
                    await db.Tolems.AddAsync(new Tolem { IdentityId = user_id, type = type, success = true, price = pg_amount, date = pg_payment_date });

                    var searcher = db.Searches.FirstOrDefault(x => x.IdentityId == user_id);
                    searcher.date = pg_payment_date;
                    searcher.pay = true;
                    searcher.enable = false;
                    searcher.count = 0;
                    await db.SaveChangesAsync();
                    return new OkObjectResult(new { result = type, bl = "sr" });
                }
                else
                {
                    await db.Tolems.AddAsync(new Tolem { IdentityId = user_id, type = type, success = true, price = pg_amount, date = pg_payment_date });

                    await db.AddAsync(new Blok { IdentityId = user_id, blok = type, enable = true, BuyDate = pg_payment_date});
                    await db.SaveChangesAsync();
                    return new OkObjectResult(new { result = type, bl = "blok" });
                }

            }
            return BadRequest();
        }


        [HttpGet("qiwi")]
        public async Task<IActionResult> Qiwi(string command, string txn_id, string account, double sum, string txn_date)
        {            
            try
            {
                var alo = await db.Qiwipays.Where(x => x.account == account).FirstAsync();
            }
            catch
            {
            string nulaccount = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
            <response>
            <osmp_txn_id>{txn_id}</osmp_txn_id>
            <result>5</result>
            <comment></comment>
            </response>";
            return Content(nulaccount, "text/xml");
            }
           
                var qaccount = await db.Qiwipays.Where(x=>x.account==account).FirstAsync();

                if (command == "check")
                {
                    if (qaccount.type == "search")
                    {
                        string ser = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <response>
                        <osmp_txn_id>{txn_id}</osmp_txn_id>
                        <result>0</result>
                        <fields>
                        <field1 name='number'>{qaccount.number}</field1>
                        <field2 name='sum'>{qaccount.sum}</field2>
                        <field3 name='type'>Поиск</field3>
                        </fields>
                        <comment></comment>
                        </response>";
                        return Content(ser, "text/xml");
                    }
                else if(qaccount.type == "all")
                {
                    string xmlString1 = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <response>
                        <osmp_txn_id>{txn_id}</osmp_txn_id>
                        <result>0</result>
                        <fields>
                        <field1 name='number'>{qaccount.number}</field1>
                        <field2 name='sum'>{qaccount.sum}</field2>
                        <field3 name='type'>Подписка блоков</field3>
                        </fields>
                        <comment></comment>
                        </response>";
                    return Content(xmlString1, "text/xml");
                }
                    else
                    {
                        string xmlString1 = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <response>
                        <osmp_txn_id>{txn_id}</osmp_txn_id>
                        <result>0</result>
                        <fields>
                        <field1 name='number'>{qaccount.number}</field1>
                        <field2 name='sum'>{qaccount.sum}</field2>
                        <field3 name='type'>{qaccount.type} ЕСКЕРТУ! НАҒЫЗ ҰБТ-ДА ЖАУАПТАРДЫҢ ШЫҒУЫНА ГАРАНТИЯ ЖОҚ! ПРОБНЫЙДЫҢ СҰРАҚТАРЫ ҚОСЫЛҒАН. ҰБТ-ДА ҚОЛДАНУҒА БОЛМАЙДЫ!</field3>
                        </fields>
                        <comment>ЕСКЕРТУ! НАҒЫЗ ҰБТ-ДА ЖАУАПТАРДЫҢ ШЫҒУЫНА ГАРАНТИЯ ЖОҚ! ПРОБНЫЙДЫҢ СҰРАҚТАРЫ ҚОСЫЛҒАН. ҰБТ-ДА ҚОЛДАНУҒА БОЛМАЙДЫ!</comment>
                        </response>";
                        return Content(xmlString1, "text/xml");
                    }
                } 
            bool pan = true;
            if (command == "pay")
                {
                    if (sum < qaccount.sum)
                    {
                    string menwe = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <response>
                    <osmp_txn_id>{txn_id}</osmp_txn_id>
                    <result>241</result>
                    <comment></comment>
                    </response>";
                        return Content(menwe, "text/xml");
                    }
                    else if (sum > qaccount.sum)
                    {
                    string moree = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <response>
                    <osmp_txn_id>{txn_id}</osmp_txn_id>
                    <result>242</result>
                    <comment></comment>
                    </response>";
                        return Content(moree, "text/xml");
                    }
                string formatString = "yyyyMMddHHmmss";
                DateTime dt = DateTime.ParseExact(txn_date, formatString, null);
                AppUsern user = await userManager.FindByNameAsync(qaccount.number);
                
                if (qaccount.pan & pan)
                {
                    var pays = await db.Ofpays.Where(x=>x.IdentityId == user.Id & x.type == qaccount.type).ToListAsync();
                    if (pays.Count == 0)
                    {
                        await db.Ofpays.AddAsync(new Ofpay { IdentityId = user.Id, type = qaccount.type, Price = String.Format("{0:0.##}", sum) });
                        qaccount.txn_date = dt;
                        qaccount.txn_id = txn_id;
                        qaccount.pay = true;
                        qaccount.sum = sum;
                        pan = false;
                        await db.SaveChangesAsync();
                    }

                    string xmlString1 = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <response>
                    <osmp_txn_id>{txn_id}</osmp_txn_id>
                    <prv_txn>{qaccount.prv_txn}</prv_txn>
                    <sum>{qaccount.sum}</sum>
                    <result>0</result>
                    <comment>СҰРАҚТАРДЫҢ БАРЛЫҒЫ ШЫҒУЫНА ГАРАНТИЯ ЖОҚ! ТЕК ҚАНА ПРОБНЫЙ СҰРАҚТАРЫ!</comment>
                    </response>";
                    return Content(xmlString1, "text/xml"); 
                }
               
                    if (qaccount.type == "search")
                    {
                        var searcher = await db.Searches.FirstOrDefaultAsync(x => x.IdentityId == user.Id);
                        searcher.date = dt;
                        searcher.count = 0;
                         searcher.enable = false;
                        searcher.pay = true;
                        qaccount.txn_date = dt;
                        qaccount.txn_id = txn_id;
                        qaccount.pay = true;
                        qaccount.sum = sum;
                        await db.SaveChangesAsync();
                        string sre = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <response>
                    <osmp_txn_id>{txn_id}</osmp_txn_id>
                    <prv_txn>{qaccount.prv_txn}</prv_txn>
                    <sum>{qaccount.sum}</sum>
                    <result>0</result>
                    <comment>search</comment>
                    </response>";
                        return Content(sre, "text/xml");
                    }
                    if(qaccount.type != "all" && qaccount.type != "search" && !qaccount.pan)
                    {
                        var phpay = await db.Phtest_Pays.Where(x => x.Number == qaccount.number && x.Type == qaccount.type).ToListAsync();
                        if(phpay.Count == 0)
                        {
                            await db.Phtest_Pays.AddAsync(new Phtest_pay
                            {
                                Number = qaccount.number,
                                Price = qaccount.sum,
                                dateTime = dt,
                                Type = qaccount.type

                            });
                            qaccount.txn_date = dt;
                            qaccount.txn_id = txn_id;
                            qaccount.pay = true;
                            qaccount.sum = sum;
                            await db.SaveChangesAsync();
                        }

                    string phay = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <response>
                        <osmp_txn_id>{txn_id}</osmp_txn_id>
                        <prv_txn>{qaccount.prv_txn}</prv_txn>
                        <sum>{qaccount.sum}</sum>
                        <result>0</result>
                        <comment>{qaccount.type} ЕСКЕРТУ! НАҒЫЗ ҰБТ-ДА ЖАУАПТАРДЫҢ ШЫҒУЫНА ГАРАНТИЯ ЖОҚ! ПРОБНЫЙДЫҢ СҰРАҚТАРЫ ҚОСЫЛҒАН. ҰБТ-ДА ҚОЛДАНУҒА БОЛМАЙДЫ!</comment>
                        </response>";
                    return Content(phay, "text/xml");
                }
                
                    if(qaccount.type == "all" )
                    {
                    var blokss = await db.Bloks.Where(x => x.IdentityId == user.Id & x.blok == qaccount.type & x.enable == true).ToListAsync();
                    var hasblok = await db.Bloks.Where(x => x.IdentityId == user.Id & x.blok == qaccount.type & x.enable == false).ToListAsync();

                    if (blokss.Count == 0 && hasblok.Count == 0)
                    {
                        await db.Bloks.AddAsync(new Blok { IdentityId = user.Id, blok = qaccount.type, enable = true, BuyDate = dt });
                        qaccount.txn_date = dt;
                        qaccount.txn_id = txn_id;
                        qaccount.pay = true;
                        qaccount.sum = sum;
                        await db.SaveChangesAsync();
                    }
                    if (blokss.Count == 0 && hasblok.Count == 1)
                    {
                        var blok = await db.Bloks.FirstOrDefaultAsync(x=>x.IdentityId == user.Id & x.blok == qaccount.type);
                        blok.enable = true;
                        blok.BuyDate = dt;
                        
                        qaccount.txn_date = dt;
                        qaccount.txn_id = txn_id;
                        qaccount.pay = true;
                        qaccount.sum = sum;
                        await db.SaveChangesAsync();
                    }



                    string ble = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <response>
                        <osmp_txn_id>{txn_id}</osmp_txn_id>
                        <prv_txn>{qaccount.prv_txn}</prv_txn>
                        <sum>{qaccount.sum}</sum>
                        <result>0</result>
                        <comment>blok</comment>
                        </response>";
                        return Content(ble, "text/xml"); 
                    } 
            } 
            string err = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
            <response>
            <osmp_txn_id>{txn_id}</osmp_txn_id>
            <result>8</result>
            <comment></comment>
            </response>";
            return Content(err, "text/xml");
        }
        [HttpPost("orderqiwi2")]
        public async Task<IActionResult> Orderqiwi([FromBody] OrderQiwiViewModel body)
        {
            if(!ModelState.IsValid)
            { 
                return BadRequest();
            }
           
            AppUsern user = await userManager.FindByNameAsync(body.num);
            if(user == null)
            {
                return Json(BadRequest());
            }
            string date = DateTime.Now.ToString("MMddHHmmss");
            string nnum = user.UserName;
            var aaa=  nnum.Substring(nnum.Length - 4);
            string acount = aaa + date;
            string prv = date + aaa;
            var oldorder = await db.Qiwipays.Where(x => x.number == body.num & x.type == body.type & x.pay == false).FirstOrDefaultAsync();
            if (oldorder == null)
            {
                await db.Qiwipays.AddAsync(new Qiwipay { account = acount, txn_date = DateTime.Now, sum = body.price, type = body.type, number = body.num, prv_txn = prv, pan = body.pan });
            }
            else
            {
                return Json("already");
            }                          
            
            await db.SaveChangesAsync();
            return new  OkObjectResult(new {acount});
        }

        [HttpPost("checkqiwi")]
        public IActionResult CheckQiwi([FromBody] CheckViewModel body)
        {
            //var model =  await db.Qiwipays.Where(x => x.number == body.num).ToListAsync();

            return Json(null);
        }

        [HttpPost("checkphpay")]
        public async Task<IActionResult> Checkphpay([FromBody] CheckViewModel body)
        {
            var model = await db.Phtest_Pays.FirstOrDefaultAsync(x=>x.Number == body.num && x.Type == body.type);
           if(model == null)
            {
                return Json("pay");
            }
            return Json("sucess");
        }

    }
}