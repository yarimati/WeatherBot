using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Models.Covid;
using System.Linq;

namespace WeatherBot.Domain.Commands
{
    public class CovidCommand: BaseClient, ITelegramCommand
    {
        public string Name => "Covid";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            var covidObj = await GetCovidData();

            var keyBoard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Weather")
                    },
                    new[]
                    {
                        new KeyboardButton("Covid")
                    }
                }
            };

            await botClient.SendTextMessageAsync(chatId, $"Covid cases in Ukraine {covidObj.Cases}", //todo only Ukraine
                parseMode: ParseMode.Html, false, false, 0, keyBoard);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        private async Task<CovidModel> GetCovidData()
        {
            var covidData = await client.GetStringAsync("Covid/Api"); //https://api.covid19api.com/

            return JsonConvert.DeserializeObject<IEnumerable<CovidModel>>(covidData).LastOrDefault();
        }
    }
}