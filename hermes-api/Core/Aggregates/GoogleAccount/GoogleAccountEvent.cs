using System;

namespace Hermes.Core
{
    public class GoogleAccountEvent : DomainEvent<string, object>
    {
        public GoogleAccountEvent(string id, int version, string stream, string eventName, DateTime timestamp, object payload)
            : base(id, version, stream, eventName, timestamp, payload) {}
    }
}