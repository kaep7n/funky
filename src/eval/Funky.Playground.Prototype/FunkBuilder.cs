using Microsoft.Extensions.DependencyInjection;
using System;

namespace Funky.Playground.Prototype
{
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
}
