using Funky.Core;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class EmptyFunk : IFunk
    {
        public Task DisableAsync() => Task.CompletedTask;

        public Task EnableAsync() => Task.CompletedTask;
    }
}
