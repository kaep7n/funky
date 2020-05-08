using Funky.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Funky.Core
{
    public class Vessel : IVessel, IConsumer
    {
        private readonly IFunk funk;
        private readonly List<Topic> acceptedTopics = new List<Topic>();

        public Vessel(IFunk funk) => this.funk = funk ?? throw new ArgumentNullException(nameof(funk));

        public ValueTask StartAsync()
        {
            var acceptedTopics = this.funk.GetType()
                .GetCustomAttributes<AcceptsTopicAttribute>()
                .Select(a => a.Topic);

            this.acceptedTopics.AddRange(acceptedTopics);

            return new ValueTask();
        }

        public ValueTask StopAsync()
        {
            this.acceptedTopics.Clear();

            return new ValueTask();
        }

        public async Task ConsumeAsync(Message message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            if (!this.acceptedTopics.Contains(message.Topic))
            {
                return;
            }

            await this.funk.ExecuteAsync()
                .ConfigureAwait(false);
        }
    }
}
