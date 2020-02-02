using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EntGlobus.Telegram.Commands
{
    public class HelloCommand : Command
    {
        public override string Name => @"/hello";

        public override bool Contains(Message message) => message.Text.Contains(Name);

        public override async Task Execute(Message message, TelegramBotClient botClient, Update update)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            await botClient.SendTextMessageAsync(chatId, $"Сіздің номеріңіз - {message.Contact.PhoneNumber}", replyToMessageId: messageId);
            await botClient.SendTextMessageAsync(chatId, $"Пароль - {message.From.Username}", replyToMessageId: messageId);

        }
    }
}
