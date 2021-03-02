using System;
using System.Collections.Generic;
using Hermes.Core;
using Hermes.Core.Ports;
using Newtonsoft.Json;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : IArticleTemplatesRepository
    {
        void InitArticleTemplatesRepository()
        {
            _articleTemplatesRepository = new EventRepository<Guid, ArticleTemplate>(new SQLEventStorage<Guid>(
                _connection.ConnectionString,
                "ArticleTemplate",
                ParseArticleTemplateEvent
            ));
        }
        object ParseArticleTemplateEvent(string eventName, string payload)
        {
            switch(eventName) {
                case "uploaded": return JsonConvert.DeserializeObject<ArticleTemplateUploadedEvent>(payload);
                case "deleted": return JsonConvert.DeserializeObject<ArticleTemplateDeletedEvent>(payload);
                default:
                    throw new InfrastructureException("Unknown Article Template Event");
            }
        }
        long? ApplyArticleTemplateEvent(ArticleTemplateEvent @event) {
            var index = _articleTemplatesRepository.StoreEvent(@event);
            if (index != null) {
                SendMessage("article_template_events", index.Value, @event.Metadata, @event.Payload);
            }
            return index;
        }

        public ArticleTemplate FetchArticleTemplate(Guid id) => _articleTemplatesRepository.Fetch(id, ArticleTemplateEvents.Apply);
    }
}