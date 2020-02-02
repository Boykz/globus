using EntGlobus.Telegram.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EntGlobus.Telegram
{
    public class Bot
    {
        private static TelegramBotClient botClient;
        private static List<Command> commandsList;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            commandsList.Add(new HelloCommand());
            //TODO: Add more commands

            botClient = new TelegramBotClient(BotSettings.Key);
            string hook = string.Format(BotSettings.Url, "api/telegram/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
