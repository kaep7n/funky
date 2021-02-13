using System;

namespace Funky.Playground.Prototype
{
    public record TimerFired(DateTimeOffset FiredAt) : IMessage;

    public record TimerFiredForwarded(DateTimeOffset FiredAt) : IMessage;
}