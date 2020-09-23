using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Models.Weather;
using WeatherBot.Domain.Models.Weather.Cities;

namespace WeatherBot.Domain.Commands
{
    public class WeatherCommand : BaseClient, ITelegramCommand
    {
        public string Name => "Weather";
        public async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            var city = message.Text.Split(' ').LastOrDefault(); // request "Weather (city)"

            var weatherObj = await GetWeatherData(city);

            if (weatherObj == null)
            {
                await botClient.SendTextMessageAsync(chatId, "Wrong value (Weather Kyiv)", ParseMode.Html);
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
                        new KeyboardButton("Covid"),
                    }
                }
            };

            await botClient.SendTextMessageAsync(chatId, $"Current weather in {city} : {weatherObj.main.temp} C",
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

            var weatherJson = await client.GetStringAsync("Weather/Api"); //https://openweathermap.org/api

            var weatherObj = JsonConvert.DeserializeObject<WeatherForecastModel>(weatherJson);

            return weatherObj;
        }

        private string GetIdFromCity(string city)
        {
            List<CityInfoModel> items;

            using (StreamReader r = new StreamReader(@"C:\Users\nikim\source\repos\WeatherBot\WeatherBot.Domain\city.list.json"))
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