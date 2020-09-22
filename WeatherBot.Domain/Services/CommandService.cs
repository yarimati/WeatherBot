using System.Collections.Generic;
using WeatherBot.Domain.Abstractions;
using WeatherBot.Domain.Commands;

namespace WeatherBot.Domain.Services
{
    public class CommandService : ICommandService
    {
        private readonly List<TelegramCommand> _commands;

        public CommandService()
        {
            _commands = new List<TelegramCommand>
            {
                new StartCommand()
            };
        }

        public List<TelegramCommand> Get() => _commands;
    }
}