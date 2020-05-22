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
            
            var cts1 = new CancellationTokenSource();
            Task.Run(() => StartConsumer(config, cts1.Token, 1));

            var cts2 = new CancellationTokenSource();
            Task.Run(() => StartConsumer(config, cts2.Token, 2));

            var cts3 = new CancellationTokenSource();
            Task.Run(() => StartConsumer(config, cts3.Token, 3));

            Console.WriteLine("waiting for messages", Color.Gray);

            var input = string.Empty;

            while (input != "exit")
            {
                input = Console.ReadLine();

                switch (input)
                {
                    case "1": 
                        cts1.Cancel();
                        break;
                    case "2":
                        cts2.Cancel();
                        break;
                    case "3":
                        cts3.Cancel();
                        break;
                    default:
                        break;
                }
            }
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
                while (!token.IsCancellationRequested)
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
