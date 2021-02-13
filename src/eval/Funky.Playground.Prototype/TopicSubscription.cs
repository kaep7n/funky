using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class TopicSubscription<TMessage> : ISubscription
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly Type targetType;
        private readonly string topic;

        public TopicSubscription(IServiceScopeFactory serviceScopeFactory, Type targetType, string topic)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this.topic = topic ?? throw new ArgumentNullException(nameof(topic));
        }

        public ValueTask EnableAsync()
        {
            var scope = this.serviceScopeFactory.CreateScope();
            var resolver = scope.ServiceProvider.GetRequiredService<QueueResolver>();
            var queue = resolver.Resolve<TMessage>(this.topic);

            _ = Task.Run(async () =>
              {
                  await foreach (var message in queue.ReadAllAsync()
                        .WithCancellation(this.cancellationTokenSource.Token))
                  {
                      using var scope = this.serviceScopeFactory.CreateScope();

                      if (scope.ServiceProvider.GetService(this.targetType) is not IFunk<TMessage> funk)
                          return;

                      await funk.ExecuteAsync(message);
                  }
              }, this.cancellationTokenSource.Token);

            return ValueTask.CompletedTask;
        }

        public ValueTask DisableAsync()
        {
            this.cancellationTokenSource.Cancel();

            return ValueTask.CompletedTask;
        }
    }
}
