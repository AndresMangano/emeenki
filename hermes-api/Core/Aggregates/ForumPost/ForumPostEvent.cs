using System;

namespace Hermes.Core
{
    public class ForumPostEvent : DomainEvent<Guid, object>
    {
        public ForumPostEvent(Guid id, int version, string eventName, object payload)
            : base(id, version, "forumPost", eventName, DateTime.UtcNow, payload) {}
    }
}