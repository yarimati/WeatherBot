using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Models.Weather;
using WeatherBot.Domain.Models.Weather.Cities;

namespace WeatherBot.Domain.Handlers
{
    public class AddWeatherCityCommandHandler : BaseClient, ITelegramCommand
    {
        public string Name => @"/addWeatherCity";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            CurrentState.State = State.Default;

            var weatherObj = await GetWeatherData(message.Text);

            if (weatherObj == null)
            {
                CurrentState.State = State.Weather;
                await botClient.SendTextMessageAsync(chatId, "City doesn`t exist try more", ParseMode.Html);
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

            await botClient.SendTextMessageAsync(chatId, $"Current weather in {message.Text} : {weatherObj.main.temp} C",
                parseMode: ParseMode.Html, false, false, 0, keyBoard);

        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
        private async Task<WeatherForecastModel> GetWeatherData(string city)
        {
            if (string.IsNullOrEmpty(city) || city == "Weather")
                return await Task.FromResult<WeatherForecastModel>(null);

            var cityId = GetIdFromCity(city);

            if (string.IsNullOrEmpty(cityId))
                return await Task.FromResult<WeatherForecastModel>(null);

            var weatherJson = await client.GetStringAsync(ApiKeys.WeatherApi.Replace("city",cityId));

            var weatherObj = JsonConvert.DeserializeObject<WeatherForecastModel>(weatherJson);

            return weatherObj;
        }

        private string GetIdFromCity(string city)
        {
            List<CityInfoModel> items;

            using (StreamReader r =
                new StreamReader(ApiKeys.PathCityList))
            {
                string jsonText = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<CityInfoModel>>(jsonText);
            }

            var cityId = items.FirstOrDefault(x => x.name.Equals(city, StringComparison.InvariantCultureIgnoreCase));

            if (cityId == null)
                return string.Empty;

            return cityId.id.ToString();
        }
    }
}