
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IVessel
    {
        Task StartAsync();

        Task StopAsync();
    }
}
