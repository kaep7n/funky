using System;

namespace Funky.Playground.Kafka
{
    public record HelloFromProcess (string ProcessId, int Sequence, DateTimeOffset CreatedAtUtc);
}
