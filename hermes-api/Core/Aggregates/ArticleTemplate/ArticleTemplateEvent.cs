using System;

namespace Hermes.Core
{
    public class ArticleTemplateEvent : DomainEvent<Guid, object>
    {
        public ArticleTemplateEvent(Guid id, int version, string eventName, object payload)
            : base(id, version, "articleTemplate", eventName, DateTime.UtcNow, payload) {}
    }
}