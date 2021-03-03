using System;

namespace Hermes.Worker.Core
{
    public record EventHeader(
        long Index,
        int Version,
        DateTime Timestamp,
        string Stream,
        string EventName
    );
}