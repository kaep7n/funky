using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class ReceiveSomething : IFunk<TimerFiredForwarded>
    {
        private readonly ILogger<ReceiveSomething> logger;

        public ReceiveSomething(ILogger<ReceiveSomething> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask ExecuteAsync(TimerFiredForwarded message)
        {
            this.logger.LogInformation($"Received Forward: {message.FiredAt}");
            return ValueTask.CompletedTask;
        }
    }
}
