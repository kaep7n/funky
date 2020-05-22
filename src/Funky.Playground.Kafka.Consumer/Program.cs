using Confluent.Kafka;
using System;
using System.Drawing;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Funky.Playground.Kafka.Consumer
{
    class Program
    {
        private static readonly string[] brokers = new[] { "localhost:9092", "localhost:9093", "localhost:9094" };

        public static void Main(string[] _)
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            var config = new ConsumerConfig
            {
                GroupId = "configuration-group",
                BootstrapServers = string.Join(",", brokers),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            Console.WriteLine("consumer started", Color.Gray);

            Task.Run(() => StartConsumer(config, cts.Token, 1));
            Task.Run(() => StartConsumer(config, cts.Token, 2));
            Task.Run(() => StartConsumer(config, cts.Token, 3));

            Console.WriteLine("waiting for messages", Color.Gray);

            Console.ReadLine();
        }

        private static void StartConsumer(ConsumerConfig config, CancellationToken token, int number)
        {
            var consumer = new ConsumerBuilder<string, ConfigurationChanged>(config)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(new JsonDeserializer<ConfigurationChanged>())
                .Build();
            consumer.Subscribe(ConfigurationChanged.TOPIC);

            Console.WriteLine($"{number}: subscribed", Color.YellowGreen);

            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(token);
                        Console.WriteLine($"{number} | consumed message '{consumeResult.Message.Value.Configuration}' {consumeResult.Message.Value.ChangedAtUtc} at: '{consumeResult.TopicPartitionOffset}'.", Color.YellowGreen);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"{number} | error occured: {e.Error.Reason}", Color.Red);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                consumer.Close();
            }
        }
    }

    public class JsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => JsonSerializer.Deserialize<T>(data);
    }
}
