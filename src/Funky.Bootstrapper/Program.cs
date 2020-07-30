using System;
using System.Threading.Tasks;
using Funky.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Funky.Bootstrapper
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateHostBuilder(args)
                .Build()
                .RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.Configure<VesselOptions>(configuration.GetSection("vessel"));
                    services.AddSingleton(p =>
                    {
                        var options = p.GetRequiredService<IOptions<VesselOptions>>();

                        return new VesselBuilder()
                            .UseContentRoot(options.Value.Path)
                            .UseAssembly(options.Value.Assembly)
                            .UseFunk(options.Value.Funk)
                            .AddLogging()
                            .Build();
                    });

                    services.AddHostedService<Bootstrapper>();
                });
    }
}
