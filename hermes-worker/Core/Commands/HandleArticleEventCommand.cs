using System;
using Hermes.Worker.Core.Model;
using Hermes.Worker.Core.Model.Events.Article;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;
using Newtonsoft.Json;

namespace Hermes.Worker.Core.Commands
{
    public static class HandleArticleEventCommand
    {
        public static void Execute<IO, dbIO>(IO io, string routingKey, string message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleCommentsRepository, IArticlesRepository, IArticleRepository, ITranslationCommentRepository, IDownVotesRepository, IUpVotesRepository,
        ISentenceRepository, ITranslationRepository {
            switch (routingKey) {
                case "main.commented": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleMainCommentedEvent>>(message).Message); break;
                case "template.taken": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleTemplateTakenEvent>>(message).Message); break;
                case "translated": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleTranslatedEvent>>(message).Message); break;
                case "commented": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleCommentedEvent>>(message).Message); break;
                case "upvoted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleUpVotedEvent>>(message).Message); break;
                case "upvote.removed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleUpVoteRemovedEvent>>(message).Message); break;
                case "downvoted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleDownVotedEvent>>(message).Message); break;
                case "downvote.removed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleDownVoteRemovedEvent>>(message).Message); break;
                case "archived": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleArchivedEvent>>(message).Message); break;
                case "main.comment.deleted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<Guid, ArticleMainCommentDeletedEvent>>(message).Message); break;
            }
        }
        // Event Handlers
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleMainCommentedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleCommentsRepository {
            io.Execute(dbIO => {
                dbIO.InsertArticleComment(
                    articleID: message.Metadata.ID,
                    commentIndex: message.Payload.CommentPos,
                    childCommentIndex: message.Payload.ChildCommentPos,
                    comment: message.Payload.Comment,
                    userID: message.Payload.UserID,
                    timestamp: message.Metadata.Timestamp
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleTemplateTakenEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticlesRepository, IArticleRepository, ISentenceRepository {
            io.Transaction(dbIO => {
                dbIO.InsertArticles(
                    articleID: message.Metadata.ID,
                    roomID: message.Payload.RoomID,
                    title: String.Join(" ", message.Payload.Title),
                    created: message.Metadata.Timestamp,
                    originalLanguageID: message.Payload.OriginalLanguageID,
                    translationLanguageID: message.Payload.TranslationLanguageID,
                    photoURL: message.Payload.PhotoURL,
                    archived: false
                );
                dbIO.InsertArticle(
                    articleID: message.Metadata.ID,
                    articleTemplateID: message.Payload.ArticleTemplateID,
                    archived: false,
                    roomID: message.Payload.RoomID,
                    originalLanguageID: message.Payload.OriginalLanguageID,
                    translationLanguageID: message.Payload.TranslationLanguageID,
                    source: message.Payload.Source,
                    photoURL: message.Payload.PhotoURL,
                    timestamp: message.Metadata.Timestamp
                );
                for (var index = 0; index < message.Payload.Text.Count; index++) {
                    dbIO.InsertSentence(
                        articleID: message.Metadata.ID,
                        inText: true,
                        sentenceIndex: index,
                        originalText: message.Payload.Text[index]
                    );
                }
                for (var index = 0; index < message.Payload.Title.Count; index++) {
                    dbIO.InsertSentence(
                        articleID: message.Metadata.ID,
                        inText: false,
                        sentenceIndex: index,
                        originalText: message.Payload.Title[index]
                    );
                }
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), "articles");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleTranslatedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : ITranslationRepository {
            io.Execute(dbIO => {
                dbIO.InsertTranslation(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    translation: message.Payload.Translation,
                    userID: message.Payload.UserID,
                    timestamp: message.Metadata.Timestamp
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleCommentedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : ITranslationCommentRepository {
            io.Execute(dbIO => {
                dbIO.InsertTranslationComment(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    commentIndex: message.Payload.CommentPos,
                    comment: message.Payload.Comment,
                    userID: message.Payload.UserID,
                    timestamp: message.Metadata.Timestamp
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleUpVotedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUpVotesRepository, IDownVotesRepository {
            io.Transaction(dbIO => {
                dbIO.InsertUpVote(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    userID: message.Payload.UserID
                );
                dbIO.DeleteDownVote(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    userID: message.Payload.UserID
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleUpVoteRemovedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUpVotesRepository {
            io.Execute(dbIO => {
                dbIO.DeleteUpVote(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    userID: message.Payload.UserID
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleDownVotedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IDownVotesRepository, IUpVotesRepository {
            io.Transaction(dbIO => {
                dbIO.InsertDownVote(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    userID: message.Payload.UserID
                );
                dbIO.DeleteUpVote(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    userID: message.Payload.UserID
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleDownVoteRemovedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IDownVotesRepository {
            io.Execute(dbIO => {
                dbIO.DeleteDownVote(
                    articleID: message.Metadata.ID,
                    inText: message.Payload.InText,
                    sentenceIndex: message.Payload.SentencePos,
                    translationIndex: message.Payload.TranslationPos,
                    userID: message.Payload.UserID
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleArchivedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleRepository, IArticlesRepository {
            io.Transaction(dbIO => {
                dbIO.UpdateArticle(message.Metadata.ID,
                    archived: new DbUpdate<bool>(true)
                );
                dbIO.UpdateArticles(message.Metadata.ID,
                    archived: new DbUpdate<bool>(true)
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(),
                $"article:{message.Metadata.ID}",
                "articles");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<Guid, ArticleMainCommentDeletedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IArticleCommentsRepository {
            io.Execute(dbIO => {
                dbIO.UpdateArticleComment(message.Metadata.ID, message.Payload.CommentPos,
                    childCommentIndex: message.Payload.ChildCommentPos == null ? null : new DbUpdate<int?>(message.Payload.ChildCommentPos),
                    deleted: new DbUpdate<bool>(true)
                );
            });
            io.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, message.Metadata.ID.ToString(), $"article:{message.Metadata.ID}");
        }
    }
}