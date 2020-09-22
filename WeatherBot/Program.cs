using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using WeatherBot.Domain.Services;

namespace WeatherBot
{
    class Program
    {
        static readonly CommandService service = new CommandService();
        static ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("Token");

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var message = e.Message;

                foreach (var command in service.Get())
                {
                    if (command.Contains(message))
                    {
                        await command.Execute(message, botClient);
                        break;
                    }
                }
            }
        }
    }
}
