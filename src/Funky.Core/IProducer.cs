using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IProducer<T>
    {
        Task ProduceAsync(T evt, CancellationToken cancellationToken = default);
    }
}
