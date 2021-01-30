using Funky.Core.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class FakeConsumer<T> : IConsumer<T>
    {
        private readonly Channel<T> messages = Channel.CreateUnbounded<T>();

        public FakeConsumer(string topic)
            => this.Topic = topic ?? throw new ArgumentNullException(nameof(topic));

        public string Topic { get; }

        public async Task SendAsync(T message)
            => await this.messages.Writer.WriteAsync(message)
                .ConfigureAwait(false);

        public IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default)
            => this.messages.Reader.ReadAllAsync(cancellationToken);
    }
}
