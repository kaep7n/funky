using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IFunk
    {
        ValueTask ExecuteAsync();
    }

    public interface IFunk<TMessage>
    {
        ValueTask ExecuteAsync(TMessage message);
    }
}
