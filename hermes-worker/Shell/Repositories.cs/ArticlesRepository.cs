using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticlesRepository
    {
        /// <summary>
        /// Original signature kept for compatibility.
        /// Defaults to non-video article with no template reference.
        /// </summary>
        public void InsertArticles(
            Guid articleID,
            string roomID,
            string title,
            DateTime created,
            string originalLanguageID,
            string translationLanguageID,
            string photoURL,
            bool archived)
        {
            InsertArticles(
                articleID: articleID,
                roomID: roomID,
                title: title,
                created: created,
                originalLanguageID: originalLanguageID,
                translationLanguageID: translationLanguageID,
                photoURL: photoURL,
                archived: archived,
                isVideo: false,
                videoURL: null,
                articleTemplateID: null
            );
        }

        /// <summary>
        /// New overload that also persists IsVideo, VideoURL and ArticleTemplateID
        /// into Query_Articles. This is what PublicView will consume.
        /// </summary>
        public void InsertArticles(
            Guid articleID,
            string roomID,
            string title,
            DateTime created,
            string originalLanguageID,
            string translationLanguageID,
            string photoURL,
            bool archived,
            bool isVideo,
            string videoURL,
            Guid? articleTemplateID)
        {
            _connection.Execute(@"
                INSERT INTO Query_Articles(
                    ArticleID,
                    RoomID,
                    Title,
                    Created,
                    OriginalLanguageID,
                    TranslationLanguageID,
                    PhotoURL,
                    Archived,
                    IsVideo,
                    VideoURL,
                    ArticleTemplateID
                )
                VALUES(
                    @articleID,
                    @roomID,
                    @title,
                    @created,
                    @originalLanguageID,
                    @translationLanguageID,
                    @photoURL,
                    @archived,
                    @isVideo,
                    @videoURL,
                    @articleTemplateID
                )
                ON DUPLICATE KEY UPDATE
                    Title                 = @title,
                    Created               = @created,
                    OriginalLanguageID    = @originalLanguageID,
                    TranslationLanguageID = @translationLanguageID,
                    PhotoURL              = @photoURL,
                    Archived              = @archived,
                    IsVideo               = @isVideo,
                    VideoURL              = @videoURL,
                    ArticleTemplateID     = @articleTemplateID;",
                new
                {
                    articleID,
                    roomID,
                    title,
                    created,
                    originalLanguageID,
                    translationLanguageID,
                    photoURL,
                    archived,
                    isVideo,
                    videoURL,
                    articleTemplateID
                },
                transaction: _transaction
            );
        }

        public void UpdateArticles(Guid articleID, DbUpdate<bool> archived = null)
        {
            if (archived != null)
            {
                _connection.Execute(@"
                    UPDATE Query_Articles
                    SET Archived = CASE @setArchived WHEN 1 THEN @archived ELSE Archived END
                    WHERE ArticleID = @articleID",
                    new
                    {
                        articleID,
                        setArchived = archived != null,
                        archived = archived?.Value
                    },
                    transaction: _transaction
                );
            }
        }
    }
}