using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class ReceiveSomething3 : IFunk<TimerFiredForwarded>
    {
        private readonly ILogger<ReceiveSomething3> logger;

        public ReceiveSomething3(ILogger<ReceiveSomething3> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask ExecuteAsync(TimerFiredForwarded message)
        {
            this.logger.LogInformation($"RECEIVER 3: Received Forward: {message.FiredAt}");

            return ValueTask.CompletedTask;
        }
    }
}
