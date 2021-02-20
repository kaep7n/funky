
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public interface IQueue<TMessage> : IQueue
    {
        IAsyncEnumerable<TMessage> ReadAllAsync();

        Task WriteAsync(TMessage message);
    }

    public interface IQueue
    {
        public IQueue<TMessage> Unwrap<TMessage>() => this as IQueue<TMessage>;
    }
}