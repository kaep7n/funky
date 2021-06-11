using Funky.Playground.ProtoActor.Homematic;
using Funky.Playground.Prototype.Actors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Proto;
using Proto.DependencyInjection;
using System;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;

namespace Funky.Playground.ProtoActor
{
    class Program
    {
        public static void Main(string[] args)
        {
            Proto.Log.SetLoggerFactory(
                LoggerFactory.Create(l => l.AddConsole().SetMinimumLevel(LogLevel.Information)));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(serviceProvider => new ActorSystem().WithServiceProvider(serviceProvider));
                    services.AddHostedService<Worker>();
                });
    }
}
