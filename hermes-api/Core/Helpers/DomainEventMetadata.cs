using System;

namespace Hermes.Core
{
    public class DomainEventMetadata<TKey>
    {
        public TKey ID { get; }
        public int Version { get; }
        public DateTime Timestamp { get; }
        public string Stream { get; }
        public string EventName { get; }

        public DomainEventMetadata(TKey id, int version, DateTime timestamp, string stream, string eventName)
        {
            ID = id;
            Version = version;
            Timestamp = timestamp;
            Stream = stream;
            EventName = eventName;
        }
    }
}