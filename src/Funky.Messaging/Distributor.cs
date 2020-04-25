using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public sealed class Distributor : IDistributor, IDisposable
    {
        private readonly Channel<IMessage> channel = Channel.CreateUnbounded<IMessage>(
            new UnboundedChannelOptions
            {
                SingleReader = true
            });

        private CancellationTokenSource tokenSource;

        private readonly IList<ISubscription> subscriptions = new List<ISubscription>();

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            this.tokenSource = new CancellationTokenSource();

            _ = Task.Run(() => this.ProcessChannelAsync(this.tokenSource.Token));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            this.tokenSource.Cancel();

            return Task.CompletedTask;
        }

        public void AddSubscription(ISubscription subscription) => this.subscriptions.Add(subscription);

        private async Task ProcessChannelAsync(CancellationToken cancellationToken = default)
        {
            await foreach (var message in this.channel.Reader.ReadAllAsync(cancellationToken))
            {
                var matchingSubscriptions = this.subscriptions.Where(s => s.Topic == message.Topic);

                foreach(var matchingSubscription in matchingSubscriptions)
                {
                    await matchingSubscription.ForwardAsync(message);
                }
            }
        }

        public async Task EnqueueAsync(IMessage message, CancellationToken cancellationToken = default) => await this.channel.Writer.WriteAsync(message, cancellationToken);

        public void Dispose() => this.tokenSource.Dispose();
    }
}
