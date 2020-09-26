using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Domain.Abstractions;

namespace WeatherBot.Domain.Commands.Common
{
    public class HelpCommand : ITelegramCommand
    {
        public string Name => @"/help";

        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Help;

            string help = "Help commands\n" +
                          @"/start" + " - help\n" +
                          @"/weather" + " - help\n" +
                          @"/covid" + " - help\n" +
                          @"/setSchedule" + " - help";

            await botClient.SendTextMessageAsync(chatId, help);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}