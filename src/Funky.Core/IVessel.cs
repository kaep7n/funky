
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IVessel
    {
        Task StartAsync(CancellationToken cancellationToken = default);

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
