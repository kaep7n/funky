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
        private readonly VesselFactory vesselFactory;
        private readonly IOptionsMonitor<VesselControllerServiceOptions> optionsMonitor;
        private readonly ILogger<VesselControllerService> logger;
        private readonly List<IVessel> vessels = new ();

        public VesselControllerService(VesselFactory vesselFactory, IOptionsMonitor<VesselControllerServiceOptions> optionsMonitor, ILogger<VesselControllerService> logger)
        {
            this.vesselFactory = vesselFactory ?? throw new ArgumentNullException(nameof(vesselFactory));
            this.optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<IVessel> Vessels => this.vessels;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("starting controller.");

            this.logger.LogDebug("reading configured funk definitions.");
            foreach (var options in this.optionsMonitor.CurrentValue.FunkDefs)
            {
                this.logger.LogDebug($"parsing funk definition '{options}'.");
                var funkDef = new FunkDef(options.Type, options.Topics);

                this.logger.LogInformation($"creating vessel for funk definition '{funkDef}.");
                var vessel = this.vesselFactory.Create(funkDef);

                this.logger.LogInformation($"starting vessel for funk definition '{funkDef}.");
                vessel.Initialize();

                this.logger.LogDebug("adding vessel to controlled vessels.");
                this.vessels.Add(vessel);
            }

            this.logger.LogInformation("controller started successfully.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
