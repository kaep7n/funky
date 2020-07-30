using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Core
{
    public class Vessel : IVessel
    {
        private readonly ILogger<Vessel> logger;

        public Vessel(ILogger<Vessel> logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public Task StartAsync()
        {
            this.logger.LogInformation("starting vessel");

            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            this.logger.LogInformation("stopping vessel");

            return Task.CompletedTask;
        }
    }
}
