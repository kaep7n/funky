using Nito.AsyncEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Funky.Messaging.Tests
{
    public class DistributorTests
    {
        [Fact]
        public async Task Test()
        {
            var topic = new TopicBuilder("test").Build();

            var consumer = new TestConsumer();
            var subscription = new ConsumerSubscription(topic, consumer);

            var expectedMessage = new TestMessage(topic);

            var distributor = new Distributor();
            distributor.AddSubscription(subscription);

            await distributor.StartAsync();
         
            await distributor.EnqueueAsync(expectedMessage);

            await consumer.WaitForConsume();

            var acutalMessage = Assert.Single(consumer.ReceivedMessages);

            Assert.Equal(acutalMessage, expectedMessage);

            await distributor.StopAsync();
        }
    }

    public class TestMessage : IMessage
    {
        public TestMessage(Topic topic)
        {
            this.CorrelationId = Guid.NewGuid();
            this.Topic = topic;
        }

        public Guid CorrelationId { get; }

        public Topic Topic { get; }
    }

    public class TestConsumer : IConsumer
    {
        private readonly AsyncAutoResetEvent resetEvent = new AsyncAutoResetEvent(false);
        private readonly Topic acceptedTopic = new TopicBuilder("test").Build();
        private readonly List<IMessage> receivedMessages = new List<IMessage>();

        public IEnumerable ReceivedMessages => this.receivedMessages;

        public IEnumerable<Topic> GetAcceptedTopics()
        {
            yield return this.acceptedTopic;
        }

        public Task ConsumeAsync(IMessage message)
        {
            this.receivedMessages.Add(message);
            this.resetEvent.Set();

            return Task.CompletedTask;
        }

        public async Task WaitForConsume(CancellationToken cancellationToken = default)
            => await this.resetEvent.WaitAsync(cancellationToken);
    }
}
