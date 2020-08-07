using System;
using System.Threading;
using System.Threading.Tasks;
using Funky.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Funky.Bootstrapper
{
    public class VesselService : IHostedService
    {
        private readonly IVessel vessel;
        private readonly ILogger<VesselService> logger;

        public VesselService(IVessel vessel, ILogger<VesselService> logger)
        {
            this.vessel = vessel ?? throw new ArgumentNullException(nameof(vessel));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("starting bootstrapper");

            await this.vessel.StartAsync(cancellationToken)
                           .ConfigureAwait(false);

            this.logger.LogInformation("started bootstrapper");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("stopping bootstrapper");

            await this.vessel.StopAsync(cancellationToken)
                .ConfigureAwait(false);

            this.logger.LogInformation("stopped bootstrapper");
        }
    }
}
