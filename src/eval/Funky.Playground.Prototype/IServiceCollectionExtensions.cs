using Microsoft.Extensions.DependencyInjection;
using System;

namespace Funky.Playground.Prototype
{
    public static class IServiceCollectionExtensions
    {
        public static IFunkBuilder AddFunk<TFunk>(this IServiceCollection services)
            => services.AddFunk(typeof(TFunk));

        public static IFunkBuilder AddFunk(this IServiceCollection services, Type funkType)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            services.AddTransient(funkType);

            return new FunkBuilder(services, funkType);
        }
    }

    public interface IFunkBuilder
    {
        ISubscriptionBuilder Subscribe();
    }

    public interface ISubscriptionBuilder
    {
        ISubscriptionBuilder Timer(double interval);

        ISubscriptionBuilder Topic<TMessage>(string topic);
    }

    internal class FunkBuilder : IFunkBuilder
    {
        private readonly IServiceCollection services;
        private readonly Type funkType;

        public FunkBuilder(IServiceCollection services, Type funkType)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.funkType = funkType ?? throw new ArgumentNullException(nameof(funkType));
        }

        public ISubscriptionBuilder Subscribe()
            => new SubscriptionBuilder(this.services, this.funkType);
    }

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
            services.AddTransient<QueueResolver>();
            services.AddSingleton<IQueue>(p => new Queue<TMessage>(topic));
            services.AddSingleton<ISubscription>(
                p => new TopicSubscription<TMessage>(
                        p.GetRequiredService<IServiceScopeFactory>(),
                        funkType,
                        topic
                    )
                );

            return this;
        }
    }
}
