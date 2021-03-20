using Bifrst;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype.Bifrst
{
    public interface IStreamSubscription
    {
        public Guid Id { get; }

        public string Pattern { get; }

        ValueTask WriteAsync(Message message, CancellationToken cancellationToken = default);
    }
}