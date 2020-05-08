using System;
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
            var payload = new JsonPayload(value);
            var message = new Message(topic, payload);

            await this.distributor.EnqueueAsync(message);
        }
    }
}
