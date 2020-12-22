using System;
using System.IO;
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

        public VesselControllerService(IOptionsMonitor<VesselControllerServiceOptions> optionsMonitor, ILogger<VesselControllerService> logger)
        {
            this.optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var configuredDef in this.optionsMonitor.CurrentValue.FunkDefs)
            {
                var funkDef = new FunkDef(configuredDef);
                var loadContext = new DirectoryLoadContext(Directory.GetCurrentDirectory());
                var assembly = loadContext.LoadFromAssemblyName(funkDef.TypeName.Assembly);

                var instance = assembly.CreateInstance(funkDef.TypeName.FullName);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
