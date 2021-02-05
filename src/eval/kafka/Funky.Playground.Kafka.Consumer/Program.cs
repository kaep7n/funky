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

        public static async Task Main(string[] _)
        {
            Console.WriteLine("consumer started", Color.Gray);

            var consumer = new Consumer();
            var cts = new CancellationTokenSource();

            await consumer.StartAsync(cts.Token);

            await foreach (var message in consumer.ReadAllAsync().ConfigureAwait(false))
            {
                Console.WriteLine($"received from process {message.ProcessId} created at {message.CreatedAtUtc}");
            }

            Console.WriteLine("waiting for messages", Color.Gray);
        }
    }

    public class JsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => JsonSerializer.Deserialize<T>(data);
    }
}
