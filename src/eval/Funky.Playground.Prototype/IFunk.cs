using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public interface IFunk<T>
        where T : IMessage
    {
        ValueTask ExecuteAsync(T message);
    }

    public interface IFunk : IFunk<Noop>
    {
    }

    public struct Noop : IMessage
    {
    }
}
