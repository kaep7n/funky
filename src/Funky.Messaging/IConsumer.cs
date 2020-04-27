using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public interface IConsumer
    {
        Task ConsumeAsync(Message message);
    }
}
