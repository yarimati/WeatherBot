using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Domain.Abstractions;

namespace WeatherBot.Domain.Commands.Covid
{
    public class CovidCommand: ITelegramCommand
    {
        public string Name => @"/covid";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Covid;

            await botClient.SendTextMessageAsync(chatId, "Enter the country to find out the number of cases covid", ParseMode.Html);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}