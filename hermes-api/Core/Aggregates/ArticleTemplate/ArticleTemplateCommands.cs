using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Hermes.Core.Ports;

namespace Hermes.Core
{
    public static class ArticleTemplateCommands
    {
        public static ArticleTemplateUploadResult Execute<IO>(IO io, ArticleTemplateUploadCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository, ILanguagesRepository, ITopicsRepository
        {
            var user = io.FetchUser(userID);
            var language = io.FetchLanguage(cmd.LanguageID);
            var topic = io.FetchTopic(cmd.TopicID);
            UserService.ValidateAdminRights(user);
            LanguageService.ValidateExistence(language);
            TopicService.ValidateExistence(topic);
            if (string.IsNullOrEmpty(cmd.Title)) throw new DomainException("Empty title");
            else if (string.IsNullOrEmpty(cmd.Text)) throw new DomainException("Empty text");
            else if (string.IsNullOrEmpty(cmd.Source)) throw new DomainException("Empty source");
            else if (string.IsNullOrEmpty(cmd.PhotoURL)) throw new DomainException("Empty photo");
            var articleTemplateID = Guid.NewGuid();
            io.StoreEvent(new ArticleTemplateEvent (
                id: articleTemplateID,
                version: 1,
                timestamp: DateTime.UtcNow,
                stream: "articleTemplate",
                eventName: "uploaded",
                payload: new ArticleTemplateUploadedEvent(
                    language.ID,
                    new List<string>(Regex.Split(cmd.Title, "\\.+\\s")),
                    new List<string>(Regex.Split(cmd.Text, "\\.+\\s")),
                    cmd.Source,
                    cmd.PhotoURL,
                    cmd.TopicID
                )
            ));
            return new ArticleTemplateUploadResult(articleTemplateID);
        }

        public static void Execute<IO>(IO io, ArticleTemplateDeleteCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository, IArticleTemplatesRepository
        {
            var user = io.FetchUser(userID);
            var articleTemplate = io.FetchArticleTemplate(cmd.ArticleTemplateID);
            UserService.ValidateAdminRights(user);
            if (articleTemplate.Deleted)
                throw new DomainException("Article Template already deleted");
            io.StoreEvent(new ArticleTemplateEvent (
                id: articleTemplate.ID,
                version: articleTemplate.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "articleTemplate",
                eventName: "deleted",
                payload: new ArticleTemplateDeletedEvent(
                    user.ID
                )
            ));
        }
    }
}