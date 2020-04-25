using System.Threading.Tasks;

namespace Funky.Core
{
    public interface IFunk
    {
        Task EnableAsync();

        Task DisableAsync();
    }
}
