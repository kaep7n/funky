using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core.Events
{
    public interface IProducer<T>
    {
        Task ProduceAsync(T evt, CancellationToken cancellationToken = default);
    }
}
