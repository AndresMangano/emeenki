using System;
using Hermes.Core;
using Hermes.Core.Ports;
using Newtonsoft.Json;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : IForumPostRepository
    {
        void InitForumPostsRepository()
        {
            ForumPostsRepository = new EventRepository<Guid, ForumPost>(new SQLEventStorage<Guid>(
                _connection.ConnectionString,
                "ForumPost",
                ParseForumPostEvent
            ));
        }

        object ParseForumPostEvent(string eventName, string payload)
        {
            switch(eventName) {
                case "created": return JsonConvert.DeserializeObject<ForumPostCreatedEvent>(payload);
                case "edited": return JsonConvert.DeserializeObject<ForumPostEditedEvent>(payload);
                case "deleted": return JsonConvert.DeserializeObject<ForumPostDeletedEvent>(payload);
                case "commented": return JsonConvert.DeserializeObject<ForumPostCommentedEvent>(payload);
                case "comment.deleted": return JsonConvert.DeserializeObject<ForumPostCommentDeletedEvent>(payload);
                default:
                    throw new InfrastructureException("Unknown Article Event");
            }
        }

        long? ApplyForumPostEvent(ForumPostEvent @event) {
            var index = ForumPostsRepository.StoreEvent(@event);
            if (index != null) {
                SendMessage(
                    "forum_post_events",
                    index.Value,
                    @event.Metadata,
                    @event.Payload,
                    obj => obj.Add("ID", @event.Metadata.ID));
            }
            return index;
        }
        
        public ForumPost FetchForumPost(Guid id) => ForumPostsRepository.Fetch(id, ForumPostEvents.Apply);
    }
}