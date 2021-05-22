using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proto;

namespace Funky.Playground.Prototype.Actors
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddProtoActor(props =>
                    {
                    });

                    services.AddHostedService<Worker>();
                });
    }
}
