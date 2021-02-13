using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public interface IFunkTrigger
    {
        ValueTask EnableAsync();

        ValueTask DisableAsync();
    }
}
