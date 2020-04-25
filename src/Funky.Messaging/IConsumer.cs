using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public interface IConsumer
    {
        IEnumerable<Topic> GetAcceptedTopics();

        Task ConsumeAsync(IMessage message);
    }
}
