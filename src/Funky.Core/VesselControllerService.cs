using System;
using System.Threading;
using System.Threading.Tasks;
using Funky.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Funky.Bootstrapper
{
    public class VesselControllerService : IHostedService
    {
        private readonly IOptionsMonitor<VesselControllerServiceOptions> optionsMonitor;
        private readonly ILogger<VesselControllerService> logger;

        public VesselControllerService(IOptionsMonitor<VesselControllerServiceOptions> optionsMonitor, ILogger<VesselControllerService> logger)
        {
            this.optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (FunkDefinition definition in this.optionsMonitor.CurrentValue.Funks)
            {
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
