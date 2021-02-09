using System;

namespace Hermes.Core
{
    public class DomainEvent<TKey, TPayload> : IDomainEvent
    {
        public DomainEventMetadata<TKey> Metadata { get; }
        public TPayload Payload { get; }

        public DomainEvent(TKey id, int version, string stream, string eventName, DateTime timestamp, TPayload payload)
        {
            Metadata = new DomainEventMetadata<TKey>(
                id: id,
                version: version,
                timestamp: timestamp,
                stream: stream,
                eventName: eventName
            );
            Payload = payload;
        }
    }
}