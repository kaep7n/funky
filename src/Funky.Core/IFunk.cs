using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IFunk
    {
        ValueTask ExecuteAsync(object input);
    }
}
