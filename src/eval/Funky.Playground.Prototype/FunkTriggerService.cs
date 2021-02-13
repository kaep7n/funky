using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class FunkTriggerService : IHostedService
    {
        private readonly IEnumerable<IFunkTrigger> triggers;

        public FunkTriggerService(IEnumerable<IFunkTrigger> triggers)
            => this.triggers = triggers ?? throw new ArgumentNullException(nameof(triggers));

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var trigger in this.triggers)
            {
                await trigger.EnableAsync();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var trigger in this.triggers)
            {
                await trigger.DisableAsync();
            }
        }
    }
}
