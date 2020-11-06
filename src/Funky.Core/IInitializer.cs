using Microsoft.Extensions.DependencyInjection;

namespace Funky.Core
{
    public interface IInitializer
    {
        void Configure(IServiceCollection services);
    }
}
