using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    class Program
    {
        static async Task Main(string[] args)
            => await Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddFunk<ForwardTimerFired>()
                        .Subscribe()
                        .Timer(1000);

                    services.AddFunk<ReceiveSomething>()
                        .Subscribe()
                        .Topic<TimerFiredForwarded>("forward");

                    services.AddHostedService<SubscriptionObserver>();
                })
                .RunConsoleAsync();
    }
}
