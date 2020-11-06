using Funky.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Funky.Fakes
{
    public class LoggingStartup : IInitializer
    {
        public void Configure(IServiceCollection services) => services.AddLogging();
    }
}
