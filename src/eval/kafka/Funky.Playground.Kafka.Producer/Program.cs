using Confluent.Kafka;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Funky.Playground.Kafka.Producer
{
    class Program
    {
        private static readonly string[] brokers = new[] { "localhost:9092", "localhost:9093", "localhost:9094" };

        public static void Main(string[] _)
        {
            Console.WriteLine("producer started", Color.Gray);

            var count = 10000;

            while (Console.ReadLine() != "exit")
            {
                Console.WriteLine($"sending {count} messages", Color.Gray);
                SendMultipleMessages(count);

            }
        }

        private static void SendMultipleMessages(int count)
        {
            var config = new ProducerConfig { BootstrapServers = string.Join(",", brokers) };

            static void handler(DeliveryReport<string, HelloFromProcess> deliveryReport)
            {
                if (deliveryReport.Error.IsError)
                {
                    Console.WriteLine($"Delivery failed: {deliveryReport.Error.Reason}", Color.Red);
                }
                else
                {
                    Console.WriteLine($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'", Color.LightGreen);
                }
            }

            using var producer = new ProducerBuilder<string, HelloFromProcess>(config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(new JsonSerializer<HelloFromProcess>())
                .Build();

            for (var i = 0; i < count; ++i)
            {
                producer.Produce("hello-from-process", new Message<string, HelloFromProcess>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = new HelloFromProcess(Environment.ProcessId.ToString(), i + 1, DateTimeOffset.UtcNow)
                }, handler);
            }

            // wait for up to 10 seconds for any inflight messages to be delivered.
            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }

    public class JsonSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context) => JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
