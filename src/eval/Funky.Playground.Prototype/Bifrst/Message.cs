using System;

namespace Bifrst
{
    public record Message(Guid Id, long Offset, object Payload);
}
