using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public interface IFunk<T>
    {
        ValueTask ExecuteAsync(T message);
    }
}
