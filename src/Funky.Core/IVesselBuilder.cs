
using Microsoft.Extensions.DependencyInjection;

namespace Funky.Core
{
    public interface IVesselBuilder
    {
        IServiceCollection Services { get; }

        IVesselBuilder UseContentRoot(string path);

        IVesselBuilder AddLogging();

        IVesselBuilder UseAssembly(string assemblyName);

        IVesselBuilder UseFunk(string funkTypeName);

        IVesselBuilder UseStartup(string startupTypeName);

        IVessel Build();
    }
}
