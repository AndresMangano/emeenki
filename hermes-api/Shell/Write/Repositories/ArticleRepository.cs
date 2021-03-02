using System;
using Hermes.Core;
using Hermes.Core.Ports;
using Newtonsoft.Json;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : IArticlesRepository
    {
        void InitArticlesRepository()
        {
            _articlesRepository = new EventRepository<Guid, Article>(new SQLEventStorage<Guid>(
                _connection.ConnectionString,
                "Article",
                ParseArticleEvent
            ));
        }
        object ParseArticleEvent(string eventName, string payload)
        {
            switch(eventName) {
                case "main.commented": return JsonConvert.DeserializeObject<ArticleMainCommentedEvent>(payload);
                case "template.taken": return JsonConvert.DeserializeObject<ArticleTemplateTakenEvent>(payload);
                case "translated": return JsonConvert.DeserializeObject<ArticleTranslatedEvent>(payload);
                case "commented": return JsonConvert.DeserializeObject<ArticleCommentedEvent>(payload);
                case "upvoted": return JsonConvert.DeserializeObject<ArticleUpVotedEvent>(payload);
                case "upvote.removed": return JsonConvert.DeserializeObject<ArticleUpVoteRemovedEvent>(payload);
                case "downvoted": return JsonConvert.DeserializeObject<ArticleDownVotedEvent>(payload);
                case "downvote.removed": return JsonConvert.DeserializeObject<ArticleDownVoteRemovedEvent>(payload);
                case "archived": return JsonConvert.DeserializeObject<ArticleArchivedEvent>(payload);
                case "main.comment.deleted": return JsonConvert.DeserializeObject<ArticleMainCommentDeletedEvent>(payload);
                default:
                    throw new InfrastructureException("Unknown Article Event");
            }
        }
        long? ApplyArticleEvent(ArticleEvent @event) {
            var index = _articlesRepository.StoreEvent(@event);
            if (index != null) {
                SendMessage("article_events", index.Value, @event.Metadata, @event.Payload);
            }
            return index;
        }

        public Article FetchArticle(Guid id) => _articlesRepository.Fetch(id, ArticleEvents.Apply);
    }
}