using Funky.Core;
using Funky.Core.Events;
using Funky.Events.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Funky.Kafka
{
    public static class VesselBuilderExtensions
    {
        private static readonly string[] brokers = new[] { "localhost:9092", "localhost:9093", "localhost:9094" };

        public static IVesselBuilder UseKafka(this IVesselBuilder builder)
        {
            builder.Services.AddTransient<IConsumer<ConfigurationChanged>>(_ => new KafkaConsumer<ConfigurationChanged>(brokers, "system", "system"));
            builder.Services.AddTransient<IProducer<ConfigurationChanged>>(_ => new KafkaProducer<ConfigurationChanged>(brokers, "system", _.GetRequiredService<ILogger<KafkaProducer<ConfigurationChanged>>>()));

            return builder;
        }
    }
}
