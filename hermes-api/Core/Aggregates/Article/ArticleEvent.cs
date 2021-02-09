using System;

namespace Hermes.Core
{
    public class ArticleEvent : DomainEvent<Guid, object>
    {
        public ArticleEvent(Guid id, int version, string stream, string eventName, DateTime timestamp, object payload)
            : base(id, version, stream, eventName, timestamp, payload) {}
    }
}