using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Domain.Abstractions;

namespace WeatherBot.Domain.Commands.Schedule
{
    public class ScheduleCommand : ITelegramCommand
    {
        public string Name => @"/setNotification";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Schedule;

            await botClient.SendTextMessageAsync(chatId, "Specify the time at which you want to receive the weather (17:25)", ParseMode.Html);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}