using Funky.Bootstrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace Funky.Core
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseFunky(this IHostBuilder hostBuilder, Action<IVesselBuilder> vesselBuilderAction)
        {
            if (hostBuilder is null)
                throw new ArgumentNullException(nameof(hostBuilder));
            if (vesselBuilderAction is null)
                throw new ArgumentNullException(nameof(vesselBuilderAction));

            hostBuilder.ConfigureServices((context, services) =>
            {
                services.Configure<VesselOptions>(context.Configuration.GetSection("funky:vessel"));

                services.AddSingleton(p =>
                {
                    var options = p.GetRequiredService<IOptions<VesselOptions>>();

                    var vesselBuilder = new VesselBuilder();

                    vesselBuilderAction(vesselBuilder);

                    return vesselBuilder.Build();
                });

                services.AddHostedService<VesselService>();
            });

            return hostBuilder;
        }
    }
}
