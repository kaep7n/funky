using Funky.Core;
using Funky.Kafka;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Funky.Bootstrapper.TestHost1
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateHostBuilder(args)
                .Build()
                .RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseFunky(builder =>
                {
                    builder.UseKafka("sys1");
                });
    }
}
