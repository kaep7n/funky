using System;

namespace Funky.Messaging
{
    public class Message
    {
        public Message(Topic topic, byte[] payload)
        {
            this.CorrelationId = Guid.NewGuid();
            this.Topic = topic;
            this.Payload = payload;
        }

        public Guid CorrelationId { get; }

        public Topic Topic { get; }

        public byte[] Payload { get; }
    }
}
