using System;
using System.Threading.Tasks;

namespace Funky.Core
{
    public class Vessel : IVessel
    {
        public Vessel()
        {
        }

        public Task StartAsync() => Task.CompletedTask;

        public Task StopAsync() => Task.CompletedTask;
    }
}
