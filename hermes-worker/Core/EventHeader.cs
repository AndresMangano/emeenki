using System;

namespace Hermes.Worker.Core
{
    public record EventHeader<TKey>(
        long Index,
        TKey ID,
        int Version,
        DateTime Timestamp,
        string Stream,
        string EventName
    );
}