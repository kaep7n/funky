using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class ReceiveSomething1 : IFunk<TimerFiredForwarded>
    {
        private readonly ILogger<ReceiveSomething1> logger;

        public ReceiveSomething1(ILogger<ReceiveSomething1> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask ExecuteAsync(TimerFiredForwarded message)
        {
            this.logger.LogInformation($"Received Forward: {message.FiredAt}");
            return ValueTask.CompletedTask;
        }
    }
}
