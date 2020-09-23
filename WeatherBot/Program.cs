using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using WeatherBot.Domain.Services;

namespace WeatherBot
{
    class Program
    {
        static readonly CommandService _service = new CommandService();
        static ITelegramBotClient _botClient;
        static void Main(string[] args)
        {
            _botClient = new TelegramBotClient("TokenTg");

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            _botClient.StopReceiving();
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var message = e.Message;

                foreach (var command in _service.Get())
                {
                    if (command.Contains(message))
                    {
                        await command.Execute(message, _botClient);
                        break;
                    }
                }
            }
        }
    }
}
