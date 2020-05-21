using Confluent.Kafka;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Funky.Playground.Kafka.Producer
{
    class Program
    {
        private static readonly string[] brokers = new[] { "localhost:9092", "localhost:9093", "localhost:9094" };

        public static async Task Main(string[] _)
        {
            Console.WriteLine("producer started", Color.Gray);
            
            var input = string.Empty;

            while (input != "exit")
            {
                Console.WriteLine("send a single message by entering 's' or 'single'", Color.White);
                Console.WriteLine("send multiple messages by typing 'm' or 'multiple'", Color.White);

                input = Console.ReadLine();

                if (input == "s" || input == "single")
                {
                    Console.WriteLine($"sending 1 message", Color.Gray);

                    await SendSingleMessage();
                }
                else if (input == "m" || input == "multiple")
                {
                    Console.WriteLine("you choosed to send multiple messages enter a count", Color.White);

                    if (int.TryParse(Console.ReadLine(), out var count) && count > 0)
                    {
                        Console.WriteLine($"sending {count} messages", Color.Gray);
                        SendMultipleMessages(count);
                    }
                    else
                    {
                        Console.WriteLine("invalid input, starting over", Color.LightYellow);
                    }
                }
            }
        }

        private static async Task SendSingleMessage()
        {
            var config = new ProducerConfig { BootstrapServers = string.Join(",", brokers) };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            
            try
            {
                var delivery = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = "test" });
                Console.WriteLine($"Delivered '{delivery.Value}' to '{delivery.TopicPartitionOffset}'", Color.LightGreen);
            }
            catch (ProduceException<Null, string> produceException)
            {
                Console.WriteLine($"Delivery failed: {produceException.Error.Reason}", Color.Red);
            }
        }

        private static void SendMultipleMessages(int count)
        {
            var config = new ProducerConfig { BootstrapServers = string.Join(",", brokers) };

            static void handler(DeliveryReport<Null, string> deliveryReport)
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

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            
            for (var i = 0; i < count; ++i)
            {
                producer.Produce("test-topic", new Message<Null, string> { Value = i.ToString() }, handler);
            }

            // wait for up to 10 seconds for any inflight messages to be delivered.
            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}
