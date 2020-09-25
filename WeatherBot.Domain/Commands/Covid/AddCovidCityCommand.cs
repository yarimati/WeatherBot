using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Models.Covid;

namespace WeatherBot.Domain.Commands.Covid
{
    public class AddCovidCityCommand : BaseClient, ITelegramCommand
    {
        public string Name => @"/addCovidCity";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Default;

            var covidObj = await GetCovidData(message.Text);

            if (covidObj == null)
            {
                CurrentState.State = State.Covid;
                await botClient.SendTextMessageAsync(chatId, "Country doesn`t exist try more");
                return;
            }
            var keyBoard = new ReplyKeyboardMarkup
            {

                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton(@"/start"),
                    },
                    new[]
                    {
                        new KeyboardButton(@"/covid"),
                    },
                    new[]
                    {
                        new KeyboardButton(@"/weather"),
                    }
                }
            };

            await botClient.SendTextMessageAsync(chatId, $"Covid cases in {message.Text} {covidObj.Cases}",
                parseMode: ParseMode.Html, false, false, 0, keyBoard);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        private async Task<CovidModel> GetCovidData(string city)
        {
            city = city.ToLowerInvariant();
            string covidData = string.Empty;

            try
            {
                covidData = await client.GetStringAsync(ApiKeys.CovidApi);
                if (string.IsNullOrEmpty(covidData))
                    return await Task.FromResult<CovidModel>(null);
            }
            catch (HttpRequestException ex)
            {
                return await Task.FromResult<CovidModel>(null);
            }
            return JsonConvert.DeserializeObject<IEnumerable<CovidModel>>(covidData).LastOrDefault();
        }
    }
}