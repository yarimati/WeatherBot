using System.Collections.Generic;

namespace WeatherBot.Domain.Abstractions
{
    public interface ICommandService
    {
        List<ITelegramCommand> Get();
    }
}