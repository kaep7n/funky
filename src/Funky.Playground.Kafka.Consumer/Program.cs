using Confluent.Kafka;
using System;
using System.Drawing;
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
                GroupId = "test-consumer-group",
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
            var c = new ConsumerBuilder<Ignore, string>(config).Build();
            c.Subscribe("test-topic");

            Console.WriteLine($"{number}: subscribed", Color.YellowGreen);

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = c.Consume(token);
                        Console.WriteLine($"{number} | consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.", Color.YellowGreen);
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
                c.Close();
            }
        }
    }
}
