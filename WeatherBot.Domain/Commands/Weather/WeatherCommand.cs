using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Domain.Abstractions;

namespace WeatherBot.Domain.Commands.Weather
{
    public class WeatherCommand : ITelegramCommand
    {
        public string Name => @"/weather";

        public async Task Execute(Message message, ITelegramBotClient botClient, params string[] values)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Weather;

            await botClient.SendTextMessageAsync(chatId, "Enter your city", ParseMode.Html);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}