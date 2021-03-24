using System;

namespace Hermes.Core
{
    public class RoomEvent : DomainEvent<string, object>
    {
        public RoomEvent(string id, int version, string eventName, object payload)
            : base(id, version, "room", eventName, DateTime.UtcNow, payload) {}
    }
}