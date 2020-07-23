using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Events.Kafka
{
    public class KafkaConsumer<T> : IConsumer<T>
    {
        private readonly IEnumerable<string> brokers;
        private readonly string topic;

        private readonly IConsumer<string, T> consumer;
        private readonly Channel<T> queue = Channel.CreateUnbounded<T>();

        public KafkaConsumer(IEnumerable<string> brokers, string topic)
        {
            if (brokers is null)
                throw new ArgumentNullException(nameof(brokers));
            if (topic is null)
                throw new ArgumentNullException(nameof(topic));

            this.brokers = brokers;
            this.topic = topic;

            var config = new ConsumerConfig
            {
                BootstrapServers = string.Join(",", this.brokers)
            };

            this.consumer = new ConsumerBuilder<string, T>(config)
               .SetKeyDeserializer(Deserializers.Utf8)
               .SetValueDeserializer(new JsonDeserializer<T>())
               .Build();
        }

        public Task EnableAsync(CancellationToken cancellationToken = default)
        {
            this.consumer.Subscribe(this.topic);

            return Task.CompletedTask;
        }

        public Task DisableAsync(CancellationToken cancellationToken = default)
        {
            this.consumer.Unsubscribe();

            return Task.CompletedTask;
        }

        public IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default) => this.queue.Reader.ReadAllAsync(cancellationToken);
    }
}
