using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Funky.Core
{
    public interface IVesselBuilder
    {
        IServiceCollection Services { get; }

        IVesselBuilder UseContentRoot(string path);

        IVesselBuilder AddLogging();

        IVesselBuilder AddLogging(Action<ILoggingBuilder> configureLogging);

        IVesselBuilder UseAssembly(string assemblyName);

        IVesselBuilder UseFunk(string funkTypeName);

        IVesselBuilder UseStartup(string startupTypeName);

        IVessel Build();
    }
}
