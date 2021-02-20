using Microsoft.Extensions.DependencyInjection;
using System;

namespace Funky.Playground.Prototype
{
    internal class SubscriptionBuilder : ISubscriptionBuilder
    {
        private readonly IServiceCollection services;
        private readonly Type funkType;

        public SubscriptionBuilder(IServiceCollection services, Type funkType)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.funkType = funkType ?? throw new ArgumentNullException(nameof(funkType));
        }

        public ISubscriptionBuilder Timer(double interval)
        {
            services.AddSingleton<ISubscription>(
                p => new TimerSubscription(
                        p.GetRequiredService<IServiceScopeFactory>(),
                        funkType,
                        interval
                    )
                );

            return this;
        }

        public ISubscriptionBuilder Topic<TMessage>(string topic)
        {
            services.AddSingleton<QueueResolver>();
            services.AddSingleton<ISubscription>(
                p => new TopicSubscription<TMessage>(
                        p.GetRequiredService<QueueResolver>(),
                        p.GetRequiredService<IServiceScopeFactory>(),
                        funkType,
                        topic
                    )
                );

            return this;
        }
    }
}
