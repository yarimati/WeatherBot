using System.Collections.Generic;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Commands;
using WeatherBot.Domain.Commands.Covid;
using WeatherBot.Domain.Commands.Weather;

namespace WeatherBot.Domain.Services
{
    public class CommandService : ICommandService
    {
        private readonly List<ITelegramCommand> _commands;
        public CommandService()
        {
            _commands = new List<ITelegramCommand>
            {
                new StartCommand(),
                new HelpCommand(),
                new WeatherCommand(),
                new AddWeatherCityCommand(),
                new CovidCommand(),
                new AddCovidCityCommand(),
            };
        }
        public List<ITelegramCommand> Get() => _commands;
    }
}