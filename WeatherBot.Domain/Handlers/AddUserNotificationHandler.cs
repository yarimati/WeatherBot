using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Domain.Abstractions;

namespace WeatherBot.Domain.Handlers
{
    public class AddUserNotificationHandler : ITelegramCommand
    {
        public string Name => @"/addNotification";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Default;

            var time = message.Text;
            var userId = message.From.Id;





        }

        public bool Contains(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}