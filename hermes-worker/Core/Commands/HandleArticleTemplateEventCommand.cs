using System;
using Hermes.Worker.Core.Model;
using Hermes.Worker.Core.Model.Events.ArticleTemplate;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;
using Newtonsoft.Json;

namespace Hermes.Worker.Core.Commands
{
    public static class HandleArticleTemplateEventCommand
    {
        public static void Execute<IO, dbIO>(IO io, string routingKey, string message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleTemplateSentenceRepository, IArticleTemplatesRepository, IArticleTemplateRepository {
            switch (routingKey) {
                case "uploaded": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleTemplateUploadedEvent>>(message).Message); break;
                case "deleted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleTemplateDeletedEvent>>(message).Message); break;
            }
        }
        // Event Handelers
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleTemplateUploadedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleTemplatesRepository, IArticleTemplateRepository, IArticleTemplateSentenceRepository {
            io.Transaction(dbIO => {
                dbIO.InsertArticleTemplates(
                    articleTemplateID: message.Metadata.ID,
                    title: String.Join(" ", message.Payload.Title),
                    created: message.Metadata.Timestamp,
                    languageID: message.Payload.LanguageID,
                    photoURL: message.Payload.PhotoURL,
                    archived: false
                );
                dbIO.InsertArticleTemplate(
                    articleTemplateID: message.Metadata.ID,
                    deleted: false,
                    languageID: message.Payload.LanguageID,
                    source: message.Payload.Source,
                    photoURL: message.Payload.PhotoURL,
                    timestamp: message.Metadata.Timestamp
                );
                for (var index = 0; index < message.Payload.Text.Count; index++) {
                    dbIO.InsertArticleTemplateSentence(
                        articleTemplateID: message.Metadata.ID,
                        inText: true,
                        sentenceIndex: index,
                        sentence: message.Payload.Text[index]
                    );
                }
                for (var index = 0; index < message.Payload.Title.Count; index++) {
                    dbIO.InsertArticleTemplateSentence(
                        articleTemplateID: message.Metadata.ID,
                        inText: false,
                        sentenceIndex: index,
                        sentence: message.Payload.Title[index]
                    );
                }
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_TEMPLATE_UPDATED, message.Metadata.ID.ToString(), "articleTemplates");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleTemplateDeletedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleTemplateRepository, IArticleTemplatesRepository {
            io.Transaction(dbIO => {
                dbIO.UpdateArticleTemplate(message.Metadata.ID,
                    deleted: new DbUpdate<bool>(true));
                dbIO.UpdateArticleTemplates(message.Metadata.ID,
                    archived: new DbUpdate<bool>(true));
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_TEMPLATE_UPDATED, message.Metadata.ID.ToString(), "articleTemplates");
        }
    }
}