using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticleTemplatesRepository
    {
        /// <summary>
        /// Original signature kept for compatibility. Defaults to non-video.
        /// </summary>
        public void InsertArticleTemplates(
            Guid articleTemplateID,
            string title,
            DateTime created,
            string topicID,
            string languageID,
            string photoURL,
            bool archived)
        {
            // Delegate to the new overload with isVideo = false, videoURL = null
            InsertArticleTemplates(
                articleTemplateID: articleTemplateID,
                title: title,
                created: created,
                topicID: topicID,
                languageID: languageID,
                photoURL: photoURL,
                archived: archived,
                isVideo: false,
                videoURL: null
            );
        }

        /// <summary>
        /// New overload that also persists IsVideo and VideoURL into Query_ArticleTemplates.
        /// This is used by video uploads so the list view can see isVideo = true.
        /// </summary>
        public void InsertArticleTemplates(
            Guid articleTemplateID,
            string title,
            DateTime created,
            string topicID,
            string languageID,
            string photoURL,
            bool archived,
            bool isVideo,
            string videoURL)
        {
            _connection.Execute(@"
                INSERT INTO Query_ArticleTemplates(
                    ArticleTemplateID,
                    Title,
                    Created,
                    TopicID,
                    LanguageID,
                    PhotoURL,
                    Archived,
                    IsVideo,
                    VideoURL
                )
                VALUES(
                    @articleTemplateID,
                    @title,
                    @created,
                    @topicID,
                    @languageID,
                    @photoURL,
                    @archived,
                    @isVideo,
                    @videoURL
                )
                ON DUPLICATE KEY UPDATE
                    Title      = @title,
                    Created    = @created,
                    TopicID    = @topicID,
                    LanguageID = @languageID,
                    PhotoURL   = @photoURL,
                    Archived   = @archived,
                    IsVideo    = @isVideo,
                    VideoURL   = @videoURL;",
                new
                {
                    articleTemplateID,
                    title,
                    created,
                    topicID,
                    languageID,
                    photoURL,
                    archived,
                    isVideo,
                    videoURL
                },
                transaction: _transaction
            );
        }

        public void UpdateArticleTemplates(Guid articleTemplateID, DbUpdate<bool> archived = null)
        {
            if (archived != null)
            {
                _connection.Execute(
                    @"  UPDATE Query_ArticleTemplates
                        SET Archived = CASE @setArchived WHEN 1 THEN @archived ELSE Archived END
                        WHERE ArticleTemplateID = @articleTemplateID",
                    new
                    {
                        setArchived = archived != null,
                        archived = archived?.Value,
                        articleTemplateID
                    },
                    transaction: _transaction
                );
            }
        }
    }
}
