using Funky.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Funky.Bootstrapper.TestHost2
{
    class Program
    {
        public static async Task Main(string[] args)
            => await CreateHostBuilder(args)
                .Build()
                .RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseFunky();
    }
}
