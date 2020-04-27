using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public class JsonProducer : IProducer
    {
        private readonly IDistributor distributor;

        public JsonProducer(IDistributor distributor)
        {
            if (distributor is null)
            {
                throw new ArgumentNullException(nameof(distributor));
            }

            this.distributor = distributor;
        }

        public async Task ProduceAsync<TValue>(Topic topic, TValue value, CancellationToken cancellationToken = default)
        {
            var serializedPayload = JsonSerializer.Serialize(value);
            var payload = Encoding.UTF8.GetBytes(serializedPayload);

            var message = new Message(topic, payload);

            await this.distributor.EnqueueAsync(message);
        }
    }
}
