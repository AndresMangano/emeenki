using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hermes.Worker.Core.Model.Events.Article;
using Hermes.Worker.Core.Model.Events.ArticleTemplate;
using Hermes.Worker.Core.Model.Events.ForumPost;
using Hermes.Worker.Core.Model.Events.Room;
using Hermes.Worker.Core.Model.Events.User;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Hermes.Worker.Core
{
    public class EventProcessor
    {
        readonly ILogger<EventProcessor> _logger;
        readonly string _connectionString;
        readonly ISignalRPort _signalR;
        readonly IRabbitMQPort _rabbitMQ;
        readonly IEventStore _eventStore;
        readonly IDictionary<string, EventQueue> _eventQueues;

        public EventProcessor(string connectionString, ISignalRPort singalR, IRabbitMQPort rabbitMQ, IEventStore eventStore, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EventProcessor>();
            _connectionString = connectionString;
            _signalR = singalR;
            _rabbitMQ = rabbitMQ;
            _eventStore = eventStore;
            _eventQueues = new Dictionary<string, EventQueue>
            {
                ["user"] = new EventQueue("user_queries", "user_events", ParseUserEvent),
                ["article"] = new EventQueue("article_queries", "article_events", ParseArticleEvent),
                ["articletemplate"] = new EventQueue("article_template_queries", "article_template_events", ParseArticleTemplateEvent),
                ["room"] = new EventQueue("room_queries", "room_events", ParseRoomEvent),
                ["forumpost"] = new EventQueue("forum_post_queries", "forum_post_events", ParseForumPostEvent)
            };
        }

        public async Task Start()
        {
            await LoadEventCounters();
            await RecoverEvents();
            _rabbitMQ.CreateModelAndWait(handler =>
            {
                foreach (var (_, queue) in _eventQueues) {
                    handler.DeclareRoute(queue.Name, queue.Exchange, async (routingKey, message) => 
                        await Handle(queue.ParseFn(routingKey, message), false));
                }
            });
        }

        private async Task Handle(IEvent @event, bool isRecovering)
        {
            if (@event != null) {
                var stream = @event.Header.Stream.ToLower();
                var index = _eventQueues[stream].Index;
                _logger.LogInformation("Handle event {stream}:{eventName}[{index}]", stream, @event.Header.EventName, @event.Header.Index);
                if (@event.Header.Index == index + 1 || isRecovering) {                    
                    using(MySqlConnection conn = new MySqlConnection(_connectionString)) {
                        conn.Open();
                        using(MySqlTransaction tran = conn.BeginTransaction()) {
                            try {
                                _logger.LogInformation("Apply event changes");
                                @event.Apply(new DBInterpreter(conn, tran));

                                _logger.LogInformation("Update handler counters");
                                conn.Execute(@"
                                    INSERT INTO Worker_Handlers(Stream, SeqID)
                                    VALUES (@stream, @seqId)
                                        ON DUPLICATE KEY UPDATE SeqID = @seqId", new {
                                        stream = @event.Header.Stream.ToLower(),
                                        seqId = @event.Header.Index
                                    });
                                tran.Commit();
                                _eventQueues[@event.Header.Stream.ToLower()].Index = @event.Header.Index;

                                if (!isRecovering)
                                    @event.Notify(_signalR);
                            } catch (Exception ex) {
                                _logger.LogError(ex, "Failed to apply event changes");

                                tran.Rollback();
                                throw;
                            }
                        }
                    }
                }
                else if (@event.Header.Index <= index) {
                    _logger.LogWarning("Event {stream}:{index} was discarded", @event.Header.Stream, @event.Header.Index);
                }
                else if (!isRecovering) {
                    await RecoverEvents();
                }
            }
            else {
                _logger.LogWarning("Event was not recognized");
            }
        }

        private async Task RecoverEvents()
        {
            try {
                foreach (var (stream, queue) in _eventQueues) {
                    _logger.LogInformation("Recover {stream} events starting at {index}", stream, queue.Index);
                    var events = await _eventStore.GetMissingEvents(stream, queue.Index);
                    foreach (var @event in events) {
                        await Handle(queue.ParseFn(@event.RoutingKey, @event.Payload), true);
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Failed to recover events");
            }
        }

        private async Task LoadEventCounters()
        {
            _logger.LogInformation("Load event counters");
            using(MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                try {
                    var result = await conn.QueryAsync(@"
                        SELECT Stream, SeqID
                        FROM Worker_Handlers");

                    foreach (var row in result) {
                        _eventQueues[row.Stream].Index = row.SeqID;
                    }
                } catch (Exception ex) {
                    _logger.LogError(ex, "Failed to load event counters");
                }
            }
        }

        private IEvent ParseArticleEvent(string routingKey, string message)
        {
            switch (routingKey)
            {
                case "main.commented": return JsonConvert.DeserializeObject<ArticleMainCommentedEvent>(message);
                case "template.taken": return JsonConvert.DeserializeObject<ArticleTemplateTakenEvent>(message);
                case "translated": return JsonConvert.DeserializeObject<ArticleTranslatedEvent>(message);
                case "commented": return JsonConvert.DeserializeObject<ArticleCommentedEvent>(message);
                case "upvoted": return JsonConvert.DeserializeObject<ArticleUpVotedEvent>(message);
                case "upvote.removed": return JsonConvert.DeserializeObject<ArticleUpVoteRemovedEvent>(message);
                case "downvoted": return JsonConvert.DeserializeObject<ArticleDownVotedEvent>(message);
                case "downvote.removed": return JsonConvert.DeserializeObject<ArticleDownVoteRemovedEvent>(message);
                case "archived": return JsonConvert.DeserializeObject<ArticleArchivedEvent>(message);
                case "main.comment.deleted": return JsonConvert.DeserializeObject<ArticleMainCommentDeletedEvent>(message);
                default:
                    return null;
            }
        }
private IEvent ParseArticleTemplateEvent(string routingKey, string message)
{
    switch (routingKey)
    {
        case "uploaded":
            return JsonConvert.DeserializeObject<ArticleTemplateUploadedEvent>(message);

        case "deleted":
            return JsonConvert.DeserializeObject<ArticleTemplateDeletedEvent>(message);

        case "video-uploaded": // 👈 NEW CASE
            return JsonConvert.DeserializeObject<ArticleTemplateVideoUploadedEvent>(message);

        default:
            return null;
    }
}


        private IEvent ParseUserEvent(string routingKey, string message)
        {
            switch (routingKey)
            {
                case "registered": return JsonConvert.DeserializeObject<UserRegisteredEvent>(message);
                case "registered.withGoogle": return JsonConvert.DeserializeObject<UserRegisteredWithGoogleEvent>(message);
                case "deleted": return JsonConvert.DeserializeObject<UserDeletedEvent>(message);
                case "rights.changed": return JsonConvert.DeserializeObject<UserRightsChangedEvent>(message);
                case "profilePhotoChanged": return JsonConvert.DeserializeObject<UserProfilePhotoChangedEvent>(message);
                case "post.added": return JsonConvert.DeserializeObject<UserPostAddedEvent>(message);
                case "post.deleted": return JsonConvert.DeserializeObject<UserPostDeletedEvent>(message);
                case "language.changed": return JsonConvert.DeserializeObject<UserLanguageChangedEvent>(message);
                case "description.changed": return JsonConvert.DeserializeObject<UserDescriptionChangedEvent>(message);
                case "country.changed": return JsonConvert.DeserializeObject<UserCountryChangedEvent>(message);
                default:
                    return null;
            }
        }

        private IEvent ParseRoomEvent(string routingKey, string message)
        {
            switch (routingKey)
            {
                case "opened": return JsonConvert.DeserializeObject<RoomOpenedEvent>(message);
                case "closed": return JsonConvert.DeserializeObject<RoomClosedEvent>(message);
                case "user.queued": return JsonConvert.DeserializeObject<RoomUserQueuedEvent>(message);
                case "user.joined": return JsonConvert.DeserializeObject<RoomUserJoinedEvent>(message);
                case "user.unqueued": return JsonConvert.DeserializeObject<RoomUserUnqueuedEvent>(message);
                case "user.left": return JsonConvert.DeserializeObject<RoomUserLeftEvent>(message);
                case "user.expelled": return JsonConvert.DeserializeObject<RoomUserExpelledEvent>(message);
                case "usersLimit.changed": return JsonConvert.DeserializeObject<RoomUsersLimitChangedEvent>(message);
                case "restricted": return JsonConvert.DeserializeObject<RoomRestrictedEvent>(message);
                case "unrestricted": return JsonConvert.DeserializeObject<RoomUnrestrictedEvent>(message);
                default:
                    return null;
            }
        }

        private IEvent ParseForumPostEvent(string routingKey, string message)
        {
            switch(routingKey) {
                case "created": return JsonConvert.DeserializeObject<ForumPostCreatedEvent>(message);
                case "edited": return JsonConvert.DeserializeObject<ForumPostEditedEvent>(message);
                case "deleted": return JsonConvert.DeserializeObject<ForumPostDeletedEvent>(message);
                case "commented": return JsonConvert.DeserializeObject<ForumPostCommentedEvent>(message);
                case "comment.deleted": return JsonConvert.DeserializeObject<ForumPostCommentDeletedEvent>(message);
                default:
                    return null;
            }
        }
    }
}