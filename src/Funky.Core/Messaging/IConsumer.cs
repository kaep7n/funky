using Funky.Core.Messaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IConsumer
    {
        Task EnableAsync(CancellationToken cancellationToken = default);

        Task DisableAsync(CancellationToken cancellationToken = default);

        IAsyncEnumerable<IMessage> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}
