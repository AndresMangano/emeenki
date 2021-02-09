using System;

namespace Hermes.Core
{
    public class ArticleTemplateEvent : DomainEvent<Guid, object>
    {
        public ArticleTemplateEvent(Guid id, int version, string stream, string eventName, DateTime timestamp, object payload)
            : base(id, version, stream, eventName, timestamp, payload) {}
    }
}