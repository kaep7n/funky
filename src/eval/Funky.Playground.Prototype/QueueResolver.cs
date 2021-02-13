using System;
using System.Collections.Generic;
using System.Linq;

namespace Funky.Playground.Prototype
{
    public class QueueResolver
    {
        private readonly IEnumerable<IQueue> queues;

        public QueueResolver(IEnumerable<IQueue> queues)
            => this.queues = queues ?? throw new ArgumentNullException(nameof(queues));

        public Queue<TMessage> Resolve<TMessage>(string topic)
            => this.queues.FirstOrDefault(q => q.Topic == topic) as Queue<TMessage>;
    }
}
