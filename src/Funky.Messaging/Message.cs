using System;

namespace Funky.Messaging
{
    public class Message
    {
        public Message(Topic topic, IPayload payload)
        {
            this.CorrelationId = Guid.NewGuid();
            this.Topic = topic;
            this.Payload = payload ?? throw new ArgumentNullException(nameof(payload));
        }

        public Guid CorrelationId { get; }

        public Topic Topic { get; }

        public IPayload Payload { get; }
    }
}
