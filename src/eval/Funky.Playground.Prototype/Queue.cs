using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class Queue<TMessage> : IQueue
    {
        private readonly Channel<TMessage> channel = Channel.CreateUnbounded<TMessage>();

        public Queue(string topic)
            => this.Topic = topic ?? throw new ArgumentNullException(nameof(topic));

        public string Topic { get; }

        public IAsyncEnumerable<TMessage> ReadAllAsync()
            => this.channel.Reader.ReadAllAsync();

        internal async Task WriteAsync(TMessage message)
            => await this.channel.Writer.WriteAsync(message);
    }
}
