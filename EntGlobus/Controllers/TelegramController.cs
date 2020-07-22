using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models;
using EntGlobus.Telegram;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace EntGlobus.Controllers
{
    [Route("api/telegram/update")]
    [ApiController]
    public class TelegramController : ControllerBase
    {

        private readonly UserManager<AppUsern> userManager;

        public TelegramController(UserManager<AppUsern> _userManager)
        {
            userManager = _userManager;
        }


        public string Get()
        {
            return "Succes get";
        }



        [HttpPost]
        public async Task<OkResult> Post([FromBody]Update update)
        {
            if (update == null) return Ok();

            //update.Message.Text = @"/start";

            Random ran = new Random();
            var commands = Bot.Commands;
            var message = update.Message;



            var botClient = await Bot.GetBotClientAsync();


            if (message.Contact.PhoneNumber != null)
            {
                var user = await userManager.FindByNameAsync(message.Contact.PhoneNumber);
                if(user != null)
                {
                    ///
                    ///           пән сатып алгандарга руксат бермеу керек
                    ///
                    string code = ran.Next(11111, 99999).ToString();

                    var _passwordValidator =
                   HttpContext.RequestServices.GetService(typeof(IPasswordValidator<AppUsern>)) as IPasswordValidator<AppUsern>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<AppUsern>)) as IPasswordHasher<AppUsern>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(userManager, user, user.PasswordHash);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, code);
                        await userManager.UpdateAsync(user);

                        message.Text = @"/hello";
                        message.From.Username = code;
                    }
                }
                else
                {
                    message.Text = @"/error";
                }
            }


            foreach (var command in commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, botClient, update);
                    break;
                }
            }
            return Ok();
        }
    }
}