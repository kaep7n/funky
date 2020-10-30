using System;
using System.Collections.Generic;

namespace Funky.Core.Events
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
    public record ConfigurationChanged(DateTimeOffset Timestamp, Dictionary<string, string> Value);
}
