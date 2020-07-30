using System;
using System.Threading;
using System.Threading.Tasks;
using Funky.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Funky.Bootstrapper
{
    public class Bootstrapper : BackgroundService
    {
        private readonly IVessel vessel;
        private readonly ILogger<Bootstrapper> logger;

        public Bootstrapper(IVessel vessel, ILogger<Bootstrapper> logger)
        {
            this.vessel = vessel ?? throw new ArgumentNullException(nameof(vessel));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("starting bootstrapper");

            await base.StartAsync(cancellationToken)
                .ConfigureAwait(false);

            await this.vessel.StartAsync()
                           .ConfigureAwait(false);

            this.logger.LogInformation("started bootstrapper");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("stopping bootstrapper");

            await this.vessel.StopAsync()
                           .ConfigureAwait(false);

            await base.StopAsync(cancellationToken)
                .ConfigureAwait(false);

            this.logger.LogInformation("stopped bootstrapper");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
