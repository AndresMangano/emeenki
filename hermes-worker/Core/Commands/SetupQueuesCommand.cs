using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Core.Commands
{
    public static class SetupQueuesCommand
    {
        public static void Execute<IO, dbIO>(IO io)
        where IO : IRabbitMQPort, IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleRepository, IArticlesRepository, IArticleCommentsRepository, IUserRepository, IUserPostsRepository, ITranslationRepository,
        ISentenceRepository, IUpVotesRepository, IDownVotesRepository, ITranslationCommentRepository, IArticleTemplateRepository, IArticleTemplatesRepository,
        IArticleTemplateSentenceRepository, IRoomRepository, IRoomUserRepository, IRoomQueueRepository {
            io.CreateModelAndWait(handler => {
                handler.DeclareRoute("user_queries", "user_events", (routingKey, message) => HandleUserEventCommand.Execute<IO, dbIO>(io, routingKey, message));
                handler.DeclareRoute("article_queries", "article_events", (routingKey, message) => HandleArticleEventCommand.Execute<IO, dbIO>(io, routingKey, message));
                handler.DeclareRoute("article_template_queries", "article_template_events", (routingKey, message) => HandleArticleTemplateEventCommand.Execute<IO, dbIO>(io, routingKey, message));
                handler.DeclareRoute("room_queries", "room_events", (routingKey, message) => HandleRoomEventCommand.Execute<IO, dbIO>(io, routingKey, message));
            });
        }
    }
}