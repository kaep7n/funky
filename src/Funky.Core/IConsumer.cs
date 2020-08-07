using Funky.Core.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IConsumer<T>
    {
        Task EnableAsync(CancellationToken cancellationToken = default);

        Task DisableAsync(CancellationToken cancellationToken = default);

        IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}
