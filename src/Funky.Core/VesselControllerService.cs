using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Funky.Core
{
    public class VesselControllerService : IHostedService
    {
        private readonly IOptionsMonitor<VesselControllerServiceOptions> optionsMonitor;
        private readonly ILogger<VesselControllerService> logger;
        private readonly List<Vessel> vessels = new ();

        public VesselControllerService(IOptionsMonitor<VesselControllerServiceOptions> optionsMonitor, ILogger<VesselControllerService> logger)
        {
            this.optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<IVessel> Vessels => this.vessels;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("starting controller.");

            this.logger.LogDebug("reading configured funk definitions.");
            foreach (var configuredDef in this.optionsMonitor.CurrentValue.FunkDefs)
            {
                this.logger.LogDebug($"parsing funk definition '{configuredDef}'.");
                var funkDef = new FunkDef(configuredDef);

                this.logger.LogInformation($"creating vessel for funk definition '{funkDef}.");
                var vessel = new Vessel(funkDef);

                this.logger.LogInformation($"starting vessel for funk definition '{funkDef}.");
                await vessel.StartAsync(cancellationToken)
                    .ConfigureAwait(false);

                this.logger.LogDebug("adding vessel to controlled vessels.");
                this.vessels.Add(vessel);
            }

            this.logger.LogInformation("controller started successfully.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var vessel in this.vessels)
            {
                this.logger.LogInformation("stopping vessel.");

                await vessel.StopAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
