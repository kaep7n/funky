using System;

namespace Funky.Messaging
{
    public interface IMessage
    {
        Guid CorrelationId { get; }

        public Topic Topic { get; }
    }
}
