using Funky.Playground.Prototype.Bifrst;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public sealed class TopicSubscription<TMessage> : ISubscription, IDisposable
    {
        private readonly SemaphoreSlim semaphore = new(0, 8);
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Bifröst bifröst;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly Type targetType;
        private readonly StreamSubscription subscription;

        public TopicSubscription(Bifröst bifröst, IServiceScopeFactory serviceScopeFactory, Type targetType, string topic)
        {
            this.bifröst = bifröst ?? throw new ArgumentNullException(nameof(bifröst));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this.subscription = new StreamSubscription(topic);
        }

        public async ValueTask EnableAsync()
        {
            await this.bifröst.SubscribeAsync(subscription);

            _ = Task.Run(async () =>
              {
                  this.semaphore.Release(8);

                  await foreach (var message in this.subscription.ReadAllAsync()
                        .WithCancellation(this.cancellationTokenSource.Token))
                  {
                      await this.semaphore.WaitAsync();

                      using var scope = this.serviceScopeFactory.CreateScope();

                      if (scope.ServiceProvider.GetService(this.targetType) is not IFunk<TMessage> funk)
                          return;

                      if (message.Payload is TMessage payload)
                      {
                          _ = funk.ExecuteAsync(payload)
                            .AsTask()
                            .ContinueWith((_) => this.semaphore.Release());
                      }
                  }
              }, this.cancellationTokenSource.Token);
        }

        public async ValueTask DisableAsync()
        {
            this.cancellationTokenSource.Cancel();
            await this.bifröst.UnsubscribeAsync(subscription);
        }

        public void Dispose() => this.semaphore.Dispose();
    }
}
