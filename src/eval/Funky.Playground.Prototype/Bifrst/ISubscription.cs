using Bifrst;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype.Bifrst
{
    public interface ISubscription
    {
        public Guid Id { get; }

        public string Pattern { get; }

        public string Group { get; }

        ValueTask WriteAsync(Message message);
    }
}