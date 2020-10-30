using Confluent.Kafka;
using Funky.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Events.Kafka
{
    public class KafkaProducer<T> : IProducer<T>, IDisposable
    {
        private readonly IProducer<string, T> producer;
        private readonly string topic;
        private readonly ILogger<KafkaProducer<T>> logger;

        private bool disposed;

        public KafkaProducer(IEnumerable<string> brokers, string topic, ILogger<KafkaProducer<T>> logger)
        {
            if (brokers is null)
                throw new ArgumentNullException(nameof(brokers));

            var config = new ProducerConfig { BootstrapServers = string.Join(",", brokers) };
            this.producer =  new ProducerBuilder<string, T>(config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(new JsonSerializer<T>())
                .Build();
            
            this.topic = topic;
            this.logger = logger;
        }

        public Task ProduceAsync(T @event, CancellationToken cancellationToken = default)
        {
            this.producer.Produce(this.topic, new Message<string, T>
            {
                Key = Guid.NewGuid().ToString(),
                Value = @event
            }, this.DeliveryReportHandler);

            return Task.CompletedTask;
        }

        private void DeliveryReportHandler(DeliveryReport<string, T> deliveryReport)
        {
            if (deliveryReport.Error.IsError)
            {
                this.logger.LogError($"Delivery failed: {deliveryReport.Error.Reason}");
            }
            else
            {
                this.logger.LogDebug($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.producer.Dispose();
                }

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
