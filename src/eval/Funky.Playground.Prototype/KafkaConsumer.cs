using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class KafkaConsumer<T>
    {
        private readonly IEnumerable<string> brokers;
        private readonly string topic;

        private readonly IConsumer<string, T> consumer;
        private readonly Channel<T> queue = Channel.CreateUnbounded<T>();

        private CancellationTokenSource tokenSource;

        public KafkaConsumer(IEnumerable<string> brokers, string topic, string group)
        {
            if (brokers is null)
                throw new ArgumentNullException(nameof(brokers));
            if (topic is null)
                throw new ArgumentNullException(nameof(topic));

            this.brokers = brokers;
            this.topic = topic;

            var config = new ConsumerConfig
            {
                BootstrapServers = string.Join(",", this.brokers),
                GroupId = group,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            this.consumer = new ConsumerBuilder<string, T>(config)
               .SetKeyDeserializer(Deserializers.Utf8)
               .SetValueDeserializer(new JsonDeserializer<T>())
               .Build();
        }

        public Task EnableAsync(CancellationToken cancellationToken = default)
        {
            this.tokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(async _ => await this.ProcessAsync(tokenSource.Token), cancellationToken, TaskCreationOptions.LongRunning);

            this.consumer.Subscribe(this.topic);

            return Task.CompletedTask;
        }

        public Task DisableAsync(CancellationToken cancellationToken = default)
        {
            this.tokenSource.Cancel();

            this.consumer.Unsubscribe();

            return Task.CompletedTask;
        }

        public IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default) => this.queue.Reader.ReadAllAsync(cancellationToken);

        private async Task ProcessAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    await this.queue.Writer.WriteAsync(consumeResult.Message.Value, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                consumer.Close();
            }
        }
    }
}
