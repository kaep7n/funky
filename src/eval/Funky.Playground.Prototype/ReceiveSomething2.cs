using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class ReceiveSomething2 : IFunk<TimerFiredForwarded>
    {
        private readonly ILogger<ReceiveSomething2> logger;

        public ReceiveSomething2(ILogger<ReceiveSomething2> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask ExecuteAsync(TimerFiredForwarded message)
        {
            this.logger.LogInformation($"Received Forward: {message.FiredAt}");
            return ValueTask.CompletedTask;
        }
    }
}
