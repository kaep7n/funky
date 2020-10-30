using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Playground.Kafka.Consumer
{
    public class Consumer
    {
        private static readonly string[] brokers = new[] { "localhost:9092", "localhost:9093", "localhost:9094" };

        private readonly Channel<ConfigurationChanged> incoming = Channel.CreateUnbounded<ConfigurationChanged>();

        public ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            Task.Factory.StartNew(async _ => await this.ProcessAsync(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning);

            return new ValueTask();
        }

        public IAsyncEnumerable<ConfigurationChanged> ReadAllAsync(CancellationToken cancellationToken = default) => this.incoming.Reader.ReadAllAsync(cancellationToken);

        private async Task ProcessAsync(CancellationToken cancellationToken = default)
        {
            var config = new ConsumerConfig
            {
                GroupId = "configuration-group",
                BootstrapServers = string.Join(",", brokers),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<string, ConfigurationChanged>(config)
               .SetKeyDeserializer(Deserializers.Utf8)
               .SetValueDeserializer(new JsonDeserializer<ConfigurationChanged>())
               .Build();
            consumer.Subscribe(ConfigurationChanged.TOPIC);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    await this.incoming.Writer.WriteAsync(consumeResult.Message.Value, cancellationToken);
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
