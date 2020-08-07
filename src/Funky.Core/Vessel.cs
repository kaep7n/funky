using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core
{
    public sealed class Vessel : IVessel
    {
        private readonly IFunk funk;
        private readonly ILogger<Vessel> logger;

        public Vessel(IFunk funk, ILogger<Vessel> logger)
        {
            this.funk = funk ?? throw new ArgumentNullException(nameof(funk));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("starting vessel");

            await this.funk.EnableAsync()
                .ConfigureAwait(false);

            this.logger.LogInformation("started vessel");
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("stopping vessel");

            await this.funk.DisableAsync()
                .ConfigureAwait(false);

            this.logger.LogInformation("stopped vessel");
        }
    }
}
