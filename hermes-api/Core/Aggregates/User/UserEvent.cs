using System;

namespace Hermes.Core
{
    public class UserEvent : DomainEvent<string, object>
    {
        public UserEvent(string id, int version, string eventName, object payload)
            : base(id.ToLower(), version, "user", eventName, DateTime.UtcNow, payload) {}
    }
}