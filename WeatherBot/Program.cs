using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Commands;
using WeatherBot.Domain.Commands.Covid;
using WeatherBot.Domain.Commands.Weather;
using WeatherBot.Domain.Handlers;
using WeatherBot.Domain.Services;
using Hangfire;
using Hangfire.SqlServer;
using WeatherBot.Domain;

namespace WeatherBot
{
    class Program
    {
        static readonly CommandService _service = new CommandService();
        static ITelegramBotClient _botClient;
        static void Main(string[] args)
        {
            _botClient = new TelegramBotClient(ApiKeys.TelegramApi);

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            CurrentState.State = State.Default;

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();


            GlobalConfiguration.Configuration.UseSqlServerStorage(ApiKeys.ConnectionString);

            using (var server = new BackgroundJobServer())
            {
                BackgroundJob.Schedule(() => Console.WriteLine("Hello, world!"),TimeSpan.FromMinutes(1));

                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }

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
                        if (CurrentState.State == State.Weather && command is AddWeatherCityCommandHandler)
                        {
                            await command.Execute(message, _botClient);
                            break;
                        }
                        else if (CurrentState.State == State.Covid && command is AddCovidCountryCommandHandler)
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
