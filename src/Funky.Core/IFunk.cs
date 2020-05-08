using Funky.Messaging;
using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IFunk
    {
        ValueTask ExecuteAsync();
    }

    public interface IFunk<TIn>
    {
        ValueTask ExecuteAsync(TIn input);
    }

    public interface IFunk<TIn, TOut>
    {
        ValueTask<TOut> ExecuteAsync(TIn input);
    }
}
