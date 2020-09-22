using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBot.Domain.Abstractions
{
    public interface ITelegramCommand
    {
        public string Name { get; }

        public Task Execute(Message message, ITelegramBotClient botClient);

        public bool Contains(Message message);
    }
}