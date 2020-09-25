using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Commands;
using WeatherBot.Domain.Commands.Covid;
using WeatherBot.Domain.Commands.Weather;
using WeatherBot.Domain.Services;

namespace WeatherBot
{
    class Program
    {
        static readonly CommandService _service = new CommandService();
        static ITelegramBotClient _botClient;
        static void Main(string[] args)
        {
            _botClient = new TelegramBotClient("Api");

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            CurrentState.State = State.Default;

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            _botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Message.From.FirstName);
            if (e.Message.Text != null)
            {
                var message = e.Message;
                bool isCommand = IsCommand(message.Text);

                foreach (var command in _service.Get())
                {
                    if (isCommand)// only commands which contains '/'
                    {
                        if (command.Contains(message))
                        {
                            await command.Execute(message, _botClient);
                            break;
                        }
                    }
                    else 
                    {
                        if (CurrentState.State == State.Weather && command is AddWeatherCityCommand)
                        {
                            await command.Execute(message, _botClient);
                            break;
                        }
                        else if (CurrentState.State == State.Covid && command is AddCovidCityCommand)
                        {
                            await command.Execute(message, _botClient);
                            break;
                        }
                    }
                    
                }
            }
        }

        static bool IsCommand(string msg)
        {
            if (msg.Contains('/'))
                return true;
            else
                return false;
        }
    }
}
