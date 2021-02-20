using System.Collections.Generic;

namespace Funky.Playground.Prototype
{
    public class QueueResolver
    {
        private readonly Dictionary<string, IQueue> queues = new();

        public IQueue<TMessage> Resolve<TMessage>(string topic)
        {
            if (!this.queues.TryGetValue(topic, out var queue))
            {
                queue = new InMemeoryQueue<TMessage>();
                this.queues.Add(topic, queue);
            }

            return queue.Unwrap<TMessage>();
        }
    }
}
