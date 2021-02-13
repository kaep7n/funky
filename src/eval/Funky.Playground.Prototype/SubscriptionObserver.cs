using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class SubscriptionObserver : IHostedService
    {
        private readonly IEnumerable<ISubscription> subsriptions;

        public SubscriptionObserver(IEnumerable<ISubscription> subscriptions)
            => this.subsriptions = subscriptions ?? throw new ArgumentNullException(nameof(subscriptions));

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var subscription in this.subsriptions)
            {
                await subscription.EnableAsync();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var subscription in this.subsriptions)
            {
                await subscription.DisableAsync();
            }
        }
    }
}
