using System.Collections.Generic;
using System.Linq;

namespace Funky.Core
{
    public class FunkDefOption
    {
        public string Type { get; set; }

        public IEnumerable<string> Topics { get; set; } = Enumerable.Empty<string>();
    }
}
