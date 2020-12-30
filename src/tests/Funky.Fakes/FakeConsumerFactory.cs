using Funky.Core;
using Funky.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class FakeConsumerFactory : IConsumerFactory
    {
        private readonly FakeConsumer fakeConsumer;

        public FakeConsumerFactory(FakeConsumer fakeConsumer)
            => this.fakeConsumer = fakeConsumer ?? throw new ArgumentNullException(nameof(fakeConsumer));

        public IConsumer Create(string topic) => fakeConsumer;
    }

    public class FakeConsumer : IConsumer
    {
        private readonly Channel<IMessage> messages = Channel.CreateUnbounded<IMessage>();

        public Task DisableAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task EnableAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public async Task SendAsync(IMessage message)
            => await this.messages.Writer.WriteAsync(message)
                .ConfigureAwait(false);

        public IAsyncEnumerable<IMessage> ReadAllAsync(CancellationToken cancellationToken = default)
            => this.messages.Reader.ReadAllAsync(cancellationToken);
    }
}
