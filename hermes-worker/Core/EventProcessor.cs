using System;
using Dapper;
using Hermes.Worker.Core.Model.Events.Article;
using Hermes.Worker.Core.Model.Events.ArticleTemplate;
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
        readonly string _connectionString;
        readonly ISignalRPort _signalR;
        readonly IRabbitMQPort _rabbitMQ;
        readonly ILogger<EventProcessor> _logger;

        public EventProcessor(string connectionString, ISignalRPort singalR, IRabbitMQPort rabbitMQ, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EventProcessor>();
            _connectionString = connectionString;
            _signalR = singalR;
            _rabbitMQ = rabbitMQ;
        }

        public void Start()
        {
            _logger.LogInformation("Create model");
            _rabbitMQ.CreateModelAndWait(handler =>
            {
                handler.DeclareRoute("user_queries", "user_events", (routingKey, message) => 
                {
                    switch (routingKey)
                    {
                        case "registered": Handle(JsonConvert.DeserializeObject<UserRegisteredEvent>(message)); break;
                        case "registered.withGoogle": Handle(JsonConvert.DeserializeObject<UserRegisteredWithGoogleEvent>(message)); break;
                        case "deleted": Handle(JsonConvert.DeserializeObject<UserDeletedEvent>(message)); break;
                        case "rights.changed": Handle(JsonConvert.DeserializeObject<UserRightsChangedEvent>(message)); break;
                        case "profilePhotoChanged": Handle(JsonConvert.DeserializeObject<UserProfilePhotoChangedEvent>(message)); break;
                        case "post.added": Handle(JsonConvert.DeserializeObject<UserPostAddedEvent>(message)); break;
                        case "post.deleted": Handle(JsonConvert.DeserializeObject<UserPostDeletedEvent>(message)); break;
                        case "language.changed": Handle(JsonConvert.DeserializeObject<UserLanguageChangedEvent>(message)); break;
                        case "description.changed": Handle(JsonConvert.DeserializeObject<UserDescriptionChangedEvent>(message)); break;
                        case "country.changed": Handle(JsonConvert.DeserializeObject<UserCountryChangedEvent>(message)); break;
                    }
                });
                handler.DeclareRoute("article_queries", "article_events", (routingKey, message) => 
                {
                    switch (routingKey)
                    {
                        case "main.commented": Handle(JsonConvert.DeserializeObject<ArticleMainCommentedEvent>(message)); break;
                        case "template.taken": Handle(JsonConvert.DeserializeObject<ArticleTemplateTakenEvent>(message)); break;
                        case "translated": Handle(JsonConvert.DeserializeObject<ArticleTranslatedEvent>(message)); break;
                        case "commented": Handle(JsonConvert.DeserializeObject<ArticleCommentedEvent>(message)); break;
                        case "upvoted": Handle(JsonConvert.DeserializeObject<ArticleUpVotedEvent>(message)); break;
                        case "upvote.removed": Handle(JsonConvert.DeserializeObject<ArticleUpVoteRemovedEvent>(message)); break;
                        case "downvoted": Handle(JsonConvert.DeserializeObject<ArticleDownVotedEvent>(message)); break;
                        case "downvote.removed": Handle(JsonConvert.DeserializeObject<ArticleDownVoteRemovedEvent>(message)); break;
                        case "archived": Handle(JsonConvert.DeserializeObject<ArticleArchivedEvent>(message)); break;
                        case "main.comment.deleted": Handle(JsonConvert.DeserializeObject<ArticleMainCommentDeletedEvent>(message)); break;
                    }
                });
                handler.DeclareRoute("article_template_queries", "article_template_events", (routingKey, message) =>
                {
                    switch (routingKey)
                    {
                        case "uploaded": Handle(JsonConvert.DeserializeObject<ArticleTemplateDeletedEvent>(message)); break;
                        case "deleted": Handle(JsonConvert.DeserializeObject<ArticleTemplateUploadedEvent>(message)); break;
                    }
                });
                handler.DeclareRoute("room_queries", "room_events", (routingKey, message) =>
                {
                    switch (routingKey)
                    {
                        case "opened": Handle(JsonConvert.DeserializeObject<RoomOpenedEvent>(message)); break;
                        case "closed": Handle(JsonConvert.DeserializeObject<RoomClosedEvent>(message)); break;
                        case "user.queued": Handle(JsonConvert.DeserializeObject<RoomUserQueuedEvent>(message)); break;
                        case "user.joined": Handle(JsonConvert.DeserializeObject<RoomUserJoinedEvent>(message)); break;
                        case "user.unqueued": Handle(JsonConvert.DeserializeObject<RoomUserUnqueuedEvent>(message)); break;
                        case "user.left": Handle(JsonConvert.DeserializeObject<RoomUserLeftEvent>(message)); break;
                        case "user.expelled": Handle(JsonConvert.DeserializeObject<RoomUserExpelledEvent>(message)); break;
                        case "usersLimit.changed": Handle(JsonConvert.DeserializeObject<RoomUsersLimitChangedEvent>(message)); break;
                        case "restricted": Handle(JsonConvert.DeserializeObject<RoomRestrictedEvent>(message)); break;
                        case "unrestricted": Handle(JsonConvert.DeserializeObject<RoomUnrestrictedEvent>(message)); break;
                    }
                });
            });
        }

        public void Handle<K>(IEvent<K> @event)
        {
            _logger.LogInformation("Handle event {eventName}", @event.Header.EventName);
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
                                stream = @event.Header.Stream,
                                seqId = @event.Header.Index
                            });
                        tran.Commit();

                        _logger.LogInformation("Notify changes to UI");
                        @event.Notify(_signalR);
                    } catch (Exception ex) {
                        _logger.LogError(ex, "Failed to apply event changes");

                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}