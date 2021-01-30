using System.Collections.Generic;
using System.Threading;

namespace Funky.Core.Events
{
    public interface IConsumer
    {
        string Topic { get; }
    }

    public interface IConsumer<T> : IConsumer
    {
        IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}