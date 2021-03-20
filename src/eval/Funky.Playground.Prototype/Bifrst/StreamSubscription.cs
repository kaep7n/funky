using Bifrst;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype.Bifrst
{
    public class StreamSubscription : IStreamSubscription
    {
        private readonly Channel<Message> stream = Channel.CreateUnbounded<Message>();

        public StreamSubscription(string pattern)
        {
            this.Id = Guid.NewGuid();
            this.Pattern = pattern;
        }

        public Guid Id { get; }

        public string Pattern { get; }

        public async ValueTask WriteAsync(Message message, CancellationToken cancellationToken = default)
            => await this.stream.Writer.WriteAsync(message, cancellationToken);

        internal IAsyncEnumerable<Message> ReadAllAsync() => this.stream.Reader.ReadAllAsync();
    }
}