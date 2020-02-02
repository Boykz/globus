using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EntGlobus.Telegram.Commands
{
    public class StartCommand : Command
    {

        public override string Name => @"/start";

        public override bool Contains(Message message) => message.Text.Contains(Name);


        public override async Task Execute(Message message, TelegramBotClient botClient, Update update)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            var userFirstName = message.From.FirstName ?? "<NoFirstName>";


            List<KeyboardButton> keyboards = new List<KeyboardButton>();
            keyboards.Add(new KeyboardButton { RequestContact = true, Text = "Номер Телефон" });

            var k = new ReplyKeyboardMarkup(keyboards, oneTimeKeyboard: true);


            await botClient.SendTextMessageAsync(chatId, $"Салем, {userFirstName}.", replyToMessageId: messageId);



            await botClient.SendTextMessageAsync(message.Chat.Id, "Номер телефона - кнопкасын басыңыз!", replyMarkup: k);

        }
    }
}
