using Bifrst;
using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype.Bifrst
{
    public class Subscription : ISubscription
    {
        private readonly Channel<Message> stream = Channel.CreateUnbounded<Message>();

        public Subscription(string pattern, string group)
        {
            this.Id = Guid.NewGuid();
            this.Pattern = pattern;
            this.Group = group;
        }

        public Guid Id { get; }

        public string Pattern { get; }

        public string Group { get; }

        public async ValueTask WriteAsync(Message message) => await this.stream.Writer.WriteAsync(message);

        internal IAsyncEnumerable<Message> ReadAllAsync() => this.stream.Reader.ReadAllAsync();
    }
}