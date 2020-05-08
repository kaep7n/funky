using Microsoft.Extensions.Logging;

namespace Funky.Fakes
{
    public class LogEntry
    {
        public LogLevel Level { get; set; }

        public string Message { get; set; }
    }
}