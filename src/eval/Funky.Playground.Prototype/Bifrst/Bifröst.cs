using Bifrst;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype.Bifrst
{
    public class Bifröst
    {
        private readonly ConcurrentDictionary<string, TopicStream> topicStreams = new();
        private readonly List<IStreamSubscription> subscribers = new();

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

                        foreach (var sub in subs)
                        {
                            try
                            {
                                await sub.WriteAsync(message);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                });

                return topicStream;
            });

            await topicStream.WriteAsync(payload);
        }

        public ValueTask SubscribeAsync(IStreamSubscription subscription)
        {
            this.subscribers.Add(subscription);

            return ValueTask.CompletedTask;
        }

        public ValueTask UnsubscribeAsync(IStreamSubscription subscription)
        {
            this.subscribers.Remove(subscription);

            return ValueTask.CompletedTask;
        }
    }
}