using Funky.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public record LogMessage(string Message);

    public class LoggingFunk : IFunk<LogMessage>
    {
        private readonly ILogger<LoggingFunk> logger;

        public LoggingFunk(ILogger<LoggingFunk> logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.logger = logger;
        }

        public ValueTask ExecuteAsync(LogMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            this.logger.LogInformation(message.Message);

            return new ValueTask();
        }
    }
}
