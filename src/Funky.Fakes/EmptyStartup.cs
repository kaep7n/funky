using Funky.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Funky.Fakes
{
    public class EmptyStartup : IStartup
    {
        public void Configure(IServiceCollection services)
        {
        }
    }
}
