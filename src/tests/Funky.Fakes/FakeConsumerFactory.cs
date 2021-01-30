using Funky.Core.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class FakeConsumerFactory : IConsumerFactory
    {
        private readonly List<IConsumer> consumers = new List<IConsumer>();

        public IConsumer<T> Create<T>(string topic)
        {
            var consumer = new FakeConsumer<T>(topic);

            this.consumers.Add(consumer);

            return consumer;
        }

        public async Task SendToTopicAsync<T>(string topic, T evt)
        {
           var consumersForTopic = this.consumers
                .Where(c => c.Topic == topic)
                .Cast<FakeConsumer<T>>();

            foreach (var consumer in consumersForTopic)
            {
                await consumer.SendAsync(evt)
                    .ConfigureAwait(false);
            }
        }
    }
}
