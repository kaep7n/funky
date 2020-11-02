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

        public static IVesselBuilder UseKafka(this IVesselBuilder builder, string consumerGroup)
        {
            builder.Services.AddTransient<IConsumer<ConfigurationChanged>>(_ => new KafkaConsumer<ConfigurationChanged>(brokers, "system.configuration", consumerGroup));
            builder.Services.AddTransient<IProducer<ConfigurationChanged>>(_ => new KafkaProducer<ConfigurationChanged>(brokers, "system.configuration", _.GetRequiredService<ILogger<KafkaProducer<ConfigurationChanged>>>()));

            return builder;
        }
    }
}
