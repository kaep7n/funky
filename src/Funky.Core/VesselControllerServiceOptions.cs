using System;
using System.Collections.Generic;
using System.Linq;

namespace Funky.Core
{
    public class VesselControllerServiceOptions
    {
        public IEnumerable<FunkDefOption> FunkDefs { get; set; } = Enumerable.Empty<FunkDefOption>();
    }
}
