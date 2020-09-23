using System.Collections.Generic;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Commands;

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
                new WeatherCommand(),
                new CovidCommand()
            };
        }

        public List<ITelegramCommand> Get() => _commands;
    }
}