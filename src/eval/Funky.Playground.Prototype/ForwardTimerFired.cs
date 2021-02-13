using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class ForwardTimerFired : IFunk<TimerFired>
    {
        private readonly QueueResolver resolver;
        private readonly ILogger<ForwardTimerFired> logger;

        public ForwardTimerFired(QueueResolver resolver, ILogger<ForwardTimerFired> logger)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask ExecuteAsync(TimerFired message)
        {
            var queue = this.resolver.Resolve<TimerFiredForwarded>("forward");
            this.logger.LogInformation($"Fired At: {message.FiredAt} Executed At: {DateTimeOffset.UtcNow}");

            await queue.WriteAsync(new TimerFiredForwarded(message.FiredAt));
        }
    }
}
