using Funky.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Funky.Fakes
{
    public class EmptyStartup : IInitializer
    {
        public void Configure(IServiceCollection services)
        {
        }
    }
}
