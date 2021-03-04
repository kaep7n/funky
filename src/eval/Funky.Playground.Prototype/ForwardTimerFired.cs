using Funky.Playground.Prototype.Bifrst;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class ForwardTimerFired : IFunk<TimerFired>
    {
        private readonly Bifröst bifröst;
        private readonly ILogger<ForwardTimerFired> logger;

        public ForwardTimerFired(Bifröst bifröst, ILogger<ForwardTimerFired> logger)
        {
            this.bifröst = bifröst ?? throw new ArgumentNullException(nameof(bifröst));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask ExecuteAsync(TimerFired message)
        {
            this.logger.LogInformation($"Fired At: {message.FiredAt} Executed At: {DateTimeOffset.UtcNow}");

            await bifröst.PublishAsync("forward", new TimerFiredForwarded(message.FiredAt));
        }
    }
}
