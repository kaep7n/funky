using Funky.Playground.Prototype.Bifrst;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class TopicSubscription<TMessage> : ISubscription
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Bifröst bifröst;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly Type targetType;
        private readonly Subscription subscription;

        public TopicSubscription(Bifröst bifröst, IServiceScopeFactory serviceScopeFactory, Type targetType, string topic, string group)
        {
            this.bifröst = bifröst ?? throw new ArgumentNullException(nameof(bifröst));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this.subscription = new Subscription(topic, group);
        }

        public async ValueTask EnableAsync()
        {
            await this.bifröst.SubscribeAsync(subscription);

            _ = Task.Run(async () =>
              {
                  await foreach (var message in this.subscription.ReadAllAsync()
                        .WithCancellation(this.cancellationTokenSource.Token))
                  {
                      using var scope = this.serviceScopeFactory.CreateScope();

                      if (scope.ServiceProvider.GetService(this.targetType) is not IFunk<TMessage> funk)
                          return;

                      if (message.Payload is TMessage payload)
                          await funk.ExecuteAsync(payload);
                  }
              }, this.cancellationTokenSource.Token);
        }

        public async ValueTask DisableAsync()
        {
            this.cancellationTokenSource.Cancel();
            await this.bifröst.UnsubscribeAsync(subscription);
        }
    }
}
