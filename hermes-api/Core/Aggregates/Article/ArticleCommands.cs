using System;
using Hermes.Core.Ports;

namespace Hermes.Core
{
    public static class ArticleCommands
    {
        public static void Execute<IO>(IO io, ArticleCommentMainCommand cmd, string userID)
        where IO : IArticlesRepository, IEventsRepository, IRoomsRepository
        {
            var article = io.FetchArticle(cmd.ArticleID);
            var room = io.FetchRoom(article.RoomID);
            ArticleService.ValidateExistence(article);
            RoomService.ValidateExistence(room);
            RoomService.ValidateRoomUser(room, userID);
            var commentPos = cmd.ParentCommentPos == null
                ? article.Comments.Count
                : cmd.ParentCommentPos.Value < article.Comments.Count
                    ? cmd.ParentCommentPos.Value
                    : throw new DomainException("Invalid parent comment");
            var childCommentPos = cmd.ParentCommentPos == null ? (int?)null : article.Comments[cmd.ParentCommentPos.Value].Replies.Count;
            io.StoreEvent(new ArticleEvent(
                id: article.ID,
                version: article.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "article",
                eventName: "main.commented",
                payload: new ArticleMainCommentedEvent(
                    commentPos: commentPos,
                    childCommentPos: childCommentPos,
                    comment: cmd.Comment,
                    userID: userID
                )
            ));
        }

        public static void Execute<IO>(IO io, ArticleArchiveCommand cmd, string userID)
        where IO : IEventsRepository, IArticlesRepository, IRoomsRepository
        {
            var article = io.FetchArticle(cmd.ArticleID);
            var room = io.FetchRoom(article.RoomID);
            RoomService.ValidateRoomAdmin(room, userID);
            if (article.Deleted)
                throw new DomainException("Article already archived");
            io.StoreEvent(new ArticleEvent(
                id: article.ID,
                version: article.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "article",
                eventName: "archived",
                payload: new ArticleArchivedEvent(
                    userID: userID)));
        }

        public static void Execute<IO>(IO io, ArticleCommentCommand cmd, string userID)
        where IO : IEventsRepository, IArticlesRepository
        {
            var article = io.FetchArticle(cmd.ArticleID);
            if (String.IsNullOrEmpty(cmd.Comment))
                throw new DomainException("Empty comment");
            else
            {
                Translation t = ArticleService.GetTranslation(article, cmd.InText, cmd.SentencePos, cmd.TranslationPos);
                io.StoreEvent(new ArticleEvent(
                    id: article.ID,
                    version: article.Version + 1,
                    timestamp: DateTime.UtcNow,
                    stream: "article",
                    eventName: "commented",
                    payload: new ArticleCommentedEvent(
                        inText: cmd.InText,
                        sentencePos: cmd.SentencePos,
                        translationPos: cmd.TranslationPos,
                        commentPos: t.Comments.Count,
                        comment: cmd.Comment,
                        userID: userID
                    )
                ));
            }
        }

        public static void Execute<IO>(IO io, ArticleVoteCommand cmd, string userID)
        where IO : IEventsRepository, IArticlesRepository
        {
            var article = io.FetchArticle(cmd.ArticleID);
            Translation t = ArticleService.GetTranslation(article, cmd.InText, cmd.SentencePos, cmd.TranslationPos);
            if (cmd.Positive)
            {
                if (t.Upvotes.Contains(userID))
                    io.StoreEvent(new ArticleEvent(
                        id: article.ID,
                        version: article.Version + 1,
                        timestamp: DateTime.UtcNow,
                        stream: "article",
                        eventName: "upvote.removed",
                        payload: new ArticleUpVoteRemovedEvent(
                            inText: cmd.InText,
                            sentencePos: cmd.SentencePos,
                            translationPos: cmd.TranslationPos,
                            userID: userID
                        )
                    ));
                else
                    io.StoreEvent(new ArticleEvent(
                        id: article.ID,
                        version: article.Version + 1,
                        timestamp: DateTime.UtcNow,
                        stream: "article",
                        eventName: "upvoted",
                        payload: new ArticleUpVotedEvent(
                            inText: cmd.InText,
                            sentencePos: cmd.SentencePos,
                            translationPos: cmd.TranslationPos,
                            userID: userID
                        )
                    ));
            }
            else
            {
                if (t.Downvotes.Contains(userID))
                    io.StoreEvent(new ArticleEvent(
                        id: article.ID,
                        version: article.Version + 1,
                        timestamp: DateTime.UtcNow,
                        stream: "article",
                        eventName: "downvote.removed",
                        payload: new ArticleDownVoteRemovedEvent(
                            inText: cmd.InText,
                            sentencePos: cmd.SentencePos,
                            translationPos: cmd.TranslationPos,
                            userID: userID
                        )
                    ));
                else
                {
                    io.StoreEvent(new ArticleEvent(
                        id: article.ID,
                        version: article.Version + 1,
                        timestamp: DateTime.UtcNow,
                        stream: "article",
                        eventName: "downvoted",
                        payload: new ArticleDownVotedEvent(
                            inText: cmd.InText,
                            sentencePos: cmd.SentencePos,
                            translationPos: cmd.TranslationPos,
                            userID: userID
                        )
                    ));
                }
            }
        }

        public static void Execute<IO>(IO io, ArticleTranslateCommand cmd, string userID)
        where IO : IEventsRepository, IArticlesRepository
        {
            var article = io.FetchArticle(cmd.ArticleID);
            if (string.IsNullOrEmpty(cmd.Translation))
                throw new DomainException("Empty translation");
            else
            {
                ArticleService.ValidateDifferentTranslation(article, cmd.InText, cmd.SentencePos, cmd.Translation);
                var shouldReplace = ArticleService.ShouldReplaceLastTranslation(article, cmd.InText, cmd.SentencePos, userID);
                Sentence s = ArticleService.GetSentence(article, cmd.InText, cmd.SentencePos);
                io.StoreEvent(new ArticleEvent(
                    id: article.ID,
                    version: article.Version + 1,
                    timestamp: DateTime.UtcNow,
                    stream: "article",
                    eventName: "translated",
                    payload: new ArticleTranslatedEvent(
                        inText: cmd.InText,
                        sentencePos: cmd.SentencePos,
                        translationPos: shouldReplace ? s.TranslationHistory.Count - 1 : s.TranslationHistory.Count,
                        translation: cmd.Translation,
                        userID: userID
                    )
                ));
            }
        }

        public static ArticleTakeTemplateResult Execute<IO>(IO io, ArticleTakeTemplateCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository, IArticleTemplatesRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            var articleTemplate = io.FetchArticleTemplate(cmd.ArticleTemplateID);
            ArticleTemplateService.ValidateExistence(articleTemplate);
            RoomService.ValidateExistence(room);
            RoomService.ValidateRoomAdmin(room, userID);
            var translationLanguageID = (room.Languages[0] == articleTemplate.LanguageID ? room.Languages[1] : room.Languages[0]);
            if (string.IsNullOrEmpty(articleTemplate.LanguageID) || string.IsNullOrEmpty(translationLanguageID))
                throw new DomainException("Invalid language");
            var articleID = Guid.NewGuid();
            io.StoreEvent(new ArticleEvent(
                id: articleID,
                version: 1,
                timestamp: DateTime.UtcNow,
                stream: "article",
                eventName: "template.taken",
                payload: new ArticleTemplateTakenEvent(
                    articleTemplateID: articleTemplate.ID,
                    roomID: room.ID,
                    originalLanguageID: articleTemplate.LanguageID,
                    translationLanguageID: translationLanguageID,
                    title: articleTemplate.Title,
                    text: articleTemplate.Text,
                    source: articleTemplate.Source,
                    photoURL: articleTemplate.PhotoURL
                )
            ));
            return new ArticleTakeTemplateResult(articleID);
        }
            
        public static void Execute<IO>(IO io, ArticleDeleteMainCommentCommand command, string userID)
        where IO : IEventsRepository, IArticlesRepository, IRoomsRepository
        {
            var article = io.FetchArticle(command.ArticleID);
            var room = io.FetchRoom(article.RoomID);
            RoomService.ValidateRoomUser(room, userID);
            ArticleService.ValidateExistence(article);
            ArticleService.ValidateOwnComment(article, command.CommentPos, command.ChildCommentPos, userID);
            io.StoreEvent(new ArticleEvent(
                id: command.ArticleID,
                version: article.Version + 1,
                stream: "article",
                eventName: "main.comment.deleted",
                timestamp: DateTime.UtcNow,
                payload: new ArticleMainCommentDeletedEvent(
                    commentPos: command.CommentPos,
                    childCommentPos: command.ChildCommentPos
                )
            ));
        }
    }
}