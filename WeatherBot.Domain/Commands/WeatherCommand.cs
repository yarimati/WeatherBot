using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Models.Weather;

namespace WeatherBot.Domain.Commands
{
    public class WeatherCommand :BaseClient, ITelegramCommand
    {
        public  string Name => "Weather";
        public  async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            
            var weatherObj = await GetWeatherData();

            var keyBoard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                    new KeyboardButton("Weather")
                    }
                }
            };
            await botClient.SendTextMessageAsync(chatId, $"Current weather in Kyiv : {weatherObj.main.temp} C",
                parseMode: ParseMode.Html, false, false, 0, keyBoard);
        }

        private async Task<WeatherForecastModel> GetWeatherData()
        {
            var weatherJson = await client.GetStringAsync(
                "api/weather");

            var weatherObj = JsonConvert.DeserializeObject<WeatherForecastModel>(weatherJson);

            return weatherObj;
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}