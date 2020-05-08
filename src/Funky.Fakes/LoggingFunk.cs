using Funky.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class LoggingFunk : IFunk
    {
        private readonly ILogger<LoggingFunk> logger;

        public LoggingFunk(ILogger<LoggingFunk> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask ExecuteAsync()
        {
            this.logger.LogInformation("loggging works!");
            return new ValueTask();
        }
    }
}
