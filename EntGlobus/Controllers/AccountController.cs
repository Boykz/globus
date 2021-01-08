using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EntGlobus.ApiServece;
using EntGlobus.Helpers;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace EntGlobus.Controllers
{

    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly entDbContext db;
        private readonly UserManager<AppUsern> userManager;
        private readonly SignInManager<AppUsern> signInManager;
        private readonly IMapper mapper;
        private IMemoryCache cache;

        public AccountController(IMapper _mapper, entDbContext _db, UserManager<AppUsern> _userManager, SignInManager<AppUsern> _signInManager, IMemoryCache memory)
        {
            this.db = _db;
            mapper = _mapper;
            userManager = _userManager;
            signInManager = _signInManager;
            cache = memory;
        }

        public class CheckingMailModel
        {
            public string email { get; set; }
        }
        [HttpPost("checkingmail")]
        public async Task<ActionResult> CheckingMail([FromBody] CheckingMailModel checkingMail)

        {
            Random random = new Random();
            var code = random.Next(1, 999999).ToString("D6");
            string sub = "Жүйеге тіркелу";
            string mess = @"Сәлем </br> Сіздің тіркелу кодыңыз: <b>" + code + "</b> </br> Бұл код 5 минутқа ғана жарамды! </br><b>Бізбен бірге 140 балл!</b>";
            Mailing mailing = new Mailing();
            cache.Set(checkingMail.email, code, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            await mailing.SendEmailAsync(checkingMail.email, sub, mess);
            return Json(checkingMail.email);
        }
        [HttpPost("GetCodeFromCahe")]
        public ActionResult GetCodeFromCahe([FromBody] CheckingMailModel checkingMail)
        {
            var cd = "";
            var code = cache.TryGetValue(checkingMail.email, out cd);
            return Json(cd);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel body)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResult(new { result = "all required" });
            }
            var userIdentity = mapper.Map<AppUsern>(body);

            userIdentity.regdate = DateTime.Now.AddHours(14);

            var ext = await userManager.FindByNameAsync(body.TelNum);

            if (ext != null)
            {
                return new ObjectResult(new { result = "number" });
            }
            // var cd = "";
            //var code = cache.TryGetValue(body.Email, out cd);
            //if(body.Code != cd)
            //{
            //    return new ObjectResult(new { result = "Code" });
            //}

            var result = await userManager.CreateAsync(userIdentity, body.Password);
            if (!result.Succeeded)
            {
                ext.offenable = false;
                await userManager.UpdateAsync(ext);
                return BadRequest("jj");
            }

            // AppUsern newuser = await userManager.FindByNameAsync(body.TelNum);
            // newuser.regdate = DateTime.Today.Date;

            //if (body.Type == "on")
            //{
            //    newuser.offenable = true;
            //}
            //else
            //{
            //    newuser.enable = true;
            //}


            //await db.Searches.AddAsync(new Search { IdentityId = userIdentity.Id, count = 30, enable = true, date = DateTime.Now });
            await userManager.AddToRoleAsync(userIdentity, "user");
            await db.SaveChangesAsync();
            

            var tokenstring = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiODc0NzkwODE4OTgiLCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJBdWRpZW5jZSJ9.pjbZR4Ac6Axl4qrM1YucW1lokXjPshbcOZEXLm2nj3c";
            var id = userIdentity.Id;


            return new ObjectResult(new { result = "success", id, tokenstring });
        }

        [HttpPost("checknum")]
        public async Task<IActionResult> Checknum([FromBody] CheckViewModel request)
        {
            AppUsern reUser = await userManager.FindByNameAsync(request.num);

            if (reUser == null)
            {
                return Json(Ok());
            }

            return Json(BadRequest());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            #region
            //if (!ModelState.IsValid)
            //{
            //    return new ObjectResult(new { result = "all required" });
            //}
            //bool bar;
            //bar = false;
            //bool enables;enables = false;
            //IQueryable<AppUser> user = (from c in db.User select c);
            //foreach (AppUser s in user)
            //{
            //    if (s.UserName == request.TelNum)
            //    {
            //         bar = true;
            //        enables = s.enable;
            //    }
            //}
            //if (!bar)
            //{
            //    return new ObjectResult(new { result = "not found" });
            //}
            #endregion

            AppUsern reUser = await userManager.FindByNameAsync(request.TelNum);

            if (reUser == null)
            {
                return new ObjectResult(new { result = "not found" });
            }
            var sign = signInManager.PasswordSignInAsync(reUser.UserName, request.Password, false, false);
            if (sign.Result.Succeeded)
            {
                #region
                //if (bar)
                //{
                //    var claims = new[]
                //            {
                //            new Claim(ClaimTypes.Name, request.TelNum)
                //         };
                //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dfghdfghdfghjsfjgwtyieyutlhknljsad"));
                //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //    var token = new JwtSecurityToken(
                //        issuer: "Issuer",
                //        audience: "Audience",
                //        claims: claims,
                //        //expires: DateTime.Now.AddMinutes(30),
                //        signingCredentials: creds);

                //    var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
                //    reUser.enable = false;
                //    await db.SaveChangesAsync();
                //    return new OkObjectResult(new { tokenstring = tokenstring, reUser.Id });
                //}
                #endregion

                var tokenstring = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiODc0NzkwODE4OTgiLCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJBdWRpZW5jZSJ9.pjbZR4Ac6Axl4qrM1YucW1lokXjPshbcOZEXLm2nj3c";
                return new OkObjectResult(new { tokenstring, reUser.Id });
            }
            return new ObjectResult(new { result = "username or password" });
        }


        [HttpPost("offlogin")]
        public async Task<IActionResult> Offlogin([FromBody] LoginViewModel request)
        {
            #region
            //if (!ModelState.IsValid)
            //{
            //    return new ObjectResult(new { result = "all required" });
            //}
            //bool bar=false;
            //bool enables= false;
            //IQueryable<AppUser> user = (from c in db.User select c);
            //foreach (AppUser s in user)
            //{
            //    if (s.UserName == request.TelNum)
            //    {
            //        bar = true;
            //        enables = s.offenable;
            //    }
            //}

            //if (!bar)
            //{
            //    return new ObjectResult(new { result = "not found" });
            //}
            #endregion

            AppUsern reUser = await userManager.FindByNameAsync(request.TelNum);
            if (reUser == null)
            {
                return new ObjectResult(new { result = "not found" });
            }
            if(reUser.offenable == true)
            {
                var sign = signInManager.PasswordSignInAsync(reUser.UserName, request.Password, false, false);
                if (sign.Result.Succeeded)
                {
                    reUser.offenable = false;
                    await userManager.UpdateAsync(reUser);

                    #region
                    //        if (bar)
                    //        {
                    //            var claims = new[]
                    //                    {
                    //                    new Claim(ClaimTypes.Name, request.TelNum)
                    //                 };
                    //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dfghdfghdfghjsfjgwtyieyutlhknljsad"));
                    //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    //            var token = new JwtSecurityToken(
                    //                issuer: "Issuer",
                    //                audience: "Audience",
                    //                claims: claims,
                    //                //expires: DateTime.Now.AddMinutes(30),
                    //                signingCredentials: creds);

                    //            var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
                    //            reUser.offenable = false;
                    //            await db.SaveChangesAsync();
                    //            return new OkObjectResult(new { tokenstring = tokenstring, reUser.Id });
                    //        }
                    #endregion
                    var tokenstring = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiODc0NzkwODE4OTgiLCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJBdWRpZW5jZSJ9.pjbZR4Ac6Axl4qrM1YucW1lokXjPshbcOZEXLm2nj3c";
                    return new OkObjectResult(new { tokenstring, reUser.Id });
                }
            }
            return new ObjectResult(new { result = "username or password" });
            
        }

        [HttpPost("resetpass")]
        public async Task<IActionResult> Resetpass([FromBody] ResetPassViewModel model)
        {
            AppUsern user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ObjectResult(new { result = "not found" });
            }
            if (!ModelState.IsValid)
            {
                return new ObjectResult(new { result = "all required" });
            }
            if (model.Id != user.Id)
            {
                return new ObjectResult(new { result = "not found" });
            }
            #region
            //if (model.Email != user.Email)
            //{
            //    return new ObjectResult(new { result = "not found" });
            //}
            //var cd = "";
            //var code = cache.TryGetValue(model.Email, out cd);
            //if (model.Code != cd)
            //{
            //    return new ObjectResult(new { result = "Code" });
            //}
            #endregion

            var _passwordValidator =
               HttpContext.RequestServices.GetService(typeof(IPasswordValidator<AppUsern>)) as IPasswordValidator<AppUsern>;
            var _passwordHasher =
                HttpContext.RequestServices.GetService(typeof(IPasswordHasher<AppUsern>)) as IPasswordHasher<AppUsern>;

            IdentityResult result =
                await _passwordValidator.ValidateAsync(userManager, user, user.PasswordHash);
            if (result.Succeeded)
            {

                user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                await userManager.UpdateAsync(user);
                return new OkObjectResult(new { result = "success" });
            }
            return BadRequest();
        }

        [HttpPost("resetnum")]
        public async Task<IActionResult> Resetnum([FromBody] ResetNumViewModel model)
        {
            AppUsern user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ObjectResult(new { result = "not found" });
            }
            if (!ModelState.IsValid)
            {
                return new ObjectResult(new { result = "all required" });
            }
            if (model.Id != user.Id)
            {
                return new ObjectResult(new { result = "not found" });
            }
            IQueryable<AppUsern> st = (from c in db.Usernew select c);
            bool cut;
            cut = false;
            foreach (AppUsern s in st)
            {
                if (s.UserName == model.NewTelNum)
                {
                    cut = true;
                }
            }
            if (cut)
            {
                return new ObjectResult(new { result = "number" });
            }

            user.UserName = model.NewTelNum;
            await userManager.UpdateAsync(user);
            return new OkObjectResult(new { result = "success" });

        }

        [HttpPost("quite")]
        public async Task<IActionResult> Quite([FromBody] CheckViewModel request)
        {
            var user = await userManager.FindByNameAsync(request.num);

            if (user == null)
            {
                return Json(BadRequest());
            }
            user.enable = true;

            await db.SaveChangesAsync();
            return Json(Ok());
        }

        [HttpPost("offquite")]
        public async Task<IActionResult> Offquite([FromBody] CheckViewModel request)
        {
            var user = await userManager.FindByNameAsync(request.num);

            if (user == null)
            {
                return Json(BadRequest());
            }
            user.offenable = true;

            await db.SaveChangesAsync();
            return Json(Ok());
        }
        [HttpPost("editpersonal")]
        public async Task<IActionResult> Editpersonal([FromBody] EditPersonalViewModel request)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            user.FirstName = request.fname;
            user.LastName = request.lname;
            user.pan1 = request.pan1;
            user.pan2 = request.pan2;
            await db.SaveChangesAsync();
            return new OkObjectResult(new { user.FirstName, user.LastName, user.pan1, user.pan2 });
        }
        [HttpPost("infoper")]
        public async Task<IActionResult> infoper([FromBody] SearchViewModel request)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            return new OkObjectResult(new { user.FirstName, user.LastName, user.pan1, user.pan2 });

        }



        public async Task<JsonResult> Social([FromBody]SocialRegisterInput sri)
        {
            if(sri == null)
            {
                return Json(new ApiError { Error = "Data Null" });
            }

            var user = await userManager.FindByNameAsync(sri.Account);
            if(user == null)
            {
                user = new AppUsern { UserName = sri.Account, FirstName = sri.FirstName };
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return Json("error");
                }
                await userManager.AddToRoleAsync(user, "user");
                await db.SaveChangesAsync();
            }

            user = await userManager.FindByNameAsync(sri.Account);

            var tokenstring = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiODc0NzkwODE4OTgiLCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJBdWRpZW5jZSJ9.pjbZR4Ac6Axl4qrM1YucW1lokXjPshbcOZEXLm2nj3c";
            return Json(new { result = "success", user.Id, tokenstring });
        }


        [HttpPost]
        [Route("LoginQr")]
        public async Task<JsonResult> LoginQr([FromBody] LoginAuthQrModel model)
        {
            AppUsern reUser = await userManager.FindByNameAsync(model.Number);

            if (reUser == null)
            {
                return new JsonResult(new { result = "Бұл номер жүйеде тіркелмеген!" });
            }
            var sign = signInManager.PasswordSignInAsync(reUser.UserName, model.Password, false, false);
            if (sign.Result.Succeeded)
            {
                return new JsonResult(new { reUser.Id, reUser.FirstName, reUser.LastName });
            }
            return new JsonResult(new { result = "Номеріңіз немесе құпия сөзіңіз дұрыс емес" });
        }



        [HttpPost]
        [Route("RegisterQr")]
        public async Task<JsonResult> RegisterQr([FromBody] RegisterAuthQrModel model)
        {
            AppUsern reUser = await userManager.FindByNameAsync(model.Number);

            if (reUser != null)
            {
                return new JsonResult(new { result = "Бұл номер жүйеде тіркелген!" });
            }

            AppUsern auser = new AppUsern { UserName = model.Number, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.Number };

            var result = await userManager.CreateAsync(auser, model.Password);
            if (result.Succeeded)
            {
                return new JsonResult(new { auser.Id });
            }

            return new JsonResult(new { result = "error" });
        }
    }
}