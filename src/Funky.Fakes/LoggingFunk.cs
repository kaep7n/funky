using Funky.Core;
using Funky.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class LoggingFunk : IFunk
    {
        private readonly ILogger<LoggingFunk> logger;

        public LoggingFunk(ILogger<LoggingFunk> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public Task EnableAsync() => Task.CompletedTask;

        public Task DisableAsync() => Task.CompletedTask;

        [AcceptsTopic("/test")]
        public ValueTask LogAsync(LogEntry logEntry)
        {
            if (logEntry is null)
                throw new ArgumentNullException(nameof(logEntry));

            this.logger.Log(logEntry.Level, logEntry.Message);

            return new ValueTask();
        }
    }
}
