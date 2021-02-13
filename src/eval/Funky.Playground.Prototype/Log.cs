using Microsoft.Extensions.Logging;

namespace Funky.Playground.Prototype
{
    public record Log(LogLevel Level, string Details) : IMessage;
}
