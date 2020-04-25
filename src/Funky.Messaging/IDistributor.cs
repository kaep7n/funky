using System.Threading;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public interface IDistributor
    {
        Task StartAsync(CancellationToken cancellationToken = default);

        Task StopAsync(CancellationToken cancellationToken = default);

        void AddSubscription(ISubscription subscription);

        Task EnqueueAsync(IMessage message, CancellationToken cancellationToken = default);
    }
}
