
namespace Funky.Core
{
    public interface IVesselBuilder
    {
        IVesselBuilder UseAssemblies(params string[] assemblies);

        IVessel Build();
    }
}
