using System;
using System.Threading.Tasks;

namespace Funky.Core
{
    public class Vessel : IVessel
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFunk funk;

        public Vessel(IServiceProvider serviceProvider, IFunk funk)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (funk is null)
            {
                throw new ArgumentNullException(nameof(funk));
            }

            this.serviceProvider = serviceProvider;
            this.funk = funk;
        }

        public Task StartAsync() => Task.CompletedTask;

        public Task StopAsync() => Task.CompletedTask;
    }
}
