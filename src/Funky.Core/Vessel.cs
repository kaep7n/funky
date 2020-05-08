using Funky.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Funky.Core
{
    public class Vessel : IVessel, IConsumer
    {
        private readonly IFunk funk;

        private readonly Dictionary<MethodInfo, IEnumerable<Topic>> methodTopicMap = new Dictionary<MethodInfo, IEnumerable<Topic>>();

        public Vessel(IFunk funk)
        {
            this.funk = funk ?? throw new ArgumentNullException(nameof(funk));
        }

        public Task StartAsync()
        {
            foreach (var method in this.funk.GetType().GetMethods())
            {
                var acceptedTopics = method.GetCustomAttributes<AcceptsTopicAttribute>().Select(t => t.Topic);

                if (!acceptedTopics.Any())
                    continue;

                this.methodTopicMap.Add(method, acceptedTopics);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync() => Task.CompletedTask;

        public async Task ConsumeAsync(Message message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            foreach (var method in this.methodTopicMap.Where(m => m.Value.Contains(message.Topic)).Select(m => m.Key))
            {
                var getAwaiter = method.ReturnType.GetMethod(nameof(Task.GetAwaiter));
                var isAwaitable = getAwaiter != null;

                var parameters = method.GetParameters();

                if (parameters.Length == 0)
                {
                    if (isAwaitable)
                    {
                        await ((Task)method.Invoke(this.funk, Array.Empty<object>()))
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        method.Invoke(this.funk, Array.Empty<object>());
                    }
                }
                else if (parameters.Length == 1)
                {
                    if (parameters[0].ParameterType == message.Payload.GetDataType())
                    {
                        if (isAwaitable)
                        {
                            await (ValueTask)method.Invoke(this.funk, new[] { message.Payload.GetData() });
                        }
                        else
                        {
                            method.Invoke(this.funk, new[] { message.Payload.GetData() });
                        }
                    }
                }
            }
        }
    }
}
