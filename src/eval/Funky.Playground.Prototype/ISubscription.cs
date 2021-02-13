using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public interface ISubscription
    {
        ValueTask EnableAsync();

        ValueTask DisableAsync();
    }
}
