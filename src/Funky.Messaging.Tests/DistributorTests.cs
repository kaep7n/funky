using Nito.AsyncEx;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Funky.Messaging.Tests
{
    public class DistributorTests
    {
        /// <summary>
        /// Should be possible to receive multiple types of messages
        /// -> Enable
        /// -> Disable (maybe for a specific amount of time)
        /// -> ConfigurationChanged
        /// -> ...
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test()
        {
            var topic = TopicBuilder.Root("test").Build();

            var consumer = new LogMessageConsumer();
            var subscription = new ConsumerSubscription(topic, consumer);

            var distributor = new Distributor();
            distributor.AddSubscription(subscription);
            await distributor.StartAsync();

            var producer = new JsonProducer(distributor);
            
            await producer.ProduceAsync(topic, 1);

            await consumer.WaitForConsume();

            var acutalMessage = Assert.Single(consumer.ReceivedMessages);

            Assert.Equal(topic, acutalMessage.Topic);

            await distributor.StopAsync();
        }
    }

    public class LogMessageConsumer : IConsumer
    {
        private readonly AsyncAutoResetEvent resetEvent = new AsyncAutoResetEvent(false);
        private readonly List<Message> receivedMessages = new List<Message>();

        public IEnumerable<Message> ReceivedMessages => this.receivedMessages;

        public Task ConsumeAsync(Message message)
        {
            this.receivedMessages.Add(message);
            this.resetEvent.Set();

            return Task.CompletedTask;
        }

        public async Task WaitForConsume(CancellationToken cancellationToken = default)
            => await this.resetEvent.WaitAsync(cancellationToken);
    }
}
