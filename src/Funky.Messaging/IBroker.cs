using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public interface IBroker
    {
        Task StartAsync(CancellationToken cancellationToken = default);

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
