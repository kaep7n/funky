using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public class ConsumerSubscription : ISubscription
    {
        private readonly IConsumer consumer;

        public ConsumerSubscription(Topic topic, IConsumer consumer)
        {
            if (consumer is null)
            {
                throw new ArgumentNullException(nameof(consumer));
            }

            this.Topic = topic;
            this.consumer = consumer;
        }

        public Topic Topic { get; }

        public async Task ForwardAsync(IMessage message) => await this.consumer.ConsumeAsync(message);
    }
}
