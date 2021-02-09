using System;

namespace Hermes.Core
{
    public class RoomEvent : DomainEvent<string, object>
    {
        public RoomEvent(string id, int version, string stream, string eventName, DateTime timestamp, object payload)
            : base(id, version, stream, eventName, timestamp, payload) {}
    }
}