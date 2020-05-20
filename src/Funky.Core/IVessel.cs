
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IVessel
    {
        ValueTask StartAsync();

        ValueTask StopAsync();
    }
}
