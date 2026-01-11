using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hermes.Core.Ports;
using Hermes.Core.Services;

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

        public static async Task<ArticleTemplateUploadResult> ExecuteAsync<IO>(IO io, ArticleTemplateUploadVideoCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository, ILanguagesRepository, ITopicsRepository
        {
            var user = io.FetchUser(userID);
            var language = io.FetchLanguage(cmd.LanguageID);
            var topic = io.FetchTopic(cmd.TopicID);
            UserService.ValidateAdminRights(user);
            LanguageService.ValidateExistence(language);
            TopicService.ValidateExistence(topic);
            if (string.IsNullOrEmpty(cmd.YoutubeURL)) throw new DomainException("Empty YouTube URL");

            // Fetch video data from YouTube
            var youtubeService = new YouTubeService();
            var videoData = await youtubeService.FetchVideoDataAsync(cmd.YoutubeURL);

            if (string.IsNullOrEmpty(videoData.Title)) throw new DomainException("Could not fetch video title");
            if (videoData.Captions == null || videoData.Captions.Count == 0) 
                throw new DomainException("No captions available for this video");

            // Split title into sentences (keep as single sentence if no period found)
            var titleSentences = Regex.Split(videoData.Title, "\\.+\\s")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new VideoSentence 
                { 
                    Text = s.Trim(),
                    StartTimeMs = null,
                    EndTimeMs = null
                })
                .ToList();

            if (titleSentences.Count == 0)
            {
                titleSentences.Add(new VideoSentence 
                { 
                    Text = videoData.Title,
                    StartTimeMs = null,
                    EndTimeMs = null
                });
            }

            // Convert captions to video sentences
            var textSentences = videoData.Captions
                .Select(c => new VideoSentence
                {
                    Text = c.Text,
                    StartTimeMs = c.StartTimeMs,
                    EndTimeMs = c.EndTimeMs
                })
                .ToList();

            var articleTemplateID = Guid.NewGuid();
            io.StoreEvent(new ArticleTemplateEvent (
                id: articleTemplateID,
                version: 1,
                eventName: "video-uploaded",
payload: new ArticleTemplateVideoUploadedEvent
{
    LanguageID = language.ID,
    Title = titleSentences,
    Text = textSentences,
    // Use the logical source, not the URL
    Source = string.IsNullOrWhiteSpace(cmd.Source) ? "youtube" : cmd.Source,
    PhotoURL = videoData.ThumbnailUrl ?? "",
    TopicID = cmd.TopicID,
    // Keep the real URL here
    VideoURL = videoData.VideoUrl
}
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
                eventName: "deleted",
                payload: new ArticleTemplateDeletedEvent(
                    user.ID
                )
            ));
        }
    }
}