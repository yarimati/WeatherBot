using System.Collections.Generic;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Commands;
using WeatherBot.Domain.Commands.Common;
using WeatherBot.Domain.Commands.Covid;
using WeatherBot.Domain.Commands.Weather;
using WeatherBot.Domain.Handlers;

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
                new AddWeatherCityCommandHandler(),
                new CovidCommand(),
                new AddCovidCountryCommandHandler(),
            };
        }
        public List<ITelegramCommand> Get() => _commands;
    }
}