using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public class Broker : IBroker
    {
        public Task StartAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task StopAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
