using Funky.Bootstrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace Funky.Core
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseFunky(this IHostBuilder hostBuilder)
        {
            if (hostBuilder is null)
                throw new ArgumentNullException(nameof(hostBuilder));

            hostBuilder.ConfigureServices((context, services) =>
            {
                services.Configure<VesselOptions>(context.Configuration.GetSection("funky:vessel"));

                services.AddSingleton(p =>
                {
                    var options = p.GetRequiredService<IOptions<VesselOptions>>();

                    return new VesselBuilder()
                        .UseContentRoot(options.Value.Dir)
                        .UseAssembly(options.Value.Assembly)
                        .UseFunk(options.Value.Funk)
                        .AddLogging()
                        .Build();
                });

                services.AddHostedService<VesselService>();
            });

            return hostBuilder;
        }
    }
}
