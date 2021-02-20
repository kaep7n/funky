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
}
