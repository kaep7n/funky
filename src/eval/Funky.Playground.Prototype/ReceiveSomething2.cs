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

        public async ValueTask ExecuteAsync(TimerFiredForwarded message)
        {
            this.logger.LogInformation($"RECEIVER 2: Received Forward: {message.FiredAt}");
            
            await Task.Delay(8000);
        }
    }
}
