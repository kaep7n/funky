using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class LogCurrentTime : IFunk
    {
        private readonly ILogger<LogCurrentTime> logger;

        public LogCurrentTime(ILogger<LogCurrentTime> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask ExecuteAsync(Noop message)
        {
            this.logger.LogInformation(DateTimeOffset.Now.ToString());
            return ValueTask.CompletedTask;
        }
    }
}
