using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public interface ISubscription
    {
        Topic Topic { get; }

        Task ForwardAsync(Message message);
    }
}
