using Microsoft.Extensions.DependencyInjection;
using System;

namespace Funky.Playground.Prototype
{
    public static class IServiceCollectionExtensions
    {
        public static IFunkBuilder AddFunk<TFunk>(this IServiceCollection services)
            where TFunk: class, IFunk
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            services.AddTransient<IFunk, TFunk>();

            return new FunkBuilder<TFunk>(services);
        }
    }

    public interface IFunkBuilder
    {
        IFunkBuilder WithTimer(double interval);

        IFunkBuilder WithSubscription(string topic);
    }

    internal class FunkBuilder<TFunk> : IFunkBuilder
        where TFunk: class, IFunk
    {
        private readonly IServiceCollection services;

        public FunkBuilder(IServiceCollection services)
            => this.services = services ?? throw new ArgumentNullException(nameof(services));

        public IFunkBuilder WithSubscription(string topic)
        {
            return this;
        }

        public IFunkBuilder WithTimer(double interval)
        {
            services.AddSingleton<IFunkTrigger>(p => new TimerFunkTrigger<TFunk>(interval, p));

            return this;
        }
    }

}
