using System;

namespace Hermes.Core
{
    public class UserEvent : DomainEvent<string, object>
    {
        public UserEvent(string id, int version, string stream, string eventName, DateTime timestamp, object payload)
            : base(id.ToLower(), version, stream, eventName, timestamp, payload) {}
    }
}