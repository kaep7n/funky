using Funky.Core;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    [Startup(StartupType = typeof(EmptyStartup))]
    public class EmptyFunk : IFunk
    {
        public ValueTask ExecuteAsync() => new();
    }
}
