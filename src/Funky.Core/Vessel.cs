using System;
using System.Threading.Tasks;

namespace Funky.Core
{
    public class Vessel : IVessel
    {
        private readonly IFunk funk;

        public Vessel(IFunk funk) => this.funk = funk ?? throw new ArgumentNullException(nameof(funk));

        public ValueTask StartAsync() => new ValueTask();

        public ValueTask StopAsync() => new ValueTask();
    }
}
