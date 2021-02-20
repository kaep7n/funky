using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    internal class InMemeoryQueue<TMessage> : IQueue<TMessage>
    {
        private readonly Channel<TMessage> channel = Channel.CreateUnbounded<TMessage>();

        public IAsyncEnumerable<TMessage> ReadAllAsync()
            => this.channel.Reader.ReadAllAsync();

        public async Task WriteAsync(TMessage message)
            => await this.channel.Writer.WriteAsync(message);
    }
}
