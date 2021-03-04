using Bifrst;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype.Bifrst
{
    public class Bifröst
    {
        private readonly ConcurrentDictionary<string, TopicStream> topicStreams = new();
        private readonly List<ISubscription> subscribers = new();

        public Bifröst()
        {
        }

        public async ValueTask PublishAsync(string topic, object payload)
            => await this.PublishAsync(topic, payload, new TopicOptions(Capacity: null));

        public async ValueTask PublishAsync(string topic, object payload, TopicOptions options)
        {
            var topicStream = this.topicStreams.GetOrAdd(topic, (key) =>
            {
                var topicStream = new TopicStream(key, options);

                Task.Run(async () =>
                {
                    await foreach (var message in topicStream.ReadAllAsync())
                    {
                        var subs = this.subscribers.Where(t => Regex.IsMatch(topicStream.Key, t.Pattern));

                        foreach (var subGroup in subs.GroupBy(s => s.Group))
                        {
                            foreach (var sub in subGroup)
                            {
                                try
                                {
                                    await sub.WriteAsync(message);
                                    break;
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                });

                return topicStream;
            });

            await topicStream.WriteAsync(payload);
        }

        public ValueTask SubscribeAsync(ISubscription subscription)
        {
            this.subscribers.Add(subscription);

            return ValueTask.CompletedTask;
        }

        public ValueTask UnsubscribeAsync(ISubscription subscription)
        {
            this.subscribers.Remove(subscription);

            return ValueTask.CompletedTask;
        }
    }

    public interface ISubscription
    {
        public string Pattern { get; }

        public string Group { get; }

        ValueTask WriteAsync(object message);
    }

    public class Subscription : ISubscription
    {
        private readonly Channel<object> stream = Channel.CreateUnbounded<object>();

        public Subscription(string pattern, string group)
        {
            this.Pattern = pattern;
            this.Group = group;
        }

        public string Pattern { get; }

        public string Group { get; }

        public async ValueTask WriteAsync(object message) => await this.stream.Writer.WriteAsync(message);
    }
}