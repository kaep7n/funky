
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IVessel
    {
        Task StartAsync(CancellationToken cancellationToken = default);

        Task ConsumeAsync();

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
