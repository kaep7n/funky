using Funky.Core;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class EmptyFunk : IFunk
    {
        public ValueTask ExecuteAsync() => new ValueTask();
    }
}
