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

        private readonly Channel<HelloFromProcess> incoming = Channel.CreateUnbounded<HelloFromProcess>();

        public ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            Task.Factory.StartNew(async _ => await this.ProcessAsync(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning);

            return new ValueTask();
        }

        public IAsyncEnumerable<HelloFromProcess> ReadAllAsync(CancellationToken cancellationToken = default)
            => this.incoming.Reader.ReadAllAsync(cancellationToken);

        private async Task ProcessAsync(CancellationToken cancellationToken = default)
        {
            var config = new ConsumerConfig
            {
                GroupId = Environment.ProcessId.ToString(),
                BootstrapServers = string.Join(",", brokers),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<string, HelloFromProcess>(config)
               .SetKeyDeserializer(Deserializers.Utf8)
               .SetValueDeserializer(new JsonDeserializer<HelloFromProcess>())
               .Build();

            consumer.Subscribe("hello-from-process");

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
                consumer.Close();
            }
        }
    }
}
