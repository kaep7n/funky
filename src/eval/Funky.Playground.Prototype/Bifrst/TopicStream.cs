﻿using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Bifrst
{
    internal class TopicStream
    {
        private readonly Channel<Message> stream;
        private long offset = 0;

        public TopicStream(string key, TopicOptions options)
        {
            this.Key = key;

            this.stream = !options.Capacity.HasValue
                ? Channel.CreateUnbounded<Message>()
                : Channel.CreateBounded<Message>(options.Capacity.Value);
        }

        public async ValueTask WriteAsync(object payload)
        {
            await this.stream.Writer.WriteAsync(new Message(Guid.NewGuid(), this.offset, payload));
            this.offset++;
        }

        public string Key { get; }

        internal IAsyncEnumerable<Message> ReadAllAsync()
            => this.stream.Reader.ReadAllAsync();
    }
}
