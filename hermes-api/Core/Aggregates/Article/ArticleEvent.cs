using System;

namespace Hermes.Core
{
    public class ArticleEvent : DomainEvent<Guid, object>
    {
        public ArticleEvent(Guid id, int version, string eventName, object payload)
            : base(id, version, "article", eventName, DateTime.UtcNow, payload) {}
    }
}