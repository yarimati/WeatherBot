using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Domain.Abstractions;

namespace WeatherBot.Domain.Commands.Common
{
    public class StartCommand : BaseClient, ITelegramCommand
    {
        public string Name => @"/start";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Start;

            var keyBoard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton(@"/weather")
                    },
                    new[]
                    {
                        new KeyboardButton(@"/covid")
                    }
                }
            };
            await botClient.SendTextMessageAsync(chatId, "Helloooo",
                parseMode: ParseMode.Html, false, false, 0, keyBoard);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

    }
}