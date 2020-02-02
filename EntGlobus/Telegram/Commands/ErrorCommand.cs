using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EntGlobus.Telegram.Commands
{
    public class ErrorCommand : Command
    {
        public override string Name => @"/error";

        public override bool Contains(Message message) => message.Text.Contains(Name);

        public override async Task Execute(Message message, TelegramBotClient botClient, Update update)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            await botClient.SendTextMessageAsync(chatId, $"Сіздің номер тіркелмеген, тағы да тексеріп көріңіз.", replyToMessageId: messageId);

        }
    }
}
