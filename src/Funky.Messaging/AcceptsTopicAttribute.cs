using System;

namespace Funky.Messaging
{
    public class AcceptsTopicAttribute : Attribute
    {
        public AcceptsTopicAttribute(string topic)
        {
            if (topic is null)
                throw new ArgumentNullException(nameof(topic));

            this.Topic = new Topic(topic);
        }

        public Topic Topic { get; }
    }
}
