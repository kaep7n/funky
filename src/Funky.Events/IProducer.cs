using System.Threading;
using System.Threading.Tasks;

namespace Funky.Events
{
    public interface IProducer<T>
    {
        Task ProduceAsync(T @event, CancellationToken cancellationToken = default);
    }
}
