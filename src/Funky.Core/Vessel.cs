using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core
{
    public sealed class Vessel : IVessel
    {
        private readonly ILogger<Vessel> logger;

        public Vessel(ILogger<Vessel> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("starting vessel");

            this.logger.LogInformation("started vessel");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("stopping vessel");

            this.logger.LogInformation("stopped vessel");

            return Task.CompletedTask;
        }
    }
}
