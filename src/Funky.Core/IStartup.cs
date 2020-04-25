using Microsoft.Extensions.DependencyInjection;

namespace Funky.Core
{
    public interface IStartup
    {
        void Configure(IServiceCollection services);
    }
}
