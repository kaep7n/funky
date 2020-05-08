using System;

namespace Funky.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
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
