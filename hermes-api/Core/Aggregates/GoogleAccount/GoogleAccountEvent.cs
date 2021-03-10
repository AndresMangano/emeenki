using System;

namespace Hermes.Core
{
    public class GoogleAccountEvent : DomainEvent<string, object>
    {
        public GoogleAccountEvent(string id, int version, string eventName, object payload)
            : base(id, version, "googleAccounts", eventName, DateTime.UtcNow, payload) {}
    }
}