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
        private readonly List<ISubscription> subscribers = new();

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

                        var grouplessSubs = subs.Where(s => s.Group is null);

                        foreach (var sub in grouplessSubs)
                        {
                            try
                            {
                                await sub.WriteAsync(message);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        var subGroups = subs
                            .Where(s => s.Group is not null)
                            .GroupBy(s => s.Group);

                        foreach (var subGroup in subGroups)
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
}